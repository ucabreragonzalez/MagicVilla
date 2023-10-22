namespace MagicVilla_VillaAPI.Logging;

public interface ILogging
{
    public void Log(string message, string? type);

    public void Log(string message);

}