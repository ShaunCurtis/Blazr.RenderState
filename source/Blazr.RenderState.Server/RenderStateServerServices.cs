/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.RenderState.Server;

public static class RenderStateServerServices
{
    public static void AddBlazrRenderStateServerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IBlazrRenderStateService, ServerRenderStateService>();
    }
}
