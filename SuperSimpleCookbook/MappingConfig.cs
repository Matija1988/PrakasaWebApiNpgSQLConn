using AutoMapper;
using SuperSimpleCookbook.Model;

namespace SuperSimpleCookbook
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Author, AuthorReadDTO>().ReverseMap();
            CreateMap<Author, AuthorCreateDTO>().ReverseMap();
            CreateMap<Author, AuthorUpdateDTO>().ReverseMap();
            CreateMap<Recipe, RecipeReadDTO>().ReverseMap();

        }
    }
}
