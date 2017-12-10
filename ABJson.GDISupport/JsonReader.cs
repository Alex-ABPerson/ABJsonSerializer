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
            char EndChar = '"';
            bool Building = false;
            bool hasFinishedName = false;
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
                            case '\'':
                                if (ch == EndChar) {
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
                            case ':':
                            case ',':                              
                                if (!IsInValue)
                                {
                                    Building = false;
                                    if (hasFinishedName) jkvp.value = BuildUp; else jkvp.name = BuildUp;
                                    BuildUp = "";
                                    if (ch == ':') hasFinishedName = true;
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
                                try { if (hasFinishedName) if (string.IsNullOrEmpty(jkvp.value.ToString())) jkvp.value = BuildUp; } catch { }
                                break; // This is here so it doesn't go into the default!
                            case '"':
                            case '\'':
                            case '{':
                            case '[':                                                      
                                IsInValue = true;
                                Building = true;
                                if (ch == '"' || ch == '\'') EndChar = ch; else if (ch == '{') EndChar = '}'; else if (ch == '[') EndChar = ']';
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
            List<string> arrayResult = new List<string>();

            int innerLevel = 0;
            string BuildUp = "";
            bool Building = false;
            bool nextIsEscape = false; // Used to skip \" and \'

            foreach (char ch in json)
            {
                if (nextIsEscape)
                {
                    nextIsEscape = false;
                    if (Building)
                    {
                        if (ch == '"' || ch == '\'') BuildUp = BuildUp.Substring(0, BuildUp.Length - 1); // Removes the \ if it's a " or ' (C# automatically puts a \" and \' in the string if needed.)
                        BuildUp += ch;
                    }
                } else {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            if (innerLevel == 0) Building = true;
                            innerLevel += 1;
                            break;
                        case '}':
                        case ']':                          
                            innerLevel -= 1;
                            break;
                        case ',':
                            if (innerLevel == 0)
                                if (Building)
                                {
                                    arrayResult.Add(BuildUp);
                                    BuildUp = "";
                                }
                            break;
                        case '"':
                        case '\'':
                            if (innerLevel == 0)
                            {
                                if (Building)
                                {
                                    Building = false;
                                    arrayResult.Add(BuildUp);
                                    BuildUp = "";
                                }
                                else
                                {
                                    Building = true;
                                    BuildUp = "";
                                }
                                continue;
                            }
                            break;
                        default:
                            // It may be a number or null or false or true!
                            if (!char.IsWhiteSpace(ch))
                                Building = true; // Build whatever it is.

                            break;

                    }
                    BuildUp += ch;
                }
            }
           
            BuildUp = BuildUp.Trim().TrimStart(',');
            arrayResult.Add(BuildUp); // Add the final one!

            string[] ret = arrayResult.ToArray();
            if (string.IsNullOrEmpty(ret[ret.Length - 1])) Array.Resize(ref ret, ret.Length - 1);

            return ret;
        }

        public static string[] GetAllKeyValues(string json)
        {
            List<string> arrayResult = new List<string>();
            string buildUp = "";

            int innerLevel = 0; // This is so if (for example) we have an array like this: [["Hello1-1", "Hello1-2"], ["Hello2-1", "Hello2-2"]] it doesn't stop at the first square bracket like this: [["Hello1-1", "Hello1-2"]
            bool IsInValue = false;
            bool IsInName = false;
            bool IsInString = false;
            bool hasFinishedName = false;
            bool receivedCommentFirstChar = false;
            bool IsInComment = false;
            bool CommentIsNewLineEnding = false;

            char EndChar = '"';
            char StringType = '"';

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
                                if (!IsInName)
                                    if (!IsInString)
                                        if (receivedCommentFirstChar)
                                        { // This is a "//" comment!
                                            IsInComment = true;
                                            CommentIsNewLineEnding = true;
                                            receivedCommentFirstChar = false;
                                        }
                                        else receivedCommentFirstChar = true;

                                break;
                            case '*':
                                if (!IsInName)
                                    if (!IsInValue)
                                        if (receivedCommentFirstChar)
                                        { // This is a "/*" comment!
                                            IsInComment = true;
                                            CommentIsNewLineEnding = false;
                                        }

                                break;
                            case '\'':
                            case '"':
                                receivedCommentFirstChar = false;

                                if (IsInString)
                                    if (StringType == ch)
                                        IsInString = false;
                                else
                                {
                                    IsInString = true;
                                    StringType = ch;
                                }

                                if (hasFinishedName)
                                    if (IsInValue)
                                    {
                                        if (EndChar == ch) IsInValue = false;
                                    }
                                    else
                                    {
                                        IsInValue = true;
                                        EndChar = ch;
                                    }
                                else
                                    if (IsInName)
                                    {
                                        if (EndChar == ch) IsInName = false;
                                    }
                                    else { IsInName = true; EndChar = ch; }

                                break;
                            case ':':
                                receivedCommentFirstChar = false;
                                if (!IsInValue) hasFinishedName = true;
                                break;
                            case '[':
                                receivedCommentFirstChar = false;
                                if (IsInValue)
                                    innerLevel += 1;
                                else
                                    if (hasFinishedName) { IsInValue = true; EndChar = ']'; }
                                break;
                            case '{':
                                receivedCommentFirstChar = false;
                                if (IsInValue)
                                    innerLevel += 1;
                                else
                                    if (hasFinishedName) { IsInValue = true; EndChar = '}'; }
                                break;
                            case ']':
                            case '}':
                                receivedCommentFirstChar = false;
                                if (innerLevel == 0)
                                {
                                    if (hasFinishedName && EndChar == ch) IsInValue = false;
                                }
                                else
                                    innerLevel -= 1;                                
                                break;
                            case ',':
                                receivedCommentFirstChar = false;
                                // Finish the buildup and reset a bunch of stuff..

                                if (!IsInValue)
                                {
                                    buildUp = buildUp.Trim().TrimStart(',');
                                    buildUp += ",";
                                    arrayResult.Add(buildUp);
                                    buildUp = "";
                                    hasFinishedName = false;
                                    IsInValue = false;
                                    IsInName = false;
                                }
                                
                                break;
                            default:
                                receivedCommentFirstChar = false;
                                break;

                            //default:
                            //    // It may be a number or null or false or true!
                            //    if (!IsInName && !IsInValue)
                            //        if (hasFinishedName) IsInValue = true; else IsInName = true; // Build whatever it is.

                            //    break;
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
