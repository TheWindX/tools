using System;
using System.Collections.Generic;
using System.Text;


namespace ns_CodeGen
{
    public static class ExportLua
    {
        public const string luaTemplate = @"
require ""extern""
require ""Include/LuaCtrl""

UITemplate = class( ""UITemplate"", function()
	return cc.Node:create()
end)

function UITemplate:ctor()
	
end

function UITemplate:preload()

end

function UITemplate:init()
end

function UITemplate:resetLayout()
	
end


function UITemplate:release()
	
end

--创建 node
function UITemplate:addSingleNode(paresentCtrl, posx, posy)
    local node = cc.Node:create()
    node:setPosition(posx, posy)
    paresentCtrl:addChild(node)
    return node
end

--创建sprite
function UITemplate:AddSprite(paresentCtrl, filePath, posx, posy, anchorx, anchory)
    local image = cc.Sprite:create( filePath );
    image:setPosition(posx, posy)
    image:setAnchorPoint(anchorx, anchory)
    paresentCtrl:addChild(image)
    return image
end

--从 image frame 创建sprite
function UITemplate:AddFrameSprite(paresentCtrl, filePath, posx, posy, anchorx, anchory)
    local image = cc.Sprite:createWithSpriteFrameName( filePath );
    image:setPosition(posx, posy)
    image:setAnchorPoint(anchorx, anchory)
    paresentCtrl:addChild(image)
    return image
end

--创建sprite
function UITemplate:addButton(paresentCtrl, filePath, pressFilePath, posx, posy, anchorx, anchory)
    local button = ccui.Button:create( filePath, pressFilePath, nil, 0)
    button:setPosition(posx, posy)
    button:setAnchorPoint(anchorx, anchory)
    paresentCtrl:addChild(button)
    return button
end

--从 image frame 创建button
function UITemplate:addFrameButton(paresentCtrl, filePath, pressFilePath, posx, posy, anchorx, anchory)
    local button = ccui.Button:create( filePath, pressFilePath, nil, 1)
    button:setPosition(posx, posy)
    button:setAnchorPoint(anchorx, anchory)
    paresentCtrl:addChild(button)
    return button
end

--创建lable
function UITemplate:addLabel(paresentCtrl, text, fontsz, colorR, colorG, colorB, posx, posy, anchorx, anchory)
    return Control.addSysFont( paresentCtrl, text, ""Microsoft Yahei"", fontsz, {colorR, colorG, colorB}, {posx, posy} , 1, 0, {anchorx, anchory});
end

--创建edit
function UITemplate:addEdit(paresentCtrl, fontsz, colorR, colorG, colorB, posx, posy, anchorx, anchory, sizex, sizey)
    local editText = CEditBoxEx:create();
	editText:InitEditBox( cc.size( sizex, sizey), ""common/common_input.png"");
	editText:setPosition( posx+(0.5-anchorx)*sizex, posy+(0.5-anchory)*sizey);
	editText:setFontName( ""Microsoft Yahei"" );
	editText:setFontSize( fontsz );
	editText:setFontColor( ccc3( colorR,colorG,colorB ) );
	editText:setMaxLength( 32 );
	paresentCtrl:addChild( editText, 10);
    return editText
end
";
        const string nodePrefix = "node_";
        const string spritePrefix = "sprite_";
        const string buttonPrefix = "button_";
        const string labelPrefix = "label_";
        const string editPrefix = "edit_";
        

        static string ctorPiece = "";
        static string preloadPiece;
        static string initPiece;
        static string resetLayoutPiece;
        static string releasePiece;
        static string endPiece;

        static void splitByString(string str, string splitStr, out string before, out string after)
        {
            int len = splitStr.Length;
            int idx = str.IndexOf(splitStr);
            before = str;
            after = "";
            if (idx != -1)
            {
                before = str.Substring(0, idx + len);
                after = str.Substring(idx + len);
            }
        }

        //分段处理
        public static void split()
        {
            string str = luaTemplate;
            splitByString(str, "ctor()\r\n", out ctorPiece, out str);
            splitByString(str, "preload()\r\n", out preloadPiece, out str);
            splitByString(str, "init()\r\n", out initPiece, out str);
            splitByString(str, "resetLayout()\r\n", out resetLayoutPiece, out str);
            splitByString(str, "release()\r\n", out releasePiece, out endPiece);
        }

        public static string exportLua(ControlTree tree, string className)
        {
            split();
            string ret = "";
            ret += ctorPiece.Replace("UITemplate", className);
            ret += constructCor(tree);
            ret += preloadPiece.Replace("UITemplate", className);
            //preload piece
            ret += constructPreload(tree);
            ret += initPiece.Replace("UITemplate", className);
            ret += resetLayoutPiece.Replace("UITemplate", className);
            ret += constructBuilder(tree);
            ret += releasePiece.Replace("UITemplate", className);
            ret += constructRelease(tree);
            ret += endPiece.Replace("UITemplate", className);
            return ret;
        }

        public static string constructPreload(ControlTree tree)
        {
            string ret = "";
            var fs = tree.resourceFiles();
            var plists = tree.listFiles();
            ret += "\n    --resources file\n";
            foreach (var elem in fs)
            {
                ret += string.Format(@"    cc.Director:getInstance():getTextureCache():addImage( ""{0}"" )
", elem); 
            }
            ret += "\n    --plist file\n";
            foreach (var elem in plists)
            {
                ret += string.Format(@"    cc.SpriteFrameCache:getInstance():addSpriteFramesWithFile(""{0}"")
", elem);
            }
            return ret;
        }


        public static string constructRelease(ControlTree tree)
        {
            string ret = "";
            var fs = tree.resourceFiles();
            var plists = tree.listFiles();
            ret += "\n    --plist file\n";
            foreach (var elem in plists)
            {
                ret += string.Format(@"    cc.SpriteFrameCache:getInstance():removeSpriteFramesFromFile(""{0}"")
", elem);
            }
            return ret;
        }


        public static string constructCor(ControlTree tree)
        {
            string ret = "";
            ret += "\n    --控件\n";

            var names = new List<string>();
            System.Action<CCNodeInfo> act = (info) =>
            {
                if (info.typeName == "SingleNodeObjectData")//精灵控件
                {
                    names.Add(nodePrefix + info.name.ToLower());
                }
                else if (info.typeName == "SpriteObjectData")//精灵控件
                {
                    names.Add(spritePrefix + info.name.ToLower());
                }
                else if (info.typeName == "ButtonObjectData")//按钮控件
                {
                    names.Add(buttonPrefix + info.name.ToLower());
                }
                else if (info.typeName == "TextObjectData")//文字控件
                {
                    names.Add(labelPrefix + info.name.ToLower());
                }
                else if (info.typeName == "TextFieldObjectData")//编辑框控件
                {
                    names.Add(editPrefix + info.name.ToLower());
                }
            };
            tree.walkTree(tree.mRoot, act);

            foreach (var elem in names)
            {
                ret += string.Format(@"    self.{0} = nil
", elem);
            }
            return ret;
        }


        public static string constructBuilder(ControlTree tree)
        {
            string ret = "";
            Stack<string> paresentCtrl = new Stack<string>();
            //paresentCtrl.Push("self");
            string currentControll = "self";
            System.Action<CCNodeInfo> onNode = info =>
                {
                    if (info.typeName == "SpriteObjectData")//精灵控件
                    {
                        currentControll = "self." + spritePrefix + info.name.ToLower();
                        if (info.frame)
                        {
                            ret += string.Format("    {0} = self:AddFrameSprite({1}, \"{2}\", {3}, {4}, {5}, {6})\n",
                                currentControll, paresentCtrl.Peek(), info.fpath, info.px, info.py, info.anchorX, info.anchorY);
                        }
                        else
                        {
                            ret += string.Format("    {0} = self:AddSprite({1}, \"{2}\", {3}, {4}, {5}, {6})\n",
                                currentControll, paresentCtrl.Peek(), info.fpath, info.px, info.py, info.anchorX, info.anchorY);
                        }
                    }
                    else if (info.typeName == "SingleNodeObjectData")//精灵控件
                    {
                        currentControll = "self."+ nodePrefix + info.name.ToLower();
                        
                            ret += string.Format("    {0} = self:addSingleNode({1}, {2}, {3})\n",
                                currentControll, paresentCtrl.Peek(), info.px, info.py);
                        
                    }
                    else if (info.typeName == "ButtonObjectData")//按钮控件
                    {
                        currentControll = "self." + buttonPrefix + info.name.ToLower();
                        if (info.frame)
                        {
                            ret += string.Format("    {7} = self:addFrameButton({0}, \"{1}\", \"{2}\", {3}, {4}, {5}, {6})\n", 
                                paresentCtrl.Peek(), info.fpath, info.pressFpath, info.px, info.py, info.anchorX, info.anchorY, currentControll);
                        }
                        else
                        {
                            ret += string.Format("    {7} = self:addButton({0}, \"{1}\", \"{2}\", {3}, {4}, {5}, {6})\n", 
                                paresentCtrl.Peek(), info.fpath, info.pressFpath, info.px, info.py, info.anchorX, info.anchorY, currentControll);
                        }
                    }
                    else if (info.typeName == "TextObjectData")//字体控件
                    {
                        currentControll = "self." + labelPrefix + info.name.ToLower();
                        ret += string.Format("    {0} = self:addLabel({1}, \"{2}\", {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})\n",
                                currentControll, paresentCtrl.Peek(), info.text, info.fontSz, info.colorR, info.colorG, info.colorB, info.px, info.py, info.anchorX, info.anchorY);
                        
                    }
                    else if (info.typeName == "TextFieldObjectData")//编辑框控件
                    {
                        currentControll = "self." + editPrefix + info.name.ToLower();
                        ret += string.Format("    {0} = self:addEdit({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})\n",
                                currentControll, paresentCtrl.Peek(), info.fontSz, info.colorR, info.colorG, info.colorB,
                                info.px, info.py, info.anchorX, info.anchorY, info.sx, info.sy);

                    }
                };
            System.Action<CCNodeInfo> onEnterChildren = info =>
                {
                    paresentCtrl.Push(currentControll);
                };
            System.Action<CCNodeInfo> onExitChildrent = info =>
                {
                    currentControll = paresentCtrl.Pop();
                };
            tree.walkTree(tree.mRoot, onNode, onEnterChildren, onExitChildrent);
            return ret;
        }
       
    }
}
