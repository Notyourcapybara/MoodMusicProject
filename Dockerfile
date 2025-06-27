# 使用官方 ASP.NET Core 运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 使用 SDK 镜像构建项目
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 复制项目文件并还原依赖
COPY AspNetMVCApp/AspNetMVCApp.csproj AspNetMVCApp/
RUN dotnet restore AspNetMVCApp/AspNetMVCApp.csproj

# 复制所有源代码并发布
COPY . .
WORKDIR /src/AspNetMVCApp
RUN dotnet publish -c Release -o /publish

# 使用发布好的输出构建最终运行镜像
FROM base AS final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "AspNetMVCApp.dll"]
