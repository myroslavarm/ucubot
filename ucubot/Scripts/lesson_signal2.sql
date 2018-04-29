ALTER TABLE lesson_signal DROP COLUMN user_id;
delete from lesson_signal;
ALTER TABLE lesson_signal ADD COLUMN student_id INT;
ALTER TABLE lesson_signal ADD CONSTRAINT FOREIGN KEY (student_id) REFERENCES student(id) ON UPDATE RESTRICT ON DELETE RESTRICT;
