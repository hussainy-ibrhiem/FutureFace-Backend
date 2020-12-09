using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Dtos;
using ServiceLayer.Services;

namespace Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductService productService,IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> AddProduct(AddEditProductInputDto addEditProductInputDto)
        {
            bool added = await _productService.AddProduct(addEditProductInputDto);
            if (added)
                return Ok(added);
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult<ProductDto>> GetProductById(ProductIDentityDto productIDentityDto)
        {
            ProductDto productDto = await _productService.GetProductById(productIDentityDto);
            if (productDto != null)
                return Ok(productDto);
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult<bool>> UpdateProduct(AddEditProductInputDto addEditProductInputDto)
        {
            bool updated = await _productService.UpdateProduct(addEditProductInputDto);
            if (updated)
                return Ok(updated);
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult<bool>> DeleteProduct(ProductIDentityDto productIDentityDto)
        {
            bool deleted = await _productService.DeleteProduct(productIDentityDto);
            if (deleted)
                return Ok(deleted);
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult<PageList<ProductDto>>> GetAll(ProductSearchDto productSearchDto)
        {
            PageList<ProductDto> pageList = await _productService.GetAll(productSearchDto);
            if (pageList != null)
                return Ok(pageList);
            else
                return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult<UploadResponse>> UploadImage()
        {
            var file = Request.Form.Files[0];
            string result = await _productService.SaveFileAsync(file, _webHostEnvironment.WebRootPath);
            UploadResponse uploadResponse = new UploadResponse { Url = _webHostEnvironment.WebRootPath+"\\", FileName = result };
            return Ok(uploadResponse);
        }
    }
}