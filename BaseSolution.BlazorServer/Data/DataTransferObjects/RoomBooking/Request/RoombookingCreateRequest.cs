﻿using BaseSolution.BlazorServer.Data.ValueObjects.Common;

namespace BaseSolution.BlazorServer.Data.DataTransferObjects.RoomBooking.Request
{
    public class RoombookingCreateRequest
    {
        public Guid CustomerId { get; set; }
        public Guid RoomDetailId { get; set; }
        public BookingType BookingType { get; set; }
        public string CodeBooking { get; set; }



    }
}