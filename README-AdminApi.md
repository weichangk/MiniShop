# API


## 使用 dockerfile 构建部署

```shell
docker build -t minishopadminapi -f Dockerfile-AdminApi .

docker run -d -p 5004:80 --restart=always -v D:/dockervolumes/minishopadminapi/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopadminapi/log:/app/log --name minishopadminapi minishopadminapi
```
使用 dockerfile 构建部署数据迁移在容器内运行不成功，要创建网络连接才可以，所以还是使用 docker-compose 管理网络。



## 使用 jenkins，使用 docker-compose 结合 .env 环境变量配置进行部署

```shell
docker-compose -f Docker-Compose-AdminApi.yml -p minishopadminapi --env-file adminapi.env  down
docker-compose -f Docker-Compose-AdminApi.yml -p minishopadminapi --env-file adminapi.env up --detach
#docker-compose -f Docker-Compose-AdminApi.yml -p minishopadminapi --env-file adminapiprod.env  down
#docker-compose -f Docker-Compose-AdminApi.yml -p minishopadminapi --env-file adminapiprod.env up --detach
```

```shell
docker-compose -f Docker-Compose-AdminApi.yml -p minishopadminapi --env-file adminapi.env up --detach --scale web=6
```
通过 --scale 水平扩展怎么实现 consul 注册绑定指定端口？

