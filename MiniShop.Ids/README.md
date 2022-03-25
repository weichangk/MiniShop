# IDS



## 使用 dockerfile 构建部署
#### 创建镜像

```shell
docker build -t minishopids -f MiniShop.Ids/Dockerfile .
```

#### 启动容器

执行脚本前需要提前准备宿主机挂载文件(否则构建容器无法启动) `D:/dockervolumes/minishopidsdata/appsettings.json` 宿主机挂载配置文件是生产环境配置

```shell
docker run -d -p 8000:80 --restart=always -v D:/dockervolumes/minishopidsdata/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopidsdata/log:/app/log --name minishopids minishopids
```

#### 数据库迁移，初始化种子数据问题
使用 `context.Database.Migrate();` 实现项目运行即实现数据库迁移的方法，在本地运行程序是可以执行的，但是使用docker运行时数据库迁移不成功！！！
这个问题暂时没有找到办法，使用 dockerfile 构建部署只能屏蔽掉 `context.Database.Migrate();` 先手动执行数据库迁移 `dotnet ef database update` 再构建启动容器。



## 使用 docker-compose 构建部署
上面使用 dockerfile 构建部署的方式，是 mysql 容器服务已经存在的情况下实现的，通过配置文件中的数据库连接字符串进行进行服务连接。
使用 docker-compose 构建部署则同一管理 mysql容器服务、项目数据库数据迁移容器服务、项目容器服务

#### 健康检查

安装 wait-for-it，文件存在 node_modules 文件夹中
```shell
npm install wait-for-it.sh@1.0.0
```

注意：node_modules 文件包已忽略上传到仓库，由 package.json 来管理第三方包，通过以下指令再次安装到本地
```shell
npm install --production
```

修改 dockerfile

```shell
COPY MiniShop.Ids/node_modules/wait-for-it.sh/bin/wait-for-it /app/wait-for-it.sh
RUN chmod +x /app/wait-for-it.sh

ENV WAITHOST=db WAITPORT=3306

ENTRYPOINT ["./wait-for-it.sh ${WAITHOST}:${WAITPORT} --timeout=0 \ && exec dotnet MiniShop.Ids.dll"]
```

