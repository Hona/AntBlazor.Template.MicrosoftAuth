#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AntBlazor.Template.MicrosoftAuth.Web/AntBlazor.Template.MicrosoftAuth.Web.csproj", "src/AntBlazor.Template.MicrosoftAuth.Web/"]
COPY ["src/AntBlazor.Template.MicrosoftAuth.Core/AntBlazor.Template.MicrosoftAuth.Core.csproj", "src/AntBlazor.Template.MicrosoftAuth.Core/"]
COPY ["src/AntBlazor.Template.MicrosoftAuth.Application/AntBlazor.Template.MicrosoftAuth.Application.csproj", "src/AntBlazor.Template.MicrosoftAuth.Application/"]
COPY ["src/AntBlazor.Template.MicrosoftAuth.Infrastructure/AntBlazor.Template.MicrosoftAuth.Infrastructure.csproj", "src/AntBlazor.Template.MicrosoftAuth.Infrastructure/"]
RUN dotnet restore "src/AntBlazor.Template.MicrosoftAuth.Web/AntBlazor.Template.MicrosoftAuth.Web.csproj"
COPY . .
WORKDIR "/src/src/AntBlazor.Template.MicrosoftAuth.Web"
RUN dotnet build "AntBlazor.Template.MicrosoftAuth.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AntBlazor.Template.MicrosoftAuth.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntBlazor.Template.MicrosoftAuth.Web.dll"]