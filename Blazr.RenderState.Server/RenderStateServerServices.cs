/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.RenderState;

namespace Blazr.RenderLogger.Server;

public static class RenderStateServerServices
{
    public static void AddBlazrRenderStateServerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IBlazrRenderStateService, ServerRenderStateService>();
    }
}
