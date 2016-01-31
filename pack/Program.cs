using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pk
{
    class Program
    {
        List<string> getAllFiles(string root, string postfix = "txt")
        {
            List<string> rets = new List<string>();
            var fs = Directory.GetFiles(root);
            foreach(var f in fs)
            {
                if (f.EndsWith(postfix))
                {
                    rets.Add(Path.GetFullPath(f));
                }
            }

            var dirs = Directory.GetDirectories(root);
            foreach (var d in dirs)
            {
                rets.AddRange(getAllFiles(d, postfix));
            }
            return rets;
        }

        public string key = "1";
        
        void packFile(string fin)
        {
            fin = fin.Replace("\\", "/");
            var idx = fin.LastIndexOf("/");
            var fname = fin.Substring(idx + 1);
            var fdir = fin.Substring(0, idx+1);
            var fname1 = fdir + packStr(fname)+".pk";
            var cont1 = packBytes(File.ReadAllBytes(fin));
            File.WriteAllBytes(fname1, cont1);
        }

        void unpackFile(string fin)
        {
            fin = fin.Replace("\\", "/");
            var idx = fin.LastIndexOf("/");
            var fname = fin.Substring(idx + 1);
            fname = fname.Substring(0, fname.Length - 3);
            var fdir = fin.Substring(0, idx + 1);
            var fname1 = fdir + unpackStr(fname);
            var cont1 = packBytes(File.ReadAllBytes(fin));
            File.WriteAllBytes(fname1, cont1);
        }

        string packStr(string str)
        {
            string ret = "";
            int idx = 0;
            foreach(char c in str)
            {
                if (idx == key.Length) idx = 0;
                ret += (char)((ushort)c ^ (ushort)key[idx]);
                idx++;
            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ret);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        string unpackStr(string str)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(str);
            var str1 = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            string ret = "";
            int idx = 0;
            foreach (char c in str1)
            {
                if (idx == key.Length) idx = 0;
                ret += (char)((ushort)c ^ (ushort)key[idx]);
                idx++;
            }
            return ret;
        }

        byte[] packBytes(byte[] bs)
        {
            List<byte> ret = new List<byte>();
            int idx = 0;
            foreach (byte b in bs)
            {
                if (idx == key.Length) idx = 0;
                var bpack = (byte)(key[idx] | 0x0f);
                ret.Add((byte)(b ^ bpack));
                idx++;
            }
            var str = System.Convert.ToBase64String(ret.ToArray());
            return System.Convert.FromBase64String(str);
        }

        byte[] unpack(byte[] bs)
        {
            List<byte> ret = new List<byte>();
            int idx = 0;
            foreach (byte b in bs)
            {
                if (idx == key.Length) idx = 0;
                var bpack = (byte)(key[idx] | 0x0f);
                ret.Add((byte)(b ^ bpack));
                idx++;
            }
            var str = System.Convert.ToBase64String(ret.ToArray());
            return System.Convert.FromBase64String(str);
        }

        void pack(string dir)
        {
            var files = getAllFiles(dir, ".txt");
            foreach (var f in files)
            {
                packFile(f);
            }
        }

        void unpack(string dir)
        {
            var files = getAllFiles(dir, ".pk");
            foreach (var f in files)
            {
                unpackFile(f);
            }
        }

        static void Main(string[] args)
        {
            var p = new Program();
            if(args.Length < 3)
            {
                return;
            }
            if(args[1] == "p")
            {
                p.key = args[2];
                p.pack(args[0]);
            }
            else
            {
                p.key = args[2];
                p.unpack(args[0]);
            }
            
        }
    }
}
