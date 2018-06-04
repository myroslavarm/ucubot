using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.DBCode
{
    public class ClassStudent : IStudentRepository
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

	    public IEnumerable<Student> getStudents(string conStr)
	    {
		    var connection = this.openConnection(conStr);

			string query = "SELECT id as Id, first_name as FirstName, " +
						   "last_name as LastName, user_id as UserId FROM student";
			var stdnt = connection.Query<Student>(query).ToList();
			return stdnt;

		}
		public Student getStudent(string conStr, long id){
			var connection = this.openConnection(conStr);

			string query = "SELECT id as Id, first_name as FirstName, " +
			               "last_name as LastName, user_id as UserId FROM student WHERE Id='"+id+"'";

			var stdnt = connection.Query<Student>(query, new {Id = id}).SingleOrDefault();
			return stdnt;
		}

	    public int insertStudent(string connStr, Student student)
	    {
		    var userId = student.UserId;
		    var firstName = student.FirstName;
		    var lastName = student.LastName;
		    var connection = new MySqlConnection(connStr);
		    try
		    {
            connection.Open();
  			    var cnt = new MySqlCommand("SELECT COUNT(*) FROM student WHERE user_id=@user_id", connection);
  			    cnt.Parameters.AddRange(new[]
  			    {
  				    new MySqlParameter("user_id", userId)
  			    });
  			    if (int.Parse(cnt.ExecuteScalar().ToString()) > 0)
  			    {
                connection.Close();
  				      return 409;
  			    }

			    cnt = new MySqlCommand("INSERT INTO student (user_id, first_name, last_name) " +
			                           "VALUES (@UserId, @FirstName, @LastName)", connection);
			    cnt.Parameters.AddRange(new[]
			    {
          		new MySqlParameter("UserId", userId),
          		new MySqlParameter("FirstName", firstName),
          		new MySqlParameter("LastName", lastName)
			    });
			    cnt.ExecuteNonQuery();
		    }
		    catch (Exception e)
		    {
			       connection.Close();
             return 409;
		    }
        connection.Close();
		    return 200;
	    }

	    public int updStudent(string connStr, Student student)
	    {
		    var userId = student.UserId;
		    var firstName = student.FirstName;
		    var lastName = student.LastName;
		    var id = student.Id;
		    var connection = new MySqlConnection(connStr);
		    try{
			    connection.Open();
			    var cnt = new MySqlCommand("UPDATE student set user_id=@UserId, first_name=@FirstName, " +
			                               "last_name=@LastName where id=@Id", connection);
			    cnt.Parameters.AddRange(new[]
			    {
				    new MySqlParameter("UserId", userId),
				    new MySqlParameter("FirstName", firstName),
				    new MySqlParameter("LastName", lastName),
				    new MySqlParameter("Id", id)
			    });
			    cnt.ExecuteNonQuery();
		    }
		    catch (Exception e)
		    {
          connection.Close();
          return 409;
		    }
        connection.Close();
		    return 200;
	    }

	    public int deleteStudent(string connStr, long id)
	    {
		    var connection = new MySqlConnection(connStr);
		    try{
			    connection.Open();
			    var cnt = new MySqlCommand("DELETE FROM student WHERE id = @id", connection);
			    cnt.Parameters.Add(new MySqlParameter("id", id));
			    cnt.ExecuteNonQuery();
		    }
		    catch (Exception e)
		    {
			    connection.Close();
          return 409;
		    }
        connection.Close();
		    return 200;
	    }
    }
}
