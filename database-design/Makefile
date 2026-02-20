up:
	docker run --name seeql --rm \
		-e MYSQL_ALLOW_EMPTY_PASSWORD=yes \
		-e MYSQL_DATABASE=warehouse \
		-v mysql-data:/var/lib/mysql \
		-p 3306:3306 \
		-d mysql:latest

drop:
	docker rm -f seeql
	docker volume rm -f mysql-data

connect:
	docker exec -it seeql mysql -u root warehouse
