using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

[Binding]
public class GamePopupViewModel : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField]
    private GameObject ParseClientGO = default;
    [SerializeField]
    private GameObject NavigationGO = default;

    private IParseClient _parseClient;
    private INavigation _navigation;

    [SerializeField]
    private PlayerConfigSO _playerConfig = default;
    [SerializeField]
    private ConfigSO _config = default;
    [SerializeField]
    private GameSO _newGame = default;
    [SerializeField]
    private Color _selectedTextColor = default;

    public int NumberOfCourses = default;
    public List<Toggle> SelectedCourses = new List<Toggle>();

    private bool popupCreated = false;


    private void Awake()
    {
        _parseClient = ParseClientGO.GetComponent<IParseClient>();
        _navigation = NavigationGO.GetComponent<INavigation>();
        Difficulty = 1;
    }

    private void OnEnable()
    {
        if (!popupCreated)
        {
            populateListView();
            popupCreated = true;
        }
    }

    private void OnDisable()
    {
        popupCreated = false;
        Courses.Clear();
        SelectedCourses.Clear();
    }
    private ObservableList<CourseViewModel> _courses = new ObservableList<CourseViewModel>();
    [Binding]
    public ObservableList<CourseViewModel> Courses
    {
        get => _courses;
        set
        {
            if (value != _courses)
            {
                _courses = value;
                OnPropertyChanged();
            }
        }
    } 

    [SerializeField]
    private float _difficulty;
    [Binding]
    public float Difficulty
    {
        get => _difficulty;
        set
        {
            if (value != _difficulty)
            {
                _difficulty = value;
                OnPropertyChanged();
            }
        }
    }

    [SerializeField]
    private bool _onTime;
    [Binding]
    public bool OnTime
    {
        get => _onTime;
        set
        {
            if (value != _onTime)
            {
                _onTime = value;
                OnPropertyChanged();
            }
        }
    }

    [SerializeField]
    private bool _longGame;
    [Binding]
    public bool LongGame
    {
        get => _longGame;
        set
        {
            if (value != _longGame)
            {
                _longGame = value;
                OnPropertyChanged();
            }
        }
    }

    public void AddSelection(Toggle toggle)
    {
        if (NumberOfCourses == 1)
        {
            if (SelectedCourses.Count != 0)
            {
                SelectedCourses[0].isOn = false;
                SelectedCourses.Clear();
            }
            SelectedCourses.Add(toggle);
            return;
        }
        if (SelectedCourses.Count < NumberOfCourses)
        {
            SelectedCourses.Add(toggle);
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void RemoveSelection(Toggle toggle)
    {
        SelectedCourses.Remove(toggle);
    }

    public void SetDifficulty(float value)
    {
        Difficulty = (int)value;
    }

    [Binding]
    public void ClosePopup()
    {
        _navigation.PopPopup();
    }

    [Binding]
    public async void StartGame()
    {
        //TODO: Fehlermeldung
        //if (SelectedCourses.Count < 1) return;
        //var numberOfQuestions = LongGame ? _config.LongGameCount : _config.ShortGameCount;
        var numberOfQuestions = 2;
        //var selectedCourse = SelectedCourses[0];
        //var courseId = _courses.Find(course => course.name == selectedCourse.name).id;
        var courseId = "sFHftdfU24";
        var game = await _parseClient.CreateGame(numberOfQuestions, (int)Difficulty, OnTime, courseId);
        _newGame.game = game;
        _navigation.SetRoot("GameScreen");
    }

    private async void populateListView()
    {
        var courses = await _parseClient.GetCourses(_playerConfig.StudyProgramId);
        Courses = new ObservableList<CourseViewModel>();
        courses.ForEach(course => {
            var courseViewModel = new CourseViewModel {
                CourseId = course.id,
                CourseText = course.name,
                CoursePressed = courseSelected,

            };
            Courses.Add(courseViewModel);
        });
    }

    private void courseSelected(string courseId, bool selected,Toggle toggleComponent)
    {
        print(courseId);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
