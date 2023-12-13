using System.Reflection;
using AdventOfCode2023;

var types = Assembly.GetExecutingAssembly().GetTypes();
Array.Sort(types, (a, b) => a.Name.CompareTo(b.Name));
foreach (var type in types)
{
    if (typeof(Day).IsAssignableFrom(type) && !(type.IsAbstract))
    {
        var ctor = type.GetConstructor(Type.EmptyTypes);
        if (ctor != null)
        {
            var day = (Day)ctor.Invoke(null);
            day.PrintAnswers();
        }
    }
}