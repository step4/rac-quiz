﻿
using System.Collections.Generic;
[System.Serializable]
class FinishGameRequest
{
    public string gameId;
    public List<GivenAnswer> givenAnswers;
    public int rightAnswerCount;
    public int score;
}

