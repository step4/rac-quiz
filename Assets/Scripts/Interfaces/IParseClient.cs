using System.Collections.Generic;
using System.Threading.Tasks;

public interface IParseClient
{
    Task<List<Faculty>> GetStudyPrograms();
    Task SetStudyProgram(string id);
}
