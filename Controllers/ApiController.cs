using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerBuilding.Models;
using ServerBuilding.DTO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ServerBuilding.Controllers
{
    [Route("[controller]")]
    [ApiController]

    //Connection to DB using DBContext
    //Connection to Enviroment???? - Need to ask Ofer!!
    public class ApiController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private TamiDBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public ApiController(TamiDBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }


         //"Register" Generation"
        [HttpPost("Register")]
        public IActionResult Register([FromBody] DTO.AppUserDTO userDto)
        {
            Models.AppUser modelsUser = new Models.AppUser()
            {
                UserName = userDto.UserName,
                UserLastName = userDto.UserLastName,
                UserEmail = userDto.UserEmail,
                UserPassword = userDto.UserPassword,
                UserPhone = userDto.UserPhone,
                ProfilePicture = userDto.ProfilePicture,    
                IsManager = userDto.IsManager
            };

            context.AppUsers.Add(modelsUser);
            context.SaveChanges();

            return Ok(true);
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] DTO.LoginInfo loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.AppUser? modelsUser = context.GetUser(loginDto.Email);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.UserPassword != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsUser.UserEmail);

                DTO.AppUserDTO dtoUser = new DTO.AppUserDTO(modelsUser);
          
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

 
        [HttpPost("updateUser")]
        // No need changes in Program
        public IActionResult UpdateUser([FromBody] DTO.AppUserDTO userDto)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.AppUser? user = context.GetUser(userEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (user == null)
                {
                    return Unauthorized("Non Manager User is trying to update a different user");
                }

                Models.AppUser appUser = new AppUser()
                {
                    Id = userDto.Id,
                    UserName = userDto.UserName,
                    UserLastName = userDto.UserLastName,
                    UserEmail = userDto.UserEmail,
                    UserPassword = userDto.UserPassword,
                    IsManager = userDto.IsManager,
                    UserPhone = userDto.UserPhone,
                    ProfilePicture = userDto.ProfilePicture
                    
                };

                context.Entry(appUser).State = EntityState.Modified;

                context.SaveChanges();

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getAllUsers")]
        // ייצוט טבלת המשתמשים לרשימה
        public IActionResult GetAllUsers()
        {
            try
            {
                // שליפת כל המשתמשים ממסד הנתונים
                List<Models.AppUser> users = context.AppUsers.ToList();

                // יצירת רשימה חדשה של DTO שמכילה את כל המשתמשים
                List<DTO.AppUserDTO> dtoUsers = new List<DTO.AppUserDTO>();

                foreach (var user in users)
                {
                    dtoUsers.Add(new DTO.AppUserDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        UserLastName = user.UserLastName,
                        UserEmail = user.UserEmail,
                        UserPassword = user.UserPassword,
                        IsManager = user.IsManager,
                        ProfilePicture = user.ProfilePicture,
                        UserPhone = user.UserPhone
                    });
                }

                // החזרת הרשימה כתשובה
                return Ok(dtoUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("deleteUserByEmail/{email}")]
        public IActionResult DeleteUserByEmail(string email)
        {
            try
            {
                // שליפת המשתמש ממסד הנתונים לפי האימייל
                Models.AppUser? user = context.AppUsers.FirstOrDefault(u => u.UserEmail == email);

                // אם המשתמש לא נמצא, מחזירים שגיאה
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // מחיקת המשתמש ממסד הנתונים
                context.AppUsers.Remove(user);
                context.SaveChanges();

                // החזרת תגובה מוצלחת
                return Ok($"User with email {email} was deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                // שליפת האימייל מתוך ה-Session (במידה והמשתמש מחובר)
                string? userEmail = HttpContext.Session.GetString("loggedInUser");

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                // שליפת פרטי המשתמש מהמסד נתונים בהתבסס על האימייל
                Models.AppUser? user = context.GetUser(userEmail);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // החזרת פרטי המשתמש כ-DTO
                DTO.AppUserDTO dtoUser = new DTO.AppUserDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserLastName = user.UserLastName,
                    UserEmail = user.UserEmail,
                    UserPhone = user.UserPhone,
                    ProfilePicture = user.ProfilePicture,
                    IsManager = user.IsManager,
                    UserPassword = user.UserPassword
                };

                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
