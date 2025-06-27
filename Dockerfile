# 使用官方 ASP.NET 运行时镜像作为基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 使用 SDK 镜像进行构建
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 把项目文件复制进去并还原依赖
COPY AspNetMVCApp/AspNetMVCApp.csproj AspNetMVCApp/
RUN dotnet restore AspNetMVCApp/AspNetMVCApp.csproj

# 复制所有项目文件并编译发布
COPY . .
WORKDIR /src/AspNetMVCApp
RUN dotnet publish -c Release -o /app/publishh

# 构建最终镜像
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AspNetMVCApp.dll"]
