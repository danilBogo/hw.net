using System.Text.RegularExpressions;

namespace TimMovie.SharedKernel.Classes;

public class Iframe
{
    private string _str;
    private readonly Regex _defaultReg = new(">");

    public Iframe()
    {
        _str = "<iframe></iframe>";
    }

    public Iframe Url(string url)
    {
        _str = _defaultReg.Replace(_str, $" src=\"{url}\">", 1);
        return this;
    }

    public Iframe Width(int width)
    {
        _str = _defaultReg.Replace(_str, $" width=\"{width}\">", 1);
        return this;
    }

    public Iframe Height(int height)
    {
        _str = _defaultReg.Replace(_str, $" height=\"{height}\">", 1);
        return this;
    }

    public override string ToString() => _str;
}