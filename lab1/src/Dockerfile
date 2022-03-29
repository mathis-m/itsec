FROM node AS ui
WORKDIR /src/ui
COPY ./ui/package*.json ./
RUN npm ci --only=production
COPY ./ui/ .
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src/api
COPY ["HackMeApi/HackMeApi/HackMeApi.csproj", "HackMeApi/"]
RUN dotnet restore "HackMeApi/HackMeApi.csproj"
COPY ./HackMeApi/HackMeApi/ .
COPY --from=ui /src/ui/build /src/ui
RUN dotnet build "HackMeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackMeApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackMeApi.dll"]