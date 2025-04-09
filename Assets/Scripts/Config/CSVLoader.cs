using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class CSVLoader
{
    private static char _columnSeperator = ',';

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

            string[] headerNames = headerLine.Split(_columnSeperator);

            // Read each row of data
            string line;
            string[] tokensInLine;
            while ((line = reader.ReadLine()) != null)
            {
                tokensInLine = line.Split(_columnSeperator);
                if (tokensInLine.Length < 2) // Skip the last empty row or empty row
                {
                    continue;
                }

                TData data = ParseRow<TData>(headerNames, tokensInLine);
                dataList.Add(data);
            }
        }

        return dataList;
    }

    private static TData ParseRow<TData>(string[] headerNames, string[] tokensInLine) where TData : new()
    {
        TData data = new TData();
        var type = typeof(TData);

        for (int i = 0; i < headerNames.Length; i++)
        {
            var field = type.GetProperty(headerNames[i]);

            // Ignore non-used headers since not every header is needed (example: comment or order column)
            if (field == null)
            {
                continue;
            }

            object parsedValue = SafeParser.ParseValue(field.PropertyType, tokensInLine[i]);
            // if parsedValue is null and this field is a value-type then the default value of this type will be used for this field
            field.SetValue(data, parsedValue);
        }

        return data;
    }
}
