using System.Threading.Tasks;

public interface IAvatarClient
{
    Task<(byte[],string)> GetRandomAvatar(int width);
}
