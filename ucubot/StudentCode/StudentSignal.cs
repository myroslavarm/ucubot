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
    public class StudentSignal
    {
        IEnumerable<StudentSignal> getStudentSignals(string conStr)
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

                string query = "SELECT id as Id, first_name as FirstName, " +
                               "last_name as LastName, user_id as UserId FROM student";
                var stdnt = connection.Query<StudentSignal>(query).ToList();
                return stdnt;
            }
        }
    }
}