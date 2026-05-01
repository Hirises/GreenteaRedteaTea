using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;

public static class CustomerDialogueBook
{
    private const string DialoguePath = "res://Data/CustomerDialogues.json";
    private const string DefaultCustomerClassName = "DefaultCustomer";
    private const string OrderToken = "{order}";

    private static readonly Dictionary<string, CustomerDialogue> EmptyDialogues = new();
    private static readonly IReadOnlyList<string> EmptyLines = Array.Empty<string>();
    private static readonly RandomNumberGenerator Random = new();
    private static Dictionary<string, CustomerDialogue> dialogues;

    public static string GetOrder(string customerClassName, string orderName)
    {
        return GetLine(customerClassName, dialogue => dialogue.Order, OrderToken + " please")
            .Replace(OrderToken, orderName);
    }

    public static string GetThank(string customerClassName)
    {
        return GetLine(customerClassName, dialogue => dialogue.Thank, "thx");
    }

    public static string GetComplaint(string customerClassName, OrderResult result)
    {
        return GetLine(customerClassName, dialogue => GetComplaintLines(dialogue, result), "");
    }

    private static string GetLine(
        string customerClassName,
        Func<CustomerDialogue, IReadOnlyList<string>> lineSelector,
        string fallback)
    {
        var dialogue = GetDialogue(customerClassName);
        var lines = dialogue == null ? EmptyLines : lineSelector(dialogue) ?? EmptyLines;

        if (lines.Count == 0 && customerClassName != DefaultCustomerClassName)
        {
            dialogue = GetDialogue(DefaultCustomerClassName);
            lines = dialogue == null ? EmptyLines : lineSelector(dialogue) ?? EmptyLines;
        }

        if (lines.Count == 0)
        {
            return fallback;
        }

        var index = lines.Count == 1 ? 0 : Random.RandiRange(0, lines.Count - 1);
        return lines[index];
    }

    private static CustomerDialogue GetDialogue(string customerClassName)
    {
        var loadedDialogues = LoadDialogues();
        return loadedDialogues.TryGetValue(customerClassName, out var dialogue) ? dialogue : null;
    }

    private static Dictionary<string, CustomerDialogue> LoadDialogues()
    {
        if (dialogues != null)
        {
            return dialogues;
        }

        if (!FileAccess.FileExists(DialoguePath))
        {
            GD.PushWarning($"Customer dialogue file not found: {DialoguePath}");
            dialogues = EmptyDialogues;
            return dialogues;
        }

        var json = FileAccess.GetFileAsString(DialoguePath);

        try
        {
            dialogues = JsonSerializer.Deserialize<Dictionary<string, CustomerDialogue>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? EmptyDialogues;
        }
        catch (JsonException exception)
        {
            GD.PushError($"Failed to parse customer dialogue file: {DialoguePath}. {exception.Message}");
            dialogues = EmptyDialogues;
        }

        return dialogues;
    }

    private static IReadOnlyList<string> GetComplaintLines(CustomerDialogue dialogue, OrderResult result)
    {
        return result switch
        {
            OrderResult.Timeout => dialogue.Complaint?.Timeout ?? EmptyLines,
            OrderResult.WrongMenu => dialogue.Complaint?.WrongMenu ?? EmptyLines,
            OrderResult.KickedOut => dialogue.Complaint?.KickedOut ?? EmptyLines,
            _ => EmptyLines
        };
    }

    public sealed class CustomerDialogue
    {
        public List<string> Order { get; set; } = new();
        public List<string> Thank { get; set; } = new();
        public CustomerComplaintDialogue Complaint { get; set; } = new();
    }

    public sealed class CustomerComplaintDialogue
    {
        public List<string> Timeout { get; set; } = new();
        public List<string> WrongMenu { get; set; } = new();
        public List<string> KickedOut { get; set; } = new();
    }
}
