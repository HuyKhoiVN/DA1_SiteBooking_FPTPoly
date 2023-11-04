﻿using Azure.Core;
using BaseSolution.Application.DataTransferObjects.Floor.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ViewModels;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.ViewModels.Floor
{
    public class FloorListWithPaginationByAdminViewModel : ViewModelBase<ViewFloorWithPaginationRequest>
    {
        private readonly IFloorReadOnlyRespository _floorReadOnlyRespository;
        private readonly ILocalizationService _localizationService;
        public FloorListWithPaginationByAdminViewModel(IFloorReadOnlyRespository floorReadOnlyRespository, ILocalizationService localizationService)
        {
            _floorReadOnlyRespository = floorReadOnlyRespository;
            _localizationService = localizationService;
        }
        public async override Task HandleAsync(ViewFloorWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _floorReadOnlyRespository.GetFloorWithPaginationByAdminAsync(request, cancellationToken);

                Data = result.Data!;
                Success = result.Success;
                ErrorItems = result.Errors;
                Message = result.Message;
                return;
            }
            catch
            {
                Success = false;
                ErrorItems = new[]
                {
                new ErrorItem
                {
                    Error = _localizationService["Error occurred while getting the list of Floor"],
                    FieldName = string.Concat(LocalizationString.Common.FailedToGet, "list of Floor")
                }
            };
            }
        }
    }
}
