using System.Text;

namespace TimMovie.SharedKernel.Extensions;

public static class StringExtensions
{
    public static string GetMailName(this string mail)
    {
        if (!mail.Contains("@"))
            throw new InvalidOperationException($"input string {mail} isn`t in mail format");
        return mail[..mail.IndexOf("@", StringComparison.Ordinal)];
    }

    public static string AddRandomEnd(this string input, int endLength = 4)
    {
        var append = new StringBuilder();
        for (var i = 0; i < endLength; i++)
        {
            append.Append(new Random().Next(0, 10));
        }

        return input + append;
    }
    
}