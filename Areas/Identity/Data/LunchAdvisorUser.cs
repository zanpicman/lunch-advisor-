using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LunchAdvisor.Models;
using Microsoft.AspNetCore.Identity;

namespace LunchAdvisor.Areas.Identity.Data;

// Add profile data for application users by adding properties to the LunchAdvisorUser class
public class LunchAdvisorUser : IdentityUser
{
    public string? imageURL { get; set; }
    public virtual ICollection<DishRating>? DishRatings { get; set; }
    public virtual ICollection<RestaurantRating>? RestaurantRatings { get; set; }
    public virtual ICollection<WaiterRating>? WaiterRatings { get; set; }
}

