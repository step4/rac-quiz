using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSO : ScriptableObject
{
    public string gameId;
    public List<Question> questions;
    public bool finished;
    public int difficulty;
    public bool withTimer;
    public List<GivenAnswer> givenAnswers = new List<GivenAnswer>();
    public int score;
    public int rightAnswerCount;
    public int currentQuestion;
    public TimeSpan timePerQuestion;
}
