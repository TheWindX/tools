/*
 * author: xiaofeng.li
 * mail: 453588006
 * */
using System;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.Threading;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ns_YAUtils
{
    public class dynaCaller
    {
        static dynaCaller mIns = null;
        private dynaCaller()
        {
            mIns = this;
        }

        public static dynaCaller Instance
        {
            get
            {
                if (mIns == null)
                {
                    mIns = new dynaCaller();
                }
                return mIns;
            }
        }

        
        String mCodeFrame = @"
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ns_YAUtils;//note: 这个随namespace改
public static class Wrapper
{{
    public static void print(Object o)
    {{
        Console.WriteLine(o.ToString() );
    }}
    public static void PerformAction()
    {{  
        {0};// user code goes here
    }}
}}";

        CSharpCodeProvider mCodeProvide = null;
        CompilerParameters mCompileOptions = null;

        public void init()
        {
            mCodeProvide = new CSharpCodeProvider();
            mCompileOptions = new CompilerParameters();

            

            mCompileOptions.GenerateInMemory = true;
            mCompileOptions.GenerateExecutable = false;

            mCompileOptions.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            mCompileOptions.ReferencedAssemblies.Add("System.dll");
            
            // mCompileOptions in system libraries
            mCompileOptions.ReferencedAssemblies.Add(typeof(dynaCaller).Assembly.Location);
        }


        public void runString(string str)
        {
            var res = mCodeProvide.CompileAssemblyFromSource(mCompileOptions, String.Format(mCodeFrame, str));

            if (res.Errors.Count > 0)
            {
                foreach (CompilerError error in res.Errors)
                {
                    var err = string.Format("Compiler Error ({0}): {1}", error.Line - 17, error.ErrorText);// 17 is location in whole codeFormat, 
                    System.IO.File.WriteAllText(Application.StartupPath + Program.logFile, err);
                }
            }
            else
            {
                var codeObject = res.CompiledAssembly.GetType("Wrapper");
                var scriptFunc = codeObject.GetMethod("PerformAction", BindingFlags.Public | BindingFlags.Static);
                if (scriptFunc != null)
                {
                    try
                    {
                        scriptFunc.Invoke(null, null);
                    }
                    catch (Exception ex)
                    {
                        var err = ex.ToString();
                        System.IO.File.WriteAllText(Application.StartupPath + Program.logFile, err);
                    }
                }
                else
                {
                    System.IO.File.WriteAllText(Application.StartupPath + Program.logFile, "runntime Error: scirptFunc == null");
                }
            }
        }
    };

    struct taskMessage
    {
        public DateTime mTime;
        public string mMessage;
    }

    public static class Program
    {
        public const string configPath = "\\task_timer.tab";
        public const int secondsClose = 60*4;//4 minits
        public const string logFile = "\\task_timer_err.txt";

        static List<taskMessage> mTasks = new List<taskMessage>();
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                //read file
                var lines = System.IO.File.ReadAllLines(Application.StartupPath + configPath);
                var lines1 = lines.Where(str =>
                {
                    str = str.Trim();
                    if (str == "")
                    {
                        return false;
                    }
                    if (str.StartsWith("#"))
                    {
                        return false;
                    }
                    var cols = str.Split(new char[] { '\t' });
                    if (cols.Length < 2) return false;

                    return true;
                });

                lines1.ForEach(str =>
                {
                    var cols = str.Split(new char[] { '\t' });
                    DateTime dt =
                        DateTime.ParseExact(cols[0], "HH_mm", System.Globalization.CultureInfo.InvariantCulture);
                    var t = new taskMessage();
                    t.mTime = dt;
                    t.mMessage = cols[1];
                    mTasks.Add(t);
                });
                //lines, filter trim, empty

                //for each parse datetime, and message

                //sort message

                //filter < deltaTime
                dynaCaller.Instance.init();
                mTasks.ForEach(t =>
                {
                    var now = DateTime.Now;
                    if (Math.Abs(t.mTime.Subtract(now).TotalSeconds) < secondsClose)
                    {
                        //MessageBox.Show(t.mMessage, "定时");
                        dynaCaller.Instance.runString(t.mMessage);
                    }
                });
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText(Application.StartupPath + Program.logFile, e.Message);
            }
            
        }
    }
}