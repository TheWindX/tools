using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ns_CodeGen
{
    delegate bool deleExecFunc(List<string> strs);
    class Program
    {
        class COption
        {
            public string name;
            public string short_name;
            public deleExecFunc exec = null;
        }

        static List<COption> executors = new List<COption>();

        static string xmlPath = "";
        static string resourcePath = "";
        static string outPath = "";
        static bool init(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"usage: 
    cc_lua_gen.exe -xml <xmlfile> -resource <resource_dir> [-out <out_path>]
    cc_lua_gen.exe -x <xmlfile> -rd <resource_dir> [-o <out_path>]

    <xmlfile>     : xml 路径(coco2dx *.csd)
    <resource_dir>: resource_dir 资源路径
    <out_path>    : 输出文件路径

example : 
    cc_lua_gen.exe -xml sample.csd -resource new_ui/
    cc_lua_gen.exe -x sample.csd -r new_ui/
    cc_lua_gen.exe -x sample.csd -r new_ui/

");
                return false;
            };


            var xmlOption = new COption();
            xmlOption.name = "-xml";
            xmlOption.short_name = "-x";
            xmlOption.exec = list =>
            {
                if (list.Count < 1)
                {
                    Console.WriteLine("没有xml参数");
                    return false;
                }

                try
                {
                    xmlPath = list[0];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                return true;
            };
            executors.Add(xmlOption);

            var resourceOption = new COption();
            resourceOption.name = "-resource";
            resourceOption.short_name = "-r";
            resourceOption.exec = list =>
            {
                if (list.Count < 1)
                {
                    Console.WriteLine("没有resource参数");
                    return false;
                }

                resourcePath = list[0];
                return true;
            };
            executors.Add(resourceOption);

            var outOption = new COption();
            outOption.name = "-out";
            outOption.short_name = "-o";
            outOption.exec = list =>
            {
                if (list.Count < 1)
                {
                    Console.WriteLine("没有out参数");
                    return false;
                }

                outPath = list[0];
                return true;
            };
            executors.Add(outOption);

            COption op = xmlOption;
            List<string> paras = new List<string>();
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
                    op.exec(paras);
                }
            }


            return true;
        }

        static void Main(string[] args)
        {
            if (!init(args)) return;

            try
            {
                ControlTree ct = new ControlTree();
                ct.loadXML(xmlPath, resourcePath);
                if (outPath == "")
                {
                    var className = System.IO.Path.GetFileNameWithoutExtension(xmlPath);
                    ExportXML.saveSimple(ct, className + "_simple.xml");
                    Console.WriteLine("输出到" + System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + className + "_simple.xml");
                    string strlua = ExportLua.exportLua(ct, className);
                    System.IO.File.WriteAllText(className + ".lua", strlua);
                    Console.WriteLine("输出到" + System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + className + ".lua");
                    
                }
                else
                {
                    var className = System.IO.Path.GetFileNameWithoutExtension(outPath);
                    ExportXML.saveSimple(ct, className + "_simple.xml");
                    Console.WriteLine("输出到" + System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + className + "_simple.xml");
                    string strlua = ExportLua.exportLua(ct, className);
                    System.IO.File.WriteAllText(outPath, strlua);
                    Console.WriteLine("输出到" + System.IO.Path.GetFullPath(outPath));
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("发生错误:\n    "+e.Message);
            }
            
        }
    }
}
