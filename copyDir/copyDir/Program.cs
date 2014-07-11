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
        static List<string> mFilterDir = new List<string>();
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
            if (mFilterDir.Contains(di.Name.ToLower() ))
            {
                isInDir.Add(true);
            }
            DirectoryInfo[] diA = di.GetDirectories();
            
            for (int i = 0; i < diA.Length; i++)
            {
                GetAllFileList(diA[i].FullName);
            }

            if (isInDir.Last())
            {
                var fiA = di.GetFiles();
                
                fiA.ToList().ForEach(fi =>
                {
                    mAllFiles.Add(fi.FullName);
                });
            }
            if (mFilterDir.Contains(di.Name.ToLower() ))
            {
                isInDir.RemoveAt(isInDir.Count - 1);
            }
        }
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(@"usage: copyDir src dst dir1 dir2...");
                return;
            }
            mSrc = args[0];
            mDst = args[1];

            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; ++i)
                {
                    mFilterDir.Add(args[i].ToLower());
                }
            }

            GetAllFileList(mSrc);
            DirectoryInfo srcDi = new DirectoryInfo(mSrc);
            DirectoryInfo dstDi = new DirectoryInfo(mDst);
            
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
