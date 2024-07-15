# Blazor.RenderState

*RenderState* is a Nuget package that provides state information on the render mode of components.

### Adding the RenderState to a Solution

On any *Blazor Web Project* template, add the following Nuget packages to the projects:

Web Server :

```xml
<PackageReference Include="Blazr.RenderState" Version="latest" />
<PackageReference Include="Blazr.RenderState.Server" Version="latest" />
```

Client :

```xml
<PackageReference Include="Blazr.RenderState" Version="latest" />
<PackageReference Include="Blazr.RenderState.WASM" Version="latest" />
```

Add the following services to the Server `Program`:

```csharp
builder.AddBlazrRenderStateServerServices();
```

And the following services to the Client `Program`:

```csharp
builder.AddBlazrRenderStateWASMServices();
```

And add the following `using` to both project's `_Imports.razor`.

```csharp
@using Blazr.RenderState
```

## Using the State

Inject `IBlazrRenderStateService` into any component where you need to check the state.

