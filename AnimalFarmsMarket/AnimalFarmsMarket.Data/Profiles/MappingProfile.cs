 using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Enum;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.ViewModels;
using AutoMapper;
using System;


namespace AnimalFarmsMarket.Data.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LivestockResponseDto, EditLivestockViewModel>().ReverseMap();
            CreateMap<LivestockUpdateDto, EditLivestockViewModel>().ReverseMap();

            CreateMap<AppUser, UserToReturnDto>().ReverseMap();

            CreateMap<AddressToReturnDto, Address>().ReverseMap();

            CreateMap<AppUser, RegisterDeliveryPersonDto>().ReverseMap();
            CreateMap<RegisterAgentDto, AppUser>().ForMember(appuser => appuser.Address, y=>y.MapFrom(agentdto=>agentdto.Address)).ReverseMap();
            CreateMap<RegisterUserDto, AppUser>().ReverseMap();
            CreateMap<AppUser, UserDto>();
            CreateMap<LivestockUpdateDto, Livestock>();
            CreateMap<Livestock, LivestockResponseDto>();
            CreateMap<LivestockToAdd, Livestock>();

            CreateMap<UserToReturnDto, ProfileViewModel>().ReverseMap();
            CreateMap<UserToReturnDto, UpdateProfileViewModel>().ReverseMap();
            CreateMap<ChangePasswordDto, ChangePasswordViewModel>().ReverseMap();


            CreateMap<Review, ReviewDto>();
            CreateMap<Review, ReviewToReturnDto>();
            CreateMap<ReviewToAddDto, Review>();

            CreateMap<AddRatingDto, Rating>().ReverseMap();
            CreateMap<GetRatingsDto, Rating>().ReverseMap();

            CreateMap<Market, MarketDto>();

            CreateMap<Market, AddLivestockMarketDto>();

            CreateMap<LivestockImages, LivestockImagesToReturnDto>();

            CreateMap<AppUser, UserDto>();

            CreateMap<Livestock, OrderLivestockResDto>();

            CreateMap<OrderToAddDto, Order>();


            CreateMap<PaymentMethod, PaymentMethodDto>();

            CreateMap<DeliveryMode, DeliveryModeDto>();

            CreateMap<ShippingPlan, ShippingDto>();

            CreateMap<TrackingHistory, TrackingHistoryDto>();

            CreateMap<AppUser, OrderUserResDto>()
                .ForMember(orderUserResDto => orderUserResDto.Address, x =>
                x.MapFrom(user => user.Address));


            CreateMap<OrderItems, OrderToreturnByOrderIdDto>()
                .ForMember(OrderToreturnDto => OrderToreturnDto.Customer, x =>
                x.MapFrom(order => order.Order.User))
                .ForMember(OrderToreturnDto => OrderToreturnDto.PaymentMethod, x =>
                x.MapFrom(order => order.Order.PaymentMethod))
                .ForMember(OrderToreturnDto => OrderToreturnDto.OrderItems, x =>
                x.MapFrom(order => order.Order.OrderItems))
                .ForMember(OrderToreturnDto => OrderToreturnDto.ShippingPlan, x =>
                x.MapFrom(order => order.Order.ShippingPlan))
                .ForMember(OrderToreturnDto => OrderToreturnDto.DeliveryMode, x =>
                x.MapFrom(order => order.Order.DeliveryMode));

            CreateMap<Order, OrderToreturnByOrderIdDto>()
                .ForMember(OrderToreturnDto => OrderToreturnDto.Customer, x =>
                x.MapFrom(order => order.User))
                .ForMember(OrderToreturnDto => OrderToreturnDto.PaymentMethod, x =>
                x.MapFrom(order => order.PaymentMethod))
                .ForMember(OrderToreturnDto => OrderToreturnDto.OrderItems, x =>
                x.MapFrom(order => order.OrderItems));

            CreateMap<Order, OrderToreturnByTrackingIdDto>()
              .ForMember(OrderToreturnDto => OrderToreturnDto.Customer, x =>
              x.MapFrom(order => order.User))
              .ForMember(OrderToreturnDto => OrderToreturnDto.PaymentMethod, x =>
              x.MapFrom(order => order.PaymentMethod))
              .ForMember(OrderToreturnDto => OrderToreturnDto.TrackingHistories, x =>
              x.MapFrom(order => order.TrackingHistories));

            CreateMap<Order, OrdersbyStatusDto>()
                .ForMember(ordersbyStatusDto => ordersbyStatusDto.OrderItems, x =>
                x.MapFrom(order => order.OrderItems));

            CreateMap<OrderItems, OrderItemResDto>()
                .ForMember(orderItemResDto => orderItemResDto.Livestock, x =>
                x.MapFrom(source => source.Livestock))
                .ForMember(orderItemResDto => orderItemResDto.DateCreated, x => x.MapFrom(orderItem => Convert.ToDateTime(orderItem.DateCreated).ToString("dddd, dd MMMM yyyy")));

            CreateMap<OrderItems, OrderItemStatusDto>()
                .ForMember(x => x.Breed, a => a.MapFrom(m => m.Livestock.Breed))
                .ForMember(x => x.Availability, a => a.MapFrom(m => m.Livestock.Availability));

            CreateMap<MarketAddress, MarketAddressDto>().ReverseMap();

            CreateMap<AppUser, UserToReturnDto>().ReverseMap();
            CreateMap<AddDeliveryPersonViewModel, RegisterDeliveryPersonDto>()
                    .ForMember(dest => dest.State, opt => opt.MapFrom(src => System.Enum.GetName(typeof(States), src.State)))
                    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => System.Enum.GetName(typeof(Countries), src.Country)))
                    .ForMember(dest => dest.Coverage, opt => opt.MapFrom(src => System.Enum.GetName(typeof(Coverage), src.Coverage)));
            CreateMap<DeliveryAssignment, ShapedListOfDeliveryAssignment>();


            //CreateMap<OrderItems, ListOfOrderDto>()
            //    .ForMember(x => x.Breed, a => a.MapFrom(m => m.Livestock.Breed))
            //    .ForMember(x => x.Availability, a => a.MapFrom(m => m.Livestock.Availability))
            //    .ForMember(x => x.CreatedAt, a => a.MapFrom(m => m.Order.DateCreated))
            //    .ForMember(x => x.Status, a => a.MapFrom(m => m.Order.Status))
            //    .ForMember(x => x.Id, a => a.MapFrom(m => m.Order.Id));

            CreateMap<Order, ListOfOrderDto>();

            CreateMap<TrackingHistory, TrackingHistoryDto>();
            CreateMap<AddAssignmentDto, DeliveryAssignment>();
            CreateMap<DeliveryAssignment, AssignmentResponseDto>();

            CreateMap<AppUser, ShipDetailsUserDto>()
              .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
              .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
              .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<OrderToAddDto, ShippingDetailsToReturnViewModel>().ReverseMap();

            CreateMap<OrderForCreationDto, Order>();

            CreateMap<TrackingHistory, UpdateTrackingDto>().ReverseMap().ForMember(x => x.DateUpdated, opt =>
              opt.MapFrom(src => DateTime.Now.ToString()));
          

            CreateMap<AddTrackingDto, TrackingHistory>();
            CreateMap<Order, PaymentHistoryDto>();

            CreateMap<TrackingHistory, TrackingHistoryForUsersDto>()
                .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => $"{x.Order.User.FirstName} {x.Order.User.LastName}"))
                .ForMember(dest => dest.DeliveryPersonName, src => src.MapFrom(x => $"{x.DeliveryPerson.AppUser.FirstName} {x.DeliveryPerson.AppUser.LastName}"));

            CreateMap<Livestock, AgentLivestockDto>();

            CreateMap<Livestock, RestockLivestockDto>().ReverseMap();
            CreateMap<AgentLivestockDto, UserToReturnDto>().ReverseMap();
            CreateMap<AgentLivestockDto, MarketDto>().ReverseMap();
            CreateMap<AgentLivestockDto, LivestockImagesToReturnDto>().ReverseMap();
            CreateMap<LivestockToReturnDto, Livestock>().ReverseMap();
            CreateMap<LivestockMarketVm, LivestockViewModel>().ReverseMap();

            CreateMap<AddTrackingHistoryViewModel, AddTrackingWithStatesStringViewModel>()
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => System.Enum.GetName(typeof(States), src.Location)));

        }
    }
}
