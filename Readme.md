Axinom DRM "Bearer" authorization license server proxy
======================================================

Implements the license request authorization protocol described in https://github.com/Dash-Industry-Forum/DASH-IF-IOP/issues/300 and forwards license requests to the real Axinom DRM testing license server.

This is a temporary solution that can be used for R&D until the Axinom DRM license server itself supports the DASH-IF mechanism.

This software is deployed to https://axinom-drm-bearer-token-proxy.azurewebsites.net and may be used via the following URLs:

* https://axinom-drm-bearer-token-proxy.azurewebsites.net/PlayReady/AcquireLicense
* https://axinom-drm-bearer-token-proxy.azurewebsites.net/Widevine/AcquireLicense
* https://axinom-drm-bearer-token-proxy.azurewebsites.net/FairPlay/AcquireLicense