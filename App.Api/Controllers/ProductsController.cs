    using App.Repositories.Products;
    using App.Service.Filters;
    using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.Update.Stock;
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


        [HttpGet("{pageNumber:int}/{pageCapacity:int}")]
        public async Task<IActionResult> GetPagedListAsnyc(int pageNumber, int pageCapacity)
            => CustomActionResult(await productService.GetPagedListAsnyc(pageNumber, pageCapacity));


        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsnyc(int id)
        => CustomActionResult(await productService.GetByIdAsync(id));


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
            => CustomActionResult(await productService.CreateAsync(request));


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request)
            => CustomActionResult(await productService.UpdateAsync(id, request));


        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => CustomActionResult(await productService.DeleteAsync(id));


        [HttpPost("{id:int}/{stockCount:int}")]
        public async Task<IActionResult> EnterDataInStock(int id, int stockCount)
            => CustomActionResult(await productService.EnterDataInStock(id, stockCount));

        [HttpPut("{id:int}/{price:decimal}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal price)
            => CustomActionResult(await productService.UpdatePrice(id, price));


        [HttpGet("GetPriceWithKdv{id:int}")]
        public async Task<IActionResult> GetPriceWithKDV(int id)
            => CustomActionResult(await productService.GetPriceWithKDV(id));


        //kısmi güncellemelerde Patch kullanılır

        [HttpPatch("/stockCount")]
        public async Task<IActionResult> UpdateProductStockRequest(UpdateProductStockRequest request)
            => CustomActionResult(await productService.UpdateProductStock(request));


    }
}
