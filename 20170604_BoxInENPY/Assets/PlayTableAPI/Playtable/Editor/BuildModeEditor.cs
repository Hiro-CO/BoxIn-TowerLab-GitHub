﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;
using UnityEditor.Callbacks;
using System.Text.RegularExpressions;

namespace Playmove
{
    public class BuildModeEditor : EditorWindow
    {
        public enum BuildType { Teste, Release, Both }

        private const string GAME_RESOURCE_PATH = "/PlaytableAPI/Playtable/Resources";

        private string _gameName = "";
        private GameVersion _version = new GameVersion(0, 0, 0, 0);
        private GameVersion _bundleVersion = new GameVersion(0, 0, 0, 0);
        private string _versionFieldMajor = "", _versionFieldMinor = "", _versionFieldRelease = "", _versionFieldTest = "";
        private string _bundleVersionFieldMajor = "", _bundleVersionFieldMinor = "", _bundleVersionFieldRelease = "", _bundleVersionFieldTest = "";

        private string _screenWidth = "1920";
        private string _screenHeight = "1080";
        private bool _fullscreen = true;
        private string _targetFrameRate = "60";
        private BuildType _buildType = BuildType.Both;
        private string _lastPath;

        private static BuildModeEditor _windowInstance;
        private static BuildModeEditor WindowInstance
        {
            get
            {
                if (_windowInstance == null)
                    _windowInstance = (BuildModeEditor)GetWindow(typeof(BuildModeEditor));
                return _windowInstance;
            }
        }

        public static string GetVersion()
        {
            WindowInstance.ReadSettingsFile();
            return WindowInstance._version.ToString();
        }
        public static string GetBundleVersion()
        {
            WindowInstance.ReadSettingsFile();
            return WindowInstance._bundleVersion.ToString();
        }

        #region Statics
        [MenuItem("PlaytableAPI/Build Config")]
        private static void Init()
        {
            _windowInstance = (BuildModeEditor)GetWindow(typeof(BuildModeEditor));
            PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;

            SceneView.onSceneGUIDelegate -= DrawValidation;
            SceneView.onSceneGUIDelegate += DrawValidation;

            _windowInstance.ReadSettingsFile();
        }

        public static string AskUserWhereToBuild()
        {
            return EditorUtility.SaveFolderPanel("Choose Location of Built Game", WindowInstance._lastPath, "");
        }
        public static void BuildGame(string path, BuildType buildType, string relativePath, bool askUserToSelectFolder = true)
        {
            if (string.IsNullOrEmpty(path))
                return;

            WindowInstance.ReadSettingsFile();

            // Excludes from build the deselected ones
            string[] levels = EditorBuildSettings.scenes.Where(item => item.enabled).Select(item => item.path).ToArray();

            // Configurate necessary editor build settings for current build type
            switch (buildType)
            {
                case BuildType.Teste:
                    EditorUserBuildSettings.development = true;
                    WindowInstance._version.Test++;
                    break;
                case BuildType.Release:
                    WindowInstance._version.Test = 0;
                    WindowInstance._version.Release++;
                    EditorUserBuildSettings.development = false;
                    EditorUserBuildSettings.connectProfiler = false;
                    EditorUserBuildSettings.allowDebugging = false;
                    break;
            }

            // Make sure all information is store in the settings file
            WindowInstance.Save(buildType);

            // Remove antiAliasing, vSync and Set Max Resolution Texture
            QualitySettings.GetQualityLevel();
            QualitySettings.antiAliasing = 0;
            QualitySettings.vSyncCount = 0;
            QualitySettings.masterTextureLimit = 0;

            BuildPipeline.BuildPlayer(levels, path + "/" + relativePath + WindowInstance._gameName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        }

        [PostProcessBuild(0)]
        private static void PostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            WindowInstance._lastPath = pathToBuiltProject;
            Process.Start("explorer.exe", "/select," + pathToBuiltProject.Replace(@"/", @"\"));

            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(pathToBuiltProject), "*.pdb"))
                File.Delete(file);
        }

        private static void DrawValidation(SceneView sceneView)
        {
            if (!Playtable.Authentication.TrueValidation) return;

            Handles.BeginGUI();

            GUI.contentColor = Color.red;

            var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            centeredStyle.alignment = TextAnchor.UpperLeft;
            centeredStyle.fontSize = 15;
            GUILayout.Label(Playtable.Authentication.Message, centeredStyle);

            Handles.EndGUI();
        }
        #endregion

        private void OnFocus()
        {
            _gameName = PlayerSettings.productName;
            _screenWidth = PlayerSettings.defaultScreenWidth.ToString();
            _screenHeight = PlayerSettings.defaultScreenHeight.ToString();
            _fullscreen = PlayerSettings.defaultIsFullScreen;

            ReadSettingsFile();
        }

        private void OnLostFocus()
        {
            ReadSettingsFile();
        }

        private void ReadSettingsFile()
        {
            PlayerSettings.showUnitySplashScreen = false;

            TextAsset gameSettings = Resources.Load<TextAsset>("gameSettings");
            if (gameSettings == null)
                return;

            string[] settings = gameSettings.text.Split('\n');

            string res = settings[PlaytableWin32.SETTING_INDEX_RESOLUTION].Replace("Resolution: ", "");
            string[] resolution = res.Split('x');
            _screenWidth = resolution[0].Trim();
            _screenHeight = resolution[1].Trim();

            string fs = settings[PlaytableWin32.SETTING_INDEX_FULLSCREEN].Replace("Fullscreen: ", "");
            _fullscreen = bool.Parse(fs.Trim());

            settings[PlaytableWin32.SETTING_INDEX_FULLSCREEN_MODE].Replace("FullscreenMode: ", "");

            string bm = settings[PlaytableWin32.SETTING_INDEX_BUILD_MODE].Replace("BuildMode: ", "");
            _buildType = (BuildType)Enum.Parse(typeof(BuildType), bm.Trim());

            _targetFrameRate = settings[PlaytableWin32.SETTING_INDEX_TARGET_FRAMERATE].Replace("Targer Frame Rate: ", "");

            string version = settings[PlaytableWin32.SETTING_INDEX_GAME_VERSION].Replace("Game Version: ", "");
            _version.Set(version.Split('.'));
            _versionFieldMajor = _version.Major.ToString();
            _versionFieldMinor = _version.Minor.ToString();
            _versionFieldRelease = _version.Release.ToString();
            _versionFieldTest = _version.Test.ToString();

            if (PlaytableWin32.SETTING_INDEX_BUNDLE_VERSION < settings.Length)
            {
                string bundleVersion = settings[PlaytableWin32.SETTING_INDEX_BUNDLE_VERSION];
                if (!string.IsNullOrEmpty(bundleVersion))
                {
                    bundleVersion = bundleVersion.Replace("Bundle Version: ", "");
                    _bundleVersion.Set(bundleVersion.Split('.'));
                    _bundleVersionFieldMajor = _bundleVersion.Major.ToString();
                    _bundleVersionFieldMinor = _bundleVersion.Minor.ToString();
                    _bundleVersionFieldRelease = _bundleVersion.Release.ToString();
                    _bundleVersionFieldTest = _bundleVersion.Test.ToString();
                }
            }

            if (PlaytableWin32.SETTING_INDEX_GAME_NAME > settings.Length - 1)
                _gameName = "DefaultGame";
            else
                _gameName = settings[PlaytableWin32.SETTING_INDEX_GAME_NAME] == null ? "DefaultGame" : settings[PlaytableWin32.SETTING_INDEX_GAME_NAME];
        }

        void OnGUI()
        {
            float distanceSeparation = 10;

            GUILayout.Label("Configurações:");
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Space(5);
            PlayerSettings.companyName = EditorGUILayout.TextField("Nome da empresa:", PlayerSettings.companyName);

            _gameName = EditorGUILayout.TextField("Nome do Jogo:", _gameName);

            #region Resolução
            GUILayout.BeginHorizontal();
            string tempW = _screenWidth;
            int nv;
            _screenWidth = EditorGUILayout.TextField("Resolução:", _screenWidth);
            if (!int.TryParse(_screenWidth, out nv))
            {
                _screenWidth = tempW;
            }

            string tempH = _screenHeight;
            _screenHeight = GUILayout.TextField(_screenHeight);
            if (!int.TryParse(_screenHeight, out nv))
            {
                _screenHeight = tempH;
            }

            GUILayout.EndHorizontal();
            #endregion

            _fullscreen = EditorGUILayout.Toggle("Fullscreen:", _fullscreen);

            // Fullscreen Mode
            GUILayout.Label("Fullscreen Mode: Empty");

            // Target frame rate
            string tempTFM = _targetFrameRate;
            int tfm;
            _targetFrameRate = EditorGUILayout.TextField("Target Frame Rate: ", _targetFrameRate);
            if (!int.TryParse(_targetFrameRate, out tfm))
            {
                _targetFrameRate = tempTFM;
            }

            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.Space(distanceSeparation);

            GUILayout.Label("Versões:");
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Space(5);
            #region Version
            GUILayout.BeginHorizontal();
            int versionNro;

            //MAJOR
            _versionFieldMajor = EditorGUILayout.TextField("Versão Jogo: " + _version.ToString(), Regex.Replace(_versionFieldMajor, @"[^0-9]", ""));
            if (int.TryParse(_versionFieldMajor, out versionNro))
                _version.Major = versionNro;
            //MINOR
            _versionFieldMinor = GUILayout.TextField(Regex.Replace(_versionFieldMinor, @"[^0-9]", ""));
            if (int.TryParse(_versionFieldMinor, out versionNro))
                _version.Minor = versionNro;
            //RELEASE
            _versionFieldRelease = GUILayout.TextField(Regex.Replace(_versionFieldRelease, @"[^0-9]", ""));
            if (int.TryParse(_versionFieldRelease, out versionNro))
                _version.Release = versionNro;
            //TEST
            _versionFieldTest = GUILayout.TextField(Regex.Replace(_versionFieldTest, @"[^0-9]", ""));
            if (int.TryParse(_versionFieldTest, out versionNro))
                _version.Test = versionNro;
            GUILayout.EndHorizontal();
            #endregion

            #region Bundle Version
            GUILayout.BeginHorizontal();
            int bundleVersionNro;

            //MAJOR
            _bundleVersionFieldMajor = EditorGUILayout.TextField("Versão Bundles: " + _bundleVersion.ToString(), Regex.Replace(_bundleVersionFieldMajor, @"[^0-9]", ""));
            if (int.TryParse(_bundleVersionFieldMajor, out bundleVersionNro))
                _bundleVersion.Major = bundleVersionNro;
            //MINOR
            _bundleVersionFieldMinor = GUILayout.TextField(Regex.Replace(_bundleVersionFieldMinor, @"[^0-9]", ""));
            if (int.TryParse(_bundleVersionFieldMinor, out bundleVersionNro))
                _bundleVersion.Minor = bundleVersionNro;
            //RELEASE
            _bundleVersionFieldRelease = GUILayout.TextField(Regex.Replace(_bundleVersionFieldRelease, @"[^0-9]", ""));
            if (int.TryParse(_bundleVersionFieldRelease, out bundleVersionNro))
                _bundleVersion.Release = bundleVersionNro;
            //TEST
            _bundleVersionFieldTest = GUILayout.TextField(Regex.Replace(_bundleVersionFieldTest, @"[^0-9]", ""));
            if (int.TryParse(_bundleVersionFieldTest, out bundleVersionNro))
                _bundleVersion.Test = bundleVersionNro;

            GUILayout.EndHorizontal();
            #endregion
            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.Space(distanceSeparation);

            // Build type
            _buildType = (BuildType)EditorGUILayout.EnumPopup("Tipo de Build: ", _buildType);

            if (!Playtable.Authentication.TrueValidation)
                GUI.contentColor = Color.green;
            else
                GUI.contentColor = Color.red;

            GUILayout.Space(distanceSeparation);

            // Save settings
            if (GUILayout.Button("Salvar"))
            {
                Save((_buildType == BuildType.Both || _buildType == BuildType.Release) ? BuildType.Release : BuildType.Teste);
            }

            // Build Game
            if (GUILayout.Button("Build"))
            {
                if (_buildType == BuildType.Both)
                {
                    string buildPath = AskUserWhereToBuild();
                    BuildGame(buildPath, BuildType.Teste, _gameName + "-Test/");
                    BuildGame(buildPath, BuildType.Release, _gameName + "-Release/");
                }
                else
                    BuildGame(AskUserWhereToBuild(), _buildType, "");
            }
            if (GUILayout.Button("Build Test and Release"))
            {
                string buildPath = AskUserWhereToBuild();
                BuildGame(buildPath, BuildType.Teste, _gameName + "-Test/");
                BuildGame(buildPath, BuildType.Release, _gameName + "-Release/");
            }

            var centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.FlexibleSpace();
            GUILayout.Label(Playtable.Authentication.Message, centeredStyle);
            GUILayout.FlexibleSpace();
        }

        private void Save(BuildType buildType)
        {
            if (!Directory.Exists("Assets" + GAME_RESOURCE_PATH))
                Directory.CreateDirectory("Assets" + GAME_RESOURCE_PATH);

            PlayerSettings.defaultScreenWidth = int.Parse(_screenWidth);
            PlayerSettings.defaultScreenHeight = int.Parse(_screenHeight);
            PlayerSettings.defaultIsFullScreen = _fullscreen;
            PlayerSettings.productName = _gameName;

            string settings =
                "Resolution: " + _screenWidth + " x " + _screenHeight + "\n" +
                "Fullscreen: " + _fullscreen.ToString() + "\n" +
                "FullscreenMode: Empty\n" +
                "BuildMode: " + buildType.ToString() + "\n" +
                _gameName + "\n" +
                "Targer Frame Rate: " + _targetFrameRate + "\n" +
                "Game Version: " + _version.ToString() + "\n" +
                "Bundle Version: " + _bundleVersion.ToString();


            File.WriteAllText(Application.dataPath + GAME_RESOURCE_PATH + "/gameSettings.txt", settings);
            AssetDatabase.Refresh();
        }
    }
}