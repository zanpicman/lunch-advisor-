namespace LunchAdvisor.Models
{
    public class Dish
    {
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public string? ImageURL { get; set; }
         public virtual Restaurant? Restaurant { get; set; }
        public virtual ICollection<DishRating>? DishRatings { get; set; }

        public string getAverageRating()
        {
            if (DishRatings.Count == 0) return "0";
            else
            {
                float avg = 0;
                foreach (DishRating rating in DishRatings)
                {
                    avg += rating.rating;
                }
                avg /= DishRatings.Count;
                return avg.ToString("F1");
            }
        }

        public float getAverageRatingf()
        {
            if (DishRatings.Count == 0) return 0;
            else
            {
                float avg = 0;
                foreach (DishRating rating in DishRatings)
                {
                    avg += rating.rating;
                }

                return avg / DishRatings.Count;
            }
        }
    }
}
