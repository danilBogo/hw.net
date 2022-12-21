FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TimMovie.Web/TimMovie.Web.csproj", "TimMovie.Web/"]
COPY ["TimMovie.Core/TimMovie.Core.csproj", "TimMovie.Core/"]
COPY ["TimMovie.SharedKernel/TimMovie.SharedKernel.csproj", "TimMovie.SharedKernel/"]
COPY ["TimMovie.Infrastructure/TimMovie.Infrastructure.csproj", "TimMovie.Infrastructure/"]
RUN dotnet restore "TimMovie.Web/TimMovie.Web.csproj"
COPY . .
WORKDIR "/src/TimMovie.Web"
RUN dotnet build "TimMovie.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimMovie.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimMovie.Web.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet TimMovie.Web.dll
