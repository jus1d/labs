USE sakila;

SET autocommit = 0;

-- Task 5: Transactions with ROLLBACK and COMMIT

DROP TABLE IF EXISTS test_isolation;

CREATE TABLE test_isolation (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50),
    balance DECIMAL(10,2)
) ENGINE=InnoDB;

INSERT INTO test_isolation (name, balance)
VALUES ('Account A', 1000.00),
       ('Account B', 2000.00);

COMMIT;

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


-- Task 6: READ UNCOMMITTED vs READ COMMITTED

-- READ UNCOMMITTED

START TRANSACTION;

UPDATE test_isolation SET balance = 1500.00 WHERE name = 'Account A';

SELECT * FROM test_isolation WHERE name = 'Account A';

-- S2
SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
START TRANSACTION;

SELECT * FROM test_isolation WHERE name = 'Account A';

COMMIT;


-- S1
ROLLBACK;

SELECT * FROM test_isolation WHERE name = 'Account A';


-- READ COMMITTED

-- S1
START TRANSACTION;

UPDATE test_isolation SET balance = 1500.00 WHERE name = 'Account A';

SELECT * FROM test_isolation WHERE name = 'Account A';


-- S2
SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED;
START TRANSACTION;

SELECT * FROM test_isolation WHERE name = 'Account A';

COMMIT;


-- S1
COMMIT;


-- S2
START TRANSACTION;

SELECT * FROM test_isolation WHERE name = 'Account A';

COMMIT;

UPDATE test_isolation SET balance = 1000.00 WHERE name = 'Account A';
COMMIT;


-- Task 7: READ COMMITTED vs REPEATABLE READ

-- READ COMMITTED

-- S1
SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED;
START TRANSACTION;

SELECT * FROM test_isolation WHERE name = 'Account B';


-- S2
START TRANSACTION;

UPDATE test_isolation SET balance = 2500.00 WHERE name = 'Account B';
COMMIT;


-- S1
SELECT * FROM test_isolation WHERE name = 'Account B';

COMMIT;

UPDATE test_isolation SET balance = 2000.00 WHERE name = 'Account B';
COMMIT;


-- REPEATABLE READ

-- S1
SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;
START TRANSACTION;

SELECT * FROM test_isolation WHERE name = 'Account B';


-- S2
START TRANSACTION;

UPDATE test_isolation SET balance = 2500.00 WHERE name = 'Account B';
COMMIT;


-- S1
SELECT * FROM test_isolation WHERE name = 'Account B';

COMMIT;

SELECT * FROM test_isolation WHERE name = 'Account B';

UPDATE test_isolation SET balance = 2000.00 WHERE name = 'Account B';
COMMIT;
