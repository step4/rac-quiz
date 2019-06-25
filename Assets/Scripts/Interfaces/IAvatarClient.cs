using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAvatarClient
{
    Task<(byte[],string)> GetRandomAvatar(int width);
    Task<(byte[], string)> GetAvatar(int width,Dictionary<string,string>config);
    Task<(byte[], string)> GetAvatar(string url);
    Dictionary<string, List<string>> GetEnums();
}
