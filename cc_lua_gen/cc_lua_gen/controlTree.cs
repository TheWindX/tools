using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;


//控件树, 数据结构

namespace ns_CodeGen
{
    //控件信息
    public class CCNodeInfo
    {
        public string name = "";
        public string typeName = "";
        public int px = 0;
        public int py = 0;
        public int sx = 64;
        public int sy = 64;
        public int innerSx = 64;
        public int innerSy = 64;
        public float anchorX = 0;
        public float anchorY = 0;
        public string fpath = "";
        public string flistPath = "";
        public string pressFpath = "";
        public string pressFlistFile = "";
        public bool frame = false;

        public string text = "";
        public int fontSz = 20;
        public int colorA = 255;
        public int colorR = 255;
        public int colorG = 255;
        public int colorB = 255;

        public bool visible = true;
        public List<CCNodeInfo> mChildren = new List<CCNodeInfo>();
    }

    //控件树
    public class ControlTree
    {
        //解析cocos stdio csd文件, 生成控件树
        public void loadXML(string fpath, string resourceStartWith)
        {
            mResourceStartWith = resourceStartWith;
            //fpath = fpath.Trim();
            var xml = System.IO.File.ReadAllText(fpath);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var node = doc.ChildNodes[0];
            parseNewNode(node);
        }
        
        public CCNodeInfo mRoot = null;
        public string mResourceStartWith = "";

        CCNodeInfo mCurrent = null;
        Stack<CCNodeInfo> mNodeStack = new Stack<CCNodeInfo>();

        void rootNode()
        {
            mRoot = new CCNodeInfo();
            mCurrent = mRoot;
        }

        void newNode(string name, string typeName)
        {
            var node = new CCNodeInfo();
            node.name = name;
            node.typeName = typeName;
            var p = mNodeStack.Peek();
            p.mChildren.Add(node);
            mCurrent = node;
        }

        void setPosition(int x, int y)
        {
            mCurrent.px = x;
            mCurrent.py = y;
        }

        void setAnchor(float x, float y)
        {
            mCurrent.anchorX = x;
            mCurrent.anchorY = y;
        }

        void setSize(int x, int y)
        {
            mCurrent.sx = x;
            mCurrent.sy = y;
        }

        void setInnerSize(int x, int y)
        {
            mCurrent.innerSx = x;
            mCurrent.innerSy = y;
        }

        void fileInfo(bool frame, string flist, string fname)
        {
            mCurrent.frame = frame;
            mCurrent.flistPath = flist;
            mCurrent.fpath = fname;
        }

        void pressFileInfo(bool frame, string flist, string fname)
        {
            mCurrent.frame = frame;
            mCurrent.pressFlistFile = flist;
            mCurrent.pressFpath = fname;
        }

        void setVisible(bool visible)
        {
            mCurrent.visible = visible;
        }

        void setColor(int a, int r, int g, int b)
        {
            mCurrent.colorA = a;
            mCurrent.colorR = r;
            mCurrent.colorG = g;
            mCurrent.colorB = b;
        }

        void enterChild()
        {
            mNodeStack.Push(mCurrent);
        }

        void exitChild()
        {
            mCurrent = mNodeStack.Peek();
            mNodeStack.Pop();
        }

        void parseNewNode(XmlNode node)
        {
            bool childNode = false;
            XmlElement elem = node as XmlElement;
            if (elem.Name == "ObjectData")
            {
                rootNode();
            }
            if (elem.Name == "NodeObjectData")
            {
                string name = elem.GetAttribute("Name");
                string ctype = elem.GetAttribute("ctype");
                newNode(name, ctype);

                string strVisible = elem.GetAttribute("Visible");
                if (strVisible != "")
                {
                    setVisible(bool.Parse(strVisible));
                }

                string labelText = elem.GetAttribute("LabelText");
                string strFontSize = elem.GetAttribute("FontSize");
                if (strFontSize != "")
                {
                    mCurrent.fontSz = int.Parse(strFontSize);
                }
                mCurrent.text = labelText;
            }
            else if (elem.Name == "Position")
            {
                string strx = elem.GetAttribute("X");
                string stry = elem.GetAttribute("Y");
                int px = 0;
                int py = 0;
                if (strx != "")
                    px = (int)float.Parse(strx);
                if (stry != "")
                    py = (int)float.Parse(stry);
                setPosition(px, py);
            }
            else if (elem.Name == "AnchorPoint")
            {
                string strx = elem.GetAttribute("ScaleX");
                string stry = elem.GetAttribute("ScaleY");
                float ax = 0;
                float ay = 0;
                if (strx != "")
                    ax = float.Parse(strx);
                if (stry != "")
                    ay = float.Parse(stry);
                setAnchor(ax, ay);
            }
            else if (elem.Name == "Size")
            {
                string strx = elem.GetAttribute("X");
                string stry = elem.GetAttribute("Y");
                int sx = 64;
                int sy = 64;
                if (strx != "")
                    sx = (int)float.Parse(strx);
                if (stry != "")
                    sy = (int)float.Parse(stry);
                setSize(sx, sy);
            }
            else if (elem.Name == "InnerNodeSize")
            {
                string strx = elem.GetAttribute("Width");
                string stry = elem.GetAttribute("Height");
                int sx = 64;
                int sy = 64;
                if (strx != "")
                    sx = (int)float.Parse(strx);
                if (stry != "")
                    sy = (int)float.Parse(stry);
                setInnerSize(sx, sy);
            }
            else if (elem.Name == "FileData")
            {
                var strType = elem.GetAttribute("Type");
                bool frame = !(strType == "Normal");
                string path = elem.GetAttribute("Path");
                string plist = elem.GetAttribute("Plist");

                if (path.StartsWith(mResourceStartWith))
                {
                    path = path.Substring(mResourceStartWith.Length);
                }
                if (plist.StartsWith(mResourceStartWith))
                {
                    plist = plist.Substring(mResourceStartWith.Length);
                }
                fileInfo(frame, plist, path);
            }
            else if (elem.Name == "PressedFileData")
            {
                var strType = elem.GetAttribute("Type");
                bool frame = !(strType == "Normal");
                string path = elem.GetAttribute("Path");
                string plist = elem.GetAttribute("Plist");
                if (path.StartsWith(mResourceStartWith))
                {
                    path = path.Substring(mResourceStartWith.Length);
                }
                if (plist.StartsWith(mResourceStartWith))
                {
                    plist = plist.Substring(mResourceStartWith.Length);
                }
                pressFileInfo(frame, plist, path);
            }
            else if (elem.Name == "NormalFileData")
            {
                var strType = elem.GetAttribute("Type");
                bool frame = !(strType == "Normal");
                string path = elem.GetAttribute("Path");
                string plist = elem.GetAttribute("Plist");
                if (path.StartsWith(mResourceStartWith))
                {
                    path = path.Substring(mResourceStartWith.Length);
                }
                if (plist.StartsWith(mResourceStartWith))
                {
                    plist = plist.Substring(mResourceStartWith.Length);
                }
                fileInfo(frame, plist, path);
            }
            else if (elem.Name == "Children")
            {
                enterChild();
                childNode = true;
            }
            else if (elem.Name == "CColor")
            {
                var stra = elem.GetAttribute("A");
                var strr = elem.GetAttribute("R");
                var strg = elem.GetAttribute("G");
                var strb = elem.GetAttribute("B");

                setColor(int.Parse(stra), int.Parse(strr), int.Parse(strg), int.Parse(strb));
            }

            var cl = elem.ChildNodes;
            foreach (XmlNode elem1 in cl)
            {
                parseNewNode(elem1);
            }
            if (childNode)
            {
                exitChild();
            }
        }

        //遍历控件树
        public void walkTree(CCNodeInfo info, System.Action<CCNodeInfo> act, System.Action<CCNodeInfo> enterChildren = null, System.Action<CCNodeInfo> exitChildren = null)
        {
            act(info);
            if (enterChildren != null)
            {
                enterChildren(info);
            }
            foreach (var elem in info.mChildren)
            {
                walkTree(elem, act, enterChildren, exitChildren);
            }
            if (exitChildren != null)
            {
                exitChildren(info);
            }
        }

        //png file
        public List<string> resourceFiles()
        {
            var ret = new List<string>();
            System.Action<CCNodeInfo> act = (info) =>
                {
                    if (!info.frame)
                    {
                        if (info.fpath != "")
                        {
                            if (!ret.Contains(info.fpath))
                                ret.Add(info.fpath);
                        }
                        else if (info.pressFpath != "")//需要保证 press 和 normal 在同一张图
                        {
                            if (!ret.Contains(info.pressFpath))
                                ret.Add(info.pressFpath);
                        }
                    }
                };
            walkTree(mRoot, act);
            return ret;
        }

        //List file
        public List<string> listFiles()//需要保证 press 和 normal 在同一张图
        {
            var ret = new List<string>();
            System.Action<CCNodeInfo> act = (info) =>
            {
                if (info.flistPath != "")
                {
                    if (!ret.Contains(info.flistPath))
                        ret.Add(info.flistPath);
                }
                else if (info.pressFlistFile != "")//需要保证 press 和 normal 在同一张图
                {
                    if (!ret.Contains(info.pressFlistFile))
                        ret.Add(info.pressFlistFile);
                }
            };
            walkTree(mRoot, act); 
            return ret;
        }

    }


}
