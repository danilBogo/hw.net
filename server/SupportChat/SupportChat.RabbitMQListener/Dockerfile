FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SupportChat.RabbitMQListener/SupportChat.RabbitMQListener.csproj", "SupportChat.RabbitMQListener/"]
COPY ["SupportChat.Domain/SupportChat.Domain.csproj", "SupportChat.Domain/"]
COPY ["SupportChat.SharedKernel/SupportChat.SharedKernel.csproj", "SupportChat.SharedKernel/"]
COPY ["SupportChat.Infrastructure/SupportChat.Infrastructure.csproj", "SupportChat.Infrastructure/"]
RUN dotnet restore "SupportChat.RabbitMQListener/SupportChat.RabbitMQListener.csproj"
COPY . .
WORKDIR "/src/SupportChat.RabbitMQListener"
RUN dotnet build "SupportChat.RabbitMQListener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SupportChat.RabbitMQListener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupportChat.RabbitMQListener.dll"]
