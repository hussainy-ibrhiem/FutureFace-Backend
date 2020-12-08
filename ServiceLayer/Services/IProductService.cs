using Microsoft.AspNetCore.Http;
using ServiceLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IProductService
    {
        Task<bool> AddProduct(AddEditProductInputDto addEditProductInputDto);
        Task<ProductDto> GetProductById(ProductIDentityDto productIDentityDto);
        Task<bool> UpdateProduct(AddEditProductInputDto addEditProductInputDto);
        Task<bool> DeleteProduct(ProductIDentityDto productIDentityDto);
        Task<PageList<ProductDto>> GetAll(ProductSearchDto productSearchDto);
        Task<string> SaveFileAsync(IFormFile formFile, string directory, string subDirectory = null);

    }
}
