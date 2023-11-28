﻿using BaseSolution.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Application.DataTransferObjects.ServiceOrder
{
    public class ServiceOrderDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; } // số lượng 
        public decimal Price { get; set; }
        public float TotalAmount { get; set; }
        public EntityStatus Status { get; set; } 

        // base on 
        public string CustomerName { get; set; }  // Tên khách hàng
        public Guid CustomerId { get; set; }
        public string Name { get; set; }// tên dịch vụ
        public Guid? RoomBookingDetailId { get; set; } // ncheck xem dịch vụ được đặt theo phong hay riêng 
    }
}
