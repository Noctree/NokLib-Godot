using System.Collections.Generic;
using Array = Godot.Collections.Array;

namespace NokLib_Godot;
public static class GodotArrayExtensions
{
    public static IEnumerable<T> EnumerateAs<T>(this Array array) {
        for (int i = 0; i < array.Count; ++i)
            yield return (T)array[i].Obj;
    }

    public static bool Any(this Array array) => array.Count != 0;
    public static bool Empty(this Array array) => array.Count == 0;
}
