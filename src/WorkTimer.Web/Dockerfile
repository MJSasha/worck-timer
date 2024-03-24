FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WorkTimer.Web/WorkTimer.Web.csproj", "WorkTimer.Web/"]
COPY ["submodules/quick-actions/QuickActions.Common/QuickActions.Common.csproj", "submodules/quick-actions/QuickActions.Common/"]
COPY ["submodules/quick-actions/QuickActions.Web.Identity/QuickActions.Web.Identity.csproj", "submodules/quick-actions/QuickActions.Web.Identity/"]
COPY ["WorkTimer.Common/WorkTimer.Common.csproj", "WorkTimer.Common/"]
COPY ["WorkTimer.Web.Common/WorkTimer.Web.Common.csproj", "WorkTimer.Web.Common/"]
RUN dotnet restore "WorkTimer.Web/WorkTimer.Web.csproj"
COPY . .
WORKDIR "/src/WorkTimer.Web"
RUN dotnet build "WorkTimer.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WorkTimer.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkTimer.Web.dll"]
