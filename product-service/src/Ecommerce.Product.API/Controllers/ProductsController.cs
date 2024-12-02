using Ecommerce.Product.Core.DTOs;
using Ecommerce.Product.Core.ServiceContacts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IValidator<CreateProductRequest> _createProductRequestValidator;
        private readonly IValidator<UpdateProductRequest> _updateProductRequestValidator;
        public ProductsController(IProductsService productsService, IValidator<CreateProductRequest> createProductRequestValidator, IValidator<UpdateProductRequest> updateProductRequestValidator)
        {
            _productsService = productsService;
            _createProductRequestValidator = createProductRequestValidator;
            _updateProductRequestValidator = updateProductRequestValidator;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            List<ProductsResponse?> products = await _productsService.GetProductsAsync();
            return Ok(products);
        }
        [HttpGet("search/{productId:guid}")]
        public async Task<IActionResult> GetProductByIdConditionAsync(Guid productId)
        {
            ProductsResponse? product = await _productsService.GetProductByConditionAsync(x => x.ProductID == productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("/search/{searchText:string}")]
        public async Task<IActionResult> GetProductBySearchTextAsync(string searchText)
        {
            List<ProductsResponse?> serachProductsByName = await _productsService.GetProductsByConditionAsync(x =>
            x.ProductName != null && x.ProductName.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            List<ProductsResponse?> serachProductsByCategory = await _productsService.GetProductsByConditionAsync(x =>
            x.Category != null && x.Category.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            var products = serachProductsByName.Union(serachProductsByCategory);
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> AddProductAsync(CreateProductRequest createProductRequest)
        {
            ValidationResult validationResult = await _createProductRequestValidator.ValidateAsync(createProductRequest);
            if (!validationResult.IsValid)
            {

                Dictionary<string, string[]> errorsResult = validationResult.Errors
                 .GroupBy(x => x.PropertyName)
                 .ToDictionary(grp => grp.Key, grp => grp
                 .Select(err => err.ErrorMessage).ToArray());
                ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(errorsResult);
                return ValidationProblem(validationProblemDetails);
            }
            ProductsResponse? product = await _productsService.AddProductAsync(createProductRequest);
            if (product == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetProductByIdConditionAsync), new { productId = product.ProductID }, product);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductASync(UpdateProductRequest updateProductRequest)
        {
            ValidationResult validationResult = await _updateProductRequestValidator.ValidateAsync(updateProductRequest);
            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errorsResult = validationResult.Errors
                 .GroupBy(x => x.PropertyName)
                 .ToDictionary(grp => grp.Key, grp => grp
                 .Select(err => err.ErrorMessage).ToArray());
                ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(errorsResult);
                return ValidationProblem(validationProblemDetails);
            }
            ProductsResponse? product = await _productsService.UpdateProductASync(updateProductRequest);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}