using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CosmoLibrary.SqlDriver
{
    public static class DocumentDbAccount
{
    public static IDocumentClient Parse(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be empty.");
        }

        if(ParseImpl(connectionString, out var ret, err => throw new FormatException(err)))
        {
            return ret;
        }

        throw new ArgumentException($"Connection string was not able to be parsed into a document client.");
    }

    public static bool TryParse(string connectionString, out IDocumentClient documentClient)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            documentClient = null;
            return false;
        }

        try
        {
            return ParseImpl(connectionString, out documentClient, err => { });
        }
        catch (Exception)
        {
            documentClient = null;
            return false;
        }
    }

    private const string AccountEndpointKey = "AccountEndpoint";
    private const string AccountKeyKey = "AccountKey";
    private static readonly HashSet<string> RequireSettings = new HashSet<string>(new [] { AccountEndpointKey, AccountKeyKey }, StringComparer.OrdinalIgnoreCase);

    internal static bool ParseImpl(string connectionString, out IDocumentClient documentClient, Action<string> error)
    {
        IDictionary<string, string> settings = ParseStringIntoSettings(connectionString, error);

        if (settings == null)
        {
            documentClient = null;
            return false;
        }

        if (!RequireSettings.IsSubsetOf(settings.Keys))
        {
            documentClient = null;
            return false;
        }

        documentClient = new DocumentClient(new Uri(settings[AccountEndpointKey]), settings[AccountKeyKey]);
        return true;
    }

    /// <summary>
    /// Tokenize input and stores name value pairs.
    /// </summary>
    /// <param name="connectionString">The string to parse.</param>
    /// <param name="error">Error reporting delegate.</param>
    /// <returns>Tokenize collection.</returns>
    private static IDictionary<string, string> ParseStringIntoSettings(string connectionString, Action<string> error)
    {
        IDictionary<string, string> settings = new Dictionary<string, string>();
        var split = connectionString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var nameValue in split)
        {
            var splitNameValue = nameValue.Split(new[] { '=' }, 2);

            if (splitNameValue.Length != 2)
            {
                error("Settings must be of the form \"name=value\".");
                return null;
            }

            if (settings.ContainsKey(splitNameValue[0]))
            {
                error(string.Format(CultureInfo.InvariantCulture, "Duplicate setting '{0}' found.", splitNameValue[0]));
                return null;
            }

            settings.Add(splitNameValue[0], splitNameValue[1]);
        }

        return settings;
    }
}
}
