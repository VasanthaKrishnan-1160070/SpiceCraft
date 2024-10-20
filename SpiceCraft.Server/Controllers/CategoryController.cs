using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Category;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(SpiceCraftContext context) : ControllerBase
    {

        [HttpGet]
        public async Task<IEnumerable<CategoryDTO>> Get()
        {
            return await context.ItemCategories
                           .Where(w => w.ParentCategoryId == null) 
                           .Select(c => new CategoryDTO
                           {
                               CategoryId = c.CategoryId,
                               CategoryName = c.CategoryName
                           }).ToListAsync();
        }

        [HttpGet("{categoryId:int}/sub-categories")]
        public async Task<IEnumerable<CategoryDTO>> GetSubCategory(int categoryId)
        {
            return await context.ItemCategories                           
                           .Where(w => w.ParentCategoryId == categoryId)
                           .Select(c => new CategoryDTO
                           {
                               CategoryId = c.CategoryId,
                               CategoryName = c.CategoryName
                           }).ToListAsync();
        }
    }
}
