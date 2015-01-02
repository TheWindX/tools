using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ns_CodeGen
{
    public static class ExportXML
    {
        public static void saveSimple(ControlTree self, string fpath)
        {
            System.IO.File.Delete(fpath);
            FileStream fout = new FileStream(fpath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fout.Close();
            string lns = "";
            saveNode(self, ref lns, self.mRoot, 0);
            System.IO.File.WriteAllText(fpath, lns);
        }

        public static void saveNode(ControlTree self, ref string ln, CCNodeInfo node, int ident)
        {
            if (node.typeName == "")
                node.typeName = "root";

            string lstart = new string(' ', ident);
            ln += string.Format(@"{0}<{1}
{2} name=""{3}""
{4} pos=""{5},{6}""
{7} size=""{8},{9}""
{10} anchor=""{11},{12}""", lstart, node.typeName, lstart, node.name, lstart, node.px, node.py, lstart, node.sx, node.sy, lstart, node.anchorX, node.anchorY);

            if (node.flistPath != "")
            {
                ln += "\n" + lstart + string.Format(@" plist=""{0}"" ", node.flistPath);
            }

            if (node.fpath != "")
            {
                ln += "\n" + lstart + string.Format(@" filePath=""{0}"" ", node.fpath);
            }
            if (node.flistPath != "")
            {
                ln += "\n" + lstart + string.Format(@" pressPlist=""{0}"" ", node.pressFlistFile);
            }
            if (node.pressFlistFile != "")
            {
                ln += "\n" + lstart + string.Format(@" pressFilepath=""{0}"" ", node.pressFpath);
            }
            if (node.typeName == "TextObjectData")
            {
                ln += "\n" + lstart + string.Format(@" color=""({0},{1},{2},{3})"" ", node.colorA, node.colorR, node.colorG, node.colorB);
                ln += "\n" + lstart + string.Format(@" text=""{0}"" ", node.text);
                ln += "\n" + lstart + string.Format(@" fontSize=""{0}"" ", node.fontSz);
            }
            ln += ">\n";
            foreach (var elem in node.mChildren)
            {
                saveNode(self, ref ln, elem, ident + 4);
            }
            ln += string.Format(@"{0}</{1}>", lstart, node.typeName);
            ln += "\n";
        }
    }
}
