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

    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }

    /// <summary>
    /// stolen from http://stackoverflow.com/a/7913534/1173800
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IEnumerable<T> Shuffle<T>(this IList<T> list, Random rng)
    {
        var choices = Enumerable.Range(0, list.Count).ToList();
        for (var n = choices.Count; n > 1; n--)
        {
            var k = rng.Next(n);
            yield return list[choices[k]];
            choices.RemoveAt(k);
        }
        yield return list[choices[0]];
    }
}