# Stargazer.Orleans.Template

基于 Orleans 的分布式应用项目模板，支持微服务架构和集群部署。

## 技术栈

- **.NET 10.0**
- **Orleans 9.2.1** - 分布式 Actor 框架
- **ASP.NET Core 10.0** - Web 主机
- **PostgreSQL** - 持久化存储
- **Serilog** - 日志框架

## 项目结构

```
src/
├── Stargazer.Orleans.Template.Host/          # ASP.NET Core Web 主机
│   ├── Controllers/                   # API 控制器
│   └── Middlewares/                 # 中间件
├── Stargazer.Orleans.Template.Silo/     # Orleans Silo 主机
├── Stargazer.Orleans.Template.Client/     # Orleans 客户端
├── Stargazer.Orleans.Template.Grains/   # Grain 实现
│   └── Grains/                        # 业务 Grain
├── Stargazer.Orleans.Template.Grains.Abstractions/  # Grain 接口定义
│   └── Users/Dtos/                     # 数据传输对象
├── Stargazer.Orleans.Template.Domain/     # 领域模型
├── Stargazer.Orleans.Template.Domain.Share/     # 领域共享
├── Stargazer.Orleans.Template.EntityFrameworkCore/  # EF Core 持久化
└── Stargazer.Orleans.Utility/       # 工具类

aspire/
├── Stargazer.AppHost/             # Aspire 应用主机
└── Stargazer.ServiceDefaults/    # 服务默认配置
```

## 快速开始

### 前置要求

- .NET 10.0 SDK
- PostgreSQL 15+

### 构建

```bash
dotnet build
```

### 运行

```bash
# 启动 Silo
dotnet run --project src/Stargazer.Orleans.Template.Silo

# 启动 Host (另一个终端)
dotnet run --project src/Stargazer.Orleans.Template.Host
```

Host 默认监听 `http://localhost:5000`，Silo 默认监听 `11111` 端口。

### API 文档

启动后访问 `/scalar` 查看 OpenAPI 文档。

## 数据库脚本

- [PostgreSQL-Main.sql](https://github.com/dotnet/orleans/blob/main/src/AdoNet/Shared/PostgreSQL-Main.sql)
- [PostgreSQL-Persistence.sql](https://github.com/dotnet/orleans/blob/main/src/AdoNet/Orleans.Persistence.AdoNet/PostgreSQL-Persistence.sql)
- [PostgreSQL-Clustering.sql](https://github.com/dotnet/orleans/tree/main/src/AdoNet/Orleans.Clustering.AdoNet/PostgreSQL-Clustering.sql)
- [PostgreSQL-Clustering-Migrations](https://github.com/dotnet/orleans/blob/main/src/AdoNet/Orleans.Clustering.AdoNet/Migrations/PostgreSQL-Clustering-3.7.0.sql)

## 配置

在 `appsettings.json` 中配置数据库连接字符串和 Orleans 集群参数。

## 全局异常处理

通过 `GlobalExceptionFilter` 统一处理异常，返回格式:

```json
{ "code": 404, "message": "资源不存在" }
```

| 异常类型 | Code | 说明 |
|----------|------|------|
| `ArgumentException` | 400 | 参数错误 |
| `UnauthorizedAccessException` | 401 | 认证失败 |
| `InvalidOperationException` | 403 | 权限不足 |
| `KeyNotFoundException` | 404 | 资源不存在 |
| `InvalidCastException` | 409 | 数据冲突 |
| 其他 `Exception` | 500 | 服务器内部错误 |

### 使用示例

```csharp
throw new ArgumentException("参数无效");           // 400
throw new UnauthorizedAccessException("登录失败"); // 401
throw new InvalidOperationException("无权限");   // 403
throw new KeyNotFoundException("用户不存在");    // 409
throw new InvalidCastException("数据类型不匹配");    // 409
throw new Exception("系统错误");             // 500
```