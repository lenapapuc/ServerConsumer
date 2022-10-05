FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KitchenPr/KitchenPr.csproj", "KitchenPr/"]
RUN dotnet restore "KitchenPr/KitchenPr.csproj"
COPY . .
WORKDIR "/src/KitchenPr"
RUN dotnet build "KitchenPr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KitchenPr.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KitchenPr.dll"]
