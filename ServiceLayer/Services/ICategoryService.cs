using ServiceLayer.Dtos.Catgory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface ICategoryService
    {
        Task<bool> Create(CreateCategoryInputDto createCategoryInputDto);
        Task<List<DDLDto>> GetCategoryDDL();
        Task<List<DDLDto>> GetCategoryByParentIdDDl(GetCategoryByParentIdInputDto getCategoryByParentIdInputDto);
    }
}
