using Sitecore.XA.Foundation.Mvc.Models;

namespace SYMB2C.Foundation.Rendering.Repositories
{
    public interface ICustomRepository
    {
        T GetModel<T>() where T : RenderingModelBase, new();
    }
}
