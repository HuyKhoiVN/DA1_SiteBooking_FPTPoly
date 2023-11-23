﻿using AutoMapper;
using BaseSolution.Application.DataTransferObjects.RoomType;
using BaseSolution.Application.DataTransferObjects.RoomType.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Pagination;
using BaseSolution.Infrastructure.ViewModels.RoomType;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeReadOnlyRepository _RoomTypeReadOnlyRespository;
        private readonly IRoomTypeReadWriteRepository _RoomTypeReadWriteRespository;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        public RoomTypesController(IRoomTypeReadOnlyRepository RoomTypeReadOnlyRepository, IRoomTypeReadWriteRepository RoomTypeReadWriteRepository, IMapper mapper, ILocalizationService localizationService)
        {
            _RoomTypeReadOnlyRespository = RoomTypeReadOnlyRepository;
            _RoomTypeReadWriteRespository = RoomTypeReadWriteRepository;
            _mapper = mapper;
            _localizationService = localizationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetListRoomTypeByAdmin([FromQuery] ViewRoomTypeWithPaginationRequest request, CancellationToken cancellationToken)
        {
            RoomTypeListWithPaginationViewModel vm = new(_RoomTypeReadOnlyRespository, _localizationService);
            await vm.HandleAsync(request, cancellationToken);
            if(vm.Success)
            {
                PaginationResponse<RoomTypeDTO> result = (PaginationResponse<RoomTypeDTO>)vm.Data;
                return Ok(result);
            }
            return BadRequest(vm);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomTypeById(Guid id, CancellationToken cancellationToken)
        {
            RoomTypeViewModel vm = new(_RoomTypeReadOnlyRespository, _localizationService);
            await vm.HandleAsync(id, cancellationToken);
            if(vm.Success)
            {
                RoomTypeDTO result = (RoomTypeDTO)vm.Data;
                return Ok(result);
            }
            return BadRequest(vm);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewRoomType(RoomTypeCreateRequest request, CancellationToken cancellationToken)
        {
            RoomTypeCreateViewModel vm = new(_RoomTypeReadOnlyRespository, _RoomTypeReadWriteRespository, _mapper, _localizationService);
            await vm.HandleAsync(request, cancellationToken);
            if(vm.Success)
            {
                return Ok(vm);
            }
            return BadRequest(vm);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoomType(RoomTypeUpdateRequest request, CancellationToken cancellationToken)
        {
            RoomTypeUpdateViewModel vm = new(_RoomTypeReadWriteRespository, _mapper, _localizationService);
            await vm.HandleAsync(request, cancellationToken);
            if (vm.Success)
            {
                return Ok(vm);
            }
            return BadRequest(vm);
            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoomType([FromQuery]RoomTypeDeleteRequest request, CancellationToken cancellationToken)
        {
            RoomTypeDeleteViewModel vm = new(_RoomTypeReadWriteRespository, _localizationService, _mapper);

            await vm.HandleAsync(request, cancellationToken);

            if (vm.Success)
            {
                return Ok(vm);
            }
            return BadRequest(vm);
        }
    }
}
