using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextColorController : MonoBehaviour
{
    public Color SelectedColorText;
    public Color UnSelectedColorText;

    private Text textComponent;
    private Toggle toggleComponent;
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponentInChildren<Text>();
        toggleComponent = GetComponentInChildren<Toggle>();
        toggleComponent.onValueChanged.AddListener(SwitchTextColor);
    }

    void SwitchTextColor(bool selected) {
        if (toggleComponent.isOn)
        {
            textComponent.color = SelectedColorText;
        }
        else
        {
            textComponent.color = UnSelectedColorText;
        }
    }


        
    
}
