using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class SafeParser
{
    public static object ParseValue(Type type, string value)
    {
        if (type == typeof(int)) return ParseInt(value);
        if (type == typeof(float)) return ParseFloat(value);
        if (type == typeof(bool)) return ParseBool(value);
        if (type == typeof(string)) return value;
        if (type == typeof(Vector3)) return ParseVector3(value);

        Debug.LogError($"SafeParser: Unsupported type: {type.Name}, using the default value of this type");
        return null;
    }

    private static int ParseInt(string value, int defaultValue = 0)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }

        Debug.LogError($"SafeParser: Invalid int: {value}, using default: {defaultValue}");
        return defaultValue;
    }

    private static float ParseFloat(string value, float defaultValue = 0f)
    {
        if (float.TryParse(value, out float result))
        {
            return result;
        }

        Debug.LogError($"SafeParser: Invalid float: {value}, using default: {defaultValue}");
        return defaultValue;
    }

    private static bool ParseBool(string value, bool defaultValue = false)
    {
        if (bool.TryParse(value, out bool result))
        {
            return result;
        }

        Debug.LogError($"SafeParser: Invalid bool: {value}, using default: {defaultValue}");
        return defaultValue;
    }

    private static Vector3 ParseVector3(string value)
    {
        return ParseVector3(value, Vector3.zero);
    }

    private static Vector3 ParseVector3(string value, Vector3 fallbackValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError($"SafeParser: Empty Vector3 string, using fallback value {fallbackValue}");
            return Vector3.zero;
        }

        string[] vectorParts = value.Replace(" ", "").Trim('(', ')').Split(';');

        if (vectorParts.Length != 3)
        {
            Debug.LogError($"SafeParser: Invalid Vector3 format: {value}, using fallback value {fallbackValue}");
            return Vector3.zero;
        }

        float x = float.Parse(vectorParts[0]);
        float y = float.Parse(vectorParts[1]);
        float z = float.Parse(vectorParts[2]);

        return new Vector3(x, y, z);
    }
}
