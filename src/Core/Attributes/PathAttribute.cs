using Core.Physics.Components;
using System.Reflection;
namespace Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
//Атрибут для имени свойства в инспекторе
public sealed class PathAttribute : Attribute
{
    //строки типа "*.png;*.txt;*." и тп для фильтров в диалоговых окнах
    public string Filter { get; init; }
    public PathAttribute(string filter)
    {
        Filter = filter;
    }

}

