using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class PlayerConfigSO : ScriptableObject
{
    public string StudyProgram;
    public string StudyProgramId;
    public string StudyProgramShort;
    public Sprite StudyProgramSprite;

    public Sprite Avatar;
    public string AvatarUrl;

    public string PlayerName;

    public void OnEnable()
    {
        _reset();
    }

    private void _reset()
    {
        StudyProgram = default;
        StudyProgramId = default;
        StudyProgramShort = default;
        StudyProgramSprite = default;

        Avatar = default;
        AvatarUrl = default;

        PlayerName = default;
    }

    public void SaveSettings()
    {

    }

    
}
