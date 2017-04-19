using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace copyDir
{
    class FileCopier
    {
        string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public string srcDir
        {
            get;set;
        }
        public string destDir
        {
            get;set;
        }
        public Func<string, bool> pathFilter
        {
            get;set;
        }

        List<string> srcFiles = new List<string>();//related paths
        
        public void copyAll()
        {
            string srcPathFull = Path.GetFullPath(srcDir);
            string desPathFull = Path.GetFullPath(destDir);
            srcFiles = searchDir(srcPathFull);
            
            foreach (var fpath in srcFiles)
            {
                var relativePath = GetRelativePath(fpath, srcPathFull);
                var destPathFull = desPathFull + "/" + relativePath;
                var dirPath = getDir(destPathFull);
                makeDir(dirPath);
                copyFile(fpath, destPathFull);
                Console.WriteLine(relativePath + " is copyed");
            }
        }

        private string getDir(string fpath)
        {
            var f = new FileInfo(fpath);
            return f.Directory.FullName;
            //fpath = fpath.Replace("\\", "/");
            //var idx = fpath.LastIndexOf("/");
            //return fpath.Substring(0, idx);
        }

        private void copyFile(string v1, string v2)
        {
            File.Copy(v1, v2, true);
        }

        private void makeDir(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        private List<string> searchDir(string srcPathFull)
        {
            List<string> rets = new List<string>();
            doSearchDir(srcPathFull, rets);
            return rets;
        }

        private void doSearchDir(string srcPathFull, List<string> rets)
        {
            if (!pathFilter(srcPathFull))
            {
                return;
            }

            DirectoryInfo di = new DirectoryInfo(srcPathFull);
            var fs = di.GetFiles();
            foreach(var f in fs)
            {
                if (!pathFilter(f.FullName))
                {
                    continue;
                }
                rets.Add(f.FullName);
            }
            var ds = di.GetDirectories();
            foreach(var d in ds)
            {
                doSearchDir(d.FullName, rets);
            }
        }

        private static bool IsSymbolic(string path)
        {
            FileInfo pathInfo = new FileInfo(path);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }

        public static void Main(string[] args)
        {
            try
            {
                var strRegFile = args[0];
                var strSrcPath = args[1];
                var strRegs = File.ReadLines(strRegFile);
                var regs = strRegs.Select(strReg => new Regex(strReg)).ToArray();


                var exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                var exeFile = new FileInfo(exePath);
                var srcDi = new DirectoryInfo(exeFile.Directory + "/" + strSrcPath);

                var strNow = DateTime.Now.ToString("yyMMddHHmmss");
                DirectoryInfo dstDi = new DirectoryInfo(srcDi + "_" + strNow);

                var fc = new FileCopier();
                fc.srcDir = srcDi.FullName;
                fc.destDir = dstDi.FullName;
                fc.pathFilter = f =>
                {
                    if (IsSymbolic(f))
                    {
                        Console.WriteLine(f + " is symbolic link, return");
                        return false;
                    }
                    foreach (var reg in regs)
                    {
                        var m = reg.Match(f);
                        if (m.Success)
                        {
                            Console.WriteLine(f + " match to " + reg.ToString() + ", return");
                            return false;
                        }
                    }
                    
                    return true;
                };
                fc.copyAll();
                var strEnd = DateTime.Now.ToString("yyMMddHHmmss");
                Console.WriteLine("finished at " + strEnd);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
