using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsAPI.Tool
{
    public static class OpenFile
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void OpenFileByPathKeyword(string path, string keyword)
        {
            try
            {
                string[] files = null;
                files = System.IO.Directory.GetFiles(path, keyword);

                string sLatestVersion = "";
                string sLatestVersionFullName = "";

                foreach (string s in files)
                {
                    // Create the FileInfo object only when needed to ensure
                    // the information is as current as possible.
                    System.IO.FileInfo fi = null;
                    try
                    {
                        fi = new System.IO.FileInfo(s);
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(sLatestVersion))
                    {
                        sLatestVersion = getVersion(fi.FullName);
                        sLatestVersionFullName = fi.FullName;
                    }
                    else
                    {
                        string ver = getVersion(fi.FullName);
                        if (isMoreLateVersion(sLatestVersion, ver))
                        {
                            sLatestVersion = ver;
                            sLatestVersionFullName = fi.FullName;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sLatestVersionFullName))
                    System.Diagnostics.Process.Start(sLatestVersionFullName);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private static string getVersion(string FileName)
        {
            // 去除副檔名
            int iStartOfFilenameExtension = 0;
            for (int i = FileName.Length - 1; i > 0; i--)
            {
                if (FileName[i] == '.')
                {
                    iStartOfFilenameExtension = i;
                    break;
                }
            }
            FileName = FileName.Substring(0, iStartOfFilenameExtension);

            // 取尾部版本號('0~9','.')
            int iStartOfVersion = 0;
            for (int i = FileName.Length - 1; i > 0; i--)
            {
                char c = FileName[i];
                if (c != '.' && (c > '9' || c < '0'))
                {
                    iStartOfVersion = i + 1;
                    break;
                }
            }
            FileName = FileName.Substring(iStartOfVersion);
            while (FileName.StartsWith("."))
            {
                FileName = FileName.Substring(1);
            }
            while (FileName.EndsWith("."))
            {
                FileName = FileName.Substring(0, FileName.Length - 1);
            }
            return FileName;
        }

        private static bool isMoreLateVersion(string ver_A, string ver_B)
        {
            if (ver_A == ver_B) return false;

            try
            {
                while (true)
                {
                    // 取當前分層數字
                    int iEndOfLayer_A = 0;
                    bool reachEnd_A = false;
                    if (ver_A.Length == 1)
                    {
                        iEndOfLayer_A = 1;
                        reachEnd_A = true;
                    }
                    for (int i = 1; i < ver_A.Length; i++)
                    {
                        if (i == ver_A.Length - 1)
                        {
                            iEndOfLayer_A = i;
                            reachEnd_A = true;
                        }
                        else if (ver_A[i] == '.')
                        {
                            iEndOfLayer_A = i;
                            break;
                        }
                    }
                    int iCurLayer_A = Convert.ToInt32(ver_A.Substring(0, iEndOfLayer_A));

                    int iEndOfLayer_B = 0;
                    bool reachEnd_B = false;
                    if (ver_B.Length == 1)
                    {
                        iEndOfLayer_B = 1;
                        reachEnd_B = true;
                    }
                    for (int i = 1; i < ver_B.Length; i++)
                    {
                        if (i == ver_A.Length - 1)
                        {
                            iEndOfLayer_B = i;
                            reachEnd_B = true;
                        }
                        else if (ver_B[i] == '.')
                        {
                            iEndOfLayer_B = i;
                            break;
                        }
                    }
                    int iCurLayer_B = Convert.ToInt32(ver_B.Substring(0, iEndOfLayer_B));

                    if (iCurLayer_A != iCurLayer_B) return iCurLayer_A < iCurLayer_B;
                    if (reachEnd_A && !reachEnd_B) return true;
                    if (!reachEnd_A && reachEnd_B) return false;
                    if (reachEnd_A && reachEnd_B) return false;
                    if (!reachEnd_A)
                        ver_A = ver_A.Substring(iEndOfLayer_A + 1);
                    if (!reachEnd_B)
                        ver_B = ver_B.Substring(iEndOfLayer_B + 1);
                }
            }
            catch
            {
                return true;
            }
        }
    }
}
