using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Net.Http;
using TMPro;
using UnityEngine.UI;

public class Initialization : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        //var imgData = await Avataaars.GetRandomImage(23);
        //var dim = (int)Mathf.Sqrt(imgData.Length);
        //Texture2D tex = new Texture2D(500, 530);
        //ImageConversion.LoadImage(tex, imgData);
        //var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        //img.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public async void loadit()
    {
        using (HttpClient client = new HttpClient())
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://loripsum.net/api");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Debug.Log(responseBody);
                text.text = responseBody;
            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nException Caught!");
                Debug.Log(e.Message);
            }
        }
    }
}
