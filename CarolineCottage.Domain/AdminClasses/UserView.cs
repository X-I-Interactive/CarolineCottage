using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarolineCottage.Repository.CarolineCottageClasses;
using CarolineCottage.Repository.CarolineCottageDatabase;

namespace CarolineCottage.Domain
{
    public class UserList
    {
        public string Name { get; set; }
        public int UserID { get; set; }
        public Guid UserGuid { get; set; }

        public UserList()
        {
            UserGuid = new Guid();
            UserID = 0;
        }
    }

    public class UserView : IUser
    {
        public DateTime DateEdited { get; set; }
        public DateTime DateSet { get; set; }
        public int EditedByID { get; set; }

        [Display(Name = "User/login name")]
        [Required(ErrorMessage = "A login name is required")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "{0} must be between {2} and {1} characters in length")]
        public string Name { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "A password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} must be between {2} and {1} characters in length")]
        public string Password { get; set; }

        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The two password fields do not match.")]
        public string RepeatPassword { get; set; }

        public string PasswordEnc { get; set; }
        public string Salt { get; set; }
        public int UserID { get; set; }
        public UserView()
        {

        }

        public bool ValidateLogin(string connectionString)
        {
            return (PasswordUtilityFunctions.GetPasswordStatus(Name, Password, connectionString) == PasswordStatus.Valid);
        }

        public static List<UserList> GetUserList(string connectionString)
        {
            List<UserList> userList = new List<UserList>();
            AutoMapper.Mapper.CreateMap<User, UserList>();
            using (CarolineCottageDbContext dbContext = new CarolineCottageDbContext(connectionString))
            {
                userList = AutoMapper.Mapper.Map<List<User>, List<UserList>>(dbContext.Users.OrderBy(x => x.Name).ToList());
            }
            return userList;
        }

        public static UserView GetUserByID(int userID, string connectionString)
        {
            AutoMapper.Mapper.CreateMap<User, UserView>();
            using (CarolineCottageDbContext dbContext = new CarolineCottageDbContext(connectionString))
            {
                User user = dbContext.Users.FirstOrDefault(x => x.UserID == userID) ?? new User();
                return AutoMapper.Mapper.Map<User, UserView>(user);
            }
        }

        public bool Save(string connectionString)
        {
            AutoMapper.Mapper.CreateMap<UserView, User>();
            using (CarolineCottageDbContext dbContext = new CarolineCottageDbContext(connectionString))
            {
                DateSet = DateTime.Now;
                Salt = PasswordUtilityFunctions.CreateSalt(6);
                PasswordEnc = PasswordUtilityFunctions.CreatePasswordHashSHA1(Password, Salt);
                dbContext.Entry(AutoMapper.Mapper.Map<UserView, User>(this)).State = this.UserID == 0 ? EntityState.Added : EntityState.Modified;
                dbContext.SaveChanges();

            }
            return true;
        }
    }
}
