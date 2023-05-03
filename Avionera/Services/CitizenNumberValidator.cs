using Avionera.Interfaces;
using Microsoft.Identity.Client;

namespace Avionera.Services
{
    public class CitizenNumberValidator : ICitizenNumberValidator
    {
        public bool IsValid(string citizenNumber)
        {
            string yearString = citizenNumber.Substring(0, 1) + citizenNumber.Substring(1, 1);
            int year = 0;

            string monthString = citizenNumber.Substring(2, 1) + citizenNumber.Substring(3, 1);
            int month = int.Parse(monthString);

            string dayString = citizenNumber.Substring(4, 1) + citizenNumber.Substring(5, 1);
            int day = int.Parse(dayString);

            if (month >= 1 && month <= 12)
            {
                year = int.Parse($"19{yearString}");
            }
            else if (month >= 41 && month <= 52)
            {
                year = int.Parse($"20{yearString}");
                month = month - 40;
            }

            DateOnly result = new DateOnly();
            if(DateOnly.TryParse($"{year}-{month}-{day}", out result))
            {
                return true;
            }
            return false;
        }
    }
}
