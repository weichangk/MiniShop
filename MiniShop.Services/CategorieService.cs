using AutoMapper;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using Orm.Core;

namespace MiniShop.Services
{
    public class CategorieService : BaseService<Categorie, CategorieDto, int>, ICategorieService, IDependency
    {
        public CategorieService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CategorieService> logger,
            Lazy<IRepository<Categorie>> _repository) : base(mapper, unitOfWork, logger, _repository)
        {

        }
    }
}
