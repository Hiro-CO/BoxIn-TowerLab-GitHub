using UnityEngine;
using System.Collections;
using System;

    namespace Playmove { public class PlaytableWin32 : MonoBehaviour
    {
        #region Instance
        static PlaytableWin32 _instance;
        public static PlaytableWin32 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PlaytableWin32>();
                return _instance;
            }
        }
        #endregion

        public const int SETTING_INDEX_RESOLUTION = 0;
        public const int SETTING_INDEX_FULLSCREEN = 1;
        public const int SETTING_INDEX_FULLSCREEN_MODE = 2;
        public const int SETTING_INDEX_BUILD_MODE = 3;
        public const int SETTING_INDEX_GAME_NAME = 4;
        public const int SETTING_INDEX_TARGET_FRAMERATE = 5;
        public const int SETTING_INDEX_GAME_VERSION = 6;
        public const int SETTING_INDEX_BUNDLE_VERSION = 7;

        public static string GameName;
        public static GameVersion GameVersion;
        public static GameVersion BundleVersion;

        public delegate void DelegateAfterCallApi(string text);
        public delegate void DelegateAfterGetScores(string ScoresData);
        public delegate void DelegateAfterGetVolume(int volume);

        public string Language { get; set; }
        public string ExpansionName { get; set; }

        private Authenticate _authentication;
        public Authenticate Authentication
        {
            get
            {
                if (_authentication == null)
                    _authentication = new Authenticate();
                return _authentication;
            }
        }

        #region Debug Dialog
        private bool _showDebugDialog = false;
        #endregion

        private void Awake()
        {
            try
            {
                if (Authentication.IsValid())
                    Debug.LogError("Dll Authenticated");
                else
                    Debug.LogError("Dll Not Authenticated");
            }
            catch (Exception e)
            {
                Debug.LogError("Playtable Launcher: Dll missing and \n" + e.Message);
                ExitGame(888);
                throw;
            }

            ReadArguments();

            if (Instance != null && Instance != this)
            {
                if (gameObject.transform.parent != null)
                    Destroy(gameObject.transform.parent.gameObject);
                else
                    Destroy(gameObject);
                return;
            }

            if (gameObject.transform.parent != null)
                DontDestroyOnLoad(gameObject.transform.parent.gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            TextAsset gameSettings = Resources.Load<TextAsset>("gameSettings");
            string[] settings = gameSettings.text.Split('\n');

            Application.targetFrameRate = int.Parse(settings[SETTING_INDEX_TARGET_FRAMERATE].Replace("Targer Frame Rate: ", "") ?? "60"); //TargetFrameRate;

            GameName = settings[SETTING_INDEX_GAME_NAME] == null ? "DefaultGame" : settings[SETTING_INDEX_GAME_NAME];

            if (gameSettings == null)
            {
                DoLauncherAuthentication(GameName);
                return;
            }

            GameVersion = new GameVersion(settings[SETTING_INDEX_GAME_VERSION].Replace("Game Version: ", ""));
            BundleVersion = new GameVersion(settings[SETTING_INDEX_BUNDLE_VERSION].Replace("Bundle Version: ", ""));

            string res = settings[SETTING_INDEX_RESOLUTION].Replace("Resolution: ", "");
            string[] resolution = res.Split('x');

            string fs = settings[SETTING_INDEX_FULLSCREEN].Replace("Fullscreen: ", "");
            bool fullscreen = bool.Parse(fs.Trim());

            Screen.SetResolution(int.Parse(resolution[0].Trim()), int.Parse(resolution[1].Trim()), fullscreen);

            string bm = settings[SETTING_INDEX_BUILD_MODE].Replace("BuildMode: ", "");
            switch (bm.Trim())
            {
                case "Teste":
                    Cursor.visible = true;

                    break;

                case "Release":
                default:
                    Cursor.visible = false;
                    DoLauncherAuthentication(GameName);

                    break;
            }
        }

        private void ReadArguments()
        {
            bool hasFoundLanguage = false;
            Language = "pt-BR";
            string[] paramsExe = System.Environment.GetCommandLineArgs();
#if UNITY_EDITOR
            paramsExe = new string[1] { "lang:" + Language };
#endif
            for (int i = 0; i < paramsExe.Length; i++)
            {
                // Check if exists some bundle for the language passed in exe param
                if (paramsExe[i].StartsWith("lang:"))
                {
                    string language = paramsExe[i].Replace("lang:", "");
                    foreach (string pathLocalization in PYBundleFolderScanner.GetGlobalLocalizationBundlesPath(language))
                    {
                        if (pathLocalization.Contains(".unity3d"))
                        {
                            Language = language;
                            hasFoundLanguage = true;
                        }
                    }
                }
                // Check if exists some bundle for the expansion passed in exe param
                else if (paramsExe[i].StartsWith("expan:"))
                {
                    string expansionName = paramsExe[i].Replace("expan:", "");
                    foreach (string pathLocalization in PYBundleFolderScanner.GetExpansionBundlesPath(expansionName, PYBundleType.Localization))
                    {
                        if (pathLocalization.Contains(".unity3d"))
                            ExpansionName = expansionName;
                    }
                }
            }

            if (!hasFoundLanguage)
                Language = "pt-BR";
        }

        #region Debug Dialog
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
                _showDebugDialog = !_showDebugDialog;

            if (_showDebugDialog)
                FpsCounter.UpdateFPS();
        }
        void OnGUI()
        {
            if (!_showDebugDialog) return;

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.BeginVertical(GUI.skin.box);

            GUI.color = GetFPSColor(FpsCounter.FPS);
            GUILayout.Label("FPS: " + FpsCounter.FPS.ToString("f4"));

            GUILayout.Space(5);
            GUI.color = Color.white;
            GUILayout.Label("Versão do jogo: " + GameVersion.ToString());
            GUILayout.Label("Versão do Bundle: " + BundleVersion.ToString());

            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        private Color GetFPSColor(float fps)
        {
            if (fps > 30) return Color.green;
            else if (fps < 15) return Color.red;

            return Color.Lerp(Color.red, Color.green, (fps - 15) / 15);
        }
        #endregion

        #region Authentication

        public void DoLauncherAuthentication(string gameTicket)
        {
            var url = string.Concat(Authentication.WebServicePath + "TicketProvider/Authenticate?gameTicket=", gameTicket);
            StartCoroutine(StartAuthentication(url, gameTicket));
        }

        public IEnumerator StartAuthentication(string url, string gameTicket)
        {
            WWW www = new WWW(url);
            yield return www;

            if (Authentication.IsValid())
            {
                if (string.IsNullOrEmpty(www.error))
                {
                    var key = www.text;
                    int step = Authentication.SopAuthentication(key);
                    Debug.LogError(step);
                    if (step != 0)
                        ExitGame(step);
                    else
                        Debug.LogError("Playtable Launcher: Awesome! Game authenticated.");
                }
                else
                {
                    Debug.LogError("Playtable Launcher: " + www.error);
                    ExitGame(999);
                }
            }
            else
            {
                Debug.LogError("Playtable Launcher " + "Autenticação falsa em execução");
            }
        }

        public void ExitGame(int step)
        {
            Application.Quit();
            throw new UnityException(string.Format("Bad: Invalid Playtable Launcher. Step: {0}", step));
        }

        #endregion

        #region Volume
        public void GetMute(DelegateAfterCallApi callback)
        {
            var url = Authentication.WebServicePath + "Settings/IsInMute";
            StartCoroutine(CallSoundApi(url, callback));
        }

        public void DoMute()
        {
            var url = Authentication.WebServicePath + "Settings/Mute";
            StartCoroutine(CallSoundApi(url));
        }

        public void DoUnMute()
        {
            var url = Authentication.WebServicePath + "Settings/UnMute";
            StartCoroutine(CallSoundApi(url));
        }

        public void GetVolume(DelegateAfterGetVolume callback)
        {
            var url = Authentication.WebServicePath + "Settings/GetVolume";
            StartCoroutine(CallSoundVolumeApi(url, callback));
        }

        public void SetVolume(int volume)
        {
            var url = string.Concat(Authentication.WebServicePath + "Settings/SetVolume?volume=", volume.ToString());
            StartCoroutine(CallSoundVolumeApi(url));
        }

        #endregion

        #region Scores
        #region Get

        /// <summary>
        /// Get the scores defined in the GameName == GameId in the level specified
        /// </summary>
        /// <param name="gameID">Defined by GameName and identify the game in a Playtable</param>
        /// <param name="level"></param>
        /// <param name="callback"></param>
        public void GetScores(string gameID, int level, DelegateAfterGetScores callback)
        {
            var url = string.Concat(Authentication.WebServicePath + "Settings/GetRecordes?gameId=", gameID, "&gameDifficult=", level);
            StartCoroutine(CallScoreApi(url, callback));
        }

        public void GetScores(int level, DelegateAfterGetScores callback)
        {
            GetScores(GameName, level, callback);
        }

        public void GetScores(DelegateAfterGetScores callback = null)
        {
            GetScores(GameName, 0, callback);
        }

        #endregion
        #region Set
        /// <summary>
        /// Set a score to a game defining a GameId (Use GameName)
        /// </summary>
        /// <param name="gameID">Defined by GameName and identify the game in a Playtable</param>
        /// <param name="level">Separate data from a game in different levels, if necessary</param>
        /// <param name="scoreData">Data to set, will be stored as a string, highly recommended to (de)serialize a class to manipulate the data</param>
        public void SetScores(string gameID, int level, string scoreData)
        {
            var url = string.Concat(Authentication.WebServicePath + "Settings/SetRecordes?gameId=", gameID, "&gameDifficult=", level, "&gameScores=", scoreData);
            StartCoroutine(CallScoreApi(url));
        }

        public void SetScores(int level, string scoreData)
        {
            SetScores(GameName, level, scoreData);
        }

        public void SetScores(string scoreData)
        {
            SetScores(GameName, 0, scoreData);
        }

        #endregion
        #endregion

        private IEnumerator StartRotation(string url)
        {
            WWW www = new WWW(url);
            yield return www;
            Debug.Log("Screen Rotated.");
        }

        private IEnumerator CallSoundApi(string url, DelegateAfterCallApi callback = null)
        {
            WWW www = new WWW(url);
            yield return www;
            if (callback != null)
            {
                string txt = "false";
                if (string.IsNullOrEmpty(www.error))
                {
                    txt = www.text;
                }

                callback(txt);
            }
        }

        private IEnumerator CallSoundVolumeApi(string url, DelegateAfterGetVolume callback = null)
        {
            WWW www = new WWW(url);
            yield return www;
            if (callback != null)
            {
                string txt = "10";
                if (string.IsNullOrEmpty(www.error))
                {
                    txt = www.text;
                }

                callback(int.Parse(txt));
            }
        }

        private IEnumerator CallScoreApi(string url, DelegateAfterGetScores callback = null)
        {
            WWW www = new WWW(url);
            yield return www;
            if (callback != null)
            {
                string txt = "";
                if (string.IsNullOrEmpty(www.error))
                    txt = www.text;
                txt = txt.Replace("\"", "");
                //txt = txt.Replace(" ", "");
                if (callback != null)
                    callback(txt);
            }
        }
    }
}