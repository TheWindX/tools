using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ExtractChinese
{

    public class FileMatchLines
    {
        public static Regex reg = new Regex(@"@?""([^""]*([\u4e00-\u9fff])[^""]*)""");
        public string mFilePath = "";
        public List<string> mLines = new List<string>();
        public List<matchLine> mMatchlines = new List<matchLine>();
        public class matchLine
        {
            public int lineIdx;
            public string mStr;
            public List<Capture> mCaps = new List<Capture>();
        }

        public FileMatchLines(string fpath)
        {
            mFilePath = fpath;
            mLines = File.ReadAllLines(fpath).ToList();
            for(int i = 0; i<mLines.Count; ++i)
            {
                var ms = reg.Matches(mLines[i]);
                if (ms.Count != 0)
                {
                    matchLine item = new matchLine();
                    item.lineIdx = i;
                    item.mStr = mLines[i];
                    foreach (Match m in ms)
                    {
                        var cap = m.Groups[0].Captures[0];
                        Console.WriteLine(fpath+","+i+"行:"+cap);
                        item.mCaps.Add(m.Groups[0].Captures[0]);
                    }
                    mMatchlines.Add(item);
                }
            }
        }
    }

    class Program
    {
        public static string[] GetAllFilesFullPath(string path, string mark = ".cs")
        {
            List<string> files = new List<string>();

            if (path.StartsWith("file"))
                path = Path.GetFullPath(path.Substring(7));

            string[] paths = Directory.GetFiles(path);
            foreach (string _p in paths)
            {
                if (_p.ToLower().EndsWith(mark))
                    files.Add(_p);
            }
            string[] dirs = Directory.GetDirectories(path);
            foreach (string _d in dirs)
            {
                files.AddRange(GetAllFilesFullPath(_d, mark));
            }
            return files.ToArray();
        }


        static void Main(string[] args)
        {
            List<FileMatchLines> fmls = new List<FileMatchLines>();
            Dictionary<string, string> str2id = new Dictionary<string, string>();
            Dictionary<string, string> id2str = new Dictionary<string, string>();
            var path = GetAllFilesFullPath(".");
            int idCount = 0;
            for (int i = 0; i < path.Count(); ++i)
            {
                FileMatchLines fml = new FileMatchLines(path[i]);
                if (fml.mMatchlines.Count > 0)
                {
                    fmls.Add(fml);
                    foreach (var ml in fml.mMatchlines)
                    {
                        foreach (var cap in ml.mCaps)
                        {
                            str2id[cap.Value] = "IDS_" + idCount++;
                        }
                    }
                }
            }

            //生成IDS文件
            List<string> IDSFileStr = new List<string>();
            foreach (var key in str2id.Keys)
            {
                id2str[str2id[key]] = key;
            }

            foreach (var id in id2str.Keys)
            {
                IDSFileStr.Add("public const string " + id + " = " + id2str[id]);
            }

            File.WriteAllLines("IDSTR.cs", IDSFileStr);

            for (int i = 0; i < fmls.Count; ++i)
            {
                var fml = fmls[i];
                
                foreach (var ml in fml.mMatchlines)
                {
                    foreach (var cap in ml.mCaps)
                    {
                        var ln = fml.mLines[ml.lineIdx];
                        fml.mLines[ml.lineIdx] = ln.Replace(cap.Value, "IDSTR." + str2id[cap.Value]);
                    }
                }
                File.WriteAllLines(fml.mFilePath, fml.mLines);
            }

            return;
        }
    }
}
