
using System;
using System.Collections.Generic;
[Serializable]
public class Question
{
    public string questionId;
    public string questionText;
    public List<Answer> answers;
    public int difficulty;
    public int customTime;
}

