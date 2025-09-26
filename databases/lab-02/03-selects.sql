-- 1. Select all subjects
SELECT id, title, semester FROM subject ORDER BY title ASC;

-- 2. Count all students
SELECT COUNT(*) AS student_count FROM student;

-- 3. Select all students with name starts with 'Iva'
SELECT record_book_no, full_name FROM student WHERE full_name LIKE 'Iva%';

-- 4. Select all students with grade 5
SELECT s.full_name FROM grade g
JOIN student s ON s.id = g.student_id
JOIN subject sub ON sub.id = g.subject_id
WHERE g.grade_value = 5 AND sub.id = 1 ORDER BY s.full_name DESC;

-- 5. Select all students with all 3/3 grades and total points greater than 12
SELECT s.record_book_no, s.full_name, SUM(g.grade_value) AS total_points FROM student s
JOIN grade g ON g.student_id = s.id WHERE g.subject_id IN (1,2,3)
GROUP BY s.record_book_no, s.full_name
HAVING COUNT(DISTINCT g.subject_id) = 3 AND SUM(g.grade_value) > 12
ORDER BY total_points DESC, s.full_name ASC;
