using App.Services.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : CustomBaseController
    {


        [HttpGet]
        public async Task<IActionResult> GetCategories()
    => CustomActionResult(await categoryService.GetAllListAsync());


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategory(int id)
            => CustomActionResult(await categoryService.GetByIdAsync(id));


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
            => CustomActionResult(await categoryService.CreateCategoryAsync(request));


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest request)
            => CustomActionResult(await categoryService.UpdateCategoryAsync(id, request));


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
            => CustomActionResult(await categoryService.DeleteCategoryAsync(id));


        [HttpGet("products")]
        public async Task<IActionResult> GetCategoryWithProductsAsync()
            => CustomActionResult(await categoryService.GetCategoryWithProductsAsync());



        [HttpGet("{id:int}/products")]
        public async Task<IActionResult> GetCategoryWithProductsAsync(int id)
            => CustomActionResult(await categoryService.GetCategoryWithProductsAsync(id));
    }
}

