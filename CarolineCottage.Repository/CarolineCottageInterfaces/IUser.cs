using System;

namespace CarolineCottage.Repository.CarolineCottageClasses
{
    public interface IUser
    {
        DateTime DateEdited { get; set; }
        DateTime DateSet { get; set; }
        int EditedByID { get; set; }
        string Name { get; set; }
        string PasswordEnc { get; set; }
        string Salt { get; set; }
        int UserID { get; set; }
    }
}