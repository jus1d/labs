execute:
	@docker exec -i seeql mysql -t -n discogs < ./execute.sql

testy:
	@docker exec -it seeql mysql -u testy -n discogs

sudo:
	@docker exec -it seeql mysql -u root -n discogs

.PHONY: execute testy sudo
