using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

namespace Yumineko.EditorSceneLoader
{
    public sealed class SettingsWindow : EditorWindow
    {
        private readonly List<string> exclusionPatterns = new();
        private readonly List<string> whitelistPatterns = new();
        private ReorderableList blackList;
        private ReorderableList whitelist;

        public static void ShowWindow()
        {
            GetWindow<SettingsWindow>("SceneLoader Settings");
        }

        private void OnEnable()
        {
            LoadPatterns();
            InitializeReorderableLists();
        }

        private void OnDisable()
        {
            SavePatterns();
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

        private void SavePatterns()
        {
            EditorUserSettings.SetConfigValue(SettingsNames.BlacklistCount, exclusionPatterns.Count.ToString());
            for (var i = 0; i < exclusionPatterns.Count; i++)
            {
                EditorUserSettings.SetConfigValue($"{SettingsNames.BlacklistPattern}_{i}", exclusionPatterns[i]);
            }

            EditorUserSettings.SetConfigValue(SettingsNames.WhitelistCount, whitelistPatterns.Count.ToString());
            for (var i = 0; i < whitelistPatterns.Count; i++)
            {
                EditorUserSettings.SetConfigValue($"{SettingsNames.WhitelistPattern}_{i}", whitelistPatterns[i]);
            }
        }

        private void InitializeReorderableLists()
        {
            whitelist = new ReorderableList(whitelistPatterns, typeof(string), true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "ホワイトリスト"),
                drawElementCallback = (rect, index, _, _) =>
                {
                    whitelistPatterns[index] = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), whitelistPatterns[index]);
                },
                onAddCallback = _ =>
                {
                    whitelistPatterns.Add("");
                },
                onRemoveCallback = list =>
                {
                    whitelistPatterns.RemoveAt(list.index);
                }
            };
            
            blackList = new ReorderableList(exclusionPatterns, typeof(string), true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "ブラックリスト"),
                drawElementCallback = (rect, index, _, _) =>
                {
                    exclusionPatterns[index] = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), exclusionPatterns[index]);
                },
                onAddCallback = _ =>
                {
                    exclusionPatterns.Add("");
                },
                onRemoveCallback = list =>
                {
                    exclusionPatterns.RemoveAt(list.index);
                }
            };
        }

        private void OnGUI()
        {
            blackList.DoLayoutList();
            EditorGUILayout.Space();
            whitelist.DoLayoutList();

            EditorGUILayout.Space();
            if (!GUILayout.Button("保存")) return;
            SavePatterns();
            ApplySettings();
        }

        private static void ApplySettings()
        {
            LoaderWindow.RefreshScenes();
        }
    }
}