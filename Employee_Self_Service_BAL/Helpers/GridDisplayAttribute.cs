using System;

[AttributeUsage(AttributeTargets.Property)]
public class GridDisplayAttribute : Attribute
{
    public string DisplayName { get; }

    public GridDisplayAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
