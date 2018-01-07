using System;

namespace CarolineCottage.Repository.CarolineCottageClasses
{
    public interface IBooking
    {
        bool AvailableForShortBreaks { get; set; }
        int BookingID { get; set; }
        BookingStatus BookingStatus { get; set; }
        string Comment { get; set; }
        int WeekPrice { get; set; }
        DateTime WeekStartDate { get; set; }
    }
}