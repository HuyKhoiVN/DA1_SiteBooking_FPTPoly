﻿namespace BaseSolution.BlazorServer.Data.DataTransferObjects.RoomType.Request
{
    public class RoomTypeCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
    }
}
