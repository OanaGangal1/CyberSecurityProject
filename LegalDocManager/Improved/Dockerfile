#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update && apt-get install -y git

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN git clone https://github.com/OanaGangal1/CyberSecurityProject.git
RUN cd /LegalDocManager/Improved
RUN dotnet build "Improved.csproj" -c Release -o /app/build
#COPY ["Improved/Improved.csproj", "Improved/"]
#COPY ["Dependencies/Dependencies.csproj", "Dependencies/"]
#RUN dotnet restore "Improved/Improved.csproj"
#COPY . .
#WORKDIR "/src/Improved"
#RUN dotnet build "Improved.csproj" -c Release -o /app/build
#
FROM build AS publish
RUN dotnet publish "Improved.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Improved.dll"]