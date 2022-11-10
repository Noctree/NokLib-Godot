using Godot;
using NokLib;

namespace NokLib_Godot;
public static class NodeExtensions
{
    public static Node GetRoot(this Node node) {
        var previous = node;
        while (node != null) {
            previous = node;
            node = node.GetParent();
        }
        return previous;
    }

    public static Node GetFirstChild(this Node node) {
        return node.GetChild(0);
    }

    public static Node GetLastChild(this Node node) {
        return node.GetChild(node.GetChildCount() - 1);
    }

    public static T? GetNodeAs<T>(this Node node) where T : Node {
        var new_node = node.Get(typeof(T).Name);
        GD.PrintS("GetNodeAs", typeof(T).Name, " => ", new_node);
        return (T)new_node.Obj;
    }

    public static string GetNodePropertyNames(this Node node) {
        return node.GetPropertyList().FormatToString(dict => (string)((Godot.Collections.Dictionary)dict)["name"]);
    }
}
