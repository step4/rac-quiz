using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AvatarClient : MonoBehaviour, IAvatarClient
{
    private Avataaars avataaars;
    public async Task<(byte[],string)> GetAvatar(int width)
    {
        return await avataaars.GetRandomImage(width);
    }

    private void Awake()
    {
        avataaars = new Avataaars();
    }
}
