﻿using BaseSolution.BlazorServer.Data.DataTransferObjects.AmenityRoomDetail;
using BaseSolution.BlazorServer.Data.DataTransferObjects.AmenityRoomDetail.Request;
using BaseSolution.BlazorServer.Data.ValueObjects.Pagination;
using BaseSolution.BlazorServer.Respository.Interfaces;

namespace BaseSolution.BlazorServer.Respository.Implements
{
    public class AmenityRoomDetailRepo : IAmenityRoomDetailRepo
    {
        private readonly HttpClient _httpClient;
        public AmenityRoomDetailRepo(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginationResponse<AmenityRoomDetailDTO>> GetAllAmentityRoomDetail(ViewAmenityRoomDetailWithPaginationRequest request)
        {
            string url = $"/api/AmenityRoomDetails?PageNumber={request.PageNumber}&PageSize={request.PageSize}";
            
            var result = await _httpClient.GetFromJsonAsync<PaginationResponse<AmenityRoomDetailDTO>>(url);
            return result;
        }
    }
}
