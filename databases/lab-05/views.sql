USE sakila;

-- Task 1: Non-updatable views with different algorithms

-- View 1: MERGE algorithm
DROP VIEW IF EXISTS actor_film_count;

CREATE ALGORITHM=MERGE VIEW actor_film_count AS
SELECT
    a.actor_id,
    a.first_name,
    a.last_name,
    COUNT(fa.film_id) AS film_count
FROM actor a
LEFT JOIN film_actor fa ON a.actor_id = fa.actor_id
GROUP BY a.actor_id, a.first_name, a.last_name;

SELECT * FROM actor_film_count ORDER BY film_count DESC LIMIT 10;

-- View 2: TEMPTABLE algorithm
DROP VIEW IF EXISTS film_category_stats;

CREATE ALGORITHM=TEMPTABLE VIEW film_category_stats AS
SELECT
    f.film_id,
    f.title,
    c.name AS category,
    f.rental_rate,
    (SELECT AVG(f2.rental_rate)
     FROM film f2
     INNER JOIN film_category fc2 ON f2.film_id = fc2.film_id
     WHERE fc2.category_id = c.category_id) AS avg_category_rental_rate
FROM film f
INNER JOIN film_category fc ON f.film_id = fc.film_id
INNER JOIN category c ON fc.category_id = c.category_id;

SELECT * FROM film_category_stats LIMIT 10;


-- Task 2: Updatable view that does NOT allow INSERT

-- Uses GROUP BY - prevents INSERT
DROP VIEW IF EXISTS customer_rental_count;

CREATE VIEW customer_rental_count AS
SELECT
    c.customer_id,
    c.first_name,
    c.last_name,
    c.email,
    COUNT(r.rental_id) AS rental_count
FROM customer c
LEFT JOIN rental r ON c.customer_id = r.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name, c.email;

SELECT * FROM customer_rental_count LIMIT 10;

UPDATE customer_rental_count
SET email = 'newemail@example.com'
WHERE customer_id = 1;

UPDATE customer_rental_count
SET email = (SELECT email FROM customer WHERE customer_id = 1)
WHERE customer_id = 1;

-- INSERT will fail due to GROUP BY
-- INSERT INTO customer_rental_count (customer_id, first_name, last_name, email, rental_count)
-- VALUES (999, 'Test', 'User', 'test@example.com', 0);


-- Task 3: Updatable view that allows INSERT

DROP VIEW IF EXISTS active_customers;

CREATE VIEW active_customers AS
SELECT
    customer_id,
    store_id,
    first_name,
    last_name,
    email,
    address_id,
    active
FROM customer
WHERE active = TRUE;

SELECT * FROM active_customers LIMIT 10;

INSERT INTO active_customers (store_id, first_name, last_name, email, address_id, active, create_date)
VALUES (1, 'Ivan', 'Ivanov', 'ivan.ivanov@test.com', 1, TRUE, NOW());

SELECT * FROM active_customers WHERE first_name = 'Ivan' AND last_name = 'Ivanov';

DELETE FROM customer WHERE first_name = 'Ivan' AND last_name = 'Ivanov' AND email = 'ivan.ivanov@test.com';


-- Task 4: Nested updatable view with WITH CHECK OPTION

-- Base view: films with PG or PG-13 rating
DROP VIEW IF EXISTS pg_films;

CREATE VIEW pg_films AS
SELECT
    film_id,
    title,
    description,
    release_year,
    language_id,
    rental_duration,
    rental_rate,
    length,
    replacement_cost,
    rating
FROM film
WHERE rating IN ('PG', 'PG-13')
WITH CHECK OPTION;

-- Nested view: PG-13 films with rental_rate < 3.00
DROP VIEW IF EXISTS affordable_pg13_films;

CREATE VIEW affordable_pg13_films AS
SELECT
    film_id,
    title,
    description,
    release_year,
    rental_rate,
    rating
FROM pg_films
WHERE rating = 'PG-13' AND rental_rate < 3.00
WITH CHECK OPTION;

SELECT * FROM affordable_pg13_films LIMIT 10;

-- These UPDATEs will fail due to CHECK OPTION:
-- UPDATE affordable_pg13_films SET rental_rate = 4.99 WHERE film_id = ...;
-- UPDATE affordable_pg13_films SET rating = 'R' WHERE film_id = ...;

-- Valid UPDATE
UPDATE affordable_pg13_films
SET rental_rate = 2.99
WHERE film_id = (SELECT film_id FROM affordable_pg13_films LIMIT 1);

UPDATE film
SET rental_rate = (SELECT rental_rate FROM film WHERE film_id = (SELECT film_id FROM affordable_pg13_films LIMIT 1))
WHERE film_id = (SELECT film_id FROM affordable_pg13_films LIMIT 1);


-- View metadata check
SELECT
    TABLE_NAME,
    VIEW_DEFINITION,
    CHECK_OPTION,
    IS_UPDATABLE
FROM information_schema.VIEWS
WHERE TABLE_SCHEMA = 'sakila'
AND TABLE_NAME IN ('actor_film_count', 'film_category_stats', 'customer_rental_count',
                   'active_customers', 'pg_films', 'affordable_pg13_films');
