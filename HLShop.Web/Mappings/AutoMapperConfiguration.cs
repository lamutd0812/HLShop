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
            });

            var mapper = config.CreateMapper();

            return mapper;
        }
    }
}