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
                var catalogPositions = await _dbContext.catalog.ToListAsync();
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
                _dbContext.catalog.Add(newCatalogPosition);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetCatalogPosition),
                    new { id = newCatalogPosition.id },
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
                var catalogPosition = await _dbContext.catalog.FindAsync(id);

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
            if (id != updatedCatalogPosition.id)
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
                var catalogPosition = await _dbContext.catalog.FindAsync(id);

                if (catalogPosition == null)
                {
                    return NotFound("Catalog position with id = " + id + " not found");
                }

                _dbContext.catalog.Remove(catalogPosition);
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

            var price = await _dbContext.price_by_location
                .Join(
                    _dbContext.address_in_locations.Where(
                        address => address.fias_house_code == fiasHouseCode
                    ),
                    price => price.location_id,
                    address => address.location_id,
                    (price, address) => new { Price = price, Address = address }
                )
                .Join(
                    _dbContext.catalog.Where(
                        catalogPosition => catalogPosition.id == catalogPositionId
                    ),
                    result => result.Price.catalog_id,
                    catalogPosition => catalogPosition.id,
                    (result, catalogPosition) => result
                )
                .Select(result => result.Price.price)
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

            if (newPriceByLocation.price_on_request == true)
            {
                newPriceByLocation.price = 0;
            }
            try
            {
                _dbContext.price_by_location.Add(newPriceByLocation);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetPriceByParameters),
                    new { id = newPriceByLocation.id },
                    newPriceByLocation
                );
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding");
            }
        }

        // PUT: api/catalog/prices/{id}
        
        [HttpPut("prices/{id}")]
        public async Task<IActionResult> UpdatePriceByLocation(
            int id,
            [FromBody] Price_by_Location updatedPriceByLocation
        )
        {
            if (id != updatedPriceByLocation.id)
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
                return StatusCode(500, "An error occurred while updating");
            }
        }

        // DELETE: api/catalog/price/{id}
        
        [HttpDelete("price/{id}")]
        public async Task<IActionResult> DeletePriceByLocation(int id)
        {
            try
            {
                var priceByLocation = await _dbContext.price_by_location.FindAsync(id);

                if (priceByLocation == null)
                {
                    return NotFound("Price by location with ID " + id + "not found");
                }

                _dbContext.price_by_location.Remove(priceByLocation);
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
            return _dbContext.catalog.Any(cp => cp.id == id);
        }

        private bool PriceByLocationExists(int id)
        {
            return _dbContext.price_by_location.Any(cp => cp.id == id);
        }

        private static bool ValidateFiasCode(string code)
        {
            string pattern =
                @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
            return Regex.IsMatch(code, pattern);
        }
    }
}
