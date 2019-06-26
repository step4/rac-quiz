using System;
using System.Collections.Generic;

[Serializable]
public class GivenAnswer
{
    public string questionId;
    public bool correctlyAnswered;
    public List<int> answerIndices;
    public int elapsedSeconds;
}

