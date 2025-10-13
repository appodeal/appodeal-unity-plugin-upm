// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace AppodealInc.Mediation.Utils.Editor
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T> {items = array};
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            var wrapper = new Wrapper<T> {items = array};
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string FixJson(string value)
        {
            value = "{\"items\":" + value + "}";
            return value;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}
