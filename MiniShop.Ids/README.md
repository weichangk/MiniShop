# IDS



#### 创建镜像

```shell
docker build -t minishopids -f Dockerfile-Ids .
```



#### 启动容器

执行脚本前需要提前准备 `D:/dockervolumes/minishopidsdata/appsettings.json` 文件

```shell
docker run -d -p 8000:80 --restart=always -v D:/dockervolumes/minishopidsdata/appsettings.json:/app/appsettings.json -v D:/dockervolumes/minishopidsdata/log:/app/log --name minishopids minishopids
```



