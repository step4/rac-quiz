using System.Collections.Generic;
using System.Threading.Tasks;

public interface IParseClient
{
    void SetSessionToken(string token);
    Task<List<Faculty>> GetStudyPrograms();
    Task SetStudyProgram(string studyProgramId);
    Task<StudyProgram> GetStudyProgram(string studyProgramId);
    Task<PlayerInfo> GetUserMe();
    Task SetUserMe(string playerName, string studyProgramId, string avatarUrl);

    Task<List<Course>> GetCourses(string studyProgramId);

    Task<Game> CreateGame( int numberOfQuestions, int difficulty, bool withTimer, string courseId);
    Task FinishGame(string gameId, List<GivenAnswer> givenAnswers,int rightAnswerCount);

    Task<UserResponse> Register(string username, string password, string email, string installationId);
    Task<UserResponse> Login(string username, string password, string installationId);
}
