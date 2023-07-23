using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using RestfullWeb.Model;

namespace RestfullWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : Controller
    {
        private readonly Context _dbContext;

        public CatalogController(Context context)
        {
            _dbContext = context;
        }

        // GET: api/catalog
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Catalog>>> GetCatalogPositions()
        {
            try
            {
                var catalogPositions = await _dbContext.Catalog.ToListAsync();
                return Ok(catalogPositions);
            }
            catch
            {
                return StatusCode(500, "An error occured while getting");
            }
        }

        // POST: api/catalog
        
        [HttpPost]
        public async Task<ActionResult<Catalog>> AddCatalogPosition(
            [FromBody] Catalog newCatalogPosition
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _dbContext.Catalog.Add(newCatalogPosition);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetCatalogPosition),
                    new { id = newCatalogPosition.Id },
                    newCatalogPosition
                );
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding");
            }
        }

        // GET: api/catalog/{id}
     
        [HttpGet("{id}")]
        public async Task<ActionResult<Catalog>> GetCatalogPosition(int id)
        {
            try
            {
                var catalogPosition = await _dbContext.Catalog.FindAsync(id);

                if (catalogPosition == null)
                {
                    return NotFound();
                }
                return Ok(catalogPosition);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while getting");
            }
        }

        // PUT: api/catalog/{id}
      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCatalogPosition(
            int id,
            [FromBody] Catalog updatedCatalogPosition
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != updatedCatalogPosition.Id)
            {
                return BadRequest("Invalid ID");
            }
            if (!CatalogPositionExists(id))
            {
                return NotFound();
            }
            try
            {
                _dbContext.Entry(updatedCatalogPosition).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating");
            }
            return NoContent();
        }

        // DELETE: api/catalog/{id}
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalogPosition(int id)
        {
            try
            {
                var catalogPosition = await _dbContext.Catalog.FindAsync(id);

                if (catalogPosition == null)
                {
                    return NotFound("Catalog position with id = " + id + " not found");
                }

                _dbContext.Catalog.Remove(catalogPosition);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting");
            }
        }

        // GET: api/catalog/price/{fiasHouseCode}/{catalogPositionId}
        
        [HttpGet("price/{fiasHouseCode}/{catalogPositionId}")]
        public async Task<ActionResult<int>> GetPriceByParameters(
            string fiasHouseCode,
            int catalogPositionId
        )
        {
            if (!ValidateFiasCode(fiasHouseCode))
            {
                return BadRequest("Invalid FIAS code");
            }

            var price = await _dbContext.Price_by_Location
                .Join(
                    _dbContext.Address_in_Locations.Where(
                        address => address.FiasHouseCode == fiasHouseCode
                    ),
                    price => price.LocationId,
                    address => address.LocationId,
                    (price, address) => new { Price = price, Address = address }
                )
                .Join(
                    _dbContext.Catalog.Where(
                        catalogPosition => catalogPosition.Id == catalogPositionId
                    ),
                    result => result.Price.CatalogId,
                    catalogPosition => catalogPosition.Id,
                    (result, catalogPosition) => result
                )
                .Select(result => result.Price.Price)
                .ToListAsync();

            if (price == null)
            {
                return NotFound("Price not found");
            }
            try
            {
                return Ok(price);
            }
            catch
            {
                return StatusCode(500, "There are several price items on request");
            }
        }

        // POST: api/catalog/price
       
        [HttpPost("price")]
        public async Task<ActionResult<Price_by_Location>> AddPriceByLocation(
            [FromBody] Price_by_Location newPriceByLocation
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newPriceByLocation.Price_on_request == true)
            {
                newPriceByLocation.Price = 0;
            }
            try
            {
                _dbContext.Price_by_Location.Add(newPriceByLocation);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetPriceByParameters),
                    new { id = newPriceByLocation.Id },
                    newPriceByLocation
                );
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding a price by location");
            }
        }

        // PUT: api/catalog/prices/{id}
        
        [HttpPut("prices/{id}")]
        public async Task<IActionResult> UpdatePriceByLocation(
            int id,
            [FromBody] Price_by_Location updatedPriceByLocation
        )
        {
            if (id != updatedPriceByLocation.Id)
            {
                return BadRequest("Invalid ID");
            }
            if (!PriceByLocationExists(id))
            {
                return NotFound("Price by loction with ID " + id + "not found");
            }
            try
            {
                _dbContext.Entry(updatedPriceByLocation).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok(updatedPriceByLocation);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the price by location");
            }
        }

        // DELETE: api/catalog/price/{id}
        
        [HttpDelete("price/{id}")]
        public async Task<IActionResult> DeletePriceByLocation(int id)
        {
            try
            {
                var priceByLocation = await _dbContext.Price_by_Location.FindAsync(id);

                if (priceByLocation == null)
                {
                    return NotFound("Price by location with ID " + id + "not found");
                }

                _dbContext.Price_by_Location.Remove(priceByLocation);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the price by location");
            }
        }

        private bool CatalogPositionExists(int id)
        {
            return _dbContext.Catalog.Any(cp => cp.Id == id);
        }

        private bool PriceByLocationExists(int id)
        {
            return _dbContext.Price_by_Location.Any(cp => cp.Id == id);
        }

        private static bool ValidateFiasCode(string code)
        {
            string pattern =
                @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
            return Regex.IsMatch(code, pattern);
        }
    }
}
