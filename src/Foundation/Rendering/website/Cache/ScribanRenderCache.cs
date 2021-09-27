using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SYMB2C.Foundation.Rendering.Cache
{
    public class ScribanRenderCache : IScribanRenderCache
    {
        private ConcurrentStack<string> _endFieldStack;

        protected virtual ConcurrentStack<string> EndFieldStack => _endFieldStack ?? (_endFieldStack = new ConcurrentStack<string>());

        public void PushEndFieldStack(string lastPart)
        {
            EndFieldStack.Push(lastPart);
        }

        public string PopEndFieldStack()
        {
            if(EndFieldStack.TryPop(out string text)) { 
                return text;
            } else
            {
                return "";
            }
        }
    }
}