FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app
COPY ./src .
RUN dotnet publish FileUploadApp -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:6.0
WORKDIR /app
COPY --from=build /app/FileUploadApp/out .
EXPOSE 5000
ENTRYPOINT dotnet FileUploadApp.dll
