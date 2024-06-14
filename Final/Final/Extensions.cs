using System.IO;

public static class Extensions
{
    public static byte[] ToByteArray(this Stream input)
    {
        using (var ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
