/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.RenderState;

public interface IBlazrRenderStateService
{
    public Guid ServiceUid { get; }
    public string Id { get; }
    public BlazrRenderState RenderState { get; }
    public bool IsPreRender => this.RenderState == BlazrRenderState.PreRender;
}

public enum BlazrRenderState
{
    None,
    PreRender,
    SSR,
    CSR
}