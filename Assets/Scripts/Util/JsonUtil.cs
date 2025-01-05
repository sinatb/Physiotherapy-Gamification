using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class JsonUtil 
{
    public static IList<T> DeserializeToList<T>(string jsonString)
    {
        var arr = JArray.Parse(jsonString);
        IList<T> objects = new List<T>();

        foreach (var item in arr)
        {
            objects.Add(item.ToObject<T>());
        }
        return objects;
    }
}
