/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.RenderState;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.RenderLogger.WASM;

public static class RenderStateWASMServices
{
    public static void AddBlazrRenderStateWASMServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<IBlazrRenderStateService, WASMRenderStateService>();
    }
}
