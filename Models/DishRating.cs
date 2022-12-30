using LunchAdvisor.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LunchAdvisor.Models
{
    public class DishRating
    {
        public int ID { get; set; }
        public int DishID { get; set; }
        public string UserID { get; set; }
        public virtual Dish? Dish { get; set; }
        public virtual LunchAdvisorUser? User { get; set; }
        [Display(Name = "Title")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Review Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public int rating { get; set; }
        public string? imageURL { get; set; }
    }
}
