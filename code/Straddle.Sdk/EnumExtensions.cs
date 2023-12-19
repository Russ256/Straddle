namespace Straddle;

using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString())!;
        DescriptionAttribute descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>(false)!;

        return descriptionAttribute?.Description ?? value.ToString();
    }
}
