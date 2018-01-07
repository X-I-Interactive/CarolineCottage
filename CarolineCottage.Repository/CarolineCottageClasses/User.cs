using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarolineCottage.Repository.CarolineCottageClasses
{
    public class User : IUser
    {
        public int UserID { get; set; }
        public string Name { get; set; }        
        public int EditedByID { get; set; }
        public DateTime DateEdited { get; set; }
        public string PasswordEnc { get; set; }
        public DateTime DateSet { get; set; }        
        public string Salt { get; set; }
    }
}
