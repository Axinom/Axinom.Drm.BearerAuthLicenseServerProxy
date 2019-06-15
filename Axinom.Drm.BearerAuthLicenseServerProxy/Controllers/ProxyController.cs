using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Axinom.Drm.BearerAuthLicenseServerProxy.Controllers
{
    public abstract class ProxyController : ControllerBase
    {
        private static HttpClient ProxyClient = new HttpClient();
        private static UTF8Encoding ErrorEncoding = new UTF8Encoding(false);

        private static string[] RequestHeadersToCopy = new string[]
        {
            "Origin"
        };

        private static string[] ResponseHeadersToCopy = new string[]
        {
            "X-AxDRM-Identity",
            "X-AxDRM-Server",
            "X-AxDRM-Version",
            "Access-Control-Allow-Origin"
        };

        protected async Task ProxyLicenseRequestAsync(string targetUrl, CancellationToken cancel)
        {
            // We expect there to be an Authorization header with "Bearer ABC" as the value, with ABC being the license token.
            // We just take it from this DASH-IF format and convert to Axinom DRM format.

            var header = Request.Headers["Authorization"].SingleOrDefault();

            if (header == null || !header.StartsWith("Bearer "))
            {
                Response.StatusCode = 400;
                Response.ContentType = "text/plain; charset=utf-8";
                var error = ErrorEncoding.GetBytes("There must be exactly 1 Authorization header of type Bearer.");
                Response.Body.Write(error, 0, error.Length);

                return;
            }

            var token = header.Substring("Bearer ".Length);

            var realContent = new StreamContent(Request.Body);
            realContent.Headers.ContentType = new MediaTypeHeaderValue(Request.ContentType ?? "application/octet-stream");
            realContent.Headers.Add("X-AxDRM-Message", token);

            foreach (var headerName in RequestHeadersToCopy)
            {
                if (!Request.Headers.ContainsKey(headerName))
                    continue;

                foreach (var headerValue in Request.Headers[headerName])
                    realContent.Headers.Add(headerName, headerValue);
            }

            realContent.Headers.Add("X-Forwarded-For", Request.HttpContext.Connection.RemoteIpAddress.ToString());

            var realResponse = await ProxyClient.PostAsync(targetUrl, realContent, cancel);
            Response.StatusCode = (int)realResponse.StatusCode;
            Response.ContentType = realResponse.Content.Headers.ContentType.ToString();

            foreach (var headerName in ResponseHeadersToCopy)
            {
                if (!realResponse.Headers.Contains(headerName))
                    continue;

                foreach (var headerValue in realResponse.Headers.GetValues(headerName))
                    Response.Headers.Add(headerName, headerValue);
            }

            await (await realResponse.Content.ReadAsStreamAsync()).CopyToAsync(Response.Body, cancel);
        }
    }
}
