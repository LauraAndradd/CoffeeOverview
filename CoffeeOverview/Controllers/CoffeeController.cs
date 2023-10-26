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
        //public ActionResult<IEnumerable<CoffeeRecommendation>> CalculateRecommendations([FromBody] List<RecentCoffeeConsumption> recentConsumptions)
        public ActionResult<IEnumerable<CoffeeRecommendation>> CalculateRecommendations([FromBody] RecommendationInput input)
        {
            List<CoffeeRecommendation> recommendations = new List<CoffeeRecommendation>();

            const double CAFFEINE_HALF_LIFE = 5.0; // horas
            const int IDEAL_CAFFEINE_LEVEL = 175; // mg

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

            int CalculateTimeToReachLevel(int desiredLevel, int currentLevel, int caffeinePerCoffee, double halfLife)
            {
                int timeToWait = 0;
                while (currentLevel < desiredLevel)
                {
                    currentLevel += caffeinePerCoffee;

                    int timeForHalfLife = (int)(halfLife * 60);
                    timeToWait += timeForHalfLife;
                }

                return timeToWait;
            }

            foreach (var recommendation in input.Recommendations)
            {
                string code = recommendation.Code;
                int time = recommendation.Time;

                CoffeeType coffeeType = coffeeTypes.FirstOrDefault(ct => ct.Code == code);

                if (coffeeType != null)
                {
                    int consumedCaffeine = input.Recommendations
                        .Where(c => c.Code == coffeeType.Code)
                        .Sum(c => c.Time <= CAFFEINE_HALF_LIFE * 60 ? coffeeType.CaffeineLevel : 0);

                    int timeToWait = 0;

                    if (consumedCaffeine < IDEAL_CAFFEINE_LEVEL)
                    {
                        int desiredLevel = IDEAL_CAFFEINE_LEVEL;
                        int currentLevel = consumedCaffeine;

                        int timeFor65mg = CalculateTimeToReachLevel(desiredLevel - 65, currentLevel, coffeeType.CaffeineLevel, CAFFEINE_HALF_LIFE);
                        int timeFor95mg = CalculateTimeToReachLevel(desiredLevel - 95, currentLevel, coffeeType.CaffeineLevel, CAFFEINE_HALF_LIFE);
                        int timeFor120mg = CalculateTimeToReachLevel(desiredLevel - 120, currentLevel, coffeeType.CaffeineLevel, CAFFEINE_HALF_LIFE);

                        if (timeFor65mg < timeFor95mg && timeFor65mg < timeFor120mg)
                        {
                            recommendations.Add(new CoffeeRecommendation
                            {
                                Name = "Espresso, Latte, Cappuccino, ou Flat White",
                                Code = coffeeType.Code,
                                Wait = timeFor65mg
                            });
                        }
                        else if (timeFor95mg < timeFor120mg)
                        {
                            recommendations.Add(new CoffeeRecommendation
                            {
                                Name = "Black Coffee",
                                Code = coffeeType.Code,
                                Wait = timeFor95mg
                            });
                        }
                        else
                        {
                            recommendations.Add(new CoffeeRecommendation
                            {
                                Name = "Cold Brew",
                                Code = coffeeType.Code,
                                Wait = timeFor120mg
                            });
                        }
                    }
                    else
                    {
                        timeToWait = 0;
                        recommendations.Add(new CoffeeRecommendation
                        {
                            Name = coffeeType.Name,
                            Code = coffeeType.Code,
                            Wait = timeToWait
                        });
                    }
                }
            }

            return Ok(recommendations);
        }
    }
}
