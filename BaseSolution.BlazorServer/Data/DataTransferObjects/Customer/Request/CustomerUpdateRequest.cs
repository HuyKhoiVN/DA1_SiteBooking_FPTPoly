﻿using BaseSolution.BlazorServer.Data.ValueObjects.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.BlazorServer.Data.DataTransferObjects.Customer.Request
{
    public class CustomerUpdateRequest
    {
        public Guid Id { get; set; }
        [RegularExpression(@"^[\p{L}\s]{5,}$", ErrorMessage = "Vui lòng nhập tên có ít nhất 5 ký tự")]
        public string Name { get; set; } = string.Empty;
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Vui lòng nhập đúng định dạng email.")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Vui lòng nhập số điện thoại từ 10 đến 11 chữ số.")]
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid? ModifiedBy { get; set; }

    }
}
