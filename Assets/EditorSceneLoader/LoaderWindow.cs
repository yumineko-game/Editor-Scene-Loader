using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;

namespace Yumineko.EditorSceneLoader
{
    public sealed class LoaderWindow : EditorWindow
    {
        private readonly List<(string Directory, List<string> Scenes)> scenesByDirectory = new();
        private readonly List<string> exclusionPatterns = new();
        private readonly List<string> whitelistPatterns = new();
        private string selectedDirectory;
        private Vector2 scrollPos;

        [MenuItem("Window/Scene Loader")]
        public static void ShowWindow()
        {
            GetWindow<LoaderWindow>("Scene Loader");
        }

        private void OnEnable()
        {
            LoadPatterns();
            LoadScenes();
            selectedDirectory = EditorUserSettings.GetConfigValue(SettingsNames.LastOpenedDirectory) ?? "";
        }

        private void OnDisable()
        {
            EditorUserSettings.SetConfigValue(SettingsNames.LastOpenedDirectory, selectedDirectory);
        }

        private void LoadPatterns()
        {
            exclusionPatterns.Clear();
            whitelistPatterns.Clear();

            var exclusionCount = EditorUserSettings.GetConfigValue(SettingsNames.BlacklistCount);
            if (int.TryParse(exclusionCount, out var exCount))
            {
                for (var i = 0; i < exCount; i++)
                {
                    var pattern = EditorUserSettings.GetConfigValue($"{SettingsNames.BlacklistPattern}_{i}");
                    if (!string.IsNullOrEmpty(pattern))
                    {
                        exclusionPatterns.Add(pattern);
                    }
                }
            }

            var whitelistCount = EditorUserSettings.GetConfigValue(SettingsNames.WhitelistCount);
            if (int.TryParse(whitelistCount, out var wlCount))
            {
                for (var i = 0; i < wlCount; i++)
                {
                    var pattern = EditorUserSettings.GetConfigValue($"{SettingsNames.WhitelistPattern}_{i}");
                    if (!string.IsNullOrEmpty(pattern))
                    {
                        whitelistPatterns.Add(pattern);
                    }
                }
            }
        }

        private void LoadScenes()
        {
            scenesByDirectory.Clear();
            var guids = AssetDatabase.FindAssets("t:Scene");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var directory = Path.GetDirectoryName(path);
                if (exclusionPatterns.Any(pattern => directory != null && directory.Contains(pattern)))
                {
                    continue;
                }

                if (whitelistPatterns.Any() && !whitelistPatterns.Any(pattern => directory != null && directory.Contains(pattern)))
                {
                    continue;
                }

                var existingEntry = scenesByDirectory.FirstOrDefault(entry => entry.Directory == directory);
                if (existingEntry == default)
                {
                    scenesByDirectory.Add((directory, new List<string> { path }));
                }
                else
                {
                    existingEntry.Scenes.Add(path);
                }
            }
        }

        public static void RefreshScenes()
        {
            var window = GetWindow<LoaderWindow>();
            window.LoadPatterns();
            window.LoadScenes();
            window.Repaint();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Directory", EditorStyles.boldLabel);
            var directories = scenesByDirectory.Select(entry => entry.Directory).ToArray();
            var selectedIndex = Array.IndexOf(directories, selectedDirectory);
            selectedIndex = EditorGUILayout.Popup(selectedIndex, directories.ToArray());
            EditorGUILayout.Space(10);
            if (selectedIndex >= 0)
            {
                selectedDirectory = directories[selectedIndex];
            }

            var selectedEntry = scenesByDirectory.FirstOrDefault(entry => entry.Directory == selectedDirectory);
            if (selectedEntry != default)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                var buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(5, 5, 3, 3)
                };
                foreach (var scenePath in selectedEntry.Scenes)
                {
                    if (GUILayout.Button(Path.GetFileNameWithoutExtension(scenePath), buttonStyle))
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }

                EditorGUILayout.EndScrollView();
            }

            // Add button to open ExclusiveSettingsWindow
            if (GUILayout.Button("設定"))
            {
                SettingsWindow.ShowWindow();
            }
        }
    }
}