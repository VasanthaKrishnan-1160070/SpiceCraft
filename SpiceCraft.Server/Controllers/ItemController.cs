using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;

namespace SpiceCraft.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController(SpiceCraftContext context, IProductLogics productLogics) : ControllerBase
{
    [HttpGet]
    public ActionResult<ResultDetail<IEnumerable<ProductCatalogItemDTO>>> GetItems([FromQuery] ProductFilterRequest filterRequest)
    {
        return productLogics.FilterProduct(filterRequest);
    }

    [HttpGet("{itemId:int}")]
    public ActionResult<ResultDetail<ProductDetailDTO>> GetItem(int itemId)
    {
        return productLogics.GetProductDetail(itemId);
    }

    [HttpGet("listing/{itemId:int}")]
    public ActionResult<ResultDetail<bool>> AddOrRemoveItem(int itemId, [FromQuery] bool isAdd = true)
    {
        if (isAdd)
        {
            return productLogics.AddProductToListing(itemId);
        }
        return productLogics.RemoveProductFromListing(itemId);
    }

    [HttpPost]
    public ActionResult<ResultDetail<bool>> CreateUpdateProduct([FromBody] ProductSummaryDTO productDetail, [FromQuery] string mainImageCode)
    {
        var uploadedImages = new List<IFormFile>();
        var hasAnyFiles = Request?.Form?.Files?.Any() ?? false;
        if (hasAnyFiles)
        {
            uploadedImages = Request.Form?.Files?.ToList();
        }
        return productLogics.CreateUpdateProductDetails(productDetail, mainImageCode, new List<IFormFile>());
    }
}