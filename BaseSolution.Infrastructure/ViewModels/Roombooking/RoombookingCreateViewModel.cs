﻿using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Roombooking.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Domain.Entities;

namespace BaseSolution.Infrastructure.ViewModels.Roombooking
{
    public class RoombookingCreateViewModel : ViewModelBase<RoombookingCreateRequest>
    {
        public readonly IRoombookingReadOnlyRepository _roombookingReadOnlyRepository;
        public readonly IRoombookingReadWriteRepository _roombookingReadWriteRespository;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        public RoombookingCreateViewModel(IRoombookingReadOnlyRepository roombookingReadOnlyRepository, IRoombookingReadWriteRepository roombookingReadWriteRepository, IMapper mapper, ILocalizationService localizationService)
        {
            _roombookingReadOnlyRepository = roombookingReadOnlyRepository;
            _roombookingReadWriteRespository = roombookingReadWriteRepository;
            _mapper = mapper;
            _localizationService = localizationService;
        }
        public async override Task HandleAsync(RoombookingCreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var createResult = await _roombookingReadWriteRespository.AddRoomBookingAsync(_mapper.Map<RoomBookingEntity>(request), cancellationToken);
                if (createResult.Success)
                {
                    var result = await _roombookingReadOnlyRepository.GetRoombookingByIdAsync(createResult.Data, cancellationToken);

                    Data = createResult!;
                    Success = result.Success;
                    ErrorItems = result.Errors;
                    Message = result.Message;
                    return;
                }

                Success = createResult.Success;
                ErrorItems = createResult.Errors;
                Message = createResult.Message;
            }
            catch
            {

                Success = false;
                ErrorItems = new[]
                {
                    new ErrorItem
                    {
                        Error = _localizationService["Error occurred while getting the RoomBooking"],
                        FieldName = string.Concat(LocalizationString.Common.FailedToGet, "RoomBooking")
                    }
                };
            }
        }
    }
}
