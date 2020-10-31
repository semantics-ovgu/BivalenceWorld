using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomFramework.KVJ2D;
using UnityEngine;

namespace CustomFramework.Localization
{   
    public class LocalizationFile : ScriptableObject
    {
        [SerializeField]
        private List<DataEntry> _data = default;
        public List<DataEntry> GetData() { return _data; }

#if UNITY_EDITOR
        [SerializeField]
        private string _workingFileAssetPath;
        public string WorkingFileAssetPath => _workingFileAssetPath;

        public void SetWorkingFilePath(string workingFileAssetPath)
        {
            _workingFileAssetPath = workingFileAssetPath;
        }
#endif

        public void SetData(Dictionary<string, List<KVJ2D_Data>> parsedData)
        {
            if (_data == null)
            {
                _data = new List<DataEntry>();
            }
            else
            {
                _data.Clear();
            }

            foreach (var key in parsedData.Keys)
            {
                var list = parsedData[key];

                if(list.Count != 1)
                {
                    Debug.LogError($"Localization files should only have one data entry for keys. But key { key } had { list.Count }");
                    continue;
                }

                _data.Add(new DataEntry(list[0]));
            }
        }

        [System.Serializable]
        public class DataEntry
        {
            public string Key;
            public string Value;
            public List<Tag> Tags;

            public DataEntry(KVJ2D_Data keyValue_J2D)
            {
                Key = keyValue_J2D.Key;
                Value = keyValue_J2D.Value;
                Tags = FindTags(Value);
            }

            private static List<Tag> FindTags(string s)
            {
                List<Tag> tags = new List<Tag>();
                List<int> tagIndices = new List<int>();

                int curIndex = 0;
                foreach (char c in s)
                {
                    if (c == '#')
                    {
                        tagIndices.Add(curIndex);
                    }

                    curIndex++;
                }

                for (int i = 0; i < tagIndices.Count; i += 2)
                {   
                    int start = tagIndices[i];

                    //happens if there is an odd number of '#', in this case don't use it as a tag
                    if (i + 1 >= tagIndices.Count)
                    {
                        break;
                    }

                    int end = tagIndices[i + 1];
                    int tagLength = (end - start) + 1;
                    var tag = s.Substring(start, tagLength);
                    tags.Add(new Tag(tag, start));
                }
                
                return tags;
            }

            [System.Serializable]
            public class Tag
            {
                public string TagString;
                public int TagStartIndex;

                public Tag(string tag, int tagStartIndex)
                {
                    TagString = tag;
                    TagStartIndex = tagStartIndex;
                }
            }
        }
    }
}