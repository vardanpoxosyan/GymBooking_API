namespace GymBooking_API.Entity
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Membership> Memberships { get; set; }= new List<Membership>();
        public ICollection<Booking> Bookings { get; set; }= new List<Booking>();
    }
}
