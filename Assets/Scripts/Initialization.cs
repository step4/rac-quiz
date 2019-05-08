using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Parse;

public class Initialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParseClient.Initialize(new ParseClient.Configuration
        {
            ApplicationID = "hochschulQuiz",
            Key = "",
            ServerURI = @"http://localhost:8080/api"
        });

        //IDictionary<string, object> param = new Dictionary<string, object>
        //{
        //    { "platyerName", "friend" }
        //};
        //ParseCloud.CallFunctionAsync<IDictionary<string, object>>("averageStars", param).ContinueWith(t =>
        //{
        //    Debug.Log(t);
        //});
    }

    // Update is called once per frame
    void Update()
    {

    }
}
