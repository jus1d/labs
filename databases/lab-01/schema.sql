CREATE TABLE groops (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    course INT NOT NULL,
    semester INT NOT NULL
);

CREATE TABLE students (
    id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(150) NOT NULL,
    gender ENUM('M', 'F') NOT NULL,
    birth_date DATE NOT NULL,
    groop_id INT,
    CONSTRAINT fk_students_groops FOREIGN KEY (groop_id) REFERENCES groops(id)
);

CREATE TABLE disciplines (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
    control_type ENUM('exam','test','project') NOT NULL,
);

CREATE TABLE performance (
    id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    discipline_id INT NOT NULL,
    total_hours INT NOT NULL,
    grade INT,
    CONSTRAINT fk_performance_student FOREIGN KEY (student_id) REFERENCES students(id),
    CONSTRAINT fk_performance_discipline FOREIGN KEY (discipline_id) REFERENCES disciplines(id)
);
