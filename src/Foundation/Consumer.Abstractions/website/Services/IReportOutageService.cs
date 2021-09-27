using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Outage;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface IReportOutageService
    {
        FindOutageResult FindOutage(PowerAccountID accountNumber);

        ReportOutageResult ReportOutage(PowerAccountID accountNumber, string phoneNumber);
    }
}
