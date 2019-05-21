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


    private void Awake()
    {
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) =>
        {
            return true;
        };

        _client.BaseAddress = new Uri(_config.ParseApi);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", _config.ParseAppId);

        print("Session now: "+_config.SessionToken);
        if (_config.SessionToken != "")
            _client.DefaultRequestHeaders.Add("X-Parse-Session-Token", _config.SessionToken);
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
}
