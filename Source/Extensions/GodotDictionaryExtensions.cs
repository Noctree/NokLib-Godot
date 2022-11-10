using Godot.Collections;

namespace NokLib_Godot;
public static class GodotDictionaryExtensions
{
    public static bool Any(this Dictionary dict) => dict.Count != 0;
    public static bool Empty(this Dictionary dict) => dict.Count == 0;
}