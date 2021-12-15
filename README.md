# QUIZ APP API

## Technical

ASP.NET 5

## Usage

#### Run Server Redis

```
docker pull redis
docker run --name myredis -p 6379:6379 -d redis
```

## Note

#### Commands docker

`docker pull redis` : download image redis in docker

`docker image ls -a` : list all image in docker

`docker ps` : list all container running

`docker run —name myredis -p 6379:6379 -d redis` : start redis

`docker exec -it myredis sh` : `redis-cli` : access redis cli

`docker stop myredis` : stop container

`docker rm myredis` : remove container

`docker rmi redis` : remove image
