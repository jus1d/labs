server:
	docker run --name mysql-server --rm \
	    -e MYSQL_ROOT_PASSWORD=sexypass \
		-e MYSQL_DATABASE=db            \
		-e MYSQL_USER=ndbtea            \
		-e MYSQL_PASSWORD=sexypass      \
		-v mysql-data:/var/lib/mysql    \
		-p 3306:3306 -d mysql:latest

connect:
	docker exec -it mysql-server mysql -u root -p

drop:
	docker rm -f mysql-server
