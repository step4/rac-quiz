using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityWeld.Binding;
using System.Threading.Tasks;
using System.Linq;

[Binding]
public class GameScreenViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;
    private IAvatarClient _avatarClient;

    [SerializeField]
    private PlayerConfigSO _playerConfig = default;
    [SerializeField]
    private ConfigSO _config = default;
    [SerializeField]
    private GameSO _currentGame = default;

    [SerializeField]
    private Sprite _rightAnswerSprite = default;
    [SerializeField]
    private Sprite _wrongAnswerSprite = default;

    [SerializeField]
    private Animator timerAC = default;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();

    }

    private void OnEnable()
    {
        _currentGame.currentQuestion = 0;
        _currentGame.score = 0;
        var secondsToPlay = _config.SecondsPerDifficulty[_currentGame.difficulty - 1];
        _currentGame.timePerQuestion = new TimeSpan(0, 0, secondsToPlay);

        updateGameInfo();
        nextQuestion();
    }
    private void OnDisable()
    {
        Answers.Clear();
    }

    private TimeSpan countdown = TimeSpan.FromSeconds(0);
    private TimeSpan elapsedTime = TimeSpan.FromSeconds(0);
    private bool questionStarted = false;
    private void Update()
    {
        if (questionStarted)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(Time.deltaTime));

            if (_currentGame.withTimer)
            {
                countdown = countdown.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
                if (countdown < TimeSpan.FromSeconds(0))
                {
                    questionStarted = false;
                    timeElapsed();
                }
            }
        }
        updateGameInfo();
    }

    private async void timeElapsed()
    {
        foreach (var answer in Answers)
        {
            answer.Clickable = false;
        }


        timerAC.SetTrigger("TimeIsUp");
        await Task.Delay(1500);

        var questionId = _currentGame.questions[_currentGame.currentQuestion - 1].questionId;
        var answerIndices = new List<int>();
        var givenAnswer = new GivenAnswer
        {
            answerIndices = answerIndices,
            questionId = questionId,
            correctlyAnswered = false,
            elapsedSeconds = -1
        };
        _currentGame.givenAnswers.Add(givenAnswer);


        nextQuestion();
    }

    private void nextQuestion()
    {
        questionStarted = false;
        if (_currentGame.currentQuestion == _currentGame.questions.Count)
        {
            finishGame();
            return;
        }
        _currentGame.currentQuestion++;


        fillQuestionAndAnswers();
        startQuestion();
    }

    private void finishGame()
    {

        _navigation.PushPopup("GameFinishedPopup", ScreenAnimation.ModalCenter);
    }

    private void startQuestion()
    {
        elapsedTime = TimeSpan.FromSeconds(0);
        questionStarted = true;

        if (!_currentGame.withTimer) return;

        var currentQuestion = _currentGame.questions[_currentGame.currentQuestion - 1];

        countdown = currentQuestion.customTime == 0 ? _currentGame.timePerQuestion : TimeSpan.FromSeconds(currentQuestion.customTime);
        
    }

    private void fillQuestionAndAnswers()
    {
        var currentQuestion = _currentGame.questions[_currentGame.currentQuestion - 1];
        var parsedQuestionText = string.Empty;

        parsedQuestionText = parseLatex(currentQuestion.questionText);


        QuestionText = parsedQuestionText;

        Answers = new ObservableList<AnswerViewModel>();
        var answerIndex = 0;
        currentQuestion.answers.ForEach(answer =>
        {
            var parsedAnswerText = string.Empty;

            parsedAnswerText = parseLatex(answer.answerText);


            var answerViewModel = new AnswerViewModel
            {
                AnswerText = parsedAnswerText,
                ToggleBG = answer.isRightAnswer ? _rightAnswerSprite : _wrongAnswerSprite,
                AnswerPressed = answerPressed,
                IsRight = answer.isRightAnswer,
                Clickable = true,
                Index = answerIndex
            };
            answerIndex++;
            Answers.Add(answerViewModel);
        });
    }

    private string parseLatex(string inputText)
    {
        var text = string.Empty;
        var inlineDelimeter = '$';
        var blockDelimeter = '©';

        inputText = inputText.Replace("$$", blockDelimeter.ToString());

        var blockDelimeterSplit = inputText.Split(blockDelimeter).ToList();
        for (int i = 0; i < blockDelimeterSplit.Count; i++)
        {
            var content = blockDelimeterSplit[i];
            if ((i + 1) % 2 == 0 && i != 0)
            {
                text += $"\n{content}\n";
            }
            else
            {
                var inlineDelimeterSplit = content.Split(inlineDelimeter).ToList();
                for (int j = 0; j < inlineDelimeterSplit.Count; j++)
                {
                    var inlineContent = inlineDelimeterSplit[j];
                    if ((j + 1) % 2 == 0 && j != 0)
                    {
                        text += inlineContent;
                    }
                    else
                    {
                        text += $@"\text{{{inlineContent}}}";
                    }
                }
            }
        }
        //var startswithFormula = inputText[0] == delimeter;
        //for (int i = 0; i < inputTextParts.Count; i++)
        //{
        //    if (i % 2 == 0)
        //    {
        //        text += $@"\text{{{inputTextParts[i]}}}";
        //    }
        //    else
        //    {
        //        text += inputTextParts[i];
        //    }

        //}
        return text;
    }

    private async void answerPressed(bool isRight, int index)
    {
        questionStarted = false;
        foreach (var answer in Answers)
        {
            answer.Clickable = false;
        }

        var questionId = _currentGame.questions[_currentGame.currentQuestion - 1].questionId;
        var answerIndices = new List<int> { index };
        var givenAnswer = new GivenAnswer
        {
            answerIndices = answerIndices,
            questionId = questionId,
            correctlyAnswered = isRight,
            elapsedSeconds = elapsedTime.Seconds
        };
        _currentGame.givenAnswers.Add(givenAnswer);

        if (isRight)
        {
            _currentGame.score += _config.SecondsPerDifficulty[_currentGame.difficulty - 1] * (_currentGame.withTimer ? 2 : 1);
            _currentGame.rightAnswerCount++;
            await Task.Delay(1000);
            nextQuestion();
        }
        else
        {
            await Task.Delay(1000);
            nextQuestion();
        }
    }

    void updateGameInfo()
    {
        QuestionNumber = $"{_currentGame.currentQuestion}/{_currentGame.questions.Count}";
        GameTime = _currentGame.withTimer ? $"{countdown.ToString(@"mm\:ss")}" : $"{elapsedTime.ToString(@"mm\:ss")}";
        Score = $"{_currentGame.score}";
    }

    #region Properties

    private string _questionText;
    [Binding]
    public string QuestionText
    {
        get => _questionText;
        set
        {
            if (value != _questionText)
            {
                _questionText = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableList<AnswerViewModel> _answers = new ObservableList<AnswerViewModel>();
    [Binding]
    public ObservableList<AnswerViewModel> Answers
    {
        get => _answers;
        set
        {
            if (value != _answers)
            {
                _answers = value;
                OnPropertyChanged();
            }
        }
    }

    [SerializeField]
    private Vector2 _answerSize;
    [Binding]
    public Vector2 AnswerSize
    {
        get => _answerSize;
        set
        {
            if (value != _answerSize)
            {
                _answerSize = value;
                OnPropertyChanged();
            }
        }
    }

    [SerializeField]
    private Vector2 _answerSpacing;
    [Binding]
    public Vector2 AnswerSpacing
    {
        get => _answerSpacing;
        set
        {
            if (value != _answerSpacing)
            {
                _answerSpacing = value;
                OnPropertyChanged();
            }
        }
    }

    private string _questionNumber;
    [Binding]
    public string QuestionNumber
    {
        get => _questionNumber;
        set
        {
            if (value != _questionNumber)
            {
                _questionNumber = value;
                OnPropertyChanged();
            }
        }
    }

    private string _gameTime;
    [Binding]
    public string GameTime
    {
        get => _gameTime;
        set
        {
            if (value != _gameTime)
            {
                _gameTime = value;
                OnPropertyChanged();
            }
        }
    }

    private string _score;
    [Binding]
    public string Score
    {
        get => _score;
        set
        {
            if (value != _score)
            {
                _score = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
