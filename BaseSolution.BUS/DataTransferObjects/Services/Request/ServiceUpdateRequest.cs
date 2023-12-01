﻿using BaseSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.Services.Request
{
    public class ServiceUpdateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Unit { get; set; } = string.Empty;
        public EntityStatus Status { get; set; }
        public bool IsRoomBookingNeed { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid ServiceTypeId { get; set; }
    }
}
