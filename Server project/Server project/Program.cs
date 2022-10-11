using Server_project;

public static class Program
{
    public static void Main()
    {
        GameManager manager = new();
        manager.StartServer();
    }
}