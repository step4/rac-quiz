using System.Threading.Tasks;

public interface IAvatarClient
{
    Task<(byte[],string)> GetAvatar(int width);
}
