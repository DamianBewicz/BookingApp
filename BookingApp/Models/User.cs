using Microsoft.AspNetCore.Identity;

namespace BookingApp.Models
{
    public class User : IdentityUser
    {
        public ICollection<House> Houses { get; } = new List<House>();
    }
}
