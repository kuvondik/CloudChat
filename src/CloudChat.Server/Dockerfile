FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CloudChat.Server/CloudChat.Server.csproj", "CloudChat.Server/"]
COPY ["CloudChat.Core/CloudChat.Core.csproj", "CloudChat.Core/"]
RUN dotnet restore "CloudChat.Server/CloudChat.Server.csproj"
COPY . .
WORKDIR "/src/CloudChat.Server"
RUN dotnet build "CloudChat.Server.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CloudChat.Server.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CloudChat.Server.dll"]