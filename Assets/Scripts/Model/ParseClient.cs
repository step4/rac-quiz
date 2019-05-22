using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
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
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) =>
        {
            return true;
        };

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

    public async Task<List<Faculty>> GetStudyPrograms()
    {
        HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/functions/get_studyprograms", new StringContent(""));

        response.EnsureSuccessStatusCode();
        var studyProgramsJson = await response.Content.ReadAsStringAsync();


        var studyProgramsResponse = JsonConvert.DeserializeObject<ParseListResponse<Faculty>>(studyProgramsJson);
        return studyProgramsResponse.result;
    }

    public async Task SetStudyProgram(string id)
    {
        var studyProgramId = new StudyProgramId() { id = id };
        var json = JsonConvert.SerializeObject(studyProgramId);

        HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/functions/set_studyprogram", new StringContent(json));
        response.EnsureSuccessStatusCode();
    }

    public async Task<PlayerConfig> GetUserMe()
    {
        print(_client.DefaultRequestHeaders);
        HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/functions/get_me", new StringContent(""));
        response.EnsureSuccessStatusCode();
        var getMeJson = await response.Content.ReadAsStringAsync();

        var studyProgramsResponse = JsonConvert.DeserializeObject<ParseObjectResponse<PlayerConfig>>(getMeJson);
        return studyProgramsResponse.result;
    }

    public async Task<StudyProgram> GetStudyProgram(string id)
    {
        var studyProgramId = new StudyProgramId() { id = id };
        var json = JsonConvert.SerializeObject(studyProgramId);

        HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/functions/get_studyprogram", new StringContent(json));
        response.EnsureSuccessStatusCode();
        var studyProgramJson = await response.Content.ReadAsStringAsync();

        var studyProgramResponse = JsonConvert.DeserializeObject<ParseObjectResponse<StudyProgram>>(studyProgramJson);
        return studyProgramResponse.result;
    }

    public async Task<List<Course>> GetCourses(string studyProgramId)
    {
        var studyProgramIdRequest = new StudyProgramId() { id = studyProgramId };
        var json = JsonConvert.SerializeObject(studyProgramIdRequest);

        HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/functions/get_courses", new StringContent(json));

        response.EnsureSuccessStatusCode();
        var coursesJson = await response.Content.ReadAsStringAsync();


        var coursesResponse = JsonConvert.DeserializeObject<ParseListResponse<Course>>(coursesJson);
        return coursesResponse.result;
    }

    public async Task<Game> CreateGame(int numberOfQuestions, int difficulty, bool withTimer, string courseId)
    {
        var newGameRequest = new GameOptions() { courseId = courseId, difficulty = difficulty, withTimer = withTimer, numberOfQuestions = numberOfQuestions };
        var json = JsonConvert.SerializeObject(newGameRequest);

        HttpResponseMessage response = await _client.PostAsync($"{_config.ParseApi}/functions/create_game", new StringContent(json));

        response.EnsureSuccessStatusCode();
        var gameJson = await response.Content.ReadAsStringAsync();


        var coursesResponse = JsonConvert.DeserializeObject<ParseObjectResponse<Game>>(gameJson);
        return coursesResponse.result;
    }
}
