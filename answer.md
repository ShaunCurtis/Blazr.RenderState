Net8 introduced the Rendermode, and with it the ability to create all sorts of variations of static. active and client side rendered pages.

The problem with this choice is it's very easy to get things wrong.

For this answer I've:

1. Deployed a *Blazor Web Project* template with *InteractiveServer* and *Per page/component* options selected.
2. Added the *Blazr.RenderState* Nuget package.
3. Added the *Blazr.RenderState* services to the server.

```csharp
builder.AddBlazrRenderStateServerServices();
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

That means that `App`, `Router`, `MainLayout`, `NavMenu` are all SSSR.  It's only `Home` and it's sub-components that are ASSR.

### `IBlazrRenderStateService`

`IBlazrRenderStateService` detects a pre-render, so we can avoid doing any long running async tasks and display the same loading page.

This version of the page loads immediately and the SSSR to ASSR context switch is much more seamless.

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

### OnAfterRender{Async}

`OnAfterRender{Async}` works in this instance because it isn't called when pre-rendering (there's no js context).  However I don't believe it's good practice to use it in this context for the same reasons given in these two questions and answers.

 - https://stackoverflow.com/questions/69374767/should-i-use-onafterrenderasync-instead-of-oninitializedasync

 - https://stackoverflow.com/questions/76442472/blazor-should-i-read-in-my-data-in-oninitializedasync-or-onafterrenderasync