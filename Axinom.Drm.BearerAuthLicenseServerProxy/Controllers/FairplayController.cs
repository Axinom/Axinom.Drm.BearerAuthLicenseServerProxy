using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Axinom.Drm.BearerAuthLicenseServerProxy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FairPlayController : ProxyController
    {
        [HttpPost("AcquireLicense")]
        public Task AcquireLicense(CancellationToken cancel)
        {
            return ProxyLicenseRequestAsync("https://drm-fairplay-licensing.axtest.net/AcquireLicense", cancel);
        }
    }
}
