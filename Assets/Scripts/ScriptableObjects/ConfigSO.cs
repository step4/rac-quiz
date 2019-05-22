using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConfigSO : ScriptableObject
{
    private void Awake()
    {
        SessionToken = PlayerPrefs.GetString("SESSION_TOKEN", "");
        Debug.Log("session from pref: " + SessionToken);
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


    public string LoginUrl;

    public string ParseApi;

    public string ParseAppId;

    [Header("Game Settings")]
    public int ShortGameCount;
    public int LongGameCount;
}
