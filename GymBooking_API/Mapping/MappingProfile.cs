using AutoMapper;
using GymBooking_API.Controllers;
using GymBooking_API.DTO.ApplicationUser;
using GymBooking_API.DTO.Booking;
using GymBooking_API.DTO.Cart;
using GymBooking_API.DTO.Coach;
using GymBooking_API.DTO.GYM;
using GymBooking_API.DTO.Membership;
using GymBooking_API.DTO.Order;
using GymBooking_API.DTO.Package;
using GymBooking_API.DTO.Packages;
using GymBooking_API.Entity;

namespace GymBooking_API.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {

            //Package-փաթեթներ
            CreateMap<PackageCreateDto, Package>();
            CreateMap<Package, PackageResponseDto>();
            CreateMap<PackageUpdateDto,Package>();
            CreateMap<Package, PackageShortResponseDto>();

            //Gym-մարզումներ    
            CreateMap<GymCreateDto, Gym>();
            CreateMap<GymUpdateDto, Gym>();
            CreateMap<Gym, GymResponseDto>();
            CreateMap<Gym, GymShortResponseDto>();
  
            //Coach-Մարզիչ
            CreateMap<CoachCreateDto, Coach>();
            CreateMap<CoachUpdateDto, Coach>();
            CreateMap<Coach,CoachResponseDto>();
            CreateMap<Coach, CoachShortResponseDto>();

            //Membership
            CreateMap<MembershipCreateDto, Membership>();
            CreateMap<MembershipUpdateDto, Membership>();
            CreateMap<Membership, MembershipResponseDto>()
                .ForMember(s => s.PackageShortResponseDto, p => p.MapFrom(src => src.Package));
            CreateMap<Membership, MembershipAdminResponseDto>();
            CreateMap<Membership, MembershipForPackageAdminDto>();

            //Booking
            CreateMap<BookingCreateDto, Booking>();
            CreateMap<BookingUpdateDto, Booking>();
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(s=>s.UserDetail,s=>s.MapFrom(s=>s.User)).ForMember(s=>s.Gym,s=>s.MapFrom(s=>s.GymClass));

            //AplicationUser
            CreateMap<ApplicationUser, ResponseForBookingaAndMembershipDTO>();

            //Cart

            CreateMap<Cart, CartResponseDto>().ForMember(
                s => s.CartId,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(
                s=>s.Items,
                sp=>sp.MapFrom(s=>s.CartItems));
            CreateMap<CartItem, CartItemResponseDto>().ForMember(
                 packageid => packageid.PackageId,
                  id => id.MapFrom(s => s.PackagId)
                )
                .ForMember(
                dest => dest.PackagName,
                name => name.MapFrom(s => s.Package.Name)
                )
                .ForMember(
                 dailyprice => dailyprice.DailyPrice,
                 price => price.MapFrom(s => s.Package.DailyPrice)
                )
                .ForMember(
                monthlyprice => monthlyprice.MonthlyPrice,
                price => price.MapFrom(s => s.Package.MonthlyPrice)
                )
                .ForMember(
                quantity=>quantity.Quntity,
                cartQuantity=>cartQuantity.MapFrom(s=>s.Quantity)
                );
            //Order

            CreateMap<Order, OrderResponseDto>().ForMember(
dest => dest.OrderId,
opt => opt.MapFrom(src => src.Id)).ForMember(
dest => dest.OrderItems,
opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(
                    dest => dest.Packagname,
                    opt => opt.MapFrom(src => src.Package.Name)
                )
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom(src => src.UnitPrice * src.Quantity)
                );
         }
    }
}
