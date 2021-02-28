using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Specification;
using API.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
 
 
     public class ProductsController : BaseApiController
     {
          private readonly IGenericRepository<Product> _productRepository; 
          private readonly IGenericRepository<ProductBrand> _productBrandRepository;
          private readonly IGenericRepository<ProductType> _productTypeRepository; 
          private readonly IMapper _mapper;
          public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository, IMapper mapper)
          {
               _mapper = mapper;
               _productTypeRepository = productTypeRepository;
               _productBrandRepository = productBrandRepository;
               _productRepository = productRepository;
          }

          [HttpGet]
          
          public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
          {
               var spec = new ProductsWithTypesAndBrandsSpecification();
               var products = await _productRepository.ListAsync(spec);

               return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
          }
          [HttpGet]
          [Route("{id}")]
          [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
          public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
          {
               var spec = new ProductsWithTypesAndBrandsSpecification(id);

               var product = await _productRepository.GetEntityWithSpec(spec);
               if(product==null) return NotFound(new ApiResponse(404));
               return _mapper.Map<Product, ProductToReturnDto>(product);
          }
          [HttpGet]
          [Route("product-brands")]
          public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
          {
               return Ok(await _productBrandRepository.ListAllAsync());
          }
          [HttpGet]
          [Route("product-types")]
          public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
          {
               return Ok(await _productTypeRepository.ListAllAsync());
          }
     }
}