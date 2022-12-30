namespace LunchAdvisor.Models
{
    public class Waiter
    {
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual ICollection<WaiterRating>? WaiterRatings { get; set; }

        public string getAverageRating()
        {
            if (WaiterRatings.Count == 0) return "0";
            else
            {
                float avg = 0;
                foreach (WaiterRating rating in WaiterRatings)
                {
                    avg += rating.rating;
                }
                avg /= WaiterRatings.Count;
                return avg.ToString("F1");
            }
        }

        public float getAverageRatingf()
        {
            if (WaiterRatings.Count == 0) return 0;
            else
            {
                float avg = 0;
                foreach (WaiterRating rating in WaiterRatings)
                {
                    avg += rating.rating;
                }

                return avg / WaiterRatings.Count;
            }
        }
    }
}
