using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace SYMB2C.Foundation.Testing
{
    public static class HttpContextMockFactory
    {
        public static HttpContext Create()
        {
            var httpRequest = new HttpRequest("", "http://google.com/", "");
            httpRequest.Browser = new HttpBrowserCapabilities();
            httpRequest.Browser.Capabilities = new Dictionary<string, string>()
            {
                ["browser"] = "Chrome"
            };

            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 10, true, HttpCookieMode.AutoDetect, SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, new[] { typeof(HttpSessionStateContainer) }, null).Invoke(new object[] { sessionContainer });

            return httpContext;
        }
    }
}
