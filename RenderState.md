# Blazor.RenderState

*RenderState* is a Nuget package that provides state information on the render mode of components.

Net8 introduced the Rendermode, and with it the ability to create all sorts of variations of static. active and client side rendered pages.

The problem with this choice is it's very easy to get things wrong.

The Demo project provides an illustration of one common problem and how *Blazr.RenderState* helps you overcome it.

It's a *Blazor Web Project* template with *InteractiveServer* and *Per page/component* options selected.

The Demo installs the *Blazor.RenderState.Server* package and adds the necessary services in `Program`.

```csharp
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.AddBlazrRenderStateServerServices();

var app = builder.Build();
```

A normal async load page looks like this.  I've used an `await Task.Delay(2000);` to fake a slow async database data get.

```caharp
@page "/"
@rendermode InteractiveServer
@using Blazr.RenderState
<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

Welcome to your new app.

@if (_loading)
{
    <div class="alert alert-warning">Loading</div>
    return;
}


<div class="alert alert-success">Loaded</div>

@code {
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        // emulate slow data load
        await Task.Delay(2000);
        _loading = false;
    }
}
```

When you first run application you will notice:

1. A lag as the page pre-renders.
2. Loaded displayed as the pre-render completes.
3. Loading displayed as the page loads interactively.
4. Loaded after the completion of the await.

If you go to another page and then navigate back you will get the same effect, except in step 1 it will lag on the page you are leaving.

The reason for this is that the application is set to *Per page/component* and the router is being rendered statically on the server. In `App` it's render mode is not set

```csharp
    <Routes />
```

It's an SSSR component.

That means that all the pages are initially statically rendered too.  `App`, `Router`, `MainLayout`, `NavMenu` are all SSSR.  It's only `Home` and it's sub-components that are ASSR.

Using the `IBlazrRenderStateService` we can detect a pre-render, not do any long running async tasks and display the same loading page.

The page loads immediately and the SSSR to ASSR context switch is much more seamless.

```caharp
@page "/Loader"
@rendermode InteractiveServer
@using Blazr.RenderState
@inject IBlazrRenderStateService RenderState
<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

Welcome to your new app.

@if (RenderState.IsPreRender || _loading)
{
    <div class="alert alert-warning">Loading</div>
    return;
}


<div class="alert alert-success">Loaded</div>

@code {
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        // exist early
        if (this.RenderState.IsPreRender)
            return;

        await Task.Delay(2000);
        _loading = false;
    }
}
```

## Logging

The library has two components you can use to log the render mode.

Add `@inherits LoggingComponentBase` to `App`, `Routes`, `Home`, etc.  Add `@inherits LoggingLayoutComponentBase` to `MainLayout`.

You will then see console logging [either in the Server console or the Browser console].  Here's an example:

```text
App - PreRender - ServiceId: 29dc
Routes - PreRender - ServiceId: 29dc
MainLayout - PreRender - ServiceId: 29dc
Home - PreRender - ServiceId: 29dc
Home - SSR - ServiceId: af2e
App - PreRender - ServiceId: 7725
Routes - PreRender - ServiceId: 7725
MainLayout - PreRender - ServiceId: 7725
Counter - PreRender - ServiceId: 7725
Counter - SSR - ServiceId: af2e
App - PreRender - ServiceId: f4e4
Routes - PreRender - ServiceId: f4e4
MainLayout - PreRender - ServiceId: f4e4
Weather - PreRender - ServiceId: f4e4
```