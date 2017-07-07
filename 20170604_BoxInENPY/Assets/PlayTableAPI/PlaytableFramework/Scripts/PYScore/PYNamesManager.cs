using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Playmove
{
    [Serializable]
    public static class PYNamesManager
    {
        [Serializable]
        public class NamesDTO
        {
            public NamesDTO()
            {
                Names = new List<NameData>();
            }
            public NamesDTO(List<NameData> list) : this()
            {
                Names = list;
            }

            public List<NameData> Names;

            public int Total { get; set; }

            public int Pages { get; set; }

            public int TotalPage { get; set; }
        }

        [Serializable]
        public class NameData
        {
            public NameData()
            {
                GameNames = new List<string>();
            }

            public NameData(int id, string name, int classId)
            {
                Id = id;
                Name = name;
                ClassId = classId;
                CreatedDate = LastUpdated = DateTime.Now;
                GameNames = new List<string>();
                GameNames.Add(PlaytableWin32.GameName);
            }

            /// <summary>
            /// Id será a posição na lista de nomes, este mesmo Id será referencia na lista de nomes no Student.
            /// </summary>
            public int Id;
            public string Name;
            public int ClassId;
            public DateTime CreatedDate;
            public DateTime LastUpdated;
            /// <summary>
            /// Contem os nomes dos jogos que utilizam esse nome
            /// </summary>
            private List<string> GameNames = new List<string>();

            public int GamesCount
            {
                get
                {
                    return GameNames.Count;
                }
            }

            public void UpdateName(int classId)
            {
                ClassId = classId;
                if (GameNames.Exists(gn => gn != PlaytableWin32.GameName))
                    GameNames.Add(PlaytableWin32.GameName);
                LastUpdated = DateTime.Now;
            }

            public bool RemoveGameName(string gameName)
            {
                return GameNames.Remove(gameName);
            }

            public override string ToString()
            {
                return string.Format("Id: {0} \nName: {1}", Id, Name);
            }
        }

        private static List<NameData> _names;
        private static List<NameData> Names
        {
            get
            {
                if (_names == null)
                    _names = new List<NameData>();
                return _names;
            }
            set
            {
                _names = value;
            }
        }

        private static List<string> _classesNames;
        public static List<string> ClassesNames
        {
            get
            {
                if (_classesNames == null)
                {
                    _classesNames = new List<string>();
                    _classesNames.Add("Sem turma");
                    _classesNames.Add("Pré-escola - 1º Ano");
                    _classesNames.Add("Pré-escola - 2º Ano");
                    _classesNames.Add("Pré-escola - 3º Ano");
                    _classesNames.Add("1º Ano - Fundamental");
                    _classesNames.Add("2º Ano - Fundamental");
                    _classesNames.Add("3º Ano - Fundamental");
                    _classesNames.Add("4º Ano - Fundamental");
                    _classesNames.Add("5º Ano - Fundamental");
                    _classesNames.Add("6º Ano - Fundamental");
                    _classesNames.Add("7º Ano - Fundamental");
                    _classesNames.Add("8º Ano - Fundamental");
                    _classesNames.Add("9º Ano - Fundamental");
                }
                return _classesNames;
            }
            private set { _classesNames = value; }
        }

        private static Action<List<NameData>> _loadcallback;

        public static void Initialize()
        {
            LoadNames();
        }

        public static string[] GetNames()
        {
            return Names.Select(n => n.Name).ToArray();
        }

        public static List<string> GetClassesNames()
        {
            return ClassesNames;
        }

        public static NameData SaveName(string name = null, int classId = 0)
        {
            NameData nameData = new NameData();
            if (!string.IsNullOrEmpty(name))
            {
                nameData = Names.Find(n => n.Name == name);
                if (nameData == null)
                {
                    int nextId = Names.Count == 0 ? 0 : Names.LastOrDefault().Id + 1;
                    nameData = new NameData(nextId, name, classId);
                    Names.Add(nameData);
                }
                else
                    nameData.UpdateName(classId);
            }

            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, Names);

            //PlaytableWin32.Instance.SopService.Save<NamesDTO>(new NamesDTO(Names), SaveCallback);

            //string to64 = Convert.ToBase64String(m.GetBuffer());
            //to64 = WWW.EscapeURL(to64);
            //PlaytableWin32.Instance.SetScores("Names", 0, to64); //m.GetBuffer());

            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            string path = dir.Parent.FullName;
            using (Stream stream = File.Open(string.Format("{0}/Names.bin", path), FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, Names);
            }

            return nameData;
        }

        public static void LoadNames(Action<List<NameData>> callback = null)
        {
            _loadcallback = callback;
            //PlaytableWin32.Instance.GetScores("Names", 0, SynchronizeNames);
            DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
            string path = dir.Parent.FullName;
            if (File.Exists(string.Format("{0}/Names.bin", path)))
            {
                try
                {
                    using (Stream stream = File.Open(string.Format("{0}/Names.bin", path), FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        Names = (List<NameData>)bin.Deserialize(stream);
                    }
                }
                catch (Exception)
                {
                    Names = new List<NameData>();
                    SaveName();
                    Debug.LogWarning("Names rebuild!");
                }
            }
            else
            {
                Names = new List<NameData>();
            }

            if (callback != null)
                callback(Names);
        }

        private static void SaveCallback(bool sopResponse)
        {
            Debug.Log("Success? " + sopResponse);
        }

        /// <summary>
        /// Irá remover o nome do jogo no registro de nomes,
        /// caso o nome não conter em mais nenhum jogo ele é apagado da lista
        /// </summary>
        /// <param name="name"></param>
        public static void DeleteName(string name)
        {
            NameData nameData = FindName(name);
            if (nameData == null) return;
            nameData.RemoveGameName(PlaytableWin32.GameName);
            if (nameData.GamesCount == 0)
                Names.RemoveAll(n => n.Name == name);
            SaveName();
        }

        private static NameData FindName(string name)
        {
            return Names.Find(nd => nd.Name == name);
        }

        /// <summary>
        /// Remove o jogo atual do registro de nomes
        /// </summary>
        /// <param name="forced">Força a exclusão de todos os nomes, independente do jogo</param>
        public static void DeleteAll(bool forced = false)
        {
            List<NameData> names = new List<NameData>();
            if (!forced)
            {
                for (int i = 0; i < Names.Count; i++)
                {
                    if (Names[i].RemoveGameName(PlaytableWin32.GameName))
                    {
                        if (Names[i].GamesCount != 0)
                            names.Add(Names[i]);
                    }
                }
            }
            Names = names;
            SaveName();
        }

        private static void SynchronizeNames(string data)
        {
            BinaryFormatter b = new BinaryFormatter();

            if (!string.IsNullOrEmpty(data))
            {
                MemoryStream m = new MemoryStream(Convert.FromBase64String(data));
                Names = (List<NameData>)b.Deserialize(m);

                if (_loadcallback != null)
                    _loadcallback(Names);
                _loadcallback = null;
            }
            else
            {
                //    // UNDONE
                //    Debug.Log("FakeNames");
                List<NameData> names = new List<NameData>();
                //    names.Add(new NameData(1, "GABRIEL", 0));
                //    names.Add(new NameData(2, "JUNO", 0));
                //    names.Add(new NameData(3, "JORGE", 0));
                //    names.Add(new NameData(4, "TIAGO", 0));
                //    names.Add(new NameData(5, "PAN", 0));
                //    names.Add(new NameData(5, "MAGAZINE", 0));
                //    names.Add(new NameData(6, "MADONNA", 0));
                //    names.Add(new NameData(7, "BATMAN", 0));
                //    names.Add(new NameData(8, "BOSS", 0));
                //    names.Add(new NameData(9, "KAROL", 0));
                //    names.Add(new NameData(10, "CRIS", 0));
                //    names.Add(new NameData(11, "MARJIE", 0));
                //    names.Add(new NameData(12, "RICARDO", 0));
                //    names.Add(new NameData(13, "RICARDO ELET", 0));
                //    names.Add(new NameData(14, "FELIPE", 1));
                //    names.Add(new NameData(15, "FRAGAS", 1));
                //    names.Add(new NameData(16, "FRAGOSO", 2));
                //    names.Add(new NameData(17, "FRAUGAS", 2));
                //    names.Add(new NameData(18, "FRAGAZOIDE", 2));
                //    names.Add(new NameData(19, "MAIS FRAGAS", 2));
                //    names.Add(new NameData(20, "FRAGUINHAS", 2));
                //    names.Add(new NameData(21, "JORJÕAO", 2));
                //    names.Add(new NameData(22, "JORGITA", 2));
                //    names.Add(new NameData(23, "JORJETE", 14));
                //    names.Add(new NameData(24, "JOANETE", 14));
                //    for (int i = 0; i < 18; i++)
                //    {
                //        NameData name = new NameData(25 + i, "JORRO" + i, 14);
                //        name.LastUpdated = name.LastUpdated.AddHours(UnityEngine.Random.Range(-5, 12));
                //        names.Add(name);
                //    }

                if (_loadcallback != null)
                    _loadcallback(names);
                _loadcallback = null;
                //    // ENDUNDONE

                Debug.Log("Lista de nomes vazia");
            }
        }
    }
}