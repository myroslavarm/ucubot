using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ninject.Infrastructure.Language;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;

namespace ucubot.DBCode
{
    public class ClassLesson : ILessonRepository
    {
        private MySqlConnection openConnection(string connStr)
        {
            using (var connection = new MySqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return connection;
            }
        }

        public IEnumerable<LessonSignalDto> getLessons(string conStr)
        {
            var connection = this.openConnection(conStr);
            string query = "SELECT lesson_signal.id as Id, lesson_signal.time_stamp as Timestamp, " +
                           "lesson_signal.signal_type as Type, student.user_id as UserId " +
                           "FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id;";

            var lsnSign = connection.Query<LessonSignalDto>(query).ToList();
            return lsnSign;
        }

        public LessonSignalDto getLesson(string conStr, long id)
        {
            var connection = this.openConnection(conStr);
            string query = "SELECT lesson_signal.id as Id, lesson_signal.time_stamp as Timestamp, " +
                           "lesson_signal.signal_type as Type, student.user_id as UserId " +
                           "FROM lesson_signal LEFT JOIN student ON lesson_signal.student_id = student.id where lesson_signal.id = @id";

            var newCommand = new MySqlCommand(query).Parameters.AddWithValue("id", id);
            var lsnSign = connection.Query<LessonSignalDto>(newCommand.ToString()).ToList();
            return lsnSign.First();
        }

        public int insertLesson(string connStr, SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();
            var connection = this.openConnection(connStr);

            var query = @"SELECT * FROM student WHERE user_id=@id";
            var cnt = connection.Query<Student>(query, new {id = userId}).SingleOrThrowException(() => { return null; });

            if (cnt == null) { return 404; }
            var command = new MySqlCommand("INSERT INTO lesson_signal (student_id, signal_type) VALUES (@userId, @signalType)",
                connection);
            command.Parameters.AddRange(new[]
            {
                new MySqlParameter("userId", cnt.Id),
                new MySqlParameter("signalType", signalType)
            });
            command.ExecuteNonQuery();
            return 200;
        }

        public int deleteLesson(string connStr, long id)
        {
            var connection = this.openConnection(connStr);
            var command = connection.CreateCommand();
            command.CommandText =
                "DELETE FROM lesson_signal WHERE ID = @id;";
            command.Parameters.Add(new MySqlParameter("id", id));
            command.ExecuteNonQuery();
            return 200;
        }
    }
}
