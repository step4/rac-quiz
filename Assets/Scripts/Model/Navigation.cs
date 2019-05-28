using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Navigation : MonoBehaviour, INavigation
{
    public GameObject ScreenTree;
    public Dictionary<string, GameObject> Screens;
    public GameObject FirstScreen;
    public int SplashDelay;

    [SerializeField]
    private Stack<GameObject> _navigationStack;

    private int _screenCount;

 
    private  void OnEnable()
    {
        _screenCount = ScreenTree.transform.childCount;
        Screens = new Dictionary<string, GameObject>(_screenCount);
        for (int i = 0; i < _screenCount; i++)
        {
            var screen = ScreenTree.transform.GetChild(i);
            if(screen.name=="BG")continue;

            var canvasGroup = screen.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            Screens.Add(screen.name, screen.gameObject);
        }

    }

    // Start is called before the first frame update
    async void Start()
    {
        await Task.Delay(SplashDelay);
        SetRoot(FirstScreen,ScreenAnimation.Fade);
    }

    

    public void Push(GameObject screen, ScreenAnimation screenAnimation)
    {
        if (_navigationStack.Count > 0)
        {
            var lastScreen = _navigationStack.Peek();
            lastScreen.SetActive(false);
            lastScreen.GetComponent<Animator>().SetTrigger(ScreenAnimation.Close.ToString());
        }
        _navigationStack.Push(screen);
        screen.SetActive(true);
        screen.GetComponent<Animator>().SetTrigger(screenAnimation.ToString());
    }

    public void PushPopup(GameObject screen, ScreenAnimation screenAnimation)
    {
        _navigationStack.Push(screen);
        screen.SetActive(true);
        screen.GetComponent<Animator>().SetTrigger(screenAnimation.ToString());
    }

    public void PushPopup(string screenName, ScreenAnimation screenAnimation)
    {
        var screen = Screens[screenName];
        PushPopup(screen, screenAnimation);
    }

    public void Push(string screenName, ScreenAnimation screenAnimation)
    {
        var screen = Screens[screenName];
        Push(screen, screenAnimation);
    }

    public GameObject Pop(ScreenAnimation screenAnimation)
    {
        GameObject lastScreen;
        if (_navigationStack.Count > 1) {
            lastScreen = _navigationStack.Peek();
            lastScreen.SetActive(false);
            lastScreen.GetComponent<Animator>().SetTrigger(ScreenAnimation.Close.ToString());
            _navigationStack.Pop();
        }
        lastScreen = _navigationStack.Peek();
        lastScreen.SetActive(true);
        lastScreen.GetComponent<Animator>().SetTrigger(screenAnimation.ToString());
        return lastScreen;
    }

    public GameObject PopPopup(ScreenAnimation screenAnimation)
    {
        GameObject lastScreen;
        if (_navigationStack.Count > 1)
        {
            lastScreen = _navigationStack.Peek();
            lastScreen.SetActive(false);
            lastScreen.GetComponent<Animator>().SetTrigger(ScreenAnimation.Close.ToString());
            _navigationStack.Pop();
        }
        lastScreen = _navigationStack.Peek();
        return lastScreen;
    }


    public void SetRoot(GameObject rootScreen, ScreenAnimation screenAnimation)
    {
        _navigationStack = new Stack<GameObject>();

        foreach (var screen in Screens.Values)
        {
            screen.SetActive(false);
        }
        Push(rootScreen, screenAnimation);
    }

    public void SetRoot(string rootScreenName, ScreenAnimation screenAnimation)
    {
        var screen = Screens[rootScreenName];
        SetRoot(screen, screenAnimation);
    }
}
