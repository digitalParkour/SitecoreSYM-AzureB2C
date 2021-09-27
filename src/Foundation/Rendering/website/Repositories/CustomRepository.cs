using SYMB2C.Foundation.DependencyInjection;
using Sitecore.XA.Foundation.Mvc.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;

namespace SYMB2C.Foundation.Rendering.Repositories
{
    [Service(typeof(ICustomRepository), Lifetime = Lifetime.Transient)]
    public class CustomRepository : ModelRepository, ICustomRepository
    {
        public T GetModel<T>()
            where T : RenderingModelBase, new()
        {
            T model = new T();
            FillBaseProperties(model);

            return model;
        }
    }
}