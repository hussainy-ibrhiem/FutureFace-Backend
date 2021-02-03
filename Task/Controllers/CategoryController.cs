using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.Catgory;
using ServiceLayer.Services;

namespace Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateCategory(CreateCategoryInputDto createCategoryInputDto)
        {
            bool created = await _categoryService.Create(createCategoryInputDto);
            if (created)
                return Ok(created);
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult<List<DDLDto>>> GetCategoryDDL()
        {
            List<DDLDto> dDLDtos = await _categoryService.GetCategoryDDL();
            return Ok(dDLDtos);
        }
        [HttpPost]
        public async Task<ActionResult<List<DDLDto>>> GetCategoryByParentIdDDl(GetCategoryByParentIdInputDto getCategoryByParentIdInputDto)
        {
            List<DDLDto> dDLDtos = await _categoryService.GetCategoryByParentIdDDl(getCategoryByParentIdInputDto);
            return Ok(dDLDtos);
        }
    }
}