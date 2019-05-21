using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour, INavigation
{
    public GameObject ScreenTree;
    public Dictionary<string, GameObject> Screens;
    public GameObject FirstScreen;

    [SerializeField]
    private Stack<GameObject> _navigationStack;

    private int _screenCount;

    private void OnEnable()
    {
        _screenCount = ScreenTree.transform.childCount;
        Screens = new Dictionary<string, GameObject>(_screenCount);
        for (int i = 0; i < _screenCount; i++)
        {
            var screen = ScreenTree.transform.GetChild(i);
            Screens.Add(screen.name, screen.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        SetRoot(FirstScreen);
    }

    

    public void Push(GameObject screen)
    {
        if (_navigationStack.Count > 0)
        {
            var lastScreen = _navigationStack.Peek();
            lastScreen.SetActive(false);
        }
        _navigationStack.Push(screen);
        screen.SetActive(true);
    }

    public void PushPopup(GameObject screen)
    {
        _navigationStack.Push(screen);
        screen.SetActive(true);
    }

    public void PushPopup(string screenName)
    {
        var screen = Screens[screenName];
        PushPopup(screen);
    }

    public void Push(string screenName)
    {
        var screen = Screens[screenName];
        Push(screen);
    }

    public GameObject Pop()
    {
        GameObject lastScreen;
        if (_navigationStack.Count > 1) {
            lastScreen = _navigationStack.Peek();
            lastScreen.SetActive(false);
            _navigationStack.Pop();
        }
        lastScreen = _navigationStack.Peek();
        lastScreen.SetActive(true);
        return lastScreen;
    }

    public GameObject PopPopup()
    {
        GameObject lastScreen;
        if (_navigationStack.Count > 1)
        {
            lastScreen = _navigationStack.Peek();
            lastScreen.SetActive(false);
            _navigationStack.Pop();
        }
        lastScreen = _navigationStack.Peek();
        return lastScreen;
    }


    public void SetRoot(GameObject rootScreen)
    {
        _navigationStack = new Stack<GameObject>();

        foreach (var screen in Screens.Values)
        {
            screen.SetActive(false);
        }
        Push(rootScreen);
    }

    public void SetRoot(string rootScreenName)
    {
        var screen = Screens[rootScreenName];
        SetRoot(screen);
    }
}
