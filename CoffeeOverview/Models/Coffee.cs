namespace CoffeeOverview.Models
{
    public class Coffee
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Coffee()
        {
            Name = string.Empty;
            Code = string.Empty;
        }
    }

    public class CoffeeRecommendation
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Wait { get; set; }
        public CoffeeRecommendation()
        {
            Name = string.Empty;
            Code = string.Empty;
        }
    }

    public class RecentCoffeeConsumption
    {
        public string Code { get; set; }
        public int Time { get; set; }
        public RecentCoffeeConsumption()
        {
            Code = string.Empty;
        }
    }

    public class CoffeeType
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int CaffeineLevel { get; set; }
        public CoffeeType()
        {
            Name = string.Empty;
            Code = string.Empty;
        }
    }
}

