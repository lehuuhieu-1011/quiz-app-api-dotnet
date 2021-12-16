FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY quiz-app-dotnet-api.csproj ./
RUN dotnet restore "quiz-app-dotnet-api.csproj"

# copy everything else and build app
COPY . .
RUN dotnet build "quiz-app-dotnet-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "quiz-app-dotnet-api.csproj" -c Release -o /app/publish

# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "quiz-app-dotnet-api.dll"]