using System;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class ParseClient : MonoBehaviour, IParseClient
{
    private HttpClient _client = new HttpClient();
    [SerializeField]
    private ConfigSO config;

    private async void Awake()
    {
        //_client.BaseAddress = new Uri(config.ParseApi);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _client.DefaultRequestHeaders.Add("X-Parse-Application-Id", config.ParseAppId);

        if (config.SessionToken != "")
            _client.DefaultRequestHeaders.Add("X-Parse-Session-Token", config.SessionToken);

    }

    public async Task<List<Faculty>> GetStudyPrograms()
    {
        HttpResponseMessage response = await _client.PostAsync($"{config.ParseApi}/functions/get_studyprograms", new StringContent(""));

        response.EnsureSuccessStatusCode();
        var studyProgramsJson = await response.Content.ReadAsStringAsync();


        print(studyProgramsJson);
        var studyProgramsResponse = JsonConvert.DeserializeObject<ParseListResponse<Faculty>>(studyProgramsJson);
        print(studyProgramsResponse);
        return studyProgramsResponse.result;
    }
}
