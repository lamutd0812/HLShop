using AutoMapper;
using HLShop.Model.Models;
using HLShop.Web.Models;

namespace HLShop.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
                cfg.CreateMap<Tag, TagViewModel>();
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductTag, ProductTagViewModel>();
                cfg.CreateMap<Footer, FooterViewModel>();
                cfg.CreateMap<Slide, SlideViewModel>();
                cfg.CreateMap<Page, PageViewModel>();
                cfg.CreateMap<ContactDetail, ContactDetailViewModel>();
                cfg.CreateMap<Cart, CartViewModel>();
                cfg.CreateMap<Order, OrderViewModel>();
                cfg.CreateMap<OrderDetail, OrderDetailViewModel>();

                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            });

            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}