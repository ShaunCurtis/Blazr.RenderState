/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.RenderState;

namespace Blazr.RenderLogger.WASM;

public class WASMRenderStateService : IBlazrRenderStateService
{
    public Guid ServiceUid { get; } = Guid.NewGuid();

    public BlazrRenderState RenderState => BlazrRenderState.CSR;
}
