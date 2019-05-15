using System;
using AvataaarsOptionEnums;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class Avataaars
{
    HttpClient client = new HttpClient();
    public Avataaars()
    {

    }
    public List<string> GetEnums()
    {
        var nameSpace = "AvataaarsOptionEnums";
        Assembly asm = Assembly.GetExecutingAssembly();
        return asm.GetTypes()
            .Where(type => type.Namespace == nameSpace)
            .Select(type => type.Name).ToList();
    }

    public string GetRandomString()
    {
        var nameSpace = "AvataaarsOptionEnums";
        Assembly asm = Assembly.GetExecutingAssembly();
        var types = asm.GetTypes()
            .Where(type => type.Namespace == nameSpace);
        var str = "";
        var rand = new System.Random();
        foreach (var type in types)
        {
            str += String.Join(" ", type.Name.Split()
    .Select(i => Char.ToLower(i[0]) + i.Substring(1))); ;
            var values = type.GetFields();
            var index = rand.Next(values.Length - 1);
            str += "=";
            str += values[index].Name;
            str += "&";
        }
        return str;
    }

    public async Task<(byte[],string)> GetRandomImage(int width)
    {

        // Call asynchronous network methods in a try/catch block to handle exceptions
        try
        {
            var url = $"https://avataaars.io/png/{width}?{GetRandomString()}";
            Debug.Log(url);
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] image = await response.Content.ReadAsByteArrayAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            return (image,url);
        }
        catch (HttpRequestException e)
        {
            throw new Exception(e.Message);
        }

    }
}
