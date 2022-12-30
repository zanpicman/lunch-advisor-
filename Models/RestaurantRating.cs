using LunchAdvisor.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LunchAdvisor.Models
{
    public class RestaurantRating
    {
        public int ID { get; set; }
        public int RestaurantID { get; set; }
        public string UserID { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual LunchAdvisorUser? User { get; set; }

        [Display(Name = "Title")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Review date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [Display(Name = "Rating")]
        [Range(0, 5)]
        public int rating { get; set; }
    }
}
