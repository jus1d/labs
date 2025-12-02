-- 1. Display all customers from specified list of countries
SELECT
    c.first_name AS 'First Name',
    c.last_name AS 'Last Name',
    co.country AS 'Country'
FROM customer c
JOIN address a ON c.address_id = a.address_id
JOIN city ci ON a.city_id = ci.city_id
JOIN country co ON ci.country_id = co.country_id
WHERE co.country IN ('United States', 'Canada', 'Mexico')
ORDER BY co.country, c.last_name, c.first_name;


-- 2. Display all films featuring specified actor
SELECT DISTINCT
    f.title AS 'Film Title',
    cat.name AS 'Genre'
FROM film f
JOIN film_actor fa ON f.film_id = fa.film_id
JOIN actor a ON fa.actor_id = a.actor_id
JOIN film_category fc ON f.film_id = fc.film_id
JOIN category cat ON fc.category_id = cat.category_id
WHERE a.actor_id = 1  -- or: a.first_name = 'PENELOPE' AND a.last_name = 'GUINESS'
ORDER BY cat.name, f.title;


-- 3. Display top 10 film genres by revenue in specified month
SELECT
    cat.name AS 'Genre',
    SUM(p.amount) AS 'Revenue'
FROM payment p
JOIN rental r ON p.rental_id = r.rental_id
JOIN inventory i ON r.inventory_id = i.inventory_id
JOIN film f ON i.film_id = f.film_id
JOIN film_category fc ON f.film_id = fc.film_id
JOIN category cat ON fc.category_id = cat.category_id
WHERE YEAR(p.payment_date) = 2005
  AND MONTH(p.payment_date) = 5
GROUP BY cat.name
ORDER BY SUM(p.amount) DESC
LIMIT 10;


-- 4. Display list of 5 customers, ordered by number of rented films with specified actor, starting from position 10
SELECT
    c.first_name AS 'First Name',
    c.last_name AS 'Last Name',
    COUNT(DISTINCT r.rental_id) AS 'Number of Rented Films'
FROM customer c
JOIN rental r ON c.customer_id = r.customer_id
JOIN inventory i ON r.inventory_id = i.inventory_id
JOIN film f ON i.film_id = f.film_id
JOIN film_actor fa ON f.film_id = fa.film_id
WHERE fa.actor_id = 1
GROUP BY c.customer_id, c.first_name, c.last_name
ORDER BY COUNT(DISTINCT r.rental_id) DESC
LIMIT 5 OFFSET 9;


-- 5. Display for each store its city, country location and total revenue for the first week of sales
SELECT
    s.store_id AS 'Store ID',
    ci.city AS 'City',
    co.country AS 'Country',
    SUM(p.amount) AS 'First Week Revenue'
FROM store s
JOIN address a ON s.address_id = a.address_id
JOIN city ci ON a.city_id = ci.city_id
JOIN country co ON ci.country_id = co.country_id
LEFT JOIN inventory i ON s.store_id = i.store_id
LEFT JOIN rental r ON i.inventory_id = r.inventory_id
LEFT JOIN payment p ON r.rental_id = p.rental_id
WHERE p.payment_date >= (
    SELECT MIN(payment_date)
    FROM payment
)
AND p.payment_date < DATE_ADD(
    (SELECT MIN(payment_date) FROM payment),
    INTERVAL 7 DAY
)
GROUP BY s.store_id, ci.city, co.country
ORDER BY s.store_id;


-- 6. Display all actors for the film with highest revenue
SELECT
    f.title AS 'Film',
    a.first_name AS 'Actor First Name',
    a.last_name AS 'Actor Last Name'
FROM film f
JOIN film_actor fa ON f.film_id = fa.film_id
JOIN actor a ON fa.actor_id = a.actor_id
WHERE f.film_id = (
    SELECT f2.film_id
    FROM film f2
    JOIN inventory i ON f2.film_id = i.film_id
    JOIN rental r ON i.inventory_id = r.inventory_id
    JOIN payment p ON r.rental_id = p.rental_id
    GROUP BY f2.film_id
    ORDER BY SUM(p.amount) DESC
    LIMIT 1
)
ORDER BY a.last_name, a.first_name;


-- 7. For all customers display information about customers and actors with same last name (LEFT JOIN)
SELECT
    c.customer_id AS 'Customer ID',
    c.first_name AS 'Customer First Name',
    c.last_name AS 'Customer Last Name',
    a.actor_id AS 'Actor ID',
    a.first_name AS 'Actor First Name',
    a.last_name AS 'Actor Last Name'
FROM customer c
LEFT JOIN actor a ON c.last_name = a.last_name
ORDER BY c.last_name, c.first_name, a.first_name;


-- 8. For all actors display information about customers and actors with same last name (RIGHT JOIN)
SELECT
    c.customer_id AS 'Customer ID',
    c.first_name AS 'Customer First Name',
    c.last_name AS 'Customer Last Name',
    a.actor_id AS 'Actor ID',
    a.first_name AS 'Actor First Name',
    a.last_name AS 'Actor Last Name'
FROM customer c
RIGHT JOIN actor a ON c.last_name = a.last_name
ORDER BY a.last_name, a.first_name, c.first_name;


-- 9. In one query display statistical data about films
SELECT
    'Longest Film' AS 'Statistic',
    MAX(length) AS 'Value',
    COUNT(*) AS 'Number of Films'
FROM film
WHERE length = (SELECT MAX(length) FROM film)

UNION ALL

SELECT
    'Shortest Film' AS 'Statistic',
    MIN(length) AS 'Value',
    COUNT(*) AS 'Number of Films'
FROM film
WHERE length = (SELECT MIN(length) FROM film)

UNION ALL

SELECT
    'Maximum Number of Actors' AS 'Statistic',
    MAX(actor_count) AS 'Value',
    COUNT(*) AS 'Number of Films'
FROM (
    SELECT f.film_id, COUNT(fa.actor_id) AS actor_count
    FROM film f
    JOIN film_actor fa ON f.film_id = fa.film_id
    GROUP BY f.film_id
) AS film_actors
WHERE actor_count = (
    SELECT MAX(actor_count)
    FROM (
        SELECT COUNT(fa2.actor_id) AS actor_count
        FROM film f2
        JOIN film_actor fa2 ON f2.film_id = fa2.film_id
        GROUP BY f2.film_id
    ) AS max_actors
)

UNION ALL

SELECT
    'Minimum Number of Actors' AS 'Statistic',
    MIN(actor_count) AS 'Value',
    COUNT(*) AS 'Number of Films'
FROM (
    SELECT f.film_id, COUNT(fa.actor_id) AS actor_count
    FROM film f
    JOIN film_actor fa ON f.film_id = fa.film_id
    GROUP BY f.film_id
) AS film_actors
WHERE actor_count = (
    SELECT MIN(actor_count)
    FROM (
        SELECT COUNT(fa2.actor_id) AS actor_count
        FROM film f2
        JOIN film_actor fa2 ON f2.film_id = fa2.film_id
        GROUP BY f2.film_id
    ) AS min_actors
);
