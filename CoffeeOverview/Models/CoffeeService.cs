using CoffeeOverview.Models;

namespace CoffeeOverview.Services
{
    public class CoffeeService
    {
        private const double CAFFEINE_HALF_LIFE = 5;

        public int CalculateTimeToWait(CoffeeType coffeeType, int consumedCaffeine)
        {
            int timeToWait = consumedCaffeine / (coffeeType.CaffeineLevel / (int)CAFFEINE_HALF_LIFE * 60);
            return timeToWait;
        }
    }
}

