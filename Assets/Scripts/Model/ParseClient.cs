using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;

public class ParseClient : MonoBehaviour, IParseClient
{
    private HttpClient _client = new HttpClient();
    [SerializeField]
    private ConfigSO _config = default;
    [SerializeField]
    private bool _injectSessionToken = default;

    private void Awake()
    {
        //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) =>
        //{
        //    return true;
        //};

        //_client.BaseAddress = new Uri(_config.ParseApi);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", _config.ParseAppId);

        if (_injectSessionToken)
            SetSessionToken(_config.SessionToken);
    }

    public void SetSessionToken(string token)
    {
        _client.DefaultRequestHeaders.Remove("X-Parse-Session-Token");
        _client.DefaultRequestHeaders.Add("X-Parse-Session-Token", token);
    }

    private async Task<string> _postWithEmptyString(string endpoint)
    {
        try
        {
            HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/{endpoint}", new StringContent(""));

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.Log(endpoint);
            Debug.LogError(ex.Message);
            throw ex;
        }
    }

    private async Task<string> _postWithData(string endpoint, string jsonData)
    {
        try
        {
            HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/{endpoint}", new StringContent(jsonData));

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.Log(endpoint);
            Debug.Log(jsonData);
            Debug.LogError(ex.Message);
            throw ex;
        }
    }

    private List<K> _deserializeParseList<T, K>(string json) where T : ParseListResponse<K> => JsonUtility.FromJson<T>(json).result;
    private K _deserializeParseObject<T, K>(string json) where T : ParseObjectResponse<K>
    {
        var x = JsonUtility.FromJson<T>(json);
        return x.result;
    }

    public async Task<List<Faculty>> GetStudyPrograms()
    {
        var studyProgramsJson = await _postWithEmptyString("functions/get_studyprograms");
        return _deserializeParseList<ParseListResponse<Faculty>, Faculty>(studyProgramsJson);
    }

    public async Task SetStudyProgram(string id)
    {
        var studyProgramId = new StudyProgramId() { id = id };
        var json = JsonUtility.ToJson(studyProgramId);

        await _postWithData("functions/set_studyprogram", json);
    }

    public async Task<PlayerInfo> GetUserMe()
    {
        var getMeJson = await _postWithEmptyString("functions/get_me");
        return _deserializeParseObject<ParseObjectResponse<PlayerInfo>, PlayerInfo>(getMeJson);

    }
    public async Task SetUserMe(string playerName, string studyProgramId, string avatarUrl)
    {
        var playerInfo = new PlayerInfo { avatarUrl = avatarUrl, playerName = playerName, studyProgramId = studyProgramId };
        var json = JsonUtility.ToJson(playerInfo);

        await _postWithData("functions/set_me", json);
    }

    public async Task<StudyProgram> GetStudyProgram(string id)
    {
        var studyProgramId = new StudyProgramId() { id = id };
        var json = JsonUtility.ToJson(studyProgramId);

        var studyProgramJson = await _postWithData("functions/get_studyprogram", json);
        return _deserializeParseObject<ParseObjectResponse<StudyProgram>, StudyProgram>(studyProgramJson);
    }

    public async Task<List<Course>> GetCourses(string studyProgramId)
    {
        var studyProgramIdRequest = new StudyProgramId() { id = studyProgramId };
        var json = JsonUtility.ToJson(studyProgramIdRequest);

        var coursesJson = await _postWithData("functions/get_courses", json);
        return _deserializeParseList<ParseListResponse<Course>, Course>(coursesJson);
    }

    public async Task<Game> CreateGame(int numberOfQuestions, int difficulty, bool withTimer, string courseId)
    {
        var newGameRequest = new GameOptions() { courseId = courseId, difficulty = difficulty, withTimer = withTimer, numberOfQuestions = numberOfQuestions };
        var json = JsonUtility.ToJson(newGameRequest);

        var gameJson = await _postWithData("functions/create_game", json);

        return _deserializeParseObject<ParseObjectResponse<Game>, Game>(gameJson);
    }

    public async Task FinishGame(string gameId, List<GivenAnswer> givenAnswers, int rightAnswerCount, int score)
    {
        var finishGameRequest = new FinishGameRequest { givenAnswers = givenAnswers, gameId = gameId, rightAnswerCount = rightAnswerCount, score = score };
        var json = JsonUtility.ToJson(finishGameRequest);

        await _postWithData("functions/finish_game", json);
    }

    public async Task<UserResponse> Register(string username, string password, string email, string installationId)
    {
        var registerRequest = new RegisterRequest { username = username, password = password, email = email, _InstallationId = installationId, _ApplicationId = _config.ParseAppId };
        var json = JsonUtility.ToJson(registerRequest);
        _client.DefaultRequestHeaders.Remove("X-Parse-Application-Id");
        var userJson = await _postWithData("users", json);
        _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", _config.ParseAppId);
        return JsonUtility.FromJson<UserResponse>(userJson);
    }

    public async Task<UserResponse> Login(string username, string password, string installationId)
    {
        var registerRequest = new LoginRequest { username = username, password = password, _InstallationId = installationId, _ApplicationId = _config.ParseAppId };
        var json = JsonUtility.ToJson(registerRequest);
        _client.DefaultRequestHeaders.Remove("X-Parse-Application-Id");
        var userJson = await _postWithData("login", json);
        _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", _config.ParseAppId);
        return JsonUtility.FromJson<UserResponse>(userJson);
    }
}
