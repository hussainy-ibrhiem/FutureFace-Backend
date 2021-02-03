using Data.Models;
using Data.Repositories;
using ServiceLayer.Dtos.Catgory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(CreateCategoryInputDto createCategoryInputDto)
        {
            bool created = default;
            bool exists = await _unitOfWork.Category.GetAnyAsync(cat => cat.Name.ToLower().Trim().Contains(createCategoryInputDto.Name.Trim().ToLower()));
            if (!exists)
            {
                Category category = new Category
                {
                    Name = createCategoryInputDto.Name,
                    ParentId = createCategoryInputDto.ParentId ?? 0
                };
                _unitOfWork.Category.CreateAsyn(category);
                created = await _unitOfWork.Commit() > default(byte);
            }
            return created;
        }

        public async Task<List<DDLDto>> GetCategoryDDL()
        {
            List<DDLDto> dDLDtos = new List<DDLDto>();
            List<Category> categories = await _unitOfWork.Category.GetWhereAsync(cat => cat.ParentId == 0);
            if (categories?.Any() ?? default)
            {
                dDLDtos = categories.Select(x => new DDLDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            return dDLDtos;
        }
        public async Task<List<DDLDto>> GetCategoryByParentIdDDl(GetCategoryByParentIdInputDto getCategoryByParentIdInputDto)
        {
            List<DDLDto> dDLDtos = new List<DDLDto>();
            List<Category> categories = await _unitOfWork.Category.GetWhereAsync(cat => cat.ParentId == getCategoryByParentIdInputDto.ParentId);
            if (categories?.Any() ?? default)
            {
                dDLDtos = categories.Select(x => new DDLDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            return dDLDtos;
        }
    }
}
