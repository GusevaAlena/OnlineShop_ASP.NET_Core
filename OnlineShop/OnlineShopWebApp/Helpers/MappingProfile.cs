using AutoMapper;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.ViewModels;
using System.Linq;

namespace OnlineShopWebApp.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, UserProfileViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<Cart,CartViewModel>().ReverseMap();
            CreateMap<Product,ProductViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Cart, CartViewModel>()
                .ForMember(x => x.Positions, op => op.MapFrom(o => o.CartPositions)).ReverseMap();      
            CreateMap<Order, OrderCreateViewModel>()
                .ForMember(x => x.User, op => op.MapFrom(o => o.Cart.User))
                .ForMember(x => x.Address, op => op.MapFrom(o => o.Address));
            CreateMap<OrderCreateViewModel, Order>()
                .ForMember(x => x.Address, op => op.MapFrom(o => o.Address));
            CreateMap<Order, OrderViewModel>()
                .ForMember(x => x.Cart, op => op.MapFrom(o => o.Cart));
            
        }
    }
}
