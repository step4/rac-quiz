using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ActiveScreenInEditor : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnEnable()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (gameObject.activeInHierarchy)
            {
                var siblingIndex = transform.GetSiblingIndex();
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    if (i != siblingIndex)
                    {
                        var sibling = transform.parent.GetChild(i);
                        sibling.gameObject.SetActive(false);
                    }
                }
            }
        }

    }
#endif
}
