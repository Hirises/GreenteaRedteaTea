using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;

public static class CustomerNameBook
{
    private const string NamePath = "res://Data/CustomerNames.json";
    private const string DefaultCustomerClassName = "DefaultCustomer";
    private const string DefaultName = "Default";

    private static readonly Dictionary<string, List<string>> EmptyNames = new();
    private static readonly IReadOnlyList<string> EmptyLines = Array.Empty<string>();
    private static readonly RandomNumberGenerator Random = new();
    private static Dictionary<string, List<string>> names;

    public static string GetName(string customerClassName)
    {
        var lines = GetNames(customerClassName);

        if (lines.Count == 0 && customerClassName != DefaultCustomerClassName)
        {
            lines = GetNames(DefaultCustomerClassName);
        }

        if (lines.Count == 0)
        {
            return DefaultName;
        }

        var index = lines.Count == 1 ? 0 : Random.RandiRange(0, lines.Count - 1);
        return lines[index];
    }

    private static IReadOnlyList<string> GetNames(string customerClassName)
    {
        var loadedNames = LoadNames();
        return loadedNames.TryGetValue(customerClassName, out var customerNames)
            ? customerNames
            : EmptyLines;
    }

    private static Dictionary<string, List<string>> LoadNames()
    {
        if (names != null)
        {
            return names;
        }

        if (!FileAccess.FileExists(NamePath))
        {
            GD.PushWarning($"Customer name file not found: {NamePath}");
            names = EmptyNames;
            return names;
        }

        var json = FileAccess.GetFileAsString(NamePath);

        try
        {
            names = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? EmptyNames;
        }
        catch (JsonException exception)
        {
            GD.PushError($"Failed to parse customer name file: {NamePath}. {exception.Message}");
            names = EmptyNames;
        }

        return names;
    }
}
