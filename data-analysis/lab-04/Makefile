.PHONY: clean

pokers.db: dump.sql schema.sql
	rm -f pokers.db
	sqlite3 pokers.db < dump.sql
	sqlite3 pokers.db < schema.sql

clean:
	rm -f *.db
