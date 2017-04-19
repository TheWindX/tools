using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
/*
 * usage: copyDir src dst dir1 dir2...
 */

namespace copyDir
{
    class Program
    {
        static List<string> mFilter = new List<string>();
        static private bool filterOrRemoveIf = true;
        static string mSrc;
        static string mDst;
        static List<string> mAllDir = new List<string>();
        static List<string> mAllFiles = new List<string>();

        static void buildDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            } 

        }

        static List<bool> isInDir = new List<bool>() { false };
        static public void GetAllFileList(string strBaseDir)
        {
            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            if (mFilter.Contains(di.Name.ToLower() ))
            {
                isInDir.Add(true);
            }
            DirectoryInfo[] diA = di.GetDirectories();
            
            for (int i = 0; i < diA.Length; i++)
            {
                GetAllFileList(diA[i].FullName);
            }

            if (filterOrRemoveIf)
            {
                if (isInDir.Last())
                {
                    var fiA = di.GetFiles();

                    fiA.ToList().ForEach(fi =>
                    {
                        mAllFiles.Add(fi.FullName);
                    });
                }
            }
            else
            {
                    FileInfo[] fiA = di.GetFiles();
                    fiA.Where(fi=>mFilter.Contains(fi.Extension.ToLower())).ToList().ForEach(fi =>
                    {
                        mAllFiles.Add(fi.FullName);
                    });
            }
            
            if (mFilter.Contains(di.Name.ToLower() ))
            {
                isInDir.RemoveAt(isInDir.Count - 1);
            }
        }
        static void Main1(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(@"usage: copyDir [filter] src dst dir1 dir2...");
                return;
            }
            int filterIdx = 2;
            if (args[0].ToLower() == "filter")
            {
                filterOrRemoveIf = true;
                if (args.Length < 3)
                {
                    Console.WriteLine(@"usage: copyDir [filter] src dst dir1 dir2...");
                    return;
                }
                mSrc = args[1];
                mDst = args[2];
                filterIdx = 3;
            }
            else
            {
                filterOrRemoveIf = false;
                mSrc = args[0];
                mDst = args[1];
                filterIdx = 2;
            }

            if (args.Length > filterIdx)
            {
                for (int i = filterIdx; i < args.Length; ++i)
                {
                    mFilter.Add(args[i].ToLower());
                }
            }

            GetAllFileList(mSrc);
            DirectoryInfo srcDi = new DirectoryInfo(mSrc);

            var strNow = DateTime.Now.ToString("yyMMddHHmm");
            DirectoryInfo dstDi = new DirectoryInfo(mDst + strNow);

            foreach(var item in mAllFiles)
            {
                FileInfo f = new FileInfo(item);
                var srcp = f.Directory.FullName;
                var dstp = srcp.Replace(srcDi.FullName, dstDi.FullName);
                buildDir(dstp);
                Console.WriteLine("\n\ncopy:" + f.FullName + "\n" + dstp + "\\" + f.Name);
                System.IO.File.Copy(f.FullName, dstp+"\\"+f.Name, true);
            }
        }
    }
}
