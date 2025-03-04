using EANValidator.Data;
using EANValidator.Models;
using EANValidator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EANValidator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EANController : ControllerBase
    {
        private readonly EANValidatorService _eanValidatorService;
        private readonly ApplicationDbContext _context;

       
        public EANController(ApplicationDbContext context, EANValidatorService eanValidatorService)
        {
            _context = context;
            _eanValidatorService = eanValidatorService;
        }


        [HttpPost("Tri")]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {                                 
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.EAN == product.EAN);

            if (existingProduct != null)
            {
                return Conflict("A product with this EAN already exists.");
            }

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(ValidateEAN), new { ean = product.EAN }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while saving the product.", details = ex.Message });
            }
        }


        [HttpGet("validate/{ean}")]
        public ActionResult<Product> ValidateEAN(string ean)
        {
         
            if (!_eanValidatorService.IsValidEAN(ean))
            {
                return BadRequest("Invalid EAN number.");
            }

            
            var product = _eanValidatorService.GetEANDetails(ean);

            if (product == null)
            {
                return NotFound("Product not found for the given EAN.");
            }

            return Ok(product);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            try
            {
                var Products = await _context.Products.ToListAsync();
                return Ok(Products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
