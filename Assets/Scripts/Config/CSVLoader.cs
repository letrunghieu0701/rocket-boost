using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class CSVLoader
{
    public static List<TData> LoadConfig<TData>(string filePath) where TData : new()
    {
        List<TData> dataList = new List<TData>();

        // Load file from Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);
        if (csvFile == null)
        {
            Debug.LogError($"CSVLoader: Cannot find file at Resources/{filePath}.csv");
            return dataList;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string headerLine = reader.ReadLine();
            if (string.IsNullOrEmpty(headerLine))
            {
                Debug.LogError($"CSVLoader: {filePath}.csv is empty!");
                return dataList;
            }

            string[] headers = headerLine.Split(',');

            // Read each row of data
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] lineValues = line.Split(',');
                if (lineValues.Length < 2) // Skip the last empty row or empty row
                {
                    continue;
                }
                TData data = ParseRow<TData>(headers, lineValues);
                dataList.Add(data);
            }
        }

        return dataList;
    }

    private static TData ParseRow<TData>(string[] headers, string[] lineValues) where TData : new()
    {
        TData data = new TData();
        var type = typeof(TData);

        for (int i = 0; i < headers.Length; i++)
        {
            var field = type.GetProperty(headers[i]);
            if (field == null) // Ignore empty cell value since not every line needs to be filled out
            {
                continue;
            }

            object parsedValue = ConvertValue(field.PropertyType, lineValues[i]);
            field.SetValue(data, parsedValue);
        }

        return data;
    }

    private static object ConvertValue(Type type, string value)
    {
        if (string.IsNullOrEmpty(value))
            return type.IsValueType ? Activator.CreateInstance(type) : null;

        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        if (type == typeof(string)) return value;
        if (type == typeof(Vector3)) return ParseVector3(value);

        return null;
    }

    private static Vector3 ParseVector3(string value)
    {
        string[] parts = value.Trim('(', ')').Replace(" ", "").Split(';');
        if (parts.Length != 3)
        {
            Debug.LogError($"CSVLoader: Invalid Vector3 format: {value}");
            return Vector3.zero;
        }

        float x = float.Parse(parts[0]);
        float y = float.Parse(parts[1]);
        float z = float.Parse(parts[2]);

        return new Vector3(x, y, z);
    }
}
