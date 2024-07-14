using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.API.Model;
using MongoDB.API.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MongoDB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandiesController : ControllerBase
    {
        private readonly ICandyRepo _candyRepo;
        private readonly ILogger<CandiesController> _logger;
        public CandiesController(ICandyRepo candyRepo, ILogger<CandiesController> logger)
        {
            _candyRepo = candyRepo;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok("Candies API is up and running");
        }
        [HttpPost]
        public IActionResult CreateCandy(Candy candy)
        {
            try
            {
                SetCandyId(candy);
                _candyRepo.CreateCandy(candy);
   
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating candy");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating candy");
            }
        }
        [HttpPost]
        [Route("bulkInsert")]
        public IActionResult BulkInsert(List<Candy> candies) { 
            try
            {
                SetCandyId(candies);
                _candyRepo.BulkCreateCandy(candies);
        
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating candy");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating candy");
            }
        }
        private void SetCandyId(Candy candy)
        {
            // Assuming Id is a string. Adjust the logic if it's a different type.
            candy.Id = Guid.NewGuid().ToString();
        }

        // Overloaded method for a List<Candy>
        private void SetCandyId(List<Candy> candies)
        {
            candies.ForEach(c => c.Id = Guid.NewGuid().ToString());
        }
    }
}
