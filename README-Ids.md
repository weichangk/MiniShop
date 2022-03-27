# IDS



## 使用 dockerfile 构建部署
#### 创建镜像

```shell
docker build -t minishopids -f Dockerfile-Ids .
```

#### 启动容器

执行脚本前需要提前准备宿主机挂载文件(否则构建容器无法启动) `D:/dockervolumes/minishopids/appsettings.json` 宿主机挂载配置文件是生产环境配置

```shell
docker run -d -p 8000:80 --restart=always -v D:/dockervolumes/minishopids/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopids/log:/app/log --name minishopids minishopids
```

#### 数据库迁移，初始化种子数据问题
先手动执行数据库迁移 `dotnet ef database update` 再构建启动容器。

也可以使用 ef core 类库 `Pomelo.EntityFrameworkCore.MySql` 方法 `context.Database.Migrate();` 实现项目运行即实现数据库迁移。

发现使用 `context.Database.Migrate();` 在本地运行程序是可以执行迁移的，但是使用 docker 运行时数据库迁移不成功！！！
好像是ssl的问题，DBeaver 连接 mysql:8.0 要配置使用ssl才能连接，而 mysql:8.0.0 则不需要。
使用 docker 运行时数据库迁移不成功的问题后来测试使用 `mysql:8.0` 镜像需要配置自定义网络来连通才能正常进行数据库迁移，换成 `mysql:8.0.0` 则不需要配置自定义网络。
好像与 sdk 也有关系，之前使用的是 `mcr.microsoft.com/sdk:3.1` 由于下载问题 换成了 `registry.cn-hangzhou.aliyuncs.com/newbe36524/aspnet:3.1-buster-slim`，发现与 `mysql:8.0.0` 也不能不通过自定义网络直接连接了，所以还是建议使用使用 docker-compose 管理网络。

如果有 `ssl_choose_client_version:unsupported protocol` 是 openssl 返回的错误，可以在 Dockerfile 中修改 `RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf` 参考：https://q.cnblogs.com/q/115080/


## 使用 docker-compose 构建部署
上面使用 dockerfile 直接构建部署的方式，是 mysql 容器服务已经存在的情况下实现的，通过配置文件中的数据库连接字符串进行进行服务连接。但是发现如果不创建网络实现 ids 容器与 mysql 容器连接的话，会出现容器间无法连接的问题。索引应该使用 docker-compose 构建部署则统一管理 mysql容器服务、项目容器服务、项目数据库数据迁移容器服务（将数据库迁移独立出来，可实现按需迁移数据库，不需要每次构建都迁移）。
使用 docker-compose 定义环境变量配置数据库连接字符串取代 appsettings.json 的数据库连接字符串配置。

#### 健康检查
为了确保数据库服务启动完成才能执行数据库迁移

安装 wait-for-it，文件存在 node_modules 文件夹中
```shell
npm install wait-for-it.sh@1.0.0
```

注意：node_modules 文件包已忽略上传到仓库，由 package.json 来管理第三方包，通过以下指令再次安装到本地
```shell
npm install --production
```

修改 dockerfile (Dockerfile-Ids-UseCompose)
```shell
COPY MiniShop.Ids/node_modules/wait-for-it.sh/bin/wait-for-it /app/wait-for-it.sh
RUN chmod +x /app/wait-for-it.sh

ENV WAITHOST=db WAITPORT=3306

ENTRYPOINT ./wait-for-it.sh ${WAITHOST}:${WAITPORT} --timeout=0 && exec dotnet MiniShop.Ids.dll
```

#### 启动容器
```shell
# 构建
docker-compose -f Docker-Compose-Ids.yml build 
# 启动
docker-compose -f Docker-Compose-Ids.yml up 
# 删除
docker-compose -f Docker-Compose-Ids.yml down 

# -p minishopids 指定前缀
docker-compose -f Docker-Compose-Ids.yml -p minishopids build 
docker-compose -f Docker-Compose-Ids.yml -p minishopids up
docker-compose -f Docker-Compose-Ids.yml -p minishopids down

# 启动、删除某个服务
docker-compose -f Docker-Compose-Ids.yml -p minishopids up dbinit
docker-compose -f Docker-Compose-Ids.yml -p minishopids down dbinit
```

jenkins 简单构建脚本
```shell
docker-compose -f Docker-Compose-Ids.yml -p minishopids down
docker-compose -f Docker-Compose-Ids.yml -p minishopids build
docker-compose -f Docker-Compose-Ids.yml -p minishopids up
```