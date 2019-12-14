using System.Linq;

public static class ArrayExtensions
{
    public static T Random<T>(this T[] source)
    {
        return source.OrderBy(x => UnityEngine.Random.value).FirstOrDefault();
    }
}
