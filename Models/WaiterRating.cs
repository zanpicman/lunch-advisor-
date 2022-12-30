using LunchAdvisor.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LunchAdvisor.Models
{
    public class WaiterRating
    {
        public int ID { get; set; }
        public int WaiterID { get; set; }
        public string UserID { get; set; }
        public virtual Waiter? Waiter { get; set; }
        public virtual LunchAdvisorUser? User { get; set; }

        [Display(Name = "Title")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Review date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        public int rating { get; set; }

    }
}
