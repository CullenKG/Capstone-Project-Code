using UnityEngine;

public class NamedArrayAttribute : PropertyAttribute
{
    public readonly string[] names;
    public NamedArrayAttribute(System.Type type)
    {
        this.names = System.Enum.GetNames(type);
    }
}