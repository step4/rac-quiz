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
    private float GridSpace = default;

    [SerializeField]
    private int GridColumns = default;

    [SerializeField]
    private int LeftPadding = default;

    [SerializeField]
    private GamePopupViewModel ViewModel = default;

    public void Create(List<Faculty> faculties)
    {
        var parentRectTransform = (RectTransform)transform;
        var facultyInset = 0f;

        var studyProgramButtonHeight = ((RectTransform)CoursePrefab.transform).rect.height;
        var headlineHeight = ((RectTransform)CoursePrefab.transform).rect.height;

        faculties.ForEach((faculty) =>
        {
            GameObject facultyGO = new GameObject(faculty.name, typeof(RectTransform));
            var facultyRectTransform = (RectTransform)facultyGO.transform;
            facultyRectTransform.SetParent(parentRectTransform);
            var studyProgramCount = faculty.studyPrograms.Count;



            var facultyHeight = headlineHeight + GridSpace + Mathf.Ceil((float)studyProgramCount / GridColumns) * (studyProgramButtonHeight + GridSpace);
            facultyRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, facultyInset, facultyHeight);
            facultyRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, parentRectTransform.rect.width);

            var headlineGO = Instantiate(CoursePrefab, facultyRectTransform);
            var textComponent = headlineGO.GetComponentInChildren<Text>();

            GameObject studyProgramsGO = new GameObject("StudyPrograms", typeof(RectTransform), typeof(GridLayoutGroup));
            var studyProgramsTransform = (RectTransform)studyProgramsGO.transform;
            studyProgramsTransform.SetParent(facultyRectTransform);
            studyProgramsTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, headlineHeight + GridSpace, facultyHeight - headlineHeight);
            studyProgramsTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, facultyRectTransform.rect.width);

            var studyProgramsGridLayout = studyProgramsGO.GetComponent<GridLayoutGroup>();
            studyProgramsGridLayout.cellSize = new Vector2 { x = studyProgramButtonHeight, y = studyProgramButtonHeight };
            studyProgramsGridLayout.spacing = new Vector2 { x = GridSpace, y = GridSpace };
            studyProgramsGridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            studyProgramsGridLayout.constraintCount = GridColumns;
            studyProgramsGridLayout.padding = new RectOffset(LeftPadding, 0, 0, 0);

            textComponent.text = faculty.name;

            facultyInset += facultyHeight + GridSpace;

            faculty.studyPrograms.ForEach(studyProgram =>
            {
                var studyProgramGO = Instantiate(CoursePrefab, studyProgramsTransform);
                var studyProgramRectTransform = (RectTransform)studyProgramGO.transform;

                studyProgramGO.name = studyProgram.name;

                var studyProgramTextComponent = studyProgramGO.GetComponentInChildren<Text>();
                studyProgramTextComponent.text = studyProgram.name;
                var studyProgramImageComponents = studyProgramGO.GetComponentsInChildren<Image>();
                var iconComponent = studyProgramImageComponents.Single(image =>image.transform.name == "Icon");


                var studyProgramButtonComponents = studyProgramGO.GetComponentInChildren<Button>();
            });
        });
    }


}
