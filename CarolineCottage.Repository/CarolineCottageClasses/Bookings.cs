using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarolineCottage.Repository.CarolineCottageClasses
{
    public class Booking : IBooking
    {
        public int BookingID { get; set; }
        public DateTime WeekStartDate { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public int WeekPrice { get; set; }
        public bool AvailableForShortBreaks { get; set; }
        public string Comment { get; set; }
        public const DayOfWeek ChangeoverDay = DayOfWeek.Saturday;

    }

    public enum BookingStatus
    {
        [Description("Available")]
        Available = 0,
        [Description("Reserved")]
        Reserved = 1,
        [Description("Booked")]
        Booked = 2,
        [Description("Booked from")]
        BookedFrom = 3,
        [Description("Booked to")]
        BookedTo = 4
    }
}
