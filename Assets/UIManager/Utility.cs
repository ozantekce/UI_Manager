using System;
using System.Collections.Generic;
using UnityEngine;


namespace UIManager
{
    public static class Utility
    {
        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


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


        public static T GetRandomElement<T>(this HashSet<T> hashSet)
        {
            int index = rng.Next(hashSet.Count);
            var enumerator = hashSet.GetEnumerator();
            for (int i = 0; i <= index; i++)
            {
                enumerator.MoveNext();
            }
            return enumerator.Current;
        }

        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

    }



}
