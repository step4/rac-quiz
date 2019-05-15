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


    public string LoginUrl;

    public string ParseApi;

    public string ParseAppId;
}
