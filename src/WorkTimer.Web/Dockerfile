FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WorkTimer.Web/WorkTimer.Web.csproj", "src/WorkTimer.Web/"]
COPY ["src/WorkTimer.Common/WorkTimer.Common.csproj", "src/WorkTimer.Common/"]
COPY ["src/WorkTimer.Web.Common/WorkTimer.Web.Common.csproj", "src/WorkTimer.Web.Common/"]
RUN dotnet restore "src/WorkTimer.Web/WorkTimer.Web.csproj"
COPY . .
WORKDIR "/src/src/WorkTimer.Web"
RUN dotnet build "WorkTimer.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WorkTimer.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkTimer.Web.dll"]
