using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ucubot.Model;

namespace ucubot.DBCode
{
    public interface ILessonRepository
    {
        IEnumerable<LessonSignalDto> getLessons();
    		LessonSignalDto getLesson(long id);
    		int insertLesson(SlackMessage message);
    		int deleteLesson(long id);
    }
}
