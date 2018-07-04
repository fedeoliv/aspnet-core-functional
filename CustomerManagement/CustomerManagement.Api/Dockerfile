FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 61794
EXPOSE 44355

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY CustomerManagement/CustomerManagement.csproj CustomerManagement/
RUN dotnet restore CustomerManagement/CustomerManagement.csproj
COPY . .
WORKDIR /src/CustomerManagement
RUN dotnet build CustomerManagement.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CustomerManagement.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CustomerManagement.dll"]
