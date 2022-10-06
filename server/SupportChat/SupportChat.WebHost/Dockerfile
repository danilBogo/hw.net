FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SupportChat.WebHost/SupportChat.WebHost.csproj", "SupportChat.WebHost/"]
RUN dotnet restore "SupportChat.WebHost/SupportChat.WebHost.csproj"
COPY . .
WORKDIR "/src/SupportChat.WebHost"
RUN dotnet build "SupportChat.WebHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SupportChat.WebHost.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls
ENTRYPOINT ["dotnet", "SupportChat.WebHost.dll"]
