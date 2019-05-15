using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyProgramPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject HeadlinePrefab;
    [SerializeField]
    private GameObject StudyProgramPrefab;

    [SerializeField]
    private float GridSpace;

    [SerializeField]
    private int GridColumns;
    public void Create(List<Faculty> faculties)
    {
        var parentRectTransform = (RectTransform)transform;
        var facultyInset = 0f;
        var gridInsetX = 0f;
        var gridInsetY = 0f;
        var gridElementCount = 0;

        var studyProgramButtonHeight = ((RectTransform)StudyProgramPrefab.transform).rect.height;
        var headlineHeight = ((RectTransform)HeadlinePrefab.transform).rect.height;

        faculties.ForEach((faculty) =>
        {
            GameObject facultyGO = new GameObject(faculty.name, typeof(RectTransform));
            var facultyRectTransform = (RectTransform)facultyGO.transform;
            facultyRectTransform.SetParent(parentRectTransform);

            var studyProgramCount = faculty.studyPrograms.Count;
            var facultyHeight = headlineHeight + GridSpace + Mathf.Ceil((float)studyProgramCount / GridColumns) * (studyProgramButtonHeight + GridSpace);
            print(facultyHeight);

            facultyRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, facultyInset, facultyHeight);
            facultyRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, parentRectTransform.rect.width);

            var headlineGO = Instantiate(HeadlinePrefab, facultyRectTransform);
            var textComponent = headlineGO.GetComponentInChildren<Text>();
            textComponent.text = faculty.name;

            facultyInset += facultyHeight + GridSpace;
            gridElementCount = 0;
            gridInsetX = 0f;
            gridInsetY = 0f;
            faculty.studyPrograms.ForEach(studyProgram =>
            {
                var studyProgramGO = Instantiate(StudyProgramPrefab, facultyRectTransform);
                gridElementCount++;
                var studyProgramRectTransform = (RectTransform)studyProgramGO.transform;
                studyProgramRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, headlineHeight + GridSpace + gridInsetY, studyProgramButtonHeight);
                studyProgramRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, GridSpace + gridInsetX, studyProgramButtonHeight);
                gridInsetY = Mathf.Floor(gridElementCount / GridColumns) * (studyProgramButtonHeight + GridSpace);
                gridInsetX = gridElementCount % GridColumns * (studyProgramButtonHeight + GridSpace);

                studyProgramGO.name = studyProgram.name;
            });
        }); ;
    }
}
