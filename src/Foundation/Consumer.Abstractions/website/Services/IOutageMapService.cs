using SYMB2C.Foundation.Consumer.Abstractions.Models.Outagemap;

namespace SYMB2C.Foundation.Consumer.Abstractions.Services
{
    public interface IOutageMapService
    {
      void PullOutageMapInfo();

      OutageMapInfo GetOutageMapInfo();
    }
}
