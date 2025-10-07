INSERT INTO groops (name, course, semester) VALUES
    ('CS-101', 1, 1),
    ('CS-102', 1, 2),
    ('CS-201', 2, 3),
    ('CS-202', 2, 4),
    ('CS-301', 3, 5),
    ('CS-302', 3, 6),
    ('CS-401', 4, 7),
    ('CS-402', 4, 8),
    ('IT-101', 1, 1),
    ('IT-201', 2, 3);

INSERT INTO students (full_name, gender, birth_date, groop_id) VALUES
    ('John Smith', 'M', '2005-03-15', 1),
    ('Mary Johnson', 'F', '2005-07-22', 1),
    ('Alex Brown', 'M', '2004-11-30', 2),
    ('Emma Wilson', 'F', '2004-05-18', 2),
    ('David Miller', 'M', '2003-09-12', 3),
    ('Anna Davis', 'F', '2003-12-25', 3),
    ('Steven Anderson', 'M', '2002-08-07', 4),
    ('Olivia Taylor', 'F', '2002-02-14', 5),
    ('Paul Martinez', 'M', '2005-06-20', 1),
    ('Tanya Robinson', 'F', '2004-10-03', 6);

INSERT INTO disciplines (name, total_hours, control_type) VALUES
    ('Calculus', 108, 'exam'),
    ('Linear Algebra', 72, 'exam'),
    ('Database Systems', 90, 'exam'),
    ('Programming', 108, 'exam'),
    ('Physics', 120, 'test'),
    ('English Language', 54, 'test'),
    ('Philosophy', 90, 'test'),
    ('Web Development', 108, 'project'),
    ('Discrete Mathematics', 96, 'exam'),
    ('History', 70, 'test');

INSERT INTO performance (student_id, discipline_id, grade) VALUES
    (1, 1, 5),
    (1, 2, 4),
    (1, 3, 5),
    (2, 1, 2),
    (2, 2, 3),
    (3, 1, 3),
    (3, 4, 4),
    (4, 3, 5),
    (5, 1, 5),
    (5, 5, NULL);
