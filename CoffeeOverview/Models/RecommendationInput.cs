using System.Collections.Generic;

namespace CoffeeOverview.Models
{
    public class RecommendationInput
    {
        public List<RecommendationItem> Recommendations { get; set; }
    }

    public class RecommendationItem
    {
        public string Code { get; set; }
        public int Time { get; set; }
    }
}

