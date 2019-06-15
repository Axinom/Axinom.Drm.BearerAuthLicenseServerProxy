using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Axinom.Drm.BearerAuthLicenseServerProxy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayReadyController : ProxyController
    {
        [HttpPost("AcquireLicense")]
        public Task AcquireLicense(CancellationToken cancel)
        {
            return ProxyLicenseRequestAsync("https://drm-playready-licensing.axtest.net/AcquireLicense", cancel);
        }
    }
}
