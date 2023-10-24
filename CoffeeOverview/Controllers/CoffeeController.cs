using CoffeeOverview.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CoffeeOverview.Services;

namespace CoffeeOverview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeService coffeeService;

        public CoffeeController(ICoffeeService coffeeService)
        {
            this.coffeeService = coffeeService;
        }

        [HttpGet("coffees")]
        public ActionResult<IEnumerable<Coffee>> GetCoffees()
        {
            List<Coffee> availableCoffees = new List<Coffee>
            {
                new Coffee { Name = "Black Coffee", Code = "blk" },
                new Coffee { Name = "Espresso", Code = "esp" },
                new Coffee { Name = "Cappuccino", Code = "cap" },
                new Coffee { Name = "Latte", Code = "lat" },
                new Coffee { Name = "Flat White", Code = "wht" },
                new Coffee { Name = "Cold Brew", Code = "cld" },
                new Coffee { Name = "Decaf Coffee", Code = "dec" }
            };

            return Ok(availableCoffees);
        }

        [HttpPost("calculate")]
        public ActionResult<IEnumerable<CoffeeRecommendation>> CalculateRecommendations([FromBody] List<RecentCoffeeConsumption> recentConsumptions)
        {
            List<CoffeeRecommendation> recommendations = new List<CoffeeRecommendation>();

            const double CAFFEINE_HALF_LIFE = 5.0; // horas

            List<CoffeeType> coffeeTypes = new List<CoffeeType>
            {
                new CoffeeType { Code = "blk", CaffeineLevel = 95 },
                new CoffeeType { Code = "esp", CaffeineLevel = 63 },
                new CoffeeType { Code = "cap", CaffeineLevel = 63 },
                new CoffeeType { Code = "lat", CaffeineLevel = 63 },
                new CoffeeType { Code = "wht", CaffeineLevel = 63 },
                new CoffeeType { Code = "cld", CaffeineLevel = 120 },
                new CoffeeType { Code = "dec", CaffeineLevel = 7 }
            };

            foreach (var recentConsumption in recentConsumptions)
            {
                CoffeeType coffeeType = coffeeTypes.FirstOrDefault(ct => ct.Code == recentConsumption.Code);

                if (coffeeType != null)
                {
                    int consumedCaffeine = recentConsumptions
                        .Where(c => c.Code == coffeeType.Code)
                        .Sum(c => c.Time <= CAFFEINE_HALF_LIFE * 60 ? coffeeType.CaffeineLevel : 0); // Converte a meia-vida para minutos

                    int timeToWait = 0;

                    timeToWait = coffeeService.CalculateTimeToWait(coffeeType, consumedCaffeine);

                    recommendations.Add(new CoffeeRecommendation
                    {
                        Name = coffeeType.Name,
                        Code = coffeeType.Code,
                        Wait = timeToWait
                    });
                }
            }

            return Ok(recommendations);
        }
    }
}
