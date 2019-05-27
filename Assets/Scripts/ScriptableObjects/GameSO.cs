using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSO : ScriptableObject
{
    public Game game;
    public int currentQuestion;
    public TimeSpan timePerQuestion;
    public int score;
    public int rightAnswerCount;
}
