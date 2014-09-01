using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Text;
using Monitor.Core.Utilities;

namespace Monitor.Core.Tests.Utilities
{
    class Program
    {
        class COption
        {
            public string name;
            public string short_name;
            public System.Action<List<string>> exec = null;
        }

        static List<COption> executors = new List<COption>();

        static void init()
        {
            var opCreate = new COption();
            opCreate.name = "-create";
            opCreate.short_name = "-c";
            opCreate.exec = list =>
            {
                if (list.Count < 2)
                {
                    Console.WriteLine("创建参数不足两个:"+list);
                    return;
                }
                
                bool overwrite = true;
                var jp = "";
                var src = "";
                if (list.Count >= 3)
                {
                    bool.TryParse(list[2], out overwrite);
                }
                if (list.Count >= 2)
                {
                    src = list[1];
                    jp = list[0];
                }

                try
                {
                    JunctionPoint.Create(jp, src, overwrite);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            };
            executors.Add(opCreate);

            var opDel = new COption();
            opDel.name = "-delete";
            opDel.short_name = "-d";
            opDel.exec = list =>
            {
                if (list.Count < 1)
                {
                    Console.WriteLine("删除参数不足一个");
                    return;
                }

                var jp = list[0];

                try
                {
                    JunctionPoint.Delete(jp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            };
            executors.Add(opDel);
        }

        static void Main(string[] args)
        {
            init();
            string opName = "";
            COption op = executors[0];//create
            List<string> paras = new List<string>();

            if (args.Length == 0)
            {
                Console.WriteLine(@"usage: 
创建链接：[-create] linkPath srcPath
删除链接: [-delete|-d] linkPath");
                return;
            }

            for (int i = 0; i < args.Length; ++i)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    for (int j = 0; j < executors.Count; ++j)
                    {
                        if (executors[j].short_name == arg || executors[j].name == arg)
                        {
                            op = executors[j];
                            paras.Clear();
                        }
                    }
                }
                else
                {
                    paras.Add(arg);
                }
            }

            if (op == null)
            {
                Console.WriteLine("不认识的操作"+args);
                return;
            }
            op.exec(paras);
        }
    }
}
