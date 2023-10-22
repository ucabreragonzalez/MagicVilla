namespace MagicVilla_VillaAPI.Logging;

public class Logging : ILogging
{
    public Logging()
    {
        Console.WriteLine("creating servicio.");
    }

    public void Log(string message, string? type)
    {
        if (type == "error")
        {
            Console.WriteLine($"ERROR - {message}");
        }
        else
        {
            Console.WriteLine(message);
        }
    }

    public void Log(string message)
    {
        Log(message, null);
    }
}