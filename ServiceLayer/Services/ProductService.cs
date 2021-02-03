using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Shared.Enums.CommonEnum;

namespace ServiceLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;

        public ProductService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
        }
        public async Task<bool> AddProduct(AddEditProductInputDto addEditProductInputDto)
        {
            bool added = default;
            bool isExist = await _unitOfWork.Product.GetAnyAsync(p => p.Deleted == false && p.Name.ToLower().Trim().Contains(addEditProductInputDto.Name.Trim().ToLower()));
            if (!isExist)
            {
                Product product = new Product
                {
                    Name = addEditProductInputDto.Name,
                    Photo = addEditProductInputDto.Photo,
                    Price = addEditProductInputDto.Price,
                    CreationDate = DateTime.Now,
                    Quantity = addEditProductInputDto.Quantity,
                    CategoryId = addEditProductInputDto.CategoryId,
                    SubCategoryId = addEditProductInputDto.SubCategoryId
                };
                _unitOfWork.Product.CreateAsyn(product);
                added = await _unitOfWork.Commit() > default(int);
            }
            return added;
        }

        public async Task<bool> DeleteProduct(ProductIDentityDto productIDentityDto)
        {
            bool deleted = default;
            Product product = await _unitOfWork.Product.FirstOrDefaultAsync(p => p.Id == productIDentityDto.Id);
            if (product != null)
            {
                product.Deleted = true;
                _unitOfWork.Product.Update(product);
                deleted = await _unitOfWork.Commit() > default(int);
            }
            return deleted;
        }

        public async Task<PageList<ProductDto>> GetAll(ProductSearchDto productSearchDto)
        {
            PageList<ProductDto> pageList = new PageList<ProductDto>();
            Expression<Func<Product, bool>> filter = e => string.IsNullOrWhiteSpace(e.Name) || e.Name.Contains(productSearchDto.Name);
            List<Product> products = new List<Product>();
            products = productSearchDto.SortingModel.SortingExpression switch
            {
                "Name" => await _unitOfWork.Product.GetPageAsync(productSearchDto.PageNumber, productSearchDto.PageSize, filter, p => p.Name, productSearchDto.SortingModel.SortingDirection, "Category"),
                _ => await _unitOfWork.Product.GetPageAsync(productSearchDto.PageNumber, productSearchDto.PageSize, filter, p => p.CreationDate, SortDirectionEnum.Descending, "Category"),
            };
            if (products?.Any() ?? default)
            {
                pageList = new PageList<ProductDto>
                {
                    DataList = products.Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Photo = p.Photo,
                        Price = p.Price,
                        LastUpdated = p.LastUpdated,
                        CategoryName = p.Category.Name,
                        SubCategoryName = GetSubCategoryName(p.SubCategoryId).GetAwaiter().GetResult(),
                        Quantity = p.Quantity,
                        CategoryId = p.CategoryId,
                        SubCategoryId = p.SubCategoryId,
                    }).ToList(),
                    TotalCount = await _unitOfWork.Product.GetCountAsync(filter)
                };
            }
            return pageList;
        }

        public async Task<ProductDto> GetProductById(ProductIDentityDto productIDentityDto)
        {
            ProductDto productDto = null;
            Product product = await _unitOfWork.Product.FirstOrDefaultAsync(p => p.Id == productIDentityDto.Id, "Category");
            if (product != null)
                productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Photo = product.Photo,
                    Price = product.Price,
                    LastUpdated = product.LastUpdated.GetValueOrDefault(),
                    CategoryName = product.Category.Name,
                    SubCategoryName = await GetSubCategoryName(product.SubCategoryId),
                    Quantity = product.Quantity,
                    CategoryId = product.CategoryId,
                    SubCategoryId = product.SubCategoryId,
                };
            return productDto;
        }

        private async Task<string> GetSubCategoryName(int? subCategoryId)
        {

            Category category = await _unitOfWork.Category.FirstOrDefaultAsync(cat => cat.Id == subCategoryId);
            string name = category != null ? category.Name : "-";
            return name;
        }

        public async Task<bool> UpdateProduct(AddEditProductInputDto addEditProductInputDto)
        {
            bool updated = default;
            bool exists = await _unitOfWork.Product.GetAnyAsync(p =>
                                                                p.Name.Trim().ToLower().Contains(addEditProductInputDto.Name.Trim().ToLower())
                                                                && p.Id != addEditProductInputDto.Id);
            if (!exists)
            {
                Product product = await _unitOfWork.Product.FirstOrDefaultAsync(p => p.Id == addEditProductInputDto.Id);
                if (product != null)
                {
                    product.Name = addEditProductInputDto.Name;
                    product.Photo = addEditProductInputDto.Photo;
                    product.Price = addEditProductInputDto.Price;
                    product.LastUpdated = DateTime.Now;
                    product.Quantity = addEditProductInputDto.Quantity;
                    product.CategoryId = addEditProductInputDto.CategoryId;
                    product.SubCategoryId = addEditProductInputDto.SubCategoryId;
                    _unitOfWork.Product.Update(product);
                    updated = await _unitOfWork.Commit() > default(int);
                }
            }
            return updated;
        }
        public async Task<string> SaveFileAsync(IFormFile formFile, string directory, string subDirectory = null)
        {
            string filePath = string.Empty;
            string fullDirectory = directory;
            if (!string.IsNullOrWhiteSpace(subDirectory))
                fullDirectory = string.Concat(directory, $"/{subDirectory}");
            if (!Directory.Exists(fullDirectory))
                Directory.CreateDirectory(fullDirectory);
            using (var file = System.IO.File.Create(string.Concat(fullDirectory, $"/{Guid.NewGuid().ToString()}") + $"{Path.GetExtension(formFile.FileName)}"))
            {
                await formFile.CopyToAsync(file);
                if (!string.IsNullOrEmpty(subDirectory))
                    filePath = string.Concat(subDirectory, $"/{Path.GetFileName(file.Name)}");
                else
                    filePath = Path.GetFileName(file.Name);
            }
            return filePath;
        }
    }
}
