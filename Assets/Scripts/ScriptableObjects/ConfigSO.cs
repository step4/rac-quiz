using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConfigSO : ScriptableObject
{
    private void Awake()
    {
        SessionToken = PlayerPrefs.GetString("SESSION_TOKEN", "");
    }
    [SerializeField]
    private string _sessionToken;

    public string SessionToken
    {
        get => _sessionToken;
        set
        {
            PlayerPrefs.SetString("SESSION_TOKEN", value);
            _sessionToken = value;
        }
    }

    [SerializeField]
    private string _loginUrl;
    public string LoginUrl { get => _loginUrl; set => _loginUrl = value; }

    public string ParseApi;
}
