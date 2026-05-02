using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

#nullable enable

namespace RedteaGreenteaTea.Domain;

public sealed class ImpossibleStringCatalog
{
	private const string DefaultJsonPath = "res://Scripts/Domain/ImpossibleStrings.json";

	private static ImpossibleStringCatalog? _current;

	private ImpossibleStringCatalog(IReadOnlyList<string> strings)
	{
		if (strings.Count == 0)
		{
			throw new ArgumentException("At least one impossible string is required.", nameof(strings));
		}

		Strings = strings;
	}

	public static ImpossibleStringCatalog Current => _current ??= LoadFromFile(DefaultJsonPath);

	public IReadOnlyList<string> Strings { get; }

	public static void Configure(ImpossibleStringCatalog catalog)
	{
		_current = catalog ?? throw new ArgumentNullException(nameof(catalog));
	}

	public static void ConfigureFromFile(string path)
	{
		_current = LoadFromFile(path);
	}

	public static ImpossibleStringCatalog LoadFromFile(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			throw new ArgumentException("Path cannot be empty.", nameof(path));
		}

		var json = ReadAllText(path);
		var settings = JsonSerializer.Deserialize<ImpossibleStringSettings>(json, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		});

		if (settings is null)
		{
			throw new InvalidOperationException($"Failed to read impossible strings from {path}.");
		}

		var strings = new List<string>();
		foreach (var value in settings.Strings)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				strings.Add(value);
			}
		}

		return new ImpossibleStringCatalog(strings);
	}

	private static string ReadAllText(string path)
	{
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase)
			|| path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			if (!Godot.FileAccess.FileExists(path))
			{
				throw new FileNotFoundException($"Could not find {path}.");
			}

			return Godot.FileAccess.GetFileAsString(path);
		}

		return File.ReadAllText(path);
	}

	public sealed record ImpossibleStringSettings(IReadOnlyList<string> Strings);
}
