namespace ucubot.Controllers
{
    public interface InterfaceStudent
    {
        IEnumerable<Student> getStudents();
	Student getStudent();
	Task<IActionResult> insertStudent();
	Task<IActionResult> deleteStudent();
    }
}
