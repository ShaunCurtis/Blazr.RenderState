# Detecting Pre-Rendering

It's simple to detect pre-rendering using the cascading `HttpContext` in Net8.0.

If the cascading value is not null then the component is pre-rendering.

We can wrap this into a simple component and cascade the state.

```csharp
// CascadingPreRenderState.razor
<CascadingValue Value="_preRenderState" IsFixed>
    @this.ChildContent
</CascadingValue>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [CascadingParameter] HttpContext? HttpContext { get; set; }

    private PreRenderState _preRenderState = new(false);

    protected override void OnInitialized()
    {
        _preRenderState = new(this.HttpContext is not null);
    }
}
```

The state object:

```csharp
public record PreRenderState(bool IsPreRender);
```

Place it in *Routes.razor*.

```csharp
<CascadingPreRenderState>

    <Router AppAssembly="typeof(Program).Assembly">
        <Found Context="routeData">
            <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
            <FocusOnNavigate RouteData="routeData" Selector="h1" />
        </Found>
    </Router>

</CascadingPreRenderState>
```

And demo it:

```csharp
@page "/weather"

<PageTitle>Weather</PageTitle>

<h1>Weather</h1>

<p>This component demonstrates showing data.</p>

@if (this.preRenderState.IsPreRender)
{
    <p><em>Pre-Render...</em></p>
    return;
}
@if (forecasts != null)
{
    <table class="table">
        <thead>
            <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var forecast in forecasts)
            {
                <tr>
                    <td>@forecast.Date.ToShortDateString()</td>
                    <td>@forecast.TemperatureC</td>
                    <td>@forecast.TemperatureF</td>
                    <td>@forecast.Summary</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    [CascadingParameter] private PreRenderState preRenderState { get; set; } = new(false);
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        if (this.preRenderState.IsPreRender)
            return;

        // Simulate asynchronous loading to demonstrate a loading indicator
        await Task.Delay(500);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();
    }

    private class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
```
