using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ucubot.Model;

namespace ucubot.DBCode
{
    public interface ILessonRepository
    {
        IEnumerable<LessonSignalDto> getLessons(string conStr);
		LessonSignalDto getLesson(string conStr, long id);
		int insertLesson(string connStr, SlackMessage message);
		int deleteLesson(string connStr, long id);
    }
}
