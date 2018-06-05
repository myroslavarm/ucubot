using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.DBCode
{
    public interface IStudentRepository
    {
      IEnumerable<Student> getStudents();
	    Student getStudent(long id);
	    int insertStudent(Student student);
	    int updStudent(Student student);
	    int deleteStudent(long id);
    }
}
