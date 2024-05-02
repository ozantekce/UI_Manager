using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{



    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> collection)
    {
        if (collection == null) return;
        if (set == null) set = new HashSet<T>();

        foreach (var item in collection)
        {
            set.Add(item);
        }
    }

    public static T GetOrAddComponent<T>(this GameObject o) where T : Component
    {
        return o.GetComponent<T>() ?? o.AddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this MonoBehaviour o) where T : Component
    {
        return o.gameObject.GetOrAddComponent<T>();
    }


    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

}
