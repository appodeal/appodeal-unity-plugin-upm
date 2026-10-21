using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using CompressionLevel = System.IO.Compression.CompressionLevel;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.Analytics.Editor
{
    internal static class ExtensionMethods
    {
        private static readonly AppodealLogger Log = AnalyticsLogging.Log;

        internal static T GetPropertyValue<T>(this Type type, string propertyName, BindingFlags flags)
        {
            if (type == null)
            {
                Log.Verbose($"Cannot get property '{propertyName}' because the '{nameof(type)}' argument is null");
                return default;
            }

            try
            {
                var propertyInfo = type.GetProperty(propertyName, flags);
                if (propertyInfo == null)
                {
                    Log.Verbose($"Property '{propertyName}' not found on type '{type.FullName}'");
                    return default;
                }

                object rawValue = propertyInfo.GetValue(null);
                if (rawValue is T value) return value;

                if (rawValue is Enum enumValue)
                {
                    var underlyingType = Enum.GetUnderlyingType(enumValue.GetType());

                    try
                    {
                        object numericValue = Convert.ChangeType(enumValue, underlyingType);
                        if (numericValue is T converted) return converted;
                    }
                    catch (Exception e)
                    {
                        Log.Verbose($"Failed to convert enum to '{underlyingType.Name}' for property '{propertyName}': {e.Message}");
                        return default;
                    }
                }

                Log.Verbose($"Property '{propertyName}' value is not of expected type '{typeof(T).FullName}'");
                return default;
            }
            catch (Exception e)
            {
                Log.Verbose($"Reflection error accessing '{type.FullName}.{propertyName}': {e.Message}");
                return default;
            }
        }

        internal static string CompressAndConvertToBase64(this string input)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(input)) return null;

                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                using var originalStream = new MemoryStream(inputBytes);
                using var compressedStream = new MemoryStream();
                using (var compressor = new GZipStream(compressedStream, CompressionLevel.Optimal))
                {
                    originalStream.CopyTo(compressor);
                    compressor.Flush();
                }
                return Convert.ToBase64String(compressedStream.ToArray());
            }
            catch (Exception e)
            {
                Log.Verbose($"Error compressing or converting string to Base64: {e.Message}");
                return null;
            }
        }
    }
}
