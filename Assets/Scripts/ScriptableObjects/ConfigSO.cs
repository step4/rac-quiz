using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConfigSO : ScriptableObject
{
    private void OnEnable()
    {
        SessionToken = PlayerPrefs.GetString("SESSION_TOKEN", SessionToken);
        Debug.Log("session from pref: " + SessionToken);
        _installationId= SystemInfo.deviceUniqueIdentifier;
    }

    private void OnDisable()
    {
        _sessionToken = string.Empty;
    }

    [Header("Parse Settings")]
    [SerializeField]
    private string _sessionToken;

    public string SessionToken
    {
        get => _sessionToken;
        set
        {
            if (_sessionToken != value)
            {
                PlayerPrefs.SetString("SESSION_TOKEN", value);
                _sessionToken = value;
            }
        }
    }


    public string _installationId;


    public string LoginUrl;

    public string ParseApi;

    public string ParseAppId;

    [Header("Game Settings")]
    public int ShortGameCount;
    public int LongGameCount;
    public List<int> SecondsPerDifficulty;

    public void Logout()
    {
        SessionToken = string.Empty;
    }
}
