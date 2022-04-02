# API


## 使用 dockerfile 构建部署

```shell
docker build -t minishopmvc -f Dockerfile-Mvc .

docker run -d -p 5003:80 --restart=always -v D:/dockervolumes/minishopmvc/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopmvc/log:/app/log --name minishopmvc minishopmvc
```




## 使用 jenkins，使用 docker-compose 结合 .env 环境变量配置进行部署

```shell
docker-compose -f Docker-Compose-Mvc.yml -p minishopmvc --env-file mvc.env  down
docker-compose -f Docker-Compose-Mvc.yml -p minishopmvc --env-file mvc.env up --detach
#docker-compose -f Docker-Compose-Mvc.yml -p minishopmvc --env-file mvcprod.env  down
#docker-compose -f Docker-Compose-Mvc.yml -p minishopmvc --env-file mvcprod.env up --detach
```


