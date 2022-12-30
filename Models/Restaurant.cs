namespace LunchAdvisor.Models
{
    public class Restaurant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string imageURL { get; set; }
        
        public virtual ICollection<RestaurantRating>? RestaurantRatings { get; set; }
        public virtual ICollection<Dish>? Dishes { get; set; }
        public virtual ICollection<Waiter>? Waiters { get; set; }

        public string getAverageRating() {
            if(RestaurantRatings.Count == 0) return "0";
            else
            {
                float avg = 0;
                foreach(RestaurantRating rating in RestaurantRatings)
                {
                    avg += rating.rating;
                }
                avg /= RestaurantRatings.Count;
                return avg.ToString("F1");
            }
        }

        public float getAverageRatingf()
        {
            if (RestaurantRatings.Count == 0) return 0;
            else
            {
                float avg = 0;
                foreach (RestaurantRating rating in RestaurantRatings)
                {
                    avg += rating.rating;
                }
                
                return avg / RestaurantRatings.Count;
            }
        }

    }
}
