using System.ComponentModel.DataAnnotations;
using ServerBuilding.Models;

namespace ServerBuilding.DTO
{
    public class AppUserDTO
    {
        public int Id { get; set; }
        public string UserLastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public bool IsManager { get; set; }


        public AppUserDTO() { }
        public AppUserDTO(Models.AppUser modelUser)
            // ask eran : למה אובייקט דיטיאו הוא מיוצר ממחלקת מודלס?
            // הרי בלקוח אין את מודלס
        {
            this.Id = modelUser.Id;
            this.UserName = modelUser.UserName;
            this.UserLastName = modelUser.UserLastName;
            this.UserEmail = modelUser.UserEmail;
            this.UserPassword = modelUser.UserPassword;
            this.IsManager = modelUser.IsManager;
            this.ProfilePicture = modelUser.ProfilePicture;
            this.UserPhone = modelUser.UserPhone;

        }

    }
}
