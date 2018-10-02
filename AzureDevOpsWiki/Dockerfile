FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50529
EXPOSE 44382

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AzureDevOpsWiki/AzureDevOpsWiki.csproj", "AzureDevOpsWiki/"]
RUN dotnet restore "AzureDevOpsWiki/AzureDevOpsWiki.csproj"
COPY . .
WORKDIR "/src/AzureDevOpsWiki"
RUN dotnet build "AzureDevOpsWiki.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AzureDevOpsWiki.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "AzureDevOpsWiki.dll"]