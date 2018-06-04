using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.DBCode
{
    public interface IStudentRepository
    {
        IEnumerable<Student> getStudents(string conStr);
	    Student getStudent(string conStr, long id);
	    int insertStudent(string connStr, Student student);
	    int updStudent(string connStr, Student student);
	    int deleteStudent(string connStr, long id);
    }
}
