using Homework1_ASP.Dto;
using System.Globalization;

namespace Homework1_ASP.Exceptions
{
    public static class Logger
    {
        public static void LogUser(UserDto user)
        {
            var week = ISOWeek.GetWeekOfYear(DateTime.Now);
            var fileName = $"Logs/Users_{DateTime.Now.Year}_Week{week}.log";
            Directory.CreateDirectory("Logs");
            File.AppendAllText(fileName, $"{DateTime.Now}: {user.Username} - {user.Email}{Environment.NewLine}");
        }
    }
}
