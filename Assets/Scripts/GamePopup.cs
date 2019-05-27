using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

public class GamePopup : MonoBehaviour
{
    [SerializeField]
    private GameObject CoursePrefab = default;

    [SerializeField]
    private GamePopupViewModel ViewModel = default;

    [SerializeField]
    private Color _unselectedTextColor = default;


    public void Create(List<Course> courses)
    {
        var parentRectTransform = (RectTransform)transform;
        var verticalGroup = GetComponent<VerticalLayoutGroup>();

        var courseHeight = ((RectTransform)CoursePrefab.transform).rect.height;

        courses.Sort((course1, course2) => {
            return course1.name.CompareTo(course2.name);
        });

        courses.ForEach((course) =>
        {
            GameObject courseGO = Instantiate(CoursePrefab, parentRectTransform);
            courseGO.name = course.name;
            var courseRectTransform = (RectTransform)courseGO.transform;

            var textComponent = courseGO.GetComponentInChildren<Text>();
            textComponent.text = course.name;

            var toggleComponent = courseGO.GetComponent<Toggle>();
            toggleComponent.onValueChanged.AddListener((_) => {
                if (toggleComponent.isOn) {
                    textComponent.color = Color.white;
                    //ViewModel.AddSelection(toggleComponent);
                }
                else {
                    textComponent.color = _unselectedTextColor;
                    //ViewModel.RemoveSelection(toggleComponent);
                }
                
            });

        });
    }


}
