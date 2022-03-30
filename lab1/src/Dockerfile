FROM node AS ui
WORKDIR /src/ui
COPY ./ui/package*.json ./
RUN npm ci --only=production
COPY ./ui/ .
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5001
ENV ASPNETCORE_URLS http://+:5001/
ENV ASPNETCORE_ENVIRONMENT Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app/src/api
COPY ["HackMeApi/HackMeApi/HackMeApi.csproj", "HackMeApi/"]
RUN dotnet restore "HackMeApi/HackMeApi.csproj"
COPY ./HackMeApi/HackMeApi/ .
RUN dotnet build "HackMeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackMeApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ui /src/ui/build /app/ui
ENTRYPOINT ["dotnet", "ItSec.Labor1.HackMeApi.dll"]