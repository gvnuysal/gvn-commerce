using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Specification;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

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
          public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductsSpecParams productParams)
          {
               var spec = new ProductsWithTypesAndBrandsSpecification(productParams);               
               var countSpec=new ProductWithFiltersForCountSpecification(productParams);
               var totalItems=await _productRepository.CountAsync(countSpec);
               var products=await _productRepository.ListAsync(spec);
               var data=_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

               return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
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