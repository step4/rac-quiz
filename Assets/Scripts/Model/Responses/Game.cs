
using System;
using System.Collections.Generic;

[Serializable]
public class Game
{
    public List<Question> questions;
    public bool finished;
    public int difficulty;
    public bool withTimer;
    public List<GivenAnswer> givenAnswers;
    public int score;
}

