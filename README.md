# ScaleHub

The ScaleHub is a lib designed for managing scales in distributed applications. It provides tools for monitoring the number of service replicas and synchronizing data across multiple instances or servers. The project includes Service Broker for real-time data updates, background services, and external synchronization components.

## Features

- **External Synchronization**: Implement external real-time synchronization to keep data consistent across instances.

- **Event Architecture Oriented**: Instantly receive notifications when changes occur, allowing your application to react promptly, with `OnSubscribing` and `OnUnsubscribing` events.

## Usage

To use the ScaleHub project, follow these steps:

1. **Configure ScaleHub**: Use the `AddScaleHub` to inject the `IScaleHub`.

2. **Specify integration**: Configure the project to connect to your external service, like SQL Server database. You can use `UseSqlServer` method for this purpose.

3. **Monitoring**: Use the `IScaleHub` to obtain infomations about your instance and the number of replicas.
    ```csharp
    public class ScaleContext
    {
        public int Replicas { get; set; }
        public int Actual { get; set; }
    }
    ```

4. **Configure Events**: [optional] Make use of the `ConfigureSubs` method to monitor and deal with replicas number change.

    - **OnSubscribing**: Configure events to trigger when a new replica is added.

    - **OnUnsubscribing**: Configure events to trigger when a old replica is removed.

## Example

```csharp
services.AddScaleHub(opt =>
{
    opt.UseSqlServer("your_connection_string");
    opt.ConfigureSubs(e =>
    {
        e.OnSubscribing += LogNewReplica;
        e.OnUnsubscribing += LogOldReplica;
    });
});

// 5. Start and monitor your distributed application using the ScaleHub system.
```

