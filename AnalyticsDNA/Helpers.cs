using QuantLibrary;
using ExcelDna.Integration;

namespace AnalyticsDNA
{
    public static class Helpers
    {
        [ExcelFunction(Category = "Object", Description = "Query a method in object given by Guid")]
        public static object Query(
            [ExcelArgument(Name = "GUID", Description = "Guid of object being queried")] string id,
            [ExcelArgument(Name = "method", Description = "Method to call")] string method,
            [ExcelArgument(Name = "argument", Description = "Argument to method to parse")] object arg)
        {
            return Bag.QueryObject(Guid.Parse(id), method, arg);
        }
    }
}
