public class SearchCriteria
{
    public string FreeText { get; set; } = "";
    public Dictionary<string, string> Filters { get; set; } = new();
}

public SearchCriteria ParseWithoutRegex(string input)
{
    var criteria = new SearchCriteria();
    var filters = new Dictionary<string, string>();

    var tokens = Tokenize(input);
    var freeTextParts = new List<string>();

    int i = 0;
    while (i < tokens.Count)
    {
        if (i + 2 < tokens.Count && (tokens[i + 1] == ":" || tokens[i + 1] == "="))
        {
            string key = NormalizeKey(tokens[i]);
            string value = tokens[i + 2].Trim();

            // Merge list values (e.g., 1,2,3) until next token is a key or separator
            while (i + 3 < tokens.Count && tokens[i + 3] != ":" && tokens[i + 3] != "=")
            {
                if (tokens[i + 3] == "," || value.EndsWith(","))
                {
                    value += ",";
                }
                else
                {
                    value += " " + tokens[i + 3];
                }
                i++;
            }

            filters[key] = value.Trim().Trim(',');
            i += 3;
        }
        else
        {
            freeTextParts.Add(tokens[i]);
            i++;
        }
    }

    criteria.FreeText = string.Join(" ", freeTextParts).Trim();
    criteria.Filters = filters;
    return criteria;
}

private string NormalizeKey(string key)
{
    return key.Trim().ToLower().Replace(" ", "");
}

// Tokenizer that splits on space, preserves quoted values, and separates : and =
private List<string> Tokenize(string input)
{
    var tokens = new List<string>();
    var sb = new StringBuilder();
    bool inQuotes = false;

    for (int i = 0; i < input.Length; i++)
    {
        char c = input[i];

        if (c == '"')
        {
            inQuotes = !inQuotes;
            continue;
        }

        if (!inQuotes && (c == ':' || c == '='))
        {
            if (sb.Length > 0)
            {
                tokens.Add(sb.ToString());
                sb.Clear();
            }
            tokens.Add(c.ToString());
        }
        else if (!inQuotes && char.IsWhiteSpace(c))
        {
            if (sb.Length > 0)
            {
                tokens.Add(sb.ToString());
                sb.Clear();
            }
        }
        else
        {
            sb.Append(c);
        }
    }

    if (sb.Length > 0)
    {
        tokens.Add(sb.ToString());
    }

    return tokens;
}
