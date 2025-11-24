USE sakila;

-- Task 5

SET autocommit = 0;

START TRANSACTION;

INSERT INTO actor (first_name, last_name)
VALUES ('John', 'Doe'),
       ('Jane', 'Smith'),
       ('Bob', 'Johnson');

SELECT * FROM actor WHERE first_name IN ('John', 'Jane', 'Bob') ORDER BY actor_id DESC;

ROLLBACK;

SELECT * FROM actor WHERE first_name IN ('John', 'Jane', 'Bob');

START TRANSACTION;

INSERT INTO actor (first_name, last_name)
VALUES ('John', 'Doe'),
       ('Jane', 'Smith'),
       ('Bob', 'Johnson');

SELECT * FROM actor WHERE first_name IN ('John', 'Jane', 'Bob') ORDER BY actor_id DESC;

COMMIT;

SELECT * FROM actor WHERE first_name IN ('John', 'Jane', 'Bob') ORDER BY actor_id DESC;

DELETE FROM actor WHERE first_name IN ('John', 'Jane', 'Bob');

COMMIT;

SET autocommit = 1;
