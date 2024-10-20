using System.ComponentModel.DataAnnotations;
using ServerBuilding.Models;


namespace ServerBuilding.DTO
{
    public class LoginInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
