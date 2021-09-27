using SYMB2C.Foundation.Consumer.Abstractions.Models.ID;
using SYMB2C.Foundation.Consumer.Abstractions.Models.Outage;

namespace SYMB2C.Foundation.Consumer.Services.WebServices
{
    public interface IReportOutageWebService
    {
        FindOutageResult FindOutage(PowerAccountID accountNumber);

        ReportOutageResult ReportaOutage(PowerAccountID accountNumber, string phoneNumber);
    }
}