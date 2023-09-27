using System;
using System.Collections.Generic;
using System.Linq;

namespace ModBuilder.Library.Utility;

public class SameVersionComparer : IComparer<string>
{
#pragma warning disable 8632
    public int Compare(string? x, string? y)
#pragma warning restore 8632
    {
        if (string.IsNullOrWhiteSpace(x))
        {
            return 1;
        }

        var firstParts = Parse(x).ToArray();
        var secondParts = Parse(y).ToArray();

        var thisLength = firstParts.Length;
        var thatLength = secondParts.Length;
        var maxLength = Math.Max(thisLength, thatLength);
        for (var i = 0; i < maxLength; i++)
        {
            var thisPart = i < thisLength ? firstParts[i] : 0;
            var thatPart = i < thatLength ? secondParts[i] : 0;
            if (thisPart < thatPart)
            {
                return -1;
            }

            if (thisPart > thatPart)
            {
                return 1;
            }
        }

        return 0;
    }

    private static IEnumerable<int> Parse(string version)
    {
        var partsString = version.Split('.');
        var parts = new int[partsString.Length];
        for (var i = 0; i < partsString.Length; i++)
        {
            int.TryParse(partsString[i], out parts[i]);
        }

        return parts;
    }
}