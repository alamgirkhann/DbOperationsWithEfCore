using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet("")]
        public IActionResult GetAllCurrencies()
        {
            //var currencies = appDbContext.Currencies.ToList();

            //var result = (from currencies in appDbContext.Currencies
            //              select currencies).ToList();

            var result = (from currencies in appDbContext.Currencies
                          select new
                          {
                              Id = currencies.Id,
                              Description = currencies.Description
                          }).AsNoTracking().ToList();
            return Ok(result);
        }


        //[HttpGet("")]
        //public async Task<IActionResult> GetAllCurrencies()
        //{
        //    //var currencies = await appDbContext.Currencies.ToListAsync();

        //    var result = await (from currencies in appDbContext.Currencies
        //                        select currencies).ToListAsync();
        //    return Ok(result);
        //}

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyByIdAsync([FromRoute] int id)
        {
            var currency = await appDbContext.Currencies.FindAsync(id);
            return Ok(currency);
        }

        //[HttpGet("{name}")]
        //public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name)
        //{
        //    // record should exists only one time
        //    //var currency = await appDbContext.Currencies.Where(x => x.Title == name).SingleOrDefaultAsync();

        //    // this will go through the whole table, and create a new list then return a matching record. where condition in this case runs first.
        //    //var currency = await appDbContext.Currencies.Where(x=> x.Title == name).FirstOrDefaultAsync();

        //    // this will not go throught the whole table, where ever it finds the first matching record it will return it. improved performance
        //    var currency = await appDbContext.Currencies.FirstOrDefaultAsync(x=> x.Title == name);

        //    return Ok(currency);
        //}

        /*
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
        {
            var currency = await appDbContext.Currencies
                .FirstOrDefaultAsync(x => 
                x.Title == name
                && (string.IsNullOrEmpty(description) || x.Description == description));

            return Ok(currency);
        }
        */

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
        {
            // always write your condition first
            var currency = await appDbContext.Currencies
                .Where(x =>
                x.Title == name
                && (string.IsNullOrEmpty(description) || x.Description == description)).ToListAsync();

            return Ok(currency);
        }

        /*
        [HttpPost("all")]
        public async Task<IActionResult> GetCurrienciesByIdsAsync([FromBody] List<int> ids)
        {
            // var ids = new List<int> { 1, 3, 4 };

            var curriencies = await appDbContext.Currencies
                .Where(x => ids.Contains(x.Id)).ToListAsync();

            return Ok(curriencies);
        }
        */

        [HttpPost("all")]
        public async Task<IActionResult> GetCurrienciesByIdsAsync([FromBody] List<int> ids)
        {
            // var ids = new List<int> { 1, 3, 4 };

            var curriencies = await appDbContext.Currencies
                
                .Where(x => 
                ids.Contains(x.Id)).Select(x => new Currency()
                {
                    Id = x.Id,
                    Description = x.Description
                }).ToListAsync();

            return Ok(curriencies);
        }
    }
}
