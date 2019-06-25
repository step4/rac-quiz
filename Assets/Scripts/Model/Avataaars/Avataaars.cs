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
    public Dictionary<string,List<string>> GetEnums()
    {
        var result = new Dictionary<string, List<string>>();
        var nameSpace = "AvataaarsOptionEnums";
        Assembly asm = Assembly.GetExecutingAssembly();
        var types = asm.GetTypes()
            .Where(type => type.Namespace == nameSpace);
        foreach (var type in types)
        {
            result.Add(type.Name, type.GetFields().Skip(1).Select(fieldInfo => fieldInfo.Name).ToList());
        }

        return result;
    }

    public async Task<(byte[], string)> GetImage(int width, Dictionary<string, string> config)
    {
        var configString = convertConfigToString(config);
        try
        {
            var url = $"https://www.rheinahrcampus.de/racmt4/avataaars/png/{width}?{configString}";
            Debug.Log(url);
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] image = await response.Content.ReadAsByteArrayAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            return (image, url);
        }
        catch (HttpRequestException e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<(byte[], string)> GetImage(string url)
    {
        
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] image = await response.Content.ReadAsByteArrayAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            return (image, url);
        }
        catch (HttpRequestException e)
        {
            throw new Exception(e.Message);
        }
    }

    private object convertConfigToString(Dictionary<string, string> config)
    {
        var str = "";
        foreach (var item in config)
        {
            var type = String.Join("",item.Key.Split().Select(i => Char.ToLower(i[0]) + i.Substring(1)));
            var selectedVal = item.Value;
            str += $"{type}={selectedVal}&";
        }
        return str;
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

        try
        {
            var url = $"https://www.rheinahrcampus.de/racmt4/avataaars/png/{width}?{GetRandomString()}";
            //Debug.Log(url);
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
