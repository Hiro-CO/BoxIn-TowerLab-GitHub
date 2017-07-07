using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

namespace Playmove
{
    public class PYLocalizationWindow : EditorWindow
    {
        // 0: BundleType: Always PYBundleType.Localization
        // 1: Language
        public static readonly string PATH_TO_GLOBAL_LOCALIZATION = Application.dataPath + PYBundleFolderScanner.GLOBAL_ASSETS_FOLDERS + "{1}/Texts/";
        // 0: Expansion name
        // 1: BundleType: Always PYBundleType.Localization
        // 2: Language
        public static readonly string PATH_TO_EXPANSION_LOCALIZATION = Application.dataPath + PYBundleFolderScanner.EXPANSION_ASSETS_FOLDERS + "{2}/Texts/";

        public class DownloadFileData
        {
            // This is the application name to query the webservice
            // this is necessary because with have genericTags and appTags
            // Generic tags should be queried as "" (empty string) while
            // the appTags should be queried as "appName"
            public string QueryFileTag;
            public string FileName;
            public string FileDescription;

            public DownloadFileData(string queryFileTag, string name, string description)
            {
                QueryFileTag = queryFileTag;
                FileName = name;
                FileDescription = description;
            }
        }

        public class GameData
        {
            public string Name;
            public string ExecutableName;

            public Dictionary<string, DownloadFileData> Files = new Dictionary<string, DownloadFileData>();

            public GameData(string name)
            {
                Name = name;
            }
        }

        [Serializable]
        public class AppLocalizationData
        {
            public string Localizacao;
        }

        private EditorCoroutine _localizationUpdateRoutine;
        private bool _isSynced = false;
        private Vector2 _mainScrollPos = Vector2.zero;

        private List<GameData> _gamesData = new List<GameData>();

        [MenuItem("PlaytableAPI/PYLocalization Files")]
        static void Init()
        {
            PYLocalizationWindow window = GetWindow<PYLocalizationWindow>();
            window.Initialize();
        }

        void Initialize()
        {
            ScanProjectFolders();
        }

        void OnFocus()
        {
            ScanProjectFolders();
            Sync();
        }

        void OnDestroy()
        {
            if (_localizationUpdateRoutine != null)
                _localizationUpdateRoutine.Stop();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Sync All"))
            {
                Sync();
            }

            GUILayout.Space(15);
            _mainScrollPos = GUILayout.BeginScrollView(_mainScrollPos);
            for (int i = 0; i < _gamesData.Count; i++)
            {
                GUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(this.position.width));

                GUI.enabled = true;
                GUILayout.BeginHorizontal();
                GUILayout.Label(_gamesData[i].Name);
                GUILayout.Space(15);

                _gamesData[i].ExecutableName = EditorGUILayout.TextField("Executable Name: ", _gamesData[i].ExecutableName);
                // Persist executableName
                if (GUI.changed)
                {
                    EditorPrefs.SetString(_gamesData[i].Name, _gamesData[i].ExecutableName);
                }

                GUILayout.EndHorizontal();

                // Disable gui if its'n synced
                GUI.enabled = _isSynced;

                GUILayout.Space(15);
                foreach (string localizationName in _gamesData[i].Files.Keys)
                {
                    GUILayout.BeginHorizontal(GUI.skin.box);
                    GUILayout.Label("Language: " + localizationName);

                    DownloadFileData fileData = _gamesData[i].Files[localizationName];
                    if (GUILayout.Button("Download file"))
                    {
                        // This will be always in the BundlesAssets/Global/Localization
                        if (i == 0)
                        {
                            DownloadLocalizationFile(string.Format(PATH_TO_GLOBAL_LOCALIZATION, PYBundleType.Localization, localizationName),
                                fileData.FileName, fileData.QueryFileTag, localizationName);
                        }
                        // This will be always in the BundlesAssets/Expansions/{0}/Localization
                        else
                        {
                            DownloadLocalizationFile(string.Format(PATH_TO_EXPANSION_LOCALIZATION, _gamesData[i].Name, PYBundleType.Localization, localizationName),
                                fileData.FileName, fileData.QueryFileTag, localizationName);
                        }
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            GUILayout.EndScrollView();

        }

        void Sync()
        {
            _isSynced = false;
            RequestLanguages(0, _gamesData, () =>
            {
                _isSynced = true;
            });
        }

        void ScanProjectFolders()
        {
            _gamesData.Clear();
            _gamesData.Add(new GameData(PlayerSettings.productName));

            string[] dirspath = Directory.GetDirectories(Application.dataPath + "/BundlesAssets/Expansions/");
            foreach (string dirPath in dirspath)
            {
                DirectoryInfo info = new DirectoryInfo(dirPath);
                _gamesData.Add(new GameData(info.Name));
                _gamesData[_gamesData.Count - 1].ExecutableName = info.Name;
            }

            // Restore executable names
            foreach (GameData data in _gamesData)
            {
                data.ExecutableName = EditorPrefs.GetString(data.Name);
            }

            // Find localization files already in project
            for (int i = 0; i < _gamesData.Count; i++)
            {
                string[] localizationFoldersPath = null;
                // The global folder
                if (i == 0)
                {
                    localizationFoldersPath = Directory.GetDirectories(Application.dataPath +
                        string.Format(PYBundleFolderScanner.GLOBAL_ASSETS_FOLDERS, PYBundleType.Localization));
                }
                // The expansions folder
                else
                {
                    localizationFoldersPath = Directory.GetDirectories(Application.dataPath +
                        string.Format(PYBundleFolderScanner.EXPANSION_ASSETS_FOLDERS, _gamesData[i].Name, PYBundleType.Localization));
                }

                foreach (string localizationPath in localizationFoldersPath)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(localizationPath);

                    // If we found files in project
                    if (Directory.GetFiles(localizationPath, "string*.json", SearchOption.AllDirectories).Length > 0)
                    {
                        _gamesData[i].Files.Add(dirInfo.Name, new DownloadFileData(_gamesData[i].ExecutableName, "string0", "test"));
                    }
                }
            }
        }

        void RequestLanguages(int index, List<GameData> gamesData, Action callbackCompleted)
        {
            RequestLocalizationForApp(gamesData[index].ExecutableName, (json) =>
                {
                    JSONData<List<AppLocalizationData>> jsonObject = JsonUtility.FromJson<JSONData<List<AppLocalizationData>>>(json);
                // If server dont returned anything we just try another
                if (jsonObject == null || jsonObject.Dados == null)
                    {
                        index++;
                        if (index < gamesData.Count)
                            RequestLanguages(index, gamesData, callbackCompleted);
                        else if (callbackCompleted != null)
                            callbackCompleted();
                        return;
                    }

                // Serve found localization
                foreach (AppLocalizationData locData in jsonObject.Dados)
                    {
                        if (!gamesData[index].Files.ContainsKey(locData.Localizacao))
                        {
                            gamesData[index].Files.Add(locData.Localizacao,
                                new DownloadFileData(gamesData[index].ExecutableName, "string0",
                                    " Localization " + locData.Localizacao + " for game " + gamesData[index].Name));
                        }
                    }

                    index++;
                    if (index < gamesData.Count)
                        RequestLanguages(index, gamesData, callbackCompleted);
                    else if (callbackCompleted != null)
                        callbackCompleted();
                });
        }

        private void DownloadAllLocalizationFiles()
        {
            //EditorUtility.DisplayProgressBar("Download", "Downloading files...", 0.5f);
        }
        private void DownloadLocalizationFile(string path, string fileName, string gameName, string localization)
        {
            RequestLocalizationFile(gameName, localization, (json) =>
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                // Parse json to object to exclude some unecessary fields
                JSONData<List<LocalizationData>> jsonObject = JsonUtility.FromJson<JSONData<List<LocalizationData>>>(json);

                    File.WriteAllText(path + fileName + ".json", JsonUtility.ToJson(jsonObject));
                    AssetDatabase.Refresh();

                    EditorUtility.DisplayDialog("Download Success", "Successfully downloaded file", "Okay");
                });
        }

        private void RequestLocalizationForApp(string executableName, Action<string> callback)
        {
            EditorCoroutine.Start(WebRequest(string.Format(@"https://api.playmove.com.br/api/v2/LocalizacaoApps/ListarLocalizacaoApp?" +
                "executavel={0}.exe", executableName), callback));
        }

        private void RequestLocalizationFile(string executableName, string localization, Action<string> callback)
        {
            EditorCoroutine.Start(WebRequest(string.Format(@"https://api.playmove.com.br/api/v2/LocalizacaoApps/ListarTodos?" +
                "Filtros.Executavel={0}.exe&Filtros.Localizacao={1}", executableName, localization), callback));
        }

        IEnumerator WebRequest(string url, Action<string> callback)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization-Id", "gerenciarapps@playmove.com.br");
            headers.Add("Authorization-Token", "gerencia@play123");
            headers.Add("Authorization-Role", "30");

            WWW www = new WWW(url, null, headers);
            while (!www.isDone)
                yield return null;

            if (callback != null)
                callback(www.text);
        }
    }
}