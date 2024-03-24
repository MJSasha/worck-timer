FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WorckTimer.Api/WorkTimer.Api.csproj", "WorckTimer.Api/"]
COPY ["submodules/quick-actions/QuickActions.Api.Identity/QuickActions.Api.Identity.csproj", "submodules/quick-actions/QuickActions.Api.Identity/"]
COPY ["submodules/quick-actions/QuickActions.Common/QuickActions.Common.csproj", "submodules/quick-actions/QuickActions.Common/"]
COPY ["submodules/quick-actions/QuickActions.Api/QuickActions.Api.csproj", "submodules/quick-actions/QuickActions.Api/"]
COPY ["WorkTimer.Common/WorkTimer.Common.csproj", "WorkTimer.Common/"]
RUN dotnet restore "WorckTimer.Api/WorkTimer.Api.csproj"
COPY . .
WORKDIR "/src/WorckTimer.Api"
RUN dotnet build "WorkTimer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WorkTimer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkTimer.Api.dll"]
