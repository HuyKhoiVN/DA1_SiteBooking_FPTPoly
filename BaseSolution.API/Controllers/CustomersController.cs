﻿using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Customer.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Extensions;
using BaseSolution.Infrastructure.ViewModels.Customer;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using BaseSolution.Application.DataTransferObjects.Customer;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Domain.Entities;
using System.Threading;

namespace BaseSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public readonly ICustomerReadOnlyRepository _CustomerReadOnlyRepository;
        public readonly ICustomerReadWriteRepository _CustomerReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private string _verifyCode = string.Empty;

        public CustomersController(ICustomerReadOnlyRepository CustomerReadOnlyRepository, ICustomerReadWriteRepository CustomerReadWriteRepository, ILocalizationService localizationService, IMapper mapper)
        {
            _CustomerReadOnlyRepository = CustomerReadOnlyRepository;
            _CustomerReadWriteRepository = CustomerReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        // GET api/<CustomerController>/5
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ViewCustomerWithPaginationRequest request, CancellationToken cancellationToken)
        {
            CustomerListWithPaginationViewModel vm = new(_CustomerReadOnlyRepository, _localizationService);

            await vm.HandleAsync(request, cancellationToken);
            if (vm.Success)
            {
                PaginationResponse<CustomerDto> result = new();
                result = (PaginationResponse<CustomerDto>)vm.Data;
                return Ok(result);
            }
            return BadRequest(vm);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            CustomerViewModel vm = new(_CustomerReadOnlyRepository, _localizationService);

            await vm.HandleAsync(id, cancellationToken);
            if (vm.Success)
            {
                CustomerDto result = (CustomerDto)vm.Data;
                return Ok(result);
            }
            return BadRequest(vm);
        }
        [HttpGet("{identification}/details")]
        public async Task<IActionResult> GetIdentificationNumber(string identification, CancellationToken cancellationToken)
        {
            CustomerIdentificationViewModel vm = new(_CustomerReadOnlyRepository, _localizationService);

            await vm.HandleAsync(identification, cancellationToken);
            if (vm.Success)
            {
                CustomerDto result = (CustomerDto)vm.Data;
                return Ok(result);
            }
            return Ok(vm);
        }
        [AllowAnonymous]
        [HttpPost("sendGmail")]
        public async Task<IActionResult> SendGmailAsync([FromBody] string emailAddress)
        {
            _verifyCode = UtilityExtensions.GenerateRandomString(6);
            if (EmailVerification.SetCodeForEmail(emailAddress, _verifyCode))
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("hainhph35304@fpt.edu.vn"));
                email.To.Add(MailboxAddress.Parse(emailAddress));
                email.Subject = "Mã đăng nhập";

                var body = new TextPart("html")
                {
                    Text = "<h1><strong>" + _verifyCode + "</strong></h1>"
                };
                email.Body = body;

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("hainhph35304@fpt.edu.vn", "jumm lvtt uavb kebq");
                    await client.SendAsync(email);
                    client.Disconnect(true);
                }
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("verifyCustomerBooking/{identification}/{code}")]
        public async Task<IActionResult> VerifyCustomerBooking(string identification, string code, CancellationToken cancellationToken)
        {
            var details = await _CustomerReadOnlyRepository.GetCustomerByIdentificationAsync(identification, cancellationToken);
            if (details.Data?.ApprovedCode == code)
            {
                return Ok("Xác nhận mã thành công");
            }
            else
            {
                return BadRequest("Xác nhận mã thất bại");
            }
        }
        [AllowAnonymous]
        [HttpPost("verifyCode/{code}/{idCard}")]
        public async Task<IActionResult> VerifyCode(string code, string idCard, CancellationToken cancellationToken)
        {
            var details = await _CustomerReadOnlyRepository.GetCustomerByIdentificationAsync(idCard, cancellationToken);
            if (details.Data?.ApprovedCodeExpiredTime < DateTime.Now)
            {
                return BadRequest("Đã hết thời gian xác nhận mã!");
            }
            else if(details.Data?.ApprovedCode != code)
            {
                return BadRequest("Mã xác nhận không đúng, vui lòng kiểm tra lại");
            }
            else if (details.Data?.ApprovedCode == code && details.Data?.ApprovedCodeExpiredTime >= DateTime.Now)
            {
                return Ok("Xác nhận mã thành công");
            }
            else
            {
                return BadRequest("Xác nhận mã thất bại");
            }
        }
        [HttpPost("SignUpOrSignIn")]
        public async Task<IActionResult> SignUpOrSignIn(CustomerCreateRequest customerCreateRequest, CancellationToken cancellationToken)
        {
            var checkExsits = await _CustomerReadOnlyRepository.GetCustomerByIdentificationAsync(customerCreateRequest.IdentificationNumber, cancellationToken);
            if (checkExsits.Data == null)
            {
                await SendGmailAsync(customerCreateRequest.Email);
                customerCreateRequest.ApprovedCode = _verifyCode;
                var newCustomers = new CustomerEntity()
                {
                    Email = customerCreateRequest.Email,
                    IdentificationNumber = customerCreateRequest.IdentificationNumber,
                    PhoneNumber = customerCreateRequest.PhoneNumber,
                    Name = customerCreateRequest.Name,
                    ApprovedCode = customerCreateRequest.ApprovedCode,
                    CreatedTime = DateTime.Now,
                    ApprovedCodeExpiredTime = DateTime.Now.AddMinutes(5),
                    Status = Domain.Enums.EntityStatus.PendingForConfirmation,
                };
                await _CustomerReadWriteRepository.AddCustomerAsync(newCustomers, cancellationToken);
                return Ok("Gửi mã thành công!");
            }
            else if (checkExsits.Data != null)
            {
                var detailCustomer = await _CustomerReadOnlyRepository.GetCustomerByIdentificationAsync(customerCreateRequest.IdentificationNumber, cancellationToken);
                if (detailCustomer.Data?.ApprovedCodeExpiredTime < DateTime.Now)
                {
                    await SendGmailAsync(customerCreateRequest.Email);
                    customerCreateRequest.ApprovedCode = _verifyCode;
                    var newCustomers = new CustomerEntity()
                    {
                        Id = detailCustomer.Data.Id,
                        Email = customerCreateRequest.Email,
                        PhoneNumber = customerCreateRequest.PhoneNumber,
                        Name = customerCreateRequest.Name,
                        ApprovedCode = customerCreateRequest.ApprovedCode,
                        ModifiedTime = DateTime.Now,
                        ApprovedCodeExpiredTime = DateTime.Now.AddMinutes(5),
                        Status = Domain.Enums.EntityStatus.PendingForConfirmation,
                    };
                    await _CustomerReadWriteRepository.UpdateCustomerAsync(newCustomers, cancellationToken);
                    return Ok("Mã đã được gửi, vui lòng kiểm tra email!");
                }
                else if (detailCustomer.Data!.Email != customerCreateRequest.Email)
                {
                    await SendGmailAsync(customerCreateRequest.Email);
                    customerCreateRequest.ApprovedCode = _verifyCode;
                    var newCustomers = new CustomerEntity()
                    {
                        Id = detailCustomer.Data.Id,
                        Email = customerCreateRequest.Email,
                        PhoneNumber = customerCreateRequest.PhoneNumber,
                        Name = customerCreateRequest.Name,
                        ApprovedCode = customerCreateRequest.ApprovedCode,
                        ModifiedTime = DateTime.Now,
                        IdentificationNumber = customerCreateRequest.IdentificationNumber,
                        ApprovedCodeExpiredTime = DateTime.Now.AddMinutes(5),
                        Status = Domain.Enums.EntityStatus.PendingForConfirmation,
                    };
                    await _CustomerReadWriteRepository.UpdateCustomerAsync(newCustomers, cancellationToken);
                    return Ok("Mã đã được gửi, vui lòng kiểm tra email!");
                }
                else if (detailCustomer.Data?.ApprovedCode == null && detailCustomer.Data?.ApprovedCodeExpiredTime == null)
                {
                    await SendGmailAsync(customerCreateRequest.Email);
                    customerCreateRequest.ApprovedCode = _verifyCode;
                    var newCustomers = new CustomerEntity()
                    {
                        Id = detailCustomer.Data!.Id,
                        Email = customerCreateRequest.Email,
                        PhoneNumber = customerCreateRequest.PhoneNumber,
                        Name = customerCreateRequest.Name,
                        ApprovedCode = customerCreateRequest.ApprovedCode,
                        ModifiedTime = DateTime.Now,
                        IdentificationNumber = customerCreateRequest.IdentificationNumber,
                        ApprovedCodeExpiredTime = DateTime.Now.AddMinutes(5),
                        Status = Domain.Enums.EntityStatus.PendingForConfirmation,
                    };
                    await _CustomerReadWriteRepository.UpdateCustomerAsync(newCustomers, cancellationToken);
                    return Ok("Mã đã được gửi, vui lòng kiểm tra email!");
                }
            }
            else
            {
                return Ok("Không thể gửi mãs!");
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Post(CustomerCreateRequest request, CancellationToken cancellationToken)
        {
            CustomerCreateViewModel vm = new(_CustomerReadOnlyRepository, _CustomerReadWriteRepository, _localizationService, _mapper);

            await vm.HandleAsync(request, cancellationToken);
            if (vm.Success)
            {
                return Ok(vm);
            }
            return BadRequest(vm);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CustomerUpdateRequest request, CancellationToken cancellationToken)
        {
            CustomerUpdateViewModel vm = new(_CustomerReadWriteRepository, _localizationService, _mapper);

            await vm.HandleAsync(request, cancellationToken);
            if (vm.Success)
            {
                return Ok(vm);
            }
            return BadRequest(vm);
        }
        [AllowAnonymous]
        [HttpPut("UpdateStatusCustomer/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, CancellationToken cancellationToken)
        {
            CustomerUpdateStatusViewModel vm = new(_CustomerReadWriteRepository, _localizationService, _mapper);

            await vm.HandleAsync(id, cancellationToken);

            return Ok(vm.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] CustomerDeleteRequest request, CancellationToken cancellationToken)
        {
            CustomerDeleteViewModel vm = new(_CustomerReadWriteRepository, _localizationService, _mapper);

            await vm.HandleAsync(request, cancellationToken);
            if (vm.Success)
            {
                return Ok(vm);
            }
            return BadRequest(vm);
        }
    }
}
