-- 1 --
SELECT full_name AS student_name, gender AS sex, YEAR(CURDATE()) - YEAR(birth_date) AS age FROM students
ORDER BY age DESC, full_name ASC;


-- 2 --
SELECT groop_id, COUNT(*) AS students_count, AVG(YEAR(CURDATE()) - YEAR(birth_date)) AS avg_age FROM students
GROUP BY groop_id ORDER BY students_count DESC;


-- 3 --
SELECT DISTINCT course FROM groops ORDER BY course;


-- 4 --
SELECT full_name AS student_name FROM students WHERE YEAR(birth_date) >= 2004 AND gender = 'M';


-- 5 --
SELECT name, control_type FROM disciplines WHERE control_type IN ('exam', 'project');


SELECT full_name, birth_date FROM students WHERE birth_date BETWEEN '2003-01-01' AND '2005-12-31';


SELECT full_name FROM students WHERE full_name LIKE '%John%';


SELECT s.full_name, d.name AS discipline, p.grade FROM performance p
JOIN students s ON p.student_id = s.id
JOIN disciplines d ON p.discipline_id = d.id
WHERE p.grade IS NULL;


-- 6 --
SELECT
    groop_id,
    COUNT(*) AS student_count,
    AVG(YEAR(CURDATE()) - YEAR(birth_date)) AS avg_age
FROM students
GROUP BY groop_id
HAVING COUNT(*) >= 2;


SELECT
    d.name AS discipline,
    COUNT(p.id) AS exam_count,
    AVG(p.grade) AS avg_grade,
    SUM(d.total_hours) AS total_hours
FROM performance p
JOIN disciplines d ON p.discipline_id = d.id
WHERE p.grade IS NOT NULL
GROUP BY d.id, d.name
HAVING AVG(p.grade) >= 4.0;


SELECT
    discipline_id,
    MAX(grade) AS max_grade,
    MIN(grade) AS min_grade,
    COUNT(*) AS total_students
FROM performance
WHERE grade IS NOT NULL
GROUP BY discipline_id
HAVING COUNT(*) > 1;


-- 7 --
SELECT s.full_name, s.gender, g.name AS group_name, g.course, g.semester FROM students s
JOIN groops g ON s.groop_id = g.id
ORDER BY g.course DESC, g.semester DESC, s.full_name ASC;


-- 8 --
SELECT
    s.full_name,
    g.name AS group_name,
    d.name AS discipline,
    p.grade,
    d.total_hours
FROM students s
INNER JOIN groops g ON s.groop_id = g.id
LEFT JOIN performance p ON s.id = p.student_id
LEFT JOIN disciplines d ON p.discipline_id = d.id
ORDER BY s.full_name, d.name;


-- 9 --
SELECT
    s.full_name AS name,
    'High Performer' AS category,
    AVG(p.grade) AS avg_grade
FROM students s
JOIN performance p ON s.id = p.student_id
WHERE p.grade IS NOT NULL
GROUP BY s.id, s.full_name
HAVING AVG(p.grade) >= 4.5

UNION

SELECT
    s.full_name AS name,
    'Low Performer' AS category,
    AVG(p.grade) AS avg_grade
FROM students s
JOIN performance p ON s.id = p.student_id
WHERE p.grade IS NOT NULL
GROUP BY s.id, s.full_name
HAVING AVG(p.grade) < 3.5
ORDER BY category, avg_grade DESC;


-- 10 --
SELECT full_name, birth_date FROM students
WHERE groop_id IN (SELECT id FROM groops WHERE course >= 2);


SELECT
    d.name AS discipline,
    AVG(p.grade) AS avg_grade,
    COUNT(*) AS student_count
FROM performance p
JOIN disciplines d ON p.discipline_id = d.id
WHERE p.grade IS NOT NULL
GROUP BY d.id, d.name
HAVING AVG(p.grade) >= (
    SELECT AVG(grade)
    FROM performance
    WHERE grade IS NOT NULL
);


-- 11 --
SELECT s.full_name, g.name AS group_name FROM students s
JOIN groops g ON s.groop_id = g.id
WHERE EXISTS (
    SELECT 1 FROM performance p WHERE p.student_id = s.id AND p.grade = 5
);


SELECT
    d.name AS discipline,
    AVG(p.grade) AS avg_grade
FROM performance p
JOIN disciplines d ON p.discipline_id = d.id
WHERE p.grade IS NOT NULL
GROUP BY d.id, d.name
HAVING AVG(p.grade) >= ALL (
    SELECT AVG(grade)
    FROM performance
    WHERE grade IS NOT NULL
    GROUP BY discipline_id
);


SELECT s.full_name, s.birth_date FROM students s
WHERE s.groop_id = ANY (SELECT id FROM groops WHERE course IN (1, 2));
