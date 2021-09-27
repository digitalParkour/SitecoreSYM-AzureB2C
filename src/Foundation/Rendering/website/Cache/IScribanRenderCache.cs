using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SYMB2C.Foundation.Rendering.Cache
{
    public interface IScribanRenderCache
    {
        void PushEndFieldStack(string lastPart);
        string PopEndFieldStack();
    }
}