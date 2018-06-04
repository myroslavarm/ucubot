using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.DBCode;
using ucubot.Model;

namespace ucubot.StudentCode
{
    public class StudentSignalRepo : IStudentSignal
    {
        public IEnumerable<StudentSignal> getStudentSignals(string conStr)
        {
            using (var connection = new MySqlConnection(conStr))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                string query = "SELECT student_signals.first_name first_name, " +
                  "student_signals.second_name LastName, student_signals.signal_type " +
                  "SignalType, student_signals.count Count FROM student_signals";
                var stdnt = connection.Query<StudentSignal>(query).ToList();
                return stdnt;
            }
        }
    }
}
