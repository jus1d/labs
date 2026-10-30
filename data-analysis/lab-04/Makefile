DB := poker.db

.PHONY: all clean

all: $(DB)

$(DB): dump.sql schema.sql
	rm -f $@
	sqlite3 $@ < dump.sql
	sqlite3 $@ "ALTER TABLE players RENAME TO poker_dataset;"
	sqlite3 $@ < schema.sql

clean:
	rm -f $(DB)
