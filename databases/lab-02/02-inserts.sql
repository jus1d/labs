INSERT INTO teacher (full_name) VALUES
    ('Ivanov Petr Sergeevich'),
    ('Sidorova Anna Mikhailovna'),
    ('Pavlov Ilya Andreevich');

INSERT INTO student (record_book_no, full_name, birth_date) VALUES
    ('RB001', 'Ivanov Ivan Igorevich', '2004-03-12'),
    ('RB002', 'Petrova Maria Olegovna', '2003-11-05'),
    ('RB003', 'Ivashchenko Darya Pavlovna', '2004-07-21'),
    ('RB004', 'Ivanchik Stepan Olegovich', '2005-01-30'),
    ('RB005', 'Smirnov Alexey Viktorovich', '2004-09-09');

INSERT INTO subject (title, semester, teacher_id) VALUES
    ('Mathematical Analysis', 1, 1),
    ('Linear Algebra', 1, 1),
    ('Programming', 1, 2),
    ('Databases', 2, 2),
    ('Discrete Mathematics', 2, 3);

INSERT INTO grade (student_id, subject_id, grade_value, exam_date) VALUES
    (1, 1, 5, '2025-06-10'),
    (1, 2, 4, '2025-06-12'),
    (1, 3, 5, '2025-06-15'),
    (2, 1, 3, '2025-06-10'),
    (2, 2, 5, '2025-06-12'),
    (2, 3, 5, '2025-06-15'),
    (3, 1, 5, '2025-06-11'),
    (3, 2, 5, '2025-06-13'),
    (3, 3, 4, '2025-06-16'),
    (4, 1, 4, '2025-06-11'),
    (4, 2, 4, '2025-06-13'),
    (4, 3, 5, '2025-06-16'),
    (5, 1, 5, '2025-06-11'),
    (5, 2, 3, '2025-06-13'),
    (5, 3, 5, '2025-06-16');
