using UnityEngine;
public interface INavigation
{
    void Push(string screenName, ScreenAnimation screenAnimation);
    void Push(GameObject screen, ScreenAnimation screenAnimation);
    void PushPopup(string screenName, ScreenAnimation screenAnimation);
    void PushPopup(GameObject screen, ScreenAnimation screenAnimation);
    GameObject Pop(ScreenAnimation screenAnimation);
    GameObject PopPopup(ScreenAnimation screenAnimation);

    void SetRoot(GameObject rootScreen, ScreenAnimation screenAnimation);
    void SetRoot(string rootScreenName, ScreenAnimation screenAnimation);

    void PushModal(string message, string buttonText, ModalIcon icon);
}
