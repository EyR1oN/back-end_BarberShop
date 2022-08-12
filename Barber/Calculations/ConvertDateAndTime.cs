using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Calculations
{
    public class ConvertDateAndTime
    {
        public static int ConvertTime(string time)
        {
            string hour;
            string minute;
            string sumTimeMinute;
            hour = time.Split(":")[0];
            minute = time.Split(":")[1];
            sumTimeMinute = ((Int32.Parse(hour) * 60) + Int32.Parse(minute)).ToString();
            return (Int32.Parse(sumTimeMinute));
        }
        public static string ConvertDate(string dateTime)
        {

            string date = dateTime.Split(' ')[0];
            string year = date.Split('.')[2];
            string month = date.Split('.')[1];
            string day = date.Split('.')[0];
            return (year + "-" + month + "-" + day);
        }
    }
}
