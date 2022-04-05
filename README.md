

## 使用 jenkins，使用 docker-compose 结合 .env 环境变量配置进行部署

```shell
docker-compose -f Docker-Compose.yml -p minishop --env-file .env  down
docker-compose -f Docker-Compose.yml -p minishop --env-file .env up --detach
#docker-compose -f Docker-Compose.yml -p minishop --env-file minishopprod.env  down
#docker-compose -f Docker-Compose.yml -p minishop --env-file minishopprod.env up --detach
```

## 注意
在 windows 使用 docker desktop 使用容器部署时，各个容器挂载的 appsettings.json 中的 localhost 应该替换为 host.docker.internal。并且在浏览器打开应用时也用 host.docker.internal:5003 打开

docker 容器中的 localhost 是容器自身，容器要访问宿主机的话用 http://host.docker.internal，这个域名是 docker 安装时自动写到你的 hosts 文件中的，对应的就是你宿主机的ip

确保 Issuer 的验证是正确匹配的。iss 是 OpenId Connect（后文简称OIDC）协议中定义的一个字段，其全称为 “Issuer Identifier”，中文意思就是：颁发者身份标识，表示 Token 颁发者的唯一标识，一般是一个 http(s) url，如 https://www.baidu.com。在 Token 的验证过程中，会将它作为验证的一个阶段，如无法匹配将会造成验证失败，最后返回 HTTP 401。

 有关 Issuer 参考：
 - https://www.cnblogs.com/stulzq/p/10339024.html
 - https://stackoverflow.com/questions/64809717/azure-net-core-app-with-is4-web-api-call-fails-with-bearer-error-invalid-toke

由于没有使用健康检查，首次构建时由于数据库服务没有完成其他服务进行了数据库迁移失败导致服务启动失败，需要手动 restart。

 ## 使用 Ocelot 网关
