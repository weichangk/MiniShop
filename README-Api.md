# API


## 使用 dockerfile 构建部署

```shell
docker build -t minishopapi -f Dockerfile-Api .

docker run -d -p 5002:80 --restart=always -v D:/dockervolumes/minishopapi/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopapi/log:/app/log --name minishopapi minishopapi
```
使用 dockerfile 构建部署数据迁移在容器内运行不成功，要创建网络连接才可以，所以还是使用 docker-compose 管理网络。



## 使用 jenkins，使用 docker-compose 结合 .env 环境变量配置进行部署

```shell
docker-compose -f Docker-Compose-Api.yml -p minishopapi --env-file api.env  down
docker-compose -f Docker-Compose-Api.yml -p minishopapi --env-file api.env up --detach
#docker-compose -f Docker-Compose-Api.yml -p minishopapi --env-file apiprod.env  down
#docker-compose -f Docker-Compose-Api.yml -p minishopapi --env-file apiprod.env up --detach
```


