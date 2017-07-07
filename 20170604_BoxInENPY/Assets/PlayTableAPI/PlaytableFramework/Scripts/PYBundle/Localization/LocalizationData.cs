using UnityEngine;
using System;
using System.Collections;
using System.IO;

namespace Playmove
{
    [Serializable]
    public class LocalizationData
    {
        public string TagNome;
        public string Texto;
        public string DataDeAlteracao;

        public DateTime LastWriteTime
        {
            get { return DateTime.Parse(DataDeAlteracao); }
        }
    }
}