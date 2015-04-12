using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


// ReSharper disable once CheckNamespace
public static class ExtensionMethods
{
    public static string IntToLetter(this int index)
    {
        return char.ConvertFromUtf32(index + 'A');
    }
}