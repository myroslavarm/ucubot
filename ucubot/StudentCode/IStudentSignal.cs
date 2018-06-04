using System.Collections.Generic;
using ucubot.Model;

namespace ucubot.StudentCode
{
    public interface IStudentSignal
    {
        IEnumerable<StudentSignal> getStudentSignals(string conStr);
    }
}