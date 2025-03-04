using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EANValidator.Models;
using EANValidator.Data;
using EANValidator.Services;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EANValidator.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UIDValidatorService _uidValidatorService; 

        public ItemsController(ApplicationDbContext context, UIDValidatorService uidValidatorService)
        {
            _context = context;
            _uidValidatorService = uidValidatorService;
        }

        [HttpPost("EANandUID")]
        public async Task<ActionResult> PostItems([FromBody] List<Nitem> items)
        {
            if (items == null || items.Count == 0)
            {
                return BadRequest("No items provided.");
            }

            try
            {
               
                var invalidItems = items.Where(item => string.IsNullOrWhiteSpace(item.UID)).ToList();
                if (invalidItems.Any())
                {
                    return BadRequest("Some items have invalid UIDs.");
                }

                
                var itemsToAdd = new List<Nitem>();

                
                foreach (var item in items)
                {
                   
                    if (_uidValidatorService.IsUIDExists(item.UID))
                    {
                       
                        continue;
                    }

                    
                    itemsToAdd.Add(item);
                }

                if (itemsToAdd.Count > 0)
                {
                 
                    await _context.Nitems.AddRangeAsync(itemsToAdd);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = $"{itemsToAdd.Count} items added successfully!" });
                }
                else
                {
                    return Ok(new { message = " UID already exist in the database." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPost("PackandClose")]
        public async Task<ActionResult> PostItems([FromBody] List<string> items)
        {
            if (items == null || items.Count == 0)
            {
                return BadRequest("No items provided.");
            }

            try
            {

                var invalidItems = items.Where(item => string.IsNullOrWhiteSpace(item)).ToList();
                if (invalidItems.Any())
                {
                    return BadRequest("Some items are invalid.");
                }


                var itemEntities = items.Select(item => new Item { UID = item }).ToList();


                _context.Items.AddRange(itemEntities);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Items added successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("UID")]
        public async Task<ActionResult<List<Item>>> GetItems()
        {
            try
            {
                var items = await _context.Items.ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Nitem>>> GetItem()
        {
            try
            {
                var items = await _context.Nitems.ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

}

// New POST api/item (validate UID and EAN, then save to database)
//[HttpPost("item")]
//public async Task<ActionResult> PostItem([FromBody] List<Nitem> items)
//{
//    if (items == null || items.Count == 0)
//    {
//        return BadRequest("No items provided.");
//    }

//    try
//    {
//        
//        var invalidUIDs = new List<string>();
//        var validItems = new List<Nitem>();

//        foreach (var item in items)
//        {
//            if (_uidValidatorService.IsUIDExists(item.UID))  // Validate each UID
//            {
//                validItems.Add(new Nitem { UID = item.UID, EAN = item.EAN });  // Add valid items (UID + EAN)
//            }
//            else
//            {
//                invalidUIDs.Add(item.UID);  // Collect invalid UIDs
//            }
//        }

//       
//        if (invalidUIDs.Any())
//        {
//            return BadRequest(new { message = "Invalid UIDs", invalidUIDs });
//        }

//        
//        _context.Nitems.AddRange(validItems);
//        await _context.SaveChangesAsync();

//        return Ok(new { message = "Items added successfully!" });
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, new { error = ex.Message });
//    }
//}

//       // POST api/items
//       [HttpPost]
//        public async Task<ActionResult> PostItems([FromBody] List<string> items)
//        {
//            if (items == null || items.Count == 0)
//            {
//                return BadRequest("No items provided.");
//            }

//            try
//            {
//                
//                var invalidItems = items.Where(item => string.IsNullOrWhiteSpace(item)).ToList();
//                if (invalidItems.Any())
//                {
//                    return BadRequest("Some items are invalid.");
//                }

//               
//                foreach (var item in items)
//                {
//                    
//                    if (_uidValidatorService.IsUIDExists(item))
//                    {
//                        return BadRequest($"UID {item} already exists in the Nitem table. Cannot add.");
//                    }
//                }

//                
//                var itemEntities = items.Select(item => new Nitem { UID = item }).ToList();

//                
//                _context.Nitems.AddRange(itemEntities);
//                await _context.SaveChangesAsync();

//                return Ok(new { message = "Items added successfully!" });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { error = ex.Message });
//            }
//        }
