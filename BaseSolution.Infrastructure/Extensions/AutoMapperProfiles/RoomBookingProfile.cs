﻿using AutoMapper;
using BaseSolution.Domain.Entities;
using BaseSolution.Application.DataTransferObjects.Roombooking;
using BaseSolution.Application.DataTransferObjects.Roombooking.Request;

namespace BaseSolution.Infrastructure.Extensions.AutoMapperProfiles
{
    public class RoomBookingProfile : Profile
    {
        public RoomBookingProfile()
        {
            CreateMap<RoomBookingEntity, RoombookingDTO>()
                     .ForMember(des => des.NameCustomer, otp => otp.MapFrom(src => src.Customer.Name))
                     .ForMember(des => des.StaffName, otp => otp.MapFrom(src => src.User.Name))
                     .ForMember(des => des.NameBuilding, otp => otp.MapFrom(src => src.RoomBookingDetails.Select(x => x.RoomDetail.Floor.Building.Name).FirstOrDefault()))
                     .ForMember(des => des.NameFloor, otp => otp.MapFrom(src => src.RoomBookingDetails.Select(x => x.RoomDetail.Floor.Name).FirstOrDefault()))
                     .ForMember(des => des.NameRoom, otp => otp.MapFrom(src => src.RoomBookingDetails.Select(x => x.RoomDetail.Name).FirstOrDefault()))
                     .ForMember(des => des.CountServices, otp => otp.MapFrom(src => src.Customer.ServiceOrders.Select(x => x.CustomerId).Count()))
                     .ForMember(des => des.ServicePrice, otp => otp.MapFrom(src => src.Customer.ServiceOrders.SelectMany(x => x.ServiceOrderDetails.Select(x => x.Price)).FirstOrDefault()))
                     .ForMember(des => des.RoomPrice, otp => otp.MapFrom(src => src.RoomBookingDetails.Select(x => x.Price).FirstOrDefault()));

            CreateMap<RoombookingCreateRequest, RoomBookingEntity>()
                .ForPath(des => des.RoomBookingDetails,opt => opt.MapFrom(src => new List<RoomBookingDetailEntity>
                {
                     new RoomBookingDetailEntity
                     {
                         RoomDetailId = src.RoomDetailId
                     }
                }))
                .ForMember(des => des.CustomerId,opt => opt.MapFrom(src => src.CustomerId))
                ;
            CreateMap<RoombookingUpdateRequest, RoomBookingEntity>()
             .ForPath(des => des.RoomBookingDetails, opt => opt.MapFrom(src => new List<RoomBookingDetailEntity>
                {
                     new RoomBookingDetailEntity
                     {
                         RoomDetailId = src.RoomDetailId
                     }
                }))
                .ForMember(des => des.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                ;
            CreateMap<RoombookingDeleteRequest, RoomBookingEntity>();
        }
    }
}
