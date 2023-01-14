using PainKiller.PowerCommands.Shared.Utils.DisplayTable;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace PainKiller.PowerCommands.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetDescription(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            return attributes.Length == 0 ? "" : ((DescriptionAttribute)attributes.First()).Description;
        }
        /// <summary>
        /// Get properties of T
        /// </summary>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetPropertiesOfT<T>(this Type instanceType)
        {
            var propertyInfos = instanceType.GetProperties().Where(t => t.PropertyType.BaseType == typeof(T)).ToList();
            return propertyInfos;
        }
        public static List<PropertyInfo> GetProperties<T>(this Type instanceType)
        {
            var propertyInfos = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            return propertyInfos;
        }
        public static void SetPropertyValue<T>(this T instance, string propertyName, string propertyValue) where T : new()
        {
            if (instance == null) return;
            var propertyInfo = instance.GetType().GetProperties().FirstOrDefault(t => t.Name == propertyName);
            if (propertyInfo == null) return;
            propertyInfo.SetValue(instance, propertyValue);
        }
        public static object GetPropertyValue<T>(this T instance, string propertyName) where T : new()
        {
            if (instance == null) return new object();
            var propertyInfo = instance.GetType().GetProperties().FirstOrDefault(t => t.Name == propertyName);
            return propertyInfo == null ? new object() : propertyInfo.GetValue(instance)!;
        }
        public static PowerCommandDesignAttribute GetPowerCommandAttribute(this IConsoleCommand command)
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(PowerCommandDesignAttribute), inherit: false);
            return attributes.Length == 0 ? new PowerCommandDesignAttribute(description: "Command have no description attribute") : (PowerCommandDesignAttribute)attributes.First();
        }
        public static TAttribute GetAttribute<TAttribute>(this IConsoleCommand command) where TAttribute : Attribute, new()
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(TAttribute), inherit: false);
            return attributes.Length == 0 ? new TAttribute() : (TAttribute)attributes.First();
        }
        public static IEnumerable<ColumnRenderOptionsAttribute> GetColumnRenderOptionsAttribute<T>(this T table) where T : IConsoleCommandTable
        {
            var retVal = new List<ColumnRenderOptionsAttribute>();
            var properties = GetProperties<T>(typeof(T));
            foreach (var propertyInfo in properties)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(ColumnRenderOptionsAttribute), inherit: false);
                if (attributes.Length == 0) continue;
                retVal.Add((ColumnRenderOptionsAttribute)attributes.First());
            }
            return retVal;
        }
        public static string GetDefaultParameter(this IConsoleCommand command)
        {
            var attribute = command.GetType().GetCustomAttributes(typeof(PowerCommandDesignAttribute), inherit: false).FirstOrDefault() as PowerCommandDesignAttribute;
            return attribute is null ? "" : attribute.Suggestions;
        }
        public static T DeepClone<T>(this T objSource) where T : class => CopyObject<T>(objSource);
        public static T CopyObject<T>(object objSource)
        {
            using MemoryStream stream = new MemoryStream();
            var jsonString = JsonSerializer.Serialize(objSource);
            return JsonSerializer.Deserialize<T>(jsonString)!;
        }
        public static string ToUsingDescription(this IConsoleCommand command)
        {
            var da = command.GetPowerCommandAttribute();
            var args = da.Arguments.Replace("!", "").Split('|');
            var quotes = da.Quotes.Replace("!", "").Split('|');
            var options = da.Options.Replace("!", "").Split('|');

            var argsMarkup = args.Any(a => !string.IsNullOrEmpty(a)) ? "[arguments]" : "";
            var quotesMarkup = quotes.Any(q => !string.IsNullOrEmpty(q)) ? "[quotes]" : "";
            var optionMarkup = options.Any(f => !string.IsNullOrEmpty(f)) ? "[options]" : "";

            return $" {argsMarkup} {quotesMarkup} {optionMarkup}".PadRight(38);
        }
    }
}