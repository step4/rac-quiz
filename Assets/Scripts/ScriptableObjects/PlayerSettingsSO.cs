using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField]
    private GameObject ParseClient;
    private IParseClient _parseClient;

    public string StudyProgram;
    public string StudyProgramId;
    public Sprite StudyProgramSprite;

    public void OnAwake()
    {
        _parseClient = ParseClient.GetComponent<IParseClient>();

    }

    public void SaveSettings()
    {

    }
}
