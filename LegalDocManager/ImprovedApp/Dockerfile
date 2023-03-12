#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ImprovedApp/ImprovedApp.csproj", "ImprovedApp/"]
RUN dotnet restore "ImprovedApp/ImprovedApp.csproj"
COPY . .
WORKDIR "/src/ImprovedApp"
RUN dotnet build "ImprovedApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImprovedApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImprovedApp.dll"]