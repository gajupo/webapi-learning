using Microsoft.AspNetCore.Mvc;
using webapi_learning.Attributes;
using webapi_learning.Extentions;
using webapi_learning.Filters;
using webapi_learning.Filters.AuthFilters;
using webapi_learning.Filters.ExceptionFilters;
using webapi_learning.Models;
using webapi_learning.Models.Core;

namespace webapi_learning.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenAuthFilterAttribute]
    public class ShirtsController : ControllerBase
    {
        private readonly ILogger<ShirtsController> _logger;
        private readonly IShirtRepository _shirtRepository;
        public ShirtsController(ILogger<ShirtsController> logger, IShirtRepository shirtRepository)
        {
            _logger = logger;
            _shirtRepository = shirtRepository;
        }

        [HttpGet]
        [RequiredClaimAttribute("read", "true")]
        [Shirt_ValidatedPagedImputParamsAttribute]
        public async Task<IActionResult> Shirts ([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 10)
        {
            var (shirts, totalCount) = await _shirtRepository.PagedGetShirts(pageNumber, pageSize);

            return Ok(shirts.ToPagedResult(pageNumber, pageSize, totalCount));

        }

        [HttpGet("{id:int}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaimAttribute("read", "true")]
        public async Task<IActionResult> Shirts(int id)
        {
            // var shirt = await _shirtRepository.GetShirtById(id);

            // get shirt fetched from db in the filter
            
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        [RequiredClaimAttribute("write", "true")]
        public async Task<IActionResult> AddShirt([FromBody]Shirt shirt)
        {
            await _shirtRepository.AddShirt(shirt);
            return CreatedAtAction(nameof(Shirts), new { id = shirt.Id }, shirt);
        }

        [HttpPost("fromform")]
        public async Task<IActionResult> AddShirtFromForm([FromForm]Shirt shirt)
        {
            await Task.Delay(1000);

            _logger.LogInformation("Recived information from a from");

            return CreatedAtAction(nameof(Shirts), new { id = shirt.Id }, shirt);
        }

        [HttpPut("{id:int}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidationUpdateShirt]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionFilterAttribute))]
        [RequiredClaimAttribute("write", "true")]
        public async Task<IActionResult> UpdateShirt(int id, Shirt shirt)
        {
            await _shirtRepository.UpdateShirt(shirt);
            
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaimAttribute("delete", "true")]
        public async Task<IActionResult> DeleteShirt(int id)
        {
            var shirt = await _shirtRepository.GetShirtById(id);

            await _shirtRepository.DeleteShirt(id);

            return Ok(shirt);
        }
    }
}
