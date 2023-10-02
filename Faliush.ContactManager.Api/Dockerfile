
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Faliush.ContactManager.Api/Faliush.ContactManager.Api.csproj", "Faliush.ContactManager.Api/"]
COPY ["Faliush.ContactManager.Core/Faliush.ContactManager.Core.csproj", "Faliush.ContactManager.Core/"]
COPY ["Faliush.ContactManager.Infrastructure/Faliush.ContactManager.Infrastructure.csproj", "Faliush.ContactManager.Infrastructure/"]
COPY ["Faliush.ContactManager.Models/Faliush.ContactManager.Models.csproj", "Faliush.ContactManager.Models/"]
RUN dotnet restore "Faliush.ContactManager.Api/Faliush.ContactManager.Api.csproj"
COPY . .
WORKDIR "/src/Faliush.ContactManager.Api"
RUN dotnet build "Faliush.ContactManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Faliush.ContactManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faliush.ContactManager.Api.dll"]