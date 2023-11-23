﻿using BaseSolution.Domain.Enums;

namespace BaseSolution.Application.DataTransferObjects.RoomType
{
    public class RoomTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EntityStatus Status { get; set; }
        //Số lượng loại phòng này trong khách sạn
        public int NumberOfRoomDetails { get; set; }
    }
}
