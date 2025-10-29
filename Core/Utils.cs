namespace Core;


public static class Utils
{
    public static bool HasProp(this object obj, string propName)
    {
        return obj.GetType().GetProperty(propName) != null;
    }
}
