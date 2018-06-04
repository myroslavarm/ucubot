namespace ucubot.Controllers
{
    public interface InterfaceLesson
    {
        IEnumerable<LessonSignalDto> getLessons();
	LessonSignalDto getLesson();
	Task<IActionResult> insertLesson();
	Task<IActionResult> deleteLesson();
    }
}
