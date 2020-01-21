dotnet build src -c Release --no-cache
dotnet test src/Tests/FileUploadApp.Tests/FileUploadApp.Tests.csproj -c Release --logger:trx