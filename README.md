# desafio-tech-ambev

# Subindo ambiente via docker
-- NetWork
docker network create -d bridge ambev-network

-- MySQL (apos subir o container rodar o arquite create-database.sql)
docker run --name mysql -e MYSQL_ROOT_PASSWORD=123456 -p 3306:3306 -v ~/docker --network=ambev-network -d mysql

--Apos criar o container do MySQL, rodar o comando abaixo para obter o IP de conexão ao banco 
--e adicionar este IP no arquivo appsettings.json

-- RabbitMQ
docker run -d --hostname localhost --name rabbit13 -p 15672:15672 -p 5672:5672 -p 25676:25676 --network=ambev-network rabbitmq:3-management

-- API (acessar a pasta onde está o projeto da API)
docker build -t devambev-api .
docker run --name devambev-api -p 8080:80 --network=ambev-network -d devambev-api

-- FRONT (acessar a pasta onde está o projeto do front)
docker build -t devambev.front .
docker run --name devambev-front -p 3000:3000 --network=ambev-network -d devambev.front
