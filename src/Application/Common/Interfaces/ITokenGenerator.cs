namespace MiniURL.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        string GetUniqueKey(int length);
    }
}