using App.Services.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService):CustomBaseController
    {


        [HttpGet]
        public async Task<IActionResult> GetAllListAsync()
            => CustomActionResult(await productService.GetAllListAsync());


        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPagedListAsnyc(int pageNumber, int pageSize)
            => CustomActionResult(await productService.GetPagedListAsnyc(pageNumber, pageSize));


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsnyc(int id)
        => CustomActionResult(await productService.GetByIdAsync(id));


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
            => CustomActionResult(await productService.CreateProductResponse(request));


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request)
            => CustomActionResult(await productService.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => CustomActionResult(await productService.DeleteAsync(id));
    }
}
