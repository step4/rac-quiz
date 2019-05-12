using UnityEngine;
public interface INavigation
{
    void Push(string screenName);
    void Push(GameObject screen);
    GameObject Pop();
}
