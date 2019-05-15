using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSettingsSO : ScriptableObject
{
    [SerializeField]
    private GameObject ParseClient = default;
    private IParseClient _parseClient;

    public string StudyProgram;
    public string StudyProgramId;
    public string StudyProgramShort;
    public Sprite StudyProgramSprite;

    public Sprite Avatar;
    public string AvatarUrl;

    public void OnAwake()
    {
        _parseClient = ParseClient.GetComponent<IParseClient>();

    }

    public void SaveSettings()
    {

    }
}
