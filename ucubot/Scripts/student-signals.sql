USE ucubot;
CREATE VIEW student_signals AS SELECT student.first_name, student.last_name,
(CASE lesson_signal.signal_type WHEN -1 THEN "Simple"
                                WHEN 0 THEN "Normal"
                                WHEN 1 THEN "Hard"
                                END) AS signal_type,
COUNT(lesson_signal.student_id) AS count
FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id
GROUP BY lesson_signal.signal_type, student.user_id;
