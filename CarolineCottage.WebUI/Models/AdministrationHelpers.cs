using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CarolineCottage.Repository.CarolineCottageClasses;

namespace CarolineCottage.WebUI.Models
{
    public static class AdministrationHelpers
    {

        public static IEnumerable<SelectListItem> GetBookingStates()
        {
            return Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>().Select(v => new SelectListItem { Text = v.ToString(), Value = ((int)v).ToString() }).ToList();

        }

        public static string GetEnumDescriptionOrName(this Enum enumItem)
        {
            var attribute = GetAttributeOfType<DescriptionAttribute>(enumItem);

            if (attribute != null)
            {
                return attribute.Description;
            }
            var attributeDisplay = GetAttributeOfType<DisplayAttribute>(enumItem);
            if (attributeDisplay != null)
            {
                return attributeDisplay.Name;
            }

            return enumItem.ToString();
        }

        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memberInfo = type.GetMember(enumVal.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
