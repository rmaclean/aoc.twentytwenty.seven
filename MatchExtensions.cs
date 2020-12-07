using System;
using System.Text.RegularExpressions;

public static class MatchExtensions 
{
    public static string Attribute(this Match match) => match.Groups["attribute"].Value;
    public static string Color(this Match match) => match.Groups["color"].Value;
    public static int Count(this Match match) => Convert.ToInt32(match.Groups["count"].Value);
    public static bool Empty(this Match match) => match.Groups["empty"].Value != "";

}