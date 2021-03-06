﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


// ReSharper disable once CheckNamespace
public static class ExtensionMethods
{
    private static Random randomGenerator = new Random(DateTime.Now.Millisecond);
    public static string IntToLetter(this int index)
    {
        return char.ConvertFromUtf32(index + 'A');
    }

    public static List<T> Clone<T>(this IList<T> listToClone)
    {
        var newList = new List<T>(listToClone.Count);
        newList.AddRange(listToClone.Select(i => i));
        return newList;
    }

    /// <summary>
    /// stolen from http://stackoverflow.com/a/7913534/1173800
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static IEnumerable<T> Shuffle<T>(this IList<T> list)
    {
        var choices = Enumerable.Range(0, list.Count).ToList();
        for (var n = choices.Count; n > 1; n--)
        {
            var k = randomGenerator.Next(n);
            yield return list[choices[k]];
            choices.RemoveAt(k);
        }
        yield return list[choices[0]];
    }

    public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> linkedListNode)
    {
        return linkedListNode.Next ?? linkedListNode.List.First;
    }

    public static IEnumerable<ModelError> GetErrors(this ModelStateDictionary modelState)
    {
        return modelState.Values.SelectMany(v => v.Errors);
    }
}