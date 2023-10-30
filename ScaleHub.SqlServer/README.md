## ScaleHub.SqlServer

SQL Server integration for ScaleHub.

## Features

- **SQL Server Integration**: Utilize SQL Server for data storage and synchronization. The project includes SQL Server-specific components and uses features like Service Broker for real-time data updates.

- **Database Synchronization**: Implement real-time database synchronization to keep data consistent across instances. With components to track database changes.

## Usage

To use the ScaleHub project, follow these steps:

**Configuration**: Configure the project to connect to your SQL Server database. You can use the `ScaleHubConfiguration` class and the `UseSqlServer` method for this purpose.

## Example

Here's an example of how to use the ScaleHub project:

```csharp
services.AddScaleHub(opt =>
{
    opt.UseSqlServer("your_connection_string");
});
```
