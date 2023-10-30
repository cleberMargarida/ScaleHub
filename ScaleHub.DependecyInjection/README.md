## ScaleHub extension for Microsoft.Extensions.DependencyInjection

Usage
---
To use, with an `IServiceCollection` instance:

``` csharp
builder.Services.AddScaleHub(opt =>
{
    //Configure here the scalehub.
});
```

This registers:

- As a singleton for the `IScaleHub`
- As a background service for the `ScaleHubBackgroundService`

To use at runtime, add a dependency on `IScaleHub`:

```c#
public class EmployeesController 
{
	private readonly IScaleHub hub;

	public EmployeesController(IScaleHub hub)
	{
		this.hub = hub;
	}

	// use the this.hub
}
```
