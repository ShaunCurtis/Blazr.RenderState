/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.RenderState.Server;

public class ServerRenderStateService : IBlazrRenderStateService
{
    private IHttpContextAccessor _httpContextAccessor;
    public Guid ServiceUid { get; } = Guid.NewGuid();
    public string Id => this.ServiceUid.ToString().Substring(0, 4);

    public BlazrRenderState RenderState =>
        !(_httpContextAccessor.HttpContext is not null && _httpContextAccessor.HttpContext.Response.HasStarted)
            ? BlazrRenderState.PreRender
            : BlazrRenderState.SSR;

    public ServerRenderStateService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
