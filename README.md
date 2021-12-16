# QUIZ APP API

## Technical

ASP.NET 5

## Usage

-   Download [Docker](https://www.docker.com/get-started)

-   Clone the Repo : `git clone https://github.com/lehuuhieu-1011/quiz-app-api-dotnet.git`

-   Change directory: `cd quiz-app-api-dotnet`

-   Build and run application: `docker-compose up`

-   Stop application, removing the containers entirely: `docker-compose down`

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

`docker run --name myredis -p 6379:6379 -d redis` : start server redis
