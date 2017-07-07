using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Playmove
{
    public class CopyDirResult
    {
        public string FileName;
        public bool SuccessfullyCopied = false;
        public bool IsDone = false;
        public bool HasError = false;
        public bool WasCancelled = false;
        public float PercentageDirCopied = 0;

        public CopyDirResult() { }
        public CopyDirResult(string fileName, bool successfullyCopied,
            bool isDone, float percentageDirCopied)
        {
            FileName = fileName;
            SuccessfullyCopied = successfullyCopied;
            IsDone = isDone;
            PercentageDirCopied = percentageDirCopied;
        }

        public void Initialize(string fileName, bool successfullyCopied,
            bool isDone, float percentageDirCopied)
        {
            FileName = fileName;
            SuccessfullyCopied = successfullyCopied;
            IsDone = isDone;
            PercentageDirCopied = percentageDirCopied;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PYStorage
    {
        public const int SAVE_IMAGE_CODE_SUCCESS = 1;
        public const int SAVE_IMAGE_CODE_NOT_ENOUGH_SPACE = 2;
        public const int SAVE_IMAGE_CODE_UNKNOW_ERROR = 3;

        public const int ANY_TYPE_DEVICE = -1;

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa364939(v=vs.85).aspx
        /// </summary>
        public enum DriveType
        {
            DRIVE_UNKNOWN,
            DRIVE_NO_ROOT_DIR,
            DRIVE_REMOVABLE,
            DRIVE_FIXED,
            DRIVE_REMOTE,
            DRIVE_CDROM,
            DRIVE_RAMDISK
        }

        public enum SizeType
        {
            B = 1,
            KB = 1024,
            MB = 1048576,
            GB = 1073741824
        }

        public class FileWriteAsyncInfo
        {
            public FileStream Stream;

            public FileWriteAsyncInfo(FileStream stream)
            {
                Stream = stream;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class StorageInfo
        {
            private char letter;
            public string Letter
            {
                get { return letter + ":/"; }
            }

            private int type;
            public DriveType Type
            {
                get { return (DriveType)type; }
            }

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            private string deviceName;
            public string DeviceName
            {
                get { return deviceName; }
            }

            // In gb
            private float freeSpace;
            public float FreeSpace
            {
                get { return freeSpace; }
            }

            public StorageInfo()
            {

            }
            public StorageInfo(char letter, int type, string deviceName, float freeSpace)
            {
                this.letter = letter;
                this.type = type;
                this.deviceName = deviceName;
                this.freeSpace = freeSpace;
            }
            public StorageInfo(StorageInfo obj)
            {
                letter = obj.letter;
                type = obj.type;
                deviceName = obj.deviceName;
                freeSpace = obj.freeSpace;
            }
        };

        [DllImport("PYUSBStorage")]
        private static extern int GetAmountDevices(int type);

        [DllImport("PYUSBStorage")]
        private static extern void GetStorageDevice(int index, int type, [In, Out] StorageInfo storage);

        private static List<StorageInfo> _cachedStorageDevices = new List<StorageInfo>();

        public static List<StorageInfo> GetAllDevices()
        {
            int amountDevices = GetAmountDevices(ANY_TYPE_DEVICE);

            /// INFO: Apenas procura por devices caso algum
            /// algum novo device ou algum device
            /// tenha sido removido
            if (_cachedStorageDevices.Count != amountDevices)
            {
                _cachedStorageDevices.Clear();
                StorageInfo device = new StorageInfo();

                for (int x = 0; x < amountDevices; x++)
                {
                    GetStorageDevice(x, -1, device);
                    _cachedStorageDevices.Add(new StorageInfo(device));
                }
            }

            return _cachedStorageDevices;
        }
        public static List<StorageInfo> GetAllDevices(DriveType type)
        {
            int typeInt = (int)type;
            int amountDevices = GetAmountDevices(ANY_TYPE_DEVICE);

            List<StorageInfo> devices = new List<StorageInfo>();

            if (_cachedStorageDevices.Count != amountDevices)
            {
                _cachedStorageDevices.Clear();
                StorageInfo device = new StorageInfo();

                for (int x = 0; x < amountDevices; x++)
                {
                    GetStorageDevice(x, typeInt, device);
                    _cachedStorageDevices.Add(new StorageInfo(device));

                    if (device.Type == type)
                        devices.Add(_cachedStorageDevices[_cachedStorageDevices.Count - 1]);
                }
            }
            else
            {
                for (int x = 0; x < amountDevices; x++)
                {
                    if (_cachedStorageDevices[x].Type == type)
                        devices.Add(_cachedStorageDevices[x]);
                }
            }

            return devices;
        }

        public static StorageInfo GetUSBDevice(char letter)
        {
            foreach (StorageInfo dev in GetAllDevices(DriveType.DRIVE_REMOVABLE))
            {
                if (dev.Letter.Equals(letter))
                    return dev;
            }
            return null;
        }
        public static StorageInfo GetUSBDevice(string deviceName)
        {
            foreach (StorageInfo dev in GetAllDevices(DriveType.DRIVE_REMOVABLE))
            {
                if (dev.DeviceName.Equals(deviceName))
                    return dev;
            }
            return null;
        }

        public static StorageInfo GetDevice(char letter)
        {
            foreach (StorageInfo dev in GetAllDevices())
                if (dev.Letter.Equals(letter))
                    return dev;
            return null;
        }
        public static StorageInfo GetDevice(string deviceName)
        {
            foreach (StorageInfo dev in GetAllDevices())
                if (dev.DeviceName.Equals(deviceName))
                    return dev;
            return null;
        }
        public static StorageInfo GetDevice(DriveType type)
        {
            List<StorageInfo> devices = GetAllDevices(type);
            return devices.Count > 0 ? devices[0] : null;
        }

        public static bool HasDevices(DriveType type)
        {
            return GetAmountDevices((int)type) != 0;
        }

        private static bool isWaitingForDevices = false;
        public static void StopWaitingForDevices()
        {
            isWaitingForDevices = false;
        }
        public static void AsyncScanForDevices(MonoBehaviour refObj, DriveType type, Action callback)
        {
            isWaitingForDevices = true;
            refObj.StartCoroutine(asyncScanForDevices(type, callback));
        }
        private static System.Collections.IEnumerator asyncScanForDevices(DriveType type, Action callback)
        {
            while (!HasDevices(type) && isWaitingForDevices)
                yield return new WaitForSeconds(5);

            if (isWaitingForDevices && callback != null)
                callback();

            isWaitingForDevices = false;
        }

        /// <summary>
        /// Save the image in the specified device.
        /// </summary>
        /// <param name="usbDevice">USB device got from GetUSBDevice</param>
        /// <param name="relativePath">Relative path to save the image, if null or empty will save 
        /// in the root of the device. Is not necessary to end with a /</param>
        /// <param name="fileName">The image name</param>
        /// <param name="img">Texture2D you want to save</param>
        /// <returns>
        /// Return Codes:
        /// 1: Saved
        /// 2: Not enough space in device
        /// 3: Img not saved, unknow reason
        /// </returns>
        public static void SaveImage(StorageInfo usbDevice, string relativePath, string fileName, Texture2D img, Action<int> resultCallback)
        {
            if (usbDevice == null || fileName == null || img == null)
                if (resultCallback != null)
                    resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);

            string path = FilterPath(usbDevice.Letter + relativePath);
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                byte[] imgBytes = img.EncodeToPNG();

                // Img size in Kb
                float imgSize = (sizeof(byte) * imgBytes.Length) / 1000;
                // Img size in Mb
                imgSize /= 1024;
                // Img size in Gb
                imgSize /= 1024;

                if (imgSize > usbDevice.FreeSpace)
                {
                    if (resultCallback != null)
                        resultCallback(SAVE_IMAGE_CODE_NOT_ENOUGH_SPACE);
                }
                else
                    AsyncWriteImg(path, fileName, imgBytes, resultCallback);
            }
            catch
            {
                if (resultCallback != null)
                    resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);
            }
        }
        public static void SaveImage(string path, string fileName, Texture2D img, Action<int> resultCallback)
        {
            try
            {
                path = FilterPath(path);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                AsyncWriteImg(path, fileName, img.EncodeToPNG(), resultCallback);
            }
            catch
            {
                if (resultCallback != null)
                    resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);
            }
        }

        private static string FilterPath(string path)
        {
            if (path == null)
                path = "";
            if (path.Length > 1)
                path = path[path.Length - 1] != '/' ? path + "/" : path;

            return path.Replace("/", @"\");
        }
        private static void AsyncWriteImg(string path, string fileName, byte[] imgBytes, Action<int> resultCallback)
        {
            string[] filterFileName = fileName.Split('.');
            fileName = filterFileName.Length > 1 ? filterFileName[0] : fileName;
            string absolutePath = path + fileName + ".png";

            // Add a index to the img name if the name matchs a already created one
            int counter = 1;
            try
            {
                while (File.Exists(absolutePath))
                {
                    absolutePath = path + fileName + string.Format(" ({0})", counter) + ".png";
                    counter++;
                }
            }
            catch
            {
                if (resultCallback != null)
                    resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);
            }

            FileStream stream = null;
            try
            {
                stream = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite, imgBytes.Length);
                stream.BeginWrite(imgBytes, 0, imgBytes.Length, new AsyncCallback((result) =>
                {
                    FileWriteAsyncInfo info = (FileWriteAsyncInfo)result.AsyncState;

                    try
                    {
                        info.Stream.EndWrite(result);

                        if (File.Exists(absolutePath))
                        {
                            if (resultCallback != null)
                                resultCallback(SAVE_IMAGE_CODE_SUCCESS);
                        }
                        else if (resultCallback != null)
                            resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);
                    }
                    catch
                    {
                        if (resultCallback != null)
                            resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);
                    }

                    info.Stream.Close();

                }), new FileWriteAsyncInfo(stream));
            }
            catch
            {
                if (stream != null)
                    stream.Close();

                if (resultCallback != null)
                    resultCallback(SAVE_IMAGE_CODE_UNKNOW_ERROR);
            }
        }

        public static void AsyncLoadImagesFromPath(MonoBehaviour refObj, string path, Action<Texture2D[]> callback)
        {
            refObj.StartCoroutine(asyncLoadImagesFromPath(path, callback));
        }
        private static System.Collections.IEnumerator asyncLoadImagesFromPath(string path, Action<Texture2D[]> callback)
        {
            List<Texture2D> imgs = new List<Texture2D>();

            if (path[path.Length - 1] != '/')
                path += "/";

            if (Directory.Exists(path))
            {
                string[] allImages = Directory.GetFiles(path, "*.png");
                foreach (string imgPath in allImages)
                {
                    Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    tex.filterMode = FilterMode.Trilinear;
                    tex.anisoLevel = 1;
                    if (tex.LoadImage(File.ReadAllBytes(imgPath)))
                        imgs.Add(tex);
                    yield return null;
                }
            }

            if (callback != null)
                callback(imgs.ToArray());
        }

        public static void AsyncCopyFile(string srcPath, string dstPath, string fileName, Action<CopyDirResult> resultCallback)
        {
            srcPath = FilterPath(srcPath);
            dstPath = FilterPath(dstPath);

            if (!File.Exists(srcPath + fileName))
            {
                if (resultCallback != null)
                    resultCallback(new CopyDirResult(fileName, false, true, 0));
                return;
            }

            using (FileStream sourceStream = new FileStream(srcPath + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] srcBuffer = new byte[sourceStream.Length];
                try
                {
                    sourceStream.Read(srcBuffer, 0, srcBuffer.Length);

                    if (!Directory.Exists(dstPath))
                        Directory.CreateDirectory(dstPath);
                }
                catch
                {
                    if (resultCallback != null)
                        resultCallback(new CopyDirResult(fileName, false, true, 0));
                    return;
                }

                FileStream destStream = new FileStream(dstPath + fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                destStream.BeginWrite(srcBuffer, 0, srcBuffer.Length, new AsyncCallback((result) =>
                {
                    FileWriteAsyncInfo info = (FileWriteAsyncInfo)result.AsyncState;

                    try
                    {
                        info.Stream.EndWrite(result);

                        if (File.Exists(dstPath + fileName))
                        {
                            if (resultCallback != null)
                                resultCallback(new CopyDirResult(fileName, true, true, 1));
                        }
                        else if (resultCallback != null)
                            resultCallback(new CopyDirResult(fileName, false, true, 0));
                    }
                    catch
                    {
                        if (resultCallback != null)
                            resultCallback(new CopyDirResult(fileName, false, true, 0));
                    }

                    info.Stream.Close();

                }), new FileWriteAsyncInfo(destStream));
            }
        }

        private static bool _cancelAsyncCopyDir = false;
        public static void AsyncCopyDir(MonoBehaviour refObj, string srcDirPath, string destDirPath, Action<CopyDirResult> resultCallback)
        {
            refObj.StartCoroutine(AsyncCopyDirRoutine(srcDirPath, destDirPath, resultCallback));
        }
        static System.Collections.IEnumerator AsyncCopyDirRoutine(string srcDirPath, string destDirPath, Action<CopyDirResult> resultCallback)
        {
            srcDirPath = FilterPath(srcDirPath);
            destDirPath = FilterPath(destDirPath);

            bool hasError = false;

            float currentFileCopied = 0;
            string[] filesPath = Directory.GetFiles(srcDirPath, "*.*", SearchOption.AllDirectories);

            CopyDirResult resultData = new CopyDirResult();

            for (int x = 0; x < filesPath.Length; x++)
            {
                string filePath = filesPath[x];
                string fileName = GetFileName(filePath);

                if (_cancelAsyncCopyDir || hasError)
                    break;

                try
                {
                    using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        string fileRelativePath = filePath.Replace(srcDirPath, "");

                        byte[] srcBuffer = new byte[sourceStream.Length];
                        try
                        {
                            sourceStream.Read(srcBuffer, 0, srcBuffer.Length);

                            DirectoryInfo dirInfo = Directory.GetParent(destDirPath + fileRelativePath);
                            if (!dirInfo.Exists)
                                Directory.CreateDirectory(dirInfo.FullName);
                        }
                        catch
                        {
                            resultData.Initialize(fileName, false, true, currentFileCopied / filesPath.Length);
                            hasError = true;
                            break;
                        }

                        if (!hasError)
                        {
                            FileStream destStream = new FileStream(destDirPath + fileRelativePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                            destStream.BeginWrite(srcBuffer, 0, srcBuffer.Length, new AsyncCallback((result) =>
                            {
                                FileWriteAsyncInfo info = (FileWriteAsyncInfo)result.AsyncState;

                                try
                                {
                                    info.Stream.EndWrite(result);
                                    currentFileCopied++;

                                    if (File.Exists(destDirPath + fileRelativePath))
                                        resultData.Initialize(GetFileName(fileRelativePath), true, currentFileCopied == filesPath.Length, currentFileCopied / filesPath.Length);
                                    else
                                        resultData.Initialize(GetFileName(fileRelativePath), false, currentFileCopied == filesPath.Length, currentFileCopied / filesPath.Length);
                                }
                                catch
                                {
                                    resultData.Initialize(GetFileName(fileRelativePath), false, currentFileCopied == filesPath.Length, currentFileCopied / filesPath.Length);
                                    hasError = true;
                                }

                                info.Stream.Close();

                            }), new FileWriteAsyncInfo(destStream));
                        }
                    }
                }
                catch
                {
                    resultData.Initialize("???", false, true, 0);
                    hasError = true;
                    break;
                }

                if (resultCallback != null)
                    resultCallback(resultData);

                yield return new WaitForSeconds(0.1f);
            }

            // Caso a copia seja cancelada ou tenha acontecido algum erro
            // tentamos apagar o que foi copiado
            if (_cancelAsyncCopyDir || hasError)
            {
                TriesToClearAsynCopyWhenCanceled(destDirPath);

                resultData.IsDone = true;
                resultData.SuccessfullyCopied = false;
                resultData.HasError = hasError;
                resultData.WasCancelled = _cancelAsyncCopyDir;
                if (resultCallback != null)
                    resultCallback(resultData);

                _cancelAsyncCopyDir = false;
            }
        }
        public static void CancelAsynCopyDir()
        {
            _cancelAsyncCopyDir = true;
        }
        private static void TriesToClearAsynCopyWhenCanceled(string destPath)
        {
            try
            {
                if (Directory.Exists(destPath))
                    Directory.Delete(destPath, true);
            }
            catch
            {
                Debug.Log("None file was copied...");
            }
        }

        public static int GetAmountFilesInDir(string dir)
        {
            dir = FilterPath(dir);
            try
            {
                if (Directory.Exists(dir))
                {
                    string[] filesPath = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                    return filesPath.Length;
                }
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static float GetDirectorySize(string dir, SizeType type = SizeType.B)
        {
            dir = FilterPath(dir);
            try
            {
                string[] filesPath = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

                float fileSize = 0;
                foreach (string name in filesPath)
                {
                    FileInfo info = new FileInfo(name);
                    fileSize += info.Length;
                    fileSize += info.FullName.Length;
                }

                return fileSize / (float)type;
            }
            catch
            {
                return 0;
            }
        }

        public static float ConvertByteSizeTo(float size, SizeType type)
        {
            return size / (float)type;
        }

        public static string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z_/]", " ");
        }

        private static string GetFileName(string path)
        {
            string[] split = path.Split('\\');
            return split[split.Length - 1];
        }
    }
}