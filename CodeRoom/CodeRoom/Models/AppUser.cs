using Microsoft.AspNetCore.Identity;

namespace CodeRoom.Models
{
    public class AppUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string ConnectId { get; set; }
        public bool isOnline { get; set; }
    }
}
