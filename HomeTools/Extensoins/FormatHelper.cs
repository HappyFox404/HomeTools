using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeTools.Extensoins;

public static class FormatHelper
{
    public static string FormatBytesSize(this long length)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = length;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1) {
            order++;
            len = len/1024;
        }
        return String.Format("{0:0.##} {1}", len, sizes[order]);
    }
    
    public static HtmlString FormatBytes(this IHtmlHelper html, long length)
    {
        return new HtmlString(length.FormatBytesSize());
    }
    
    public static HtmlString FormatDate(this IHtmlHelper html, DateTime date)
    {
        string result = $"{date.ToShortDateString()} {date.ToShortTimeString()}";
        return new HtmlString(result);
    }
}