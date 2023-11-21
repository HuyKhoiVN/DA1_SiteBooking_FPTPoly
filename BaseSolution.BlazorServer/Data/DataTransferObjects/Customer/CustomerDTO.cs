﻿using BaseSolution.BlazorServer.Data.ValueObjects.Common;
using BaseSolution.BlazorServer.Enums;

namespace BaseSolution.BlazorServer.Data.DataTransferObjects.Customer
{
    public class CustomerDTO
    {
        public string Name { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ApprovedCode { get; set; }
        public DateTimeOffset? ApprovedCodeExpiredTime { get; set; }
        public CustomerType CustomerType { get; set; }
        public EntityStatus Status { get; set; }

    }
}
