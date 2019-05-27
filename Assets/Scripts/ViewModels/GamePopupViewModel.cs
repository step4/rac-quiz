﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using System.Linq;

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
    private Color _unselectedTextColor = default;

    public int NumberOfCourses = default;
    public List<string> SelectedCoursesId = new List<string>();

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
        SelectedCoursesId.Clear();
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

    public void AddSelection(string courseId)
    {
        if (NumberOfCourses == 1)
        {
            if (SelectedCoursesId.Count != 0)
            {
                _deactivateToggle(SelectedCoursesId[0]);
                SelectedCoursesId.Clear();
            }
            SelectedCoursesId.Add(courseId);
            return;
        }
        if (SelectedCoursesId.Count < NumberOfCourses)
        {
            SelectedCoursesId.Add(courseId);
        }
        else
        {
            _deactivateToggle(courseId);
        }
    }

    private void _activateToggle(string id) => Courses.Single(course => course.CourseId == id).Selected = true;
    private void _deactivateToggle(string id) => Courses.Single(course => course.CourseId == id).Selected = false;

    public void RemoveSelection(string courseId)
    {
        SelectedCoursesId.Remove(courseId);
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
        if (SelectedCoursesId.Count < 1) return;
        var numberOfQuestions = LongGame ? _config.LongGameCount : _config.ShortGameCount;
        var selectedCourseId = SelectedCoursesId[0];
        var game = await _parseClient.CreateGame(numberOfQuestions, (int)Difficulty, OnTime, selectedCourseId);
        _newGame.game = game;
        _navigation.SetRoot("GameScreen");
    }

    private async void populateListView()
    {
        var courses = await _parseClient.GetCourses(_playerConfig.StudyProgramId);
        courses.Sort((course1, course2) => {
            return course1.name.CompareTo(course2.name);
        });
        Courses = new ObservableList<CourseViewModel>();
        courses.ForEach(course => {
            var courseViewModel = new CourseViewModel {
                CourseId = course.id,
                CourseText = course.name,
                CoursePressed = courseTapped,
                Selected = false,
                SelectedTextColor = Color.white,
                UnselectedTextColor = _unselectedTextColor,
                TextColor = _unselectedTextColor
            };
            Courses.Add(courseViewModel);
        });
    }

    private void courseTapped(string courseId, bool selected)
    {
        if (selected) {
            AddSelection(courseId);
        }
        else
        {
            RemoveSelection(courseId);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}