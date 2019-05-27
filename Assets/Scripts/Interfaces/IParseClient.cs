using System.Collections.Generic;
using System.Threading.Tasks;

public interface IParseClient
{
    void SetSessionToken(string token);
    Task<List<Faculty>> GetStudyPrograms();
    Task SetStudyProgram(string studyProgramId);
    Task<StudyProgram> GetStudyProgram(string studyProgramId);
    Task<PlayerConfig> GetUserMe();

    Task<List<Course>> GetCourses(string studyProgramId);

    Task<Game> CreateGame(int numberOfQuestions, int difficulty, bool withTimer, string courseId);
    Task FinishGame(string gameId, List<GivenAnswer> givenAnswers);
}
