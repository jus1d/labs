server:
	docker run --name mysql-server --rm \
	    -e MYSQL_ALLOW_EMPTY_PASSWORD=yes \
	    -e MYSQL_DATABASE=db \
	    -e MYSQL_USER=ndbtea \
	    -e MYSQL_PASSWORD=sexypass \
	    -v mysql-data:/var/lib/mysql \
	    -p 3306:3306 -d mysql:latest

connect:
	docker exec -it mysql-server mysql -u root

sakila:
	docker exec -i mysql-server mysql -u root < ./sakila/01-schema.sql
	docker exec -i mysql-server mysql -u root < ./sakila/02-insert-data.sql

lab05:
	docker exec -i mysql-server mysql -u root < ./01-schema.sql
	docker exec -i mysql-server mysql -u root < ./02-views.sql
	# docker exec -i mysql-server mysql -u root < ./03-transactions.sql

lab05-connect:
	docker exec -it mysql-server mysql -u root -D lab05_db

schema:
	docker exec -i mysql-server mysql -u root < ./01-schema.sql

views:
	docker exec -i mysql-server mysql -u root < ./02-views.sql

transactions:
	docker exec -i mysql-server mysql -u root < ./03-transactions.sql

stop:
	docker rm -f mysql-server

drop:
	docker volume rm -f mysql-data
