#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Token_Eport/Token_Eport.csproj", "Token_Eport/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]
RUN dotnet restore "Token_Eport/Token_Eport.csproj"
COPY . .
WORKDIR "/src/Token_Eport"
RUN dotnet build "Token_Eport.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Token_Eport.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Token_Eport.dll"]
