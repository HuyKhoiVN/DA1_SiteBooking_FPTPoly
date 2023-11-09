﻿using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Roombooking.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.ViewModels.Roombooking;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoombookingsController : ControllerBase
{
    private readonly IRoombookingReadOnlyRepository _roombookingrReadOnlyRespository;
    private readonly IRoombookingReadWriteRepository _roombookingReadWriteRespository;
    private readonly IMapper _mapper;
    private readonly ILocalizationService _localizationService;
    public RoombookingsController(IRoombookingReadOnlyRepository roombookingReadOnlyRepository, IRoombookingReadWriteRepository roombookingReadWriteRepository, IMapper mapper, ILocalizationService localizationService)
    {
        _roombookingrReadOnlyRespository = roombookingReadOnlyRepository;
        _roombookingReadWriteRespository = roombookingReadWriteRepository;
        _mapper = mapper;
        _localizationService = localizationService;
    }
    [HttpGet]
    public async Task<IActionResult> GetListRoomBookingDetailByAdmin([FromQuery] ViewRoombookingWithPaginationRequest request, CancellationToken cancellationToken)
    {
        RoombookingListWithPaginationViewModel vm = new(_roombookingrReadOnlyRespository, _localizationService);
        await vm.HandleAsync(request, cancellationToken);

        return Ok(vm);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoomBookingDetailByAdmin(Guid id, CancellationToken cancellationToken)
    {
        RoombookingViewModel vm = new(_roombookingrReadOnlyRespository, _localizationService);
        await vm.HandleAsync(id, cancellationToken);
        return Ok(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Post(RoombookingCreateRequest request, CancellationToken cancellationToken)
    {
        RoombookingCreateViewModel vm = new(_roombookingrReadOnlyRespository, _roombookingReadWriteRespository, _mapper, _localizationService);
        await vm.HandleAsync(request, cancellationToken);

        return Ok(vm);
    }

    [HttpPut]
    public async Task<IActionResult> Put(RoombookingUpdateRequest request, CancellationToken cancellationToken)
    {
        RoombookingUpdateViewModel vm = new(_roombookingReadWriteRespository, _mapper, _localizationService);
        await vm.HandleAsync(request, cancellationToken);

        return Ok(vm);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(RoombookingDeleteRequest request, CancellationToken cancellationToken)
    {
        RoombookingDeleteViewModel vm = new(_roombookingReadWriteRespository, _localizationService, _mapper);

        await vm.HandleAsync(request, cancellationToken);

        return Ok(vm);
    }
}