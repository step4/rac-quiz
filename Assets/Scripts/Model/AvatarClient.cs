using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AvatarClient : MonoBehaviour, IAvatarClient
{
    private Avataaars avataaars;

    public async Task<(byte[], string)> GetAvatar(int width, Dictionary<string, string> config)
    {
        return await avataaars.GetImage(width,config);
    }

    public async Task<(byte[], string)> GetAvatar(string url)
    {
        return await avataaars.GetImage(url);
    }

    public Dictionary<string, List<string>> GetEnums()
    {
        return avataaars.GetEnums();
    }

    public async Task<(byte[],string)> GetRandomAvatar(int width)
    {
        return await avataaars.GetRandomImage(width);
    }

    private void Awake()
    {
        avataaars = new Avataaars();
    }
}
