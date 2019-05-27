
using System;
using System.Collections.Generic;

[Serializable]
public class Game
{
    public string gameId;
    public List<Question> questions;
    public bool finished;
    public int difficulty;
    public bool withTimer;
    public List<GivenAnswer> givenAnswers = new List<GivenAnswer>();
    public int score;
}

