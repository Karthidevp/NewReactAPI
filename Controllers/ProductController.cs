using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reactapp.Interfaces;
using Reactapp.Models;
using static Reactapp.Models.ProductModel;


namespace NewReact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IProductServices productService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _environment = environment;
        }

        [HttpPost("createorupdate")]
        public async Task<IActionResult> CreateOrUpdateProduct([FromForm] ProductDto dto)
        {
            try
            {
                string? imagePath = null;

                if (dto.ProductImage != null && dto.ProductImage.Length > 0)
                {
                    // Ensure "Images" folder exists inside wwwroot
                    var uploadDir = Path.Combine(_environment.WebRootPath, "Images");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Unique filename
                    var fileName = $"{Guid.NewGuid()}_{dto.ProductImage.FileName}";
                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ProductImage.CopyToAsync(stream);
                    }

                    // Save relative path in DB (this will map correctly to static file URL)
                    imagePath = Path.Combine("Images", fileName).Replace("\\", "/");
                }

                var product = new Product
                {
                    ProductId = dto.ProductId,
                    ProductName = dto.ProductName,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    Description = dto.Description,
                    ProductImage = imagePath // e.g. "Images/xxx.webp"
                };

                var response = await _productService.CreateOrUpdateProductAsync(product);

                if (response.Success)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            //result
            return Ok(result);
        }
    }

}