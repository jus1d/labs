.PHONY: clean

poker.db: dump.sql schema.sql
	rm -f poker.db
	sqlite3 poker.db < dump.sql
	# sqlite3 poker.db "ALTER TABLE players RENAME TO poker_dataset;"
	sqlite3 poker.db < schema.sql

clean:
	rm -f *.db
