using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
///     From https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class/6944605
///     User: Jacobs Data Solutions
/// </summary>
public static class ReflectiveEnumerator
{
    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable<T>
    {
        var objects = new List<T>();
        foreach (var type in
                 Assembly.GetAssembly(typeof(T)).GetTypes()
                     .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            objects.Add((T)Activator.CreateInstance(type, constructorArgs));
        objects.Sort();
        return objects;
    }
}