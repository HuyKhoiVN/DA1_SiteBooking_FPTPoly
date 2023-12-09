﻿using AutoMapper;
using BaseSolution.Application.DataTransferObjects.ServiceOrder;
using BaseSolution.Application.DataTransferObjects.ServiceOrder.Request;
using BaseSolution.Domain.Entities;


namespace BaseSolution.Infrastructure.Extensions.AutoMapperProfiles
{
    public class ServiceOrderProfile : Profile
    {
        public ServiceOrderProfile()
        {
            CreateMap<ServiceOrderEntity, ServiceOrderDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Id).FirstOrDefault()))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Name).FirstOrDefault()))
                .ForMember(des => des.Price, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Price).FirstOrDefault()));

            CreateMap<ServiceOrderEntity, ServiceOrderForRoomBookingDTO>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Id).FirstOrDefault()))
               .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Name).FirstOrDefault()))
               .ForMember(dest => dest.RoomBookingDetailId, opt => opt.MapFrom(src => src.RoomBookingDetailId))
               .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.RoomBookingDetail.RoomBooking.CustomerId));


            CreateMap<ServiceOrderEntity, ServiceOrderForServiceOrderDTO>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Id).FirstOrDefault()))
               .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceOrderDetails.Select(x => x.Service.Name).FirstOrDefault()))
                 .ForMember(des => des.CustomerId, opt => opt.MapFrom(src => src.CustomerId));


            CreateMap<ServiceOrderCreateRequest, ServiceOrderEntity>()
                  .ForPath(des => des.ServiceOrderDetails, opt => opt.MapFrom(src =>
                 new List<ServiceOrderDetailEntity>
                {
                     new ServiceOrderDetailEntity
                     {
                         ServiceId = src.ServiceId,
                     }
                }))
                .ForMember(des => des.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                ;

            CreateMap<ServiceOrderUpdateRequest, ServiceOrderEntity>();


            CreateMap<ServiceOrderCreateForRoomBookingRequest, ServiceOrderEntity>()
                .ForMember(des => des.RoomBookingDetailId, opt => opt.MapFrom(src => src.RoomBookingDetailId))
                .ForPath(des => des.ServiceOrderDetails, opt => opt.MapFrom(src =>
                 new List<ServiceOrderDetailEntity>
                {
                     new ServiceOrderDetailEntity
                     {
                         ServiceId = src.ServiceId,
                     }
                }))
                ;
            CreateMap<ServiceOrderCreateForCustomerRequest, ServiceOrderEntity>()
                .ForMember(des => des.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForPath(des => des.ServiceOrderDetails, opt => opt.MapFrom(src =>
                 new List<ServiceOrderDetailEntity>
                {
                     new ServiceOrderDetailEntity
                     {
                         ServiceId = src.ServiceId,
                     }
                }))
                ;
        }
    }
}
