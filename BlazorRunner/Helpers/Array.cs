using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner.Helpers
{
    public static class Array
    {
        public static ref T[] Append<T>(ref T[] array, T item)
        {
            int index = array.Length;

            System.Array.Resize(ref array, index + 1);

            array[index] = item;

            return ref array;
        }
    }

    public static class Dictionary
    {
        public static IDictionary<T, U[]> Append<T, U>(IDictionary<T, U[]> dictionary, T Key, U item)
        {
            if (dictionary.ContainsKey(Key))
            {
                U[] arr = dictionary[Key];

                Array.Append(ref arr, item);

                dictionary[Key] = arr;
            }
            else
            {
                dictionary.Add(Key, new U[] { item });
            }

            return dictionary;
        }
    }
}
