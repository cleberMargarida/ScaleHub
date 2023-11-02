Notes
---

### Publish nuget packages
```bash
dotnet pack YourProjectFolder/YourProject.csproj -p:GenerateDocumentationFile=true -c Release -o output
```

### Run Sql-server
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -e "MSSQL_PID=Evaluation" -p 1433:1433  --name sqlpreview --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04
```

### Build docker images
```bash
docker-compose build
```

### Run docker-compose with replicas
```bash
docker-compose up -d --scale scalehub.samplewebapp=3
```