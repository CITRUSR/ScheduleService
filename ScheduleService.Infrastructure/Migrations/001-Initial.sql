CREATE TABLE rooms (
    id SERIAL PRIMARY KEY,
    name VARCHAR(10) NOT NULL UNIQUE,
    full_name VARCHAR(128)
);

CREATE TABLE subjects (
    id SERIAL PRIMARY KEY,
    name VARCHAR(10) NOT NULL UNIQUE,
    abbreviation VARCHAR(128)
);

CREATE TABLE weekdays (
    id SERIAL PRIMARY KEY,
    name VARCHAR(10) NOT NULL UNIQUE
);

CREATE TABLE colors (
    id SERIAL PRIMARY KEY,
    name VARCHAR(10) NOT NULL UNIQUE
);

CREATE TABLE classes (
    id SERIAL PRIMARY KEY,
    group_fk INTEGER NOT NULL,
    subject_fk INTEGER NOT NULL,
    weekday_fk INTEGER NOT NULL,
    color_fk INTEGER,
    starts_at TIME NOT NULL,
    ends_at TIME NOT NULL,
    change_on DATE,
    irrelevant_since DATE NULL,
    FOREIGN KEY (subject_fk) REFERENCES subjects (id),
    FOREIGN KEY (color_fk) REFERENCES colors (id),
    FOREIGN KEY (weekday_fk) REFERENCES weekdays
);

CREATE TABLE teachers_classes (
    class_fk INTEGER NOT NULL,
    teacher_fk UUID NOT NULL,
    FOREIGN KEY (class_fk) REFERENCES classes (id)
);

CREATE TABLE classes_rooms (
    class_fk INTEGER NOT NULL,
    room_fk INTEGER NOT NULL,
    FOREIGN KEY (class_fk) REFERENCES classes (id),
    FOREIGN KEY (room_fk) REFERENCES rooms (id)
);

CREATE TABLE current_weekday (
    id SERIAL PRIMARY KEY,
    color VARCHAR(10) NOT NULL,
    interval INTERVAL NOT NULL,
    updated_at TIMESTAMP
);

CREATE TABLE specialities_teachers_subjects (
    speciality_fk INTEGER,
    course INTEGER,
    subgroup INTEGER,
    PRIMARY KEY (
        speciality_fk,
        course,
        subgroup
    ),
    teacher_fk UUID NOT NULL,
    subject_fk INTEGER NOT NULL,
    UNIQUE (teacher_fk, subject_fk),
    FOREIGN KEY (subject_fk) REFERENCES subjects (id)
);