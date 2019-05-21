using UnityEngine;
public interface INavigation
{
    void Push(string screenName);
    void Push(GameObject screen);
    void PushPopup(string screenName);
    void PushPopup(GameObject screen);
    GameObject Pop();
    GameObject PopPopup();

    void SetRoot(GameObject rootScreen);
    void SetRoot(string rootScreenName);
}
