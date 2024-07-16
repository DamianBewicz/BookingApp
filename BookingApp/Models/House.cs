using System.ComponentModel.DataAnnotations;
using BookingApp.Enums;

namespace BookingApp.Models
{
    public class House
    {
        [Key]
        public int HouseId { get; set; }
        public User Owner { get; set; } = null!;
        [MaxLength(150)]
        public string Title { get; set; } = null!;
        [MaxLength(150)]
        public string Description { get; set; } = null!;
        public HouseStatus Status { get; set; } = HouseStatus.NEW;

        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value must be a number bigger than 0")]
        public int LivingSpace { get; set; }
    }
}
