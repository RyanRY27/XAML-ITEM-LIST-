using SQLite;

namespace Final.Models
{
    public class UserDetails
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfileImagePath { get; set; } 
    }
}
