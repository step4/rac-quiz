using System.Threading.Tasks;

public interface IAvatarClient
{
    Task<byte[]> GetAvatar(int width);
}
