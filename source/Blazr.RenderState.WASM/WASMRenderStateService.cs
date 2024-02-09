/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.RenderState.WASM;

public class WASMRenderStateService : IBlazrRenderStateService
{
    public Guid ServiceUid { get; } = Guid.NewGuid();
    public string Id => this.ServiceUid.ToString().Substring(0, 4);

    public BlazrRenderState RenderState => BlazrRenderState.CSR;
}
