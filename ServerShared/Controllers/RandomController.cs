namespace ServerShared.Controller;

public static class RandomController
{
    public static Random RNG = new();

    public static string RandomString(int lenght)
    {
        string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string ran = string.Empty;

        for (int i = 0; i < lenght; i++)
        {
            int x = RNG.Next(str.Length);
            ran = ran + str[x];
        }

        return ran;
    }

    public static string RandomCDKey()
    {
        return $"{RandomString(3)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}-{RandomString(4)}";
    }
}
