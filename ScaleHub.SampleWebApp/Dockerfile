#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ScaleHub.SampleWebApp/ScaleHub.SampleWebApp.csproj", "ScaleHub.SampleWebApp/"]
COPY ["ScaleHub.DependecyInjection/ScaleHub.DependecyInjection.csproj", "ScaleHub.DependecyInjection/"]
COPY ["ScaleHub.Core/ScaleHub.Core.csproj", "ScaleHub.Core/"]
COPY ["ScaleHub.SqlServer/ScaleHub.SqlServer.csproj", "ScaleHub.SqlServer/"]
RUN dotnet restore "ScaleHub.SampleWebApp/ScaleHub.SampleWebApp.csproj"
COPY . .
WORKDIR "/src/ScaleHub.SampleWebApp"
RUN dotnet build "ScaleHub.SampleWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScaleHub.SampleWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScaleHub.SampleWebApp.dll"]