using System.Collections.Generic;
using System.Threading.Tasks;

public interface IParseClient
{
    void SetSessionToken(string token);
    Task<List<Faculty>> GetStudyPrograms();
    Task SetStudyProgram(string id);
    Task<StudyProgram> GetStudyProgram(string id);
    Task<PlayerConfig> GetUserMe();
}
