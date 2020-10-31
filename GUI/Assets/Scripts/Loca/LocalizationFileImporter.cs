#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomFramework.KVJ2D;
using UnityEditor;
using UnityEngine;

namespace CustomFramework.Localization.Editor
{
    public class LocalizationFileImporter : AssetPostprocessor
    {
        private const string RAW_LOC_FILE_ENDING = ".loc";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            LocalizationDB database = LocalizationDB.Instance;

            if (!database)
            {
                Debug.LogWarning("Couldn't find LocalizationDB. This is normal if the projects opens for the first time. Otherwise check if LocalizationDS exists.");
                return;
            }

            var settings = database.GetImportSettings();
            if (!settings.InitForImport())
            {
                Debug.LogWarning("Input folder and/or output folder not set for Localization import!", database);
                return;
            }
            
            //TODO: handle deleted and moved assets:
            HandleImportedAssets(importedAssets, settings);
            database.ReloadCurrentLanguage();
        }

        private static void HandleImportedAssets(string[] importedAssets, Settings settings)
        {
            foreach (string tmpAssetPath in importedAssets)
            {
                if (HasRawLocFileEnding(tmpAssetPath))
                {
                    if (!settings.IsCorrectInputPath(tmpAssetPath))
                    {
                        continue;
                    }

                    string outputAssetPath = settings.GetGeneratedAssetPath(tmpAssetPath);
                    ImportLocalizationFile(tmpAssetPath, outputAssetPath, settings);
                }
            }
        }

        private static void ImportLocalizationFile(string inputAssetPath, string outputAssetPath, Settings settings)
        {
            bool isNewFile = false;
            var assetFile = AssetDatabase.LoadAssetAtPath<LocalizationFile>(outputAssetPath);

            //if the file does not exist, create it:
            if (assetFile == null)
            {
                isNewFile = true;
                assetFile = ScriptableObject.CreateInstance<LocalizationFile>();
            }

            //file as actually stored on the computer:
            string absoluteInputFilePath = GetGlobalFilePath(inputAssetPath);
            var parsedData = settings.Parser.ParseFromFile(absoluteInputFilePath);

            assetFile.SetData(parsedData);
            assetFile.SetWorkingFilePath(inputAssetPath);

            if (isNewFile)
            {
                CheckOutputAssetPathFolders(settings, outputAssetPath);
                AssetDatabase.CreateAsset(assetFile, outputAssetPath);
            }
            else
            {
                EditorUtility.SetDirty(assetFile);
            }

            Debug.Log("Imported LocalizationFile at " + outputAssetPath, assetFile);
        }

        /// <summary> Creates all folders within outputAssetPath that currently don't exist. </summary>
        private static void CheckOutputAssetPathFolders(Settings settings, string outputAssetPath)
        {
            char[] split = { '/' };

            string outputFolderPath = settings.GetGeneratedFolderPath();
            string subPath = settings.GetRelativePath(outputFolderPath, outputAssetPath);

            var paths = subPath.Split(split, StringSplitOptions.RemoveEmptyEntries);

            if (paths.Length > 1)
            {
                string parentPath = outputFolderPath;

                //-1 because the remaining part is for the asset file itself:
                for (int i = 0; i < paths.Length - 1; i++)
                {
                    string tmpPath = parentPath + "/" + paths[i];

                    if (!AssetDatabase.IsValidFolder(tmpPath))
                    {
                        AssetDatabase.CreateFolder(parentPath, paths[i]);
                    }

                    parentPath = tmpPath;
                }
            }
        }

        public static bool HasRawLocFileEnding(string path)
        {
            return path.EndsWith(RAW_LOC_FILE_ENDING);
        }

        /// <summary>
        /// Maps the path relative to 'Assets/xy...' to a global system path e.g. 'C:/UnityProject/Assets/xy...'
        /// </summary>
        private static string GetGlobalFilePath(string inputAssetPath)
        {
            string globalPath = Application.dataPath;
            //remove trailing "Assets" (is already in inputAssetPath)
            globalPath = globalPath.Substring(0, globalPath.Length - 6);
            return globalPath + inputAssetPath;
        }

        [System.Serializable]
        public class Settings
        {
            [SerializeField]
            private UnityEditor.DefaultAsset _rawFilesFolder = default;
            [SerializeField]
            private UnityEditor.DefaultAsset _generatedFolder = default;

            private KVJ2D_Reader _parser;
            public KVJ2D_Reader Parser => _parser;

            public bool InitForImport()
            {
                _parser = new KVJ2D_Reader(false);
                return GetRawFilesFolderPath().Length > 0 && GetGeneratedFolderPath().Length > 0;
            }

            public bool IsCorrectInputPath(string filePath)
            {
                var rawFilesFolderPath = GetRawFilesFolderPath();
                bool isCorrect = filePath.StartsWith(rawFilesFolderPath);

                if (!isCorrect)
                {
                    Debug.LogWarning("Please put LocalizationFiles in folder (or subfolder): " + rawFilesFolderPath);
                }

                ;

                return isCorrect;
            }

            /// <summary>
            /// Removes <paramref name="basePath"/> from <paramref name="otherPath"/> and accounts for slashes.
            /// </summary>
            public string GetRelativePath(string basePath, string otherPath)
            {
                int startIndex = basePath.Length;

                //add + 1 for missing "/" in basePath:
                if (!basePath.EndsWith("/"))
                {
                    startIndex++;
                }

                int count = otherPath.Length - startIndex;
                return otherPath.Substring(startIndex, count);
            }

            /// <summary> Returns an Asset path for generated files for the given rawFilePath </summary>
            public string GetGeneratedAssetPath(string rawFilePath)
            {
                string fileName = GetRelativePath(GetRawFilesFolderPath(), rawFilePath);

                //Remove fileEnding
                string rawFileName = fileName.Substring(0, fileName.Length - RAW_LOC_FILE_ENDING.Length);

                return string.Concat(GetGeneratedFolderPath(), "/", rawFileName, ".asset");
            }

            public string GetRawFilesFolderPath()
            {
                return AssetDatabase.GetAssetPath(_rawFilesFolder);
            }

            public string GetGeneratedFolderPath()
            {
                return AssetDatabase.GetAssetPath(_generatedFolder);
            }
        }
    }
}
#endif