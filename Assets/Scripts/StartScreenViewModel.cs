using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StartScreenViewModel : MonoBehaviour
{
    [SerializeField]
    private GameObject ParseClientGO;

    private IWebClient parseClient;

    private void Awake()
    {
        parseClient = ParseClientGO.GetComponent<IWebClient>();
    }
    // Start is called before the first frame update
    void Start()
    {
        parseClient.Send();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
