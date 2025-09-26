CREATE TABLE students (
    id              BIGINT PRIMARY KEY AUTO_INCREMENT,
    record_book  VARCHAR(20) NOT NULL UNIQUE,
    full_name       VARCHAR(200) NOT NULL,
    birth_date      DATE NOT NULL,
    created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE teachers (
    id         INT PRIMARY KEY AUTO_INCREMENT,
    full_name  VARCHAR(200) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE subjects (
    id          INT PRIMARY KEY AUTO_INCREMENT,
    title       VARCHAR(200) NOT NULL,
    semester    TINYINT NOT NULL,
    teacher_id  INT NOT NULL,
    created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_subjects_teachers FOREIGN KEY (teacher_id) REFERENCES teachers(id) ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE grades (
    id           BIGINT PRIMARY KEY AUTO_INCREMENT,
    student_id   BIGINT NOT NULL,
    subject_id   INT NOT NULL,
    grade_value  TINYINT NOT NULL CHECK (grade_value BETWEEN 2 AND 5),
    exam_date    DATE NOT NULL,
    created_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_grades_unique_attempt UNIQUE (student_id, subject_id),
    CONSTRAINT fk_grades_students FOREIGN KEY (student_id) REFERENCES students(id) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_grades_subjects FOREIGN KEY (subject_id) REFERENCES subjects(id) ON UPDATE CASCADE ON DELETE RESTRICT
);
