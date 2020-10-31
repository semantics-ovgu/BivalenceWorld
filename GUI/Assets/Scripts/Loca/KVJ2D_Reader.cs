using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomFramework.KVJ2D
{
    /*  
        Parses the following format:
        - tags (keys) must start in a new line!
     
        test_tag_00
        {
        Im test Tag _00
        And im multi line, but mostly simple. I Guess
        }

        test_tag_01 {
        Peter }

        test_tag_02 { multiline, andTags, etc. pp
        in same line }

        test_tag_03 
        { multiline, andTags in same line, ololo }
     */
    public class KVJ2D_Reader
    {
        private string[] _singleComments = { "//" };

        private string[] _commentStart = { "/*" };
        private string[] _commentEnd = { "*/" };

        private string[] _bodyStart = { "{" };
        private string[] _bodyEnd = { "}" };

        private char[] _lineSeperator = { '\n' };

        private enum State { AwaitKey, AwaitBodyStart, AwaitBodyEnd }

        private bool _multiKeys = false;

        public KVJ2D_Reader(bool multiKeys)
        {
            _multiKeys = multiKeys;
        }

        public Dictionary<string, List<KVJ2D_Data>> ParseFromFile(string fullFilePath)
        {
            if (File.Exists(fullFilePath))
            {
                return ParseFromCollection(File.ReadAllLines(fullFilePath), new ContextInfo(fullFilePath));
            }

            Debug.LogWarning($"Could not load file { fullFilePath }");
            return new Dictionary<string, List<KVJ2D_Data>>();
        }

        public Dictionary<string, List<KVJ2D_Data>> ParseFromString(string text, ContextInfo context)
        {
            var lines = text.Split(_lineSeperator, StringSplitOptions.RemoveEmptyEntries);
            return ParseFromCollection(lines, context);
        }

        public Dictionary<string, List<KVJ2D_Data>> ParseFromCollection(ICollection<string> lines, ContextInfo context)
        {   
            LineProcessor helperClass = new LineProcessor();
            bool isPartOfComment = false;

            foreach (var curLine in lines)
            {   
                var trimmedLine = curLine.Trim();

                if (trimmedLine.Length == 0)
                {
                    continue;
                }

                if (StringStartsWithAny(trimmedLine, _singleComments))
                {   
                    continue;
                }

                if (StringStartsWithAny(trimmedLine, _commentStart))
                {
                    isPartOfComment = true;
                    continue;
                }

                if (StringEndsWithAny(trimmedLine, _commentEnd))
                {
                    isPartOfComment = false;
                    continue;
                }

                if (isPartOfComment)
                {
                    continue;
                }

                helperClass.Process(this, trimmedLine, context);
            }

            return helperClass.KeyValues;
        }

        private static string ReplaceFromList(string s, string[] words, string newString)
        {
            string result = s;

            for (int i = 0; i < words.Length; i++)
            {
                result = result.Replace(words[i], newString);
            }

            return result;
        }

        private static bool StringContainsAny(string s, string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (s.Contains(words[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool StringStartsWithAny(string s, string[] words)
        {   
            for (int i = 0; i < words.Length; i++)
            {
                if (s.StartsWith(words[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool StringEndsWithAny(string s, string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (s.EndsWith(words[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public class ContextInfo
        {
            public string Message;

            public ContextInfo(string message)
            {
                Message = message;
            }
        }

        private class LineProcessor
        {
            private const string REPLACER = "";

            private State _state = State.AwaitKey;

            private string _currentKey = "";
            private string _currentBody = "";

            public Dictionary<string, List<KVJ2D_Data>> KeyValues = new Dictionary<string, List<KVJ2D_Data>>();

            public void Process(KVJ2D_Reader parser, string trimmedLine, ContextInfo context)
            {
                if (_state == State.AwaitKey)
                {
                    ProcessAwaitKey(parser, trimmedLine, context);
                }
                else if(_state == State.AwaitBodyStart)
                {
                    ProcessAwaitBodyStart(parser, trimmedLine, context);
                }
                else if(_state == State.AwaitBodyEnd)
                {
                    ProcessAwaitBodyEnd(parser, trimmedLine, context);
                }
            }

            private void ProcessAwaitKey(KVJ2D_Reader parser, string trimmedLine, ContextInfo context)
            {
                //body starts here aswell:
                if (StringContainsAny(trimmedLine, parser._bodyStart))
                {
                    var splitted = trimmedLine.Split(parser._bodyStart, StringSplitOptions.RemoveEmptyEntries);
                    _currentKey = splitted[0].Trim();

                    _state = State.AwaitBodyEnd;

                    //also has some parts of the body here:
                    if(splitted.Length > 1)
                    {
                        Debug.Assert(splitted.Length == 2);
                        _currentBody = splitted[1].Trim();

                        //key and body is already complete in that line:
                        if(StringContainsAny(_currentBody, parser._bodyEnd))
                        {
                            _currentBody = ReplaceFromList(_currentBody, parser._bodyEnd, REPLACER).Trim();
                            FinalizeKeyValue(parser, context);
                            _state = State.AwaitKey;
                        }
                    }
                }
                else
                {
                    _currentKey = trimmedLine;
                    _state = State.AwaitBodyStart;
                }
            }

            private void ProcessAwaitBodyStart(KVJ2D_Reader parser, string trimmedLine, ContextInfo context)
            {
                //we found the start of the body:
                if(StringStartsWithAny(trimmedLine, parser._bodyStart))
                {
                    _state = State.AwaitBodyEnd;

                    _currentBody = ReplaceFromList(trimmedLine, parser._bodyStart, REPLACER).Trim();

                    if(StringEndsWithAny(_currentBody, parser._bodyEnd))
                    {
                        _currentBody = ReplaceFromList(_currentBody, parser._bodyEnd, REPLACER).Trim();
                        _state = State.AwaitKey;
                        FinalizeKeyValue(parser, context);
                    }
                }
            }

            private void ProcessAwaitBodyEnd(KVJ2D_Reader parser, string trimmedLine, ContextInfo context)
            {
                //either line starts or ends with bodyEnd string
                if (StringStartsWithAny(trimmedLine, parser._bodyEnd) || StringEndsWithAny(trimmedLine, parser._bodyEnd))
                {
                    AppendBody(ReplaceFromList(trimmedLine, parser._bodyEnd, REPLACER));

                    FinalizeKeyValue(parser, context);
                    _state = State.AwaitKey;
                }
                else
                {
                    //just part of the regular body;
                    AppendBody(trimmedLine);
                }
            }

            private void AppendBody(string nextPart)
            {
                //NOTE: Adding white space might not be intended?!
                _currentBody += " " + nextPart.TrimEnd();
            }

            private void FinalizeKeyValue(KVJ2D_Reader parser, ContextInfo context)
            {
                var key = _currentKey.Trim();
                var value = _currentBody.Trim();

                List<KVJ2D_Data> list = null;

                if(!KeyValues.ContainsKey(key))
                {
                    list = new List<KVJ2D_Data>();
                    KeyValues.Add(key, list);
                }
                else
                {
                    list = KeyValues[key];
                }

                if(list.Count == 0 || parser._multiKeys)
                {
                    list.Add(new KVJ2D_Data(key, value));
                }
                else
                {   
                    Debug.LogError($"Multiple same keys found {key}. Context: { context.Message }");
                }

                _currentKey = "";
                _currentBody = "";
            }
        }
    }
}
