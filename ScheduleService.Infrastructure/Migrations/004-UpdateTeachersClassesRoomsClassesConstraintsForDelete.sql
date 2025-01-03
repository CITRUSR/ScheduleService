ALTER TABLE teachers_classes
DROP CONSTRAINT teachers_classes_class_fk_fkey;

ALTER TABLE teachers_classes
ADD CONSTRAINT teachers_classes_class_fk_fkey FOREIGN KEY (class_fk) REFERENCES classes (id) ON DELETE CASCADE;

ALTER TABLE classes_rooms
DROP CONSTRAINT classes_rooms_class_fk_fkey;

ALTER TABLE classes_rooms
ADD CONSTRAINT classes_rooms_class_fk_fkey FOREIGN KEY (class_fk) REFERENCES classes (id) ON DELETE CASCADE;