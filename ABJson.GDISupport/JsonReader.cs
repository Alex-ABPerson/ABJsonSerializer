using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABJson.GDISupport
{
    public static class JsonReader
    {
        public static JsonKeyValuePair GetKeyValueData(string json)
        {
            JsonKeyValuePair jkvp = new JsonKeyValuePair();
            string BuildUp = "";
            bool Building = false;
            bool hasFinishedName = false;
            bool SingleQuoteString = false;
            bool IsInValue = false;
            bool nextIsEscape = false; // Used to skip \" and \'

            foreach (char ch in json)
            {
                if (nextIsEscape)
                {
                    nextIsEscape = false;
                    if (Building) {
                        if (ch == '"' || ch == '\'') BuildUp = BuildUp.Substring(0, BuildUp.Length - 1); // Removes the "\" if it's a " or '
                        BuildUp += ch;
                    }
                } 
                else
                {
                    if (Building)
                    {

                        switch (ch)
                        {
                            case '}':
                            case ']':
                            case '"':
                                if (!SingleQuoteString)
                                {
                                    IsInValue = false;
                                    Building = false;
                                    if (hasFinishedName) jkvp.value = BuildUp; else jkvp.name = BuildUp;
                                    BuildUp = "";
                                    continue;
                                }
                                break;
                            case '\'':
                                if (SingleQuoteString)
                                {
                                    IsInValue = false;
                                    Building = false;
                                    if (hasFinishedName) jkvp.value = BuildUp; else jkvp.name = BuildUp;
                                    BuildUp = "";
                                    continue;
                                }
                                break;
                            case '\\':
                                nextIsEscape = true;
                                break;
                            case ',':
                                if (!IsInValue)
                                {
                                    Building = false;
                                    if (hasFinishedName) jkvp.value = BuildUp; else jkvp.name = BuildUp;
                                    BuildUp = "";
                                    continue;
                                }
                                break;
                        }
                        BuildUp += ch;
                    }
                    else
                    {
                        switch (ch)
                        {
                            case ',':
                                break; // This is here so it doesn't go into the default!
                            case '"':
                            case '{':
                            case '[':
                                SingleQuoteString = false;
                                IsInValue = true;
                                Building = true;
                                break;
                            case '\'':
                                SingleQuoteString = true;
                                IsInValue = true;
                                Building = true;
                                break;
                            case ':':
                                hasFinishedName = true;
                                break;
                            default:
                                if (!char.IsWhiteSpace(ch))
                                {
                                    Building = true;
                                    BuildUp += ch.ToString();
                                }

                                break;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(BuildUp))
            {
                Building = false;
                if (hasFinishedName) jkvp.value = BuildUp; else jkvp.name = BuildUp;
                BuildUp = "";
            }

            return jkvp;
        }

        public static string[] GetAllValuesInArray(string json)
        {
            throw new NotImplementedException();
        }

        public static string[] GetAllKeyValues(string json)
        {
            List<string> arrayResult = new List<string>();
            string buildUp = "";

            bool IsInValue = false;
            bool IsInName = false;
            bool hasFinishedName = false;
            bool receivedCommentFirstChar = false;
            bool IsInComment = false;
            bool CommentIsNewLineEnding = false;

            foreach (char ch in json)
            {
                if (IsInComment)
                {
                    switch (ch)
                    {
                        case '\n':
                            if (CommentIsNewLineEnding)
                                IsInComment = false;
                            break;
                        case '/':
                            if (receivedCommentFirstChar)
                                IsInComment = false;
                            break;
                        case '*':
                            receivedCommentFirstChar = true;
                            break;
                        default:
                            receivedCommentFirstChar = false;
                            break;
                    }
                } else {
                    if (!char.IsWhiteSpace(ch))
                        switch (ch)
                        {
                            case '/':
                                if (!IsInValue)
                                {
                                    if (receivedCommentFirstChar)
                                    { // This is a "//" comment!
                                        IsInComment = true;
                                        CommentIsNewLineEnding = true;
                                        receivedCommentFirstChar = false;
                                    }
                                    else receivedCommentFirstChar = true;
                                }
                                break;
                            case '*':
                                if (!IsInValue)
                                {
                                    if (receivedCommentFirstChar)
                                    {
                                        IsInComment = true;
                                        CommentIsNewLineEnding = false;
                                    }
                                }
                                break;
                            case '"':
                                if (hasFinishedName) if (IsInValue) IsInValue = false; else IsInValue = true;
                                else
                                    if (IsInName) IsInName = false; else IsInName = true;
                                break;
                            case ':':
                                if (!IsInValue) hasFinishedName = true;
                                break;
                            case '[':
                            case '{':
                                if (hasFinishedName) IsInValue = true;
                                break;
                            case ']':
                            case '}':
                                if (hasFinishedName) IsInValue = false;
                                break;
                            case ',':
                                // Finish the buildup and reset a bunch of stuff..

                                if (!IsInValue)
                                {
                                    buildUp = buildUp.Trim().TrimStart(',');
                                    buildUp += ",";
                                    arrayResult.Add(buildUp);
                                    buildUp = "";
                                }

                                hasFinishedName = false;
                                IsInValue = false;
                                IsInName = false;
                                break;
                        }

                    buildUp += ch;
                }
            }
            buildUp = buildUp.Trim().TrimStart(',');
            arrayResult.Add(buildUp); // Add the final one!

            return arrayResult.ToArray();
        }
    }
}
