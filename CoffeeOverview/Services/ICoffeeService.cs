using CoffeeOverview.Models;

namespace CoffeeOverview.Services
{
    public interface ICoffeeService
    {
        int CalculateTimeToWait(CoffeeType coffeeType, int consumedCaffeine);
    }
}
