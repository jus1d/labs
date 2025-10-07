-- 1. students results by discipline
SELECT s.id AS student_id,
       s.full_name AS student_name,
       d.name AS discipline,
       d.total_hours,
       d.control_type,
       p.grade
FROM performance p
JOIN students s ON p.student_id = s.id
JOIN disciplines d ON p.discipline_id = d.id
WHERE s.id = 2;

-- 2. students results by disciplines and groups
SELECT g.name AS group_name,
       d.name AS discipline,
       s.full_name AS student_name,
       p.grade
FROM performance p
JOIN students s ON p.student_id = s.id
JOIN groops g ON s.groop_id = g.id
JOIN disciplines d ON p.discipline_id = d.id
ORDER BY g.name, d.name, s.full_name;

-- 3. groups with highest results
SELECT g.name AS group_name,
       AVG(p.grade) AS avg_grade
FROM performance p
JOIN students s ON p.student_id = s.id
JOIN groops g ON s.groop_id = g.id
WHERE p.grade IS NOT NULL
GROUP BY g.id, g.name
ORDER BY avg_grade DESC
LIMIT 1;

-- 4. disciplines with highest results
SELECT d.name AS discipline,
       AVG(p.grade) AS avg_grade
FROM performance p
JOIN disciplines d ON p.discipline_id = d.id
WHERE p.grade IS NOT NULL
GROUP BY d.id, d.name
ORDER BY avg_grade DESC
LIMIT 1;

-- 5. students' age with highest results
SELECT TIMESTAMPDIFF(YEAR, s.birth_date, CURDATE()) AS age,
       AVG(p.grade) AS avg_grade
FROM performance p
JOIN students s ON p.student_id = s.id
WHERE p.grade IS NOT NULL
GROUP BY age
ORDER BY avg_grade DESC
LIMIT 1;

-- 6. disciplines studied
SELECT DISTINCT d.name AS discipline,
       g.name AS group_name,
       g.course,
       g.semester
FROM performance p
JOIN students s ON p.student_id = s.id
JOIN groops g ON s.groop_id = g.id
JOIN disciplines d ON p.discipline_id = d.id
WHERE g.course = ? OR g.semester = ?;

-- 7. disciplines that are not studied by any students
SELECT d.name AS discipline
FROM disciplines d
LEFT JOIN performance p ON d.id = p.discipline_id
WHERE p.id IS NULL;
