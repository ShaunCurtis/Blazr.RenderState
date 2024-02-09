using Blazr.RenderState;
using Microsoft.AspNetCore.Components;

namespace Blazor.RenderState;

public class LoggingLayoutComponentBase : LayoutComponentBase
{
    [Inject] private IBlazrRenderStateService BlazrRenderModeStateService { get; set; } = default!;

    private bool _firstRender = true;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (_firstRender)
        {
            Console.WriteLine($"{this.GetType().Name} - {this.BlazrRenderModeStateService.RenderState} - ServiceId: {this.BlazrRenderModeStateService.Id}");
            _firstRender = false;
        }
        return base.SetParametersAsync(ParameterView.Empty);
    }
}
