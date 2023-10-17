using Faliush.ContactManager.Core.Services.Interfaces;

namespace Faliush.ContactManager.Core.Services.Implementations;

public class DateCalculatorService : IDateCalcualtorService
{
    public int GetTotalYears(DateTime dateOfBirth)
    {
        var dateTimeToday = DateTime.UtcNow;
        var firstDay = new DateTime(1, 1, 1);

        TimeSpan difference = dateTimeToday.Subtract(dateOfBirth);

        int totalYears = (firstDay + difference).Year - 1;

        return totalYears;
    }
}
