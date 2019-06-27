using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridCellSize : MonoBehaviour
{
    GridLayoutGroup gridLayout;
    Camera cam;
    private void OnEnable()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        //gridLayout.cellSize = new Vector2 { x = (Screen.width / 2.0f -40)*Camera.main.aspect, y = 330f };
        cam = Camera.main;
    }

    private void Update()
    {
        gridLayout.cellSize = new Vector2 { x = (Screen.width*cam.aspect / 2.0f - 40), y = 330f };
    }
}
