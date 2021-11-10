using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class CategorieProfile: Profile
    {
        public CategorieProfile()
        {
            CreateMap<CategorieCreateDto, Categorie>();
        }
    }
}
