namespace ucubot.Controllers
{
    public class ClassStudent : InterfaceStudent
    {
        IEnumerable<Student> getStudents(){
	}
	Student getStudent(){
	}
	Task<IActionResult> insertStudent();
	Task<IActionResult> deleteStudent();
    }
}
