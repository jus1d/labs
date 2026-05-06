

Create user

```sql
CREATE USER 'testy'@'local' IDENTIFIED BY '';
```

```sql
GRANT USAGE ON discogs.* TO 'testy'@'localhost';
GRANT SHOW VIEW ON discogs.* TO 'testy'@'localhost';
```

```sql
GRANT SELECT ON discogs.artist TO 'testy';
```

```sql
REVOKE SELECT ON discogs.artist FROM 'testy';
```

---

Connect: `make sudo`, `make testy`.
