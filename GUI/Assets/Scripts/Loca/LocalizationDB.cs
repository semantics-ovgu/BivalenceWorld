using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomFramework.Localization
{
 [CreateAssetMenu()]
	public class LocalizationDB : ScriptableObject
    {   
        public enum ELanguage
        {
            /// <summary> German </summary>
            de,
            /// <summary> English </summary>
            en,
        }

        private static LocalizationDB _instance;

        public static LocalizationDB Instance
        {
            get
            {
                if (CheckInstance())
                {
                    return _instance;
                }

                return null;
            }
        }

        [SerializeField]
        private ELanguage _currentLanguage = ELanguage.de;

        [SerializeField]
        private ELanguage _oldLanguage = ELanguage.de;


        [SerializeField]
        private string _resourcesBasePath = default;

#if UNITY_EDITOR
        [Header("Editor")]
        [SerializeField]
        private Editor.LocalizationFileImporter.Settings _importSettings = default;
        public Editor.LocalizationFileImporter.Settings GetImportSettings() => _importSettings;

        [SerializeField, HideInInspector]
        private string _cachedGeneratedFolderPath;
#endif

        private LanguageInfo _curLanguageInfo;
        public LanguageInfo CurrentLanguageInfo => _curLanguageInfo;


        /// <summary> Maps "LocFilePath" and "LocStringKey" to the actual localized string. </summary>
        private Dictionary<string, Dictionary<string, LocalizedEntry>> _loadedLocalizedStrings;

        /// <summary> Called after a language has been (re)loaded or changed. </summary>
        public GenericEvent<LocalizationDB> LanguageLoadedEvent = new GenericEvent<LocalizationDB>();

        private static char[] _fullPathSplitChar = { '.' };

        //TODO: fix errors/warnings when opening a project for the first time, maybe with this:
        //private void OnEnable() { UnityEditor.EditorApplication.update += EditorUpdate; }

        private void OnValidate()
        {
#if UNITY_EDITOR
            var generatedFolderPath = _importSettings.GetGeneratedFolderPath();

            if (_cachedGeneratedFolderPath != generatedFolderPath)
            {
                _cachedGeneratedFolderPath = generatedFolderPath;
                var indexOfLastSlash = generatedFolderPath.LastIndexOf("/");
                _resourcesBasePath = generatedFolderPath.Substring(indexOfLastSlash + 1, generatedFolderPath.Length - indexOfLastSlash - 1) + "/";
            }
#endif

	        if (_currentLanguage != _oldLanguage)
	        {
		        _oldLanguage = _currentLanguage;
                ReloadCurrentLanguage();
	        }

        }

        private void Init()
        {
            LoadLanguageSettingsFromFile();
            LoadLanguage(_currentLanguage);
        }

        public void LoadLanguage(ELanguage lang)
        {
            //just get rid of all loaded data (will be reimported anyway when calling a certain string):
            _loadedLocalizedStrings = new Dictionary<string, Dictionary<string, LocalizedEntry>>();
            WriteLanguageSettingsFile(_currentLanguage, lang);

            _currentLanguage = lang;
            _curLanguageInfo = new LanguageInfo(lang, LoadCultureInfo(lang));

            LanguageLoadedEvent.InvokeEvent(this);
        }

        /// <summary>
        /// Reloades the current loaded languages. Useful if localized texts have changed.
        /// </summary>
        public void ReloadCurrentLanguage()
        {
            LoadLanguage(_currentLanguage);
        }

        /// <summary>
        /// Returns a localized string if it exsists. Returns a warning string instead.
        /// </summary>
        /// <param name="fullPath"> Contains the path and the key for example: "Path0/.../Path.key" or "Path.key" </param>
        public string LocalizeFullPath(string fullPath, object context)
        {

            if (!CreateLocaStrings(fullPath, out string path, out string key, context))
            {
                return "INVALID_FULL_PATH: " + fullPath;
            }

            return Localize(path, key, context);
        }

        /// <summary>
        /// Returns a localized string if it exsists. Returns a warning string instead.
        /// </summary>
        /// <param name="path"> Path and name of a localization file. Basically the path after "Resources/.../LocFiles/" </param>
        public string Localize(string path, string key, object context)
        {
            BuildLocaStringsFor(ref path, ref key);

            TryInitLoadedLocalizedStrings();

            if (!_loadedLocalizedStrings.ContainsKey(path))
            {
                if (!TryLoadLocalizationFile(path, key, context))
                {
                    return LogWarning("LOC_FILE_PATH_NOT_FOUND: " + path, "Container at path '" + path + "' not found! Key was: '" + key + "'", context);
                }
            }

            var container = _loadedLocalizedStrings[path];

            if (!container.ContainsKey(key))
            {
                return LogWarning("KEY_NOT_FOUND: " + key, "Key '" + key + "' not found! Path was: '" + path + "'", context);
            }

            return container[key].Text;
        }

        private bool TryLoadLocalizationFile(string containerPath, string locaKey, object context)
        {
            if (_loadedLocalizedStrings.ContainsKey(containerPath))
            {
                Debug.LogWarning("LocFile with name " + containerPath + " is already loaded!");
                return true;
            }

            var locFile = LoadLocaFileFromResources(containerPath);

            if (locFile)
            {
                var data = locFile.GetData();
                var locaFileDict = new Dictionary<string, LocalizedEntry>();

                for (int i = 0; i < data.Count; i++)
                {
                    locaFileDict.Add(data[i].Key, CreateLocalizedData(data[i]));
                }
                
                //TODO: sometimes throws errors, even though it was catched above?! Vermutung: passiert beim Szenenwechsel, wenn die alte Loca zerstört wurde und bei OnValidate was aufgerufen wird?
                try
                {
                    _loadedLocalizedStrings.Add(containerPath, locaFileDict);
                }
                catch
                {
                    LogWarning("?!", "LocaFile at path '" + containerPath + "' was already loaded?! Key was: " + locaKey, context);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Builds loca path and key for the current language. The parameter <param name="fullPath"></param> should NOT contain the language suffix!
        /// </summary>
        public bool CreateLocaStrings(string fullPath, out string path, out string key, object context)
        {
            var parts = fullPath.Split(_fullPathSplitChar, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                LogWarning("INVALID_FULL_PATH: " + fullPath, "FullPath should contain the path AND the localization key, seperated by a '.' E.g. 'Prepath/Path.testKey'. But fullPath was: '" + fullPath + "'", context);
                path = "";
                key = "";
                return false;
            }

            path = parts[0];
            key = parts[1];
            return true;
        }

        /// <summary>
        /// Builds loca path and key for the current language. The parameter <param name="path"></param> should NOT contain the language suffix!
        /// </summary>
        public void BuildLocaStringsFor(ref string path, ref string key)
        {
            //TODO: maybe rather use assertions / warnigns, instead of "hiding" a problem:
            if (path == null)
            {
                path = "";
            }

            //building the path for the associated loca file:
            path = path.Trim() + "_" + _currentLanguage.ToString();

            //TODO: maybe rather use assertions / warnigns, instead of "hiding" a problem:
            if (key == null)
            {
                key = "";
            }

            key = key.Trim();
        }

        public LocalizationFile LoadLocaFileFromResources(string locaPath)
        {
            var locFile = Resources.Load<LocalizationFile>(_resourcesBasePath + locaPath);
            return locFile;
        }

        private void TryInitLoadedLocalizedStrings()
        {
            if (_loadedLocalizedStrings == null)
            {
                _loadedLocalizedStrings = new Dictionary<string, Dictionary<string, LocalizedEntry>>();
            }
        }

        private LocalizedEntry CreateLocalizedData(LocalizationFile.DataEntry data)
        {   
            return new LocalizedEntry(data, this);
        }

        private void LoadLanguageFromInspector(ELanguage language)
        {
            LoadLanguage(language);
        }

        private static bool CheckInstance()
        {
            if (_instance == null)
            {
                //assumes that LocalizationDB sits directly under "Resources"
                _instance = Resources.Load<LocalizationDB>(nameof(LocalizationDB));

                if (_instance)
                {
                    _instance.Init();
                    return true;
                }

                Debug.LogWarning("Couldn't find LocalizationDB. This is normal if the projects opens for the first time. Otherwise check if LocalizationDB exists.");
                return false;
            }

            return true;
        }

        private static string LogWarning(string returnValue, string warningMessage, object genericContext)
        {
            string contextString = genericContext.GetType().Name;
            var unityObject = genericContext as UnityEngine.Object;
            Object context = unityObject;

            Debug.LogWarning(warningMessage + ". Class: " + contextString, context);
            return returnValue;
        }

        private void WriteLanguageSettingsFile(ELanguage oldLang, ELanguage newLang)
        {
            var path = GetLanguageSettingsFilePath();
            if (!File.Exists(path) || oldLang != newLang)
            {   
                try
                {
                    File.WriteAllText(path, newLang.ToString());
                }
                catch
                {
                    Debug.LogWarning("Could not write language settings file", this);
                }
            }
        }

        private void LoadLanguageSettingsFromFile()
        {
            try
            {   
                var filePath = GetLanguageSettingsFilePath();
                if (File.Exists(filePath))
                {
                    var text = File.ReadAllText(filePath);

                    ELanguage result = ELanguage.de;
                    if (Enum.TryParse<ELanguage>(text, out result))
                    {
                        _currentLanguage = result;
                    }
                }
            }
            catch
            {
                Debug.LogWarning("Could not load language settings file", this);
            }
        }

        private CultureInfo LoadCultureInfo(ELanguage lang)
        {
            //TODO: settings like these should probably go to something like a language settings file (ScriptableObject):
            switch (lang)
            {
                case ELanguage.de:
                    return new CultureInfo("de-DE", false);
                case ELanguage.en:
                    return new CultureInfo("en-US", false);
                default:
                    return CultureInfo.InvariantCulture;
            }
        }

        private static string GetLanguageSettingsFilePath()
        {
            return Application.persistentDataPath + "/language.txt";
        }

        public static void OpenLocaAtAssetPath(string path)
        {
#if UNITY_EDITOR
            var locaFile = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(path);
            if (locaFile != null)
            {
                UnityEditor.AssetDatabase.OpenAsset(locaFile);
            }
            else
            {
                Debug.LogWarning($"Loca File at path {path} not found!");
            }
#endif
        }

        /// <summary>
        /// Should NOT contain language endings!
        /// </summary>
        public static void OpenLocaFileAtRawPath(string path)
        {
#if UNITY_EDITOR
            string key = "";
            Instance.BuildLocaStringsFor(ref path, ref key);

            var locaFile = LocalizationDB.Instance.LoadLocaFileFromResources(path);

            if(locaFile != null)
            {
                OpenLocaAtAssetPath(locaFile.WorkingFileAssetPath);
            }
#endif
        }

        [System.Serializable]
        public class LanguageInfo
        {
            [SerializeField]
            private ELanguage _languageEnum = default;
            public ELanguage LanguageEnum => _languageEnum;

            [SerializeField]
            private CultureInfo _cultureInfo = default;
            public CultureInfo CultureInfo => _cultureInfo;

            public LanguageInfo(ELanguage l, CultureInfo info)
            {
                _languageEnum = l;
                _cultureInfo = info;
            }
        }

        private class LocalizedEntry
        {
            public string Text;
            public string RawText;

            public LocalizedEntry(LocalizationFile.DataEntry locaEntry, LocalizationDB db)
            {   
                var finalText = locaEntry.Value;

                //the space after <n> is intended, otherwise there is an empty space after new line (dunno why)
                Text = finalText.Replace("<n> ", "\n").Trim();
                RawText = locaEntry.Value;
            }
        }
    }
}