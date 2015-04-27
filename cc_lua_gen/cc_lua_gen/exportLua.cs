using System;
using System.Collections.Generic;
using System.Text;


namespace ns_CodeGen
{
    public static class ExportLua
    {
        public const string luaTemplate = @"
--生成代码开始--
require ""extern""
require ""Include/LuaCtrl""
require ""Logic/base/scene_base""

UITemplateToReplace = class( ""UITemplateToReplace"", function()
	return layout:createScene()
end)

function UITemplateToReplace:ctor()
	
    self:_Myptr( self )
end

function UITemplateToReplace:preload()
    
end

function UITemplateToReplace:initScene()
    self:preload()
    local mainlayer = self:resetLayout()
end

function UITemplateToReplace:exitScene()
	
end

--创建 node
function UITemplateToReplace:addSingleNode(name, paresentCtrl, posx, posy)
    local node = ccui.Widget:create()
    node:setName(name)
    node:setPosition(posx, posy)
    paresentCtrl:addChild(node)
    return node
end

--创建sprite
function UITemplateToReplace:AddSprite(name, paresentCtrl, filePath, posx, posy, anchorx, anchory)
    local image = ccui.ImageView:create( filePath, 0);
    image:setName(name)
    image:setPosition(posx, posy)
    image:setAnchorPoint(anchorx, anchory)
    paresentCtrl:addChild(image)
    return image
end

--从 image frame 创建sprite
function UITemplateToReplace:AddFrameSprite(name, paresentCtrl, filePath, posx, posy, anchorx, anchory)
    local image = ccui.ImageView:create( filePath, 1);
    image:setName(name)
    image:setPosition(posx, posy)
    image:setAnchorPoint(anchorx, anchory)
    paresentCtrl:addChild(image)
    return image
end

--创建sprite
function UITemplateToReplace:addButton(name, paresentCtrl, filePath, pressFilePath, disableFilePath, posx, posy, anchorx, anchory)
    local r = Control.addButton2(paresentCtrl, filePath, pressFilePath, nil, nil, {posx, posy}, 0, 0, 0)
    r:setAnchorPoint(anchorx, anchory)
    r:setName(name)
    return r
end

--从 image frame 创建button
function UITemplateToReplace:addFrameButton(name, paresentCtrl, filePath, pressFilePath, disableFilePath, posx, posy, anchorx, anchory)
    local r = Control.addButton2(paresentCtrl, filePath, pressFilePath, nil, nil, {posx, posy}, 0, 0, 1)
    r:setAnchorPoint(anchorx, anchory)
    r:setName(name)
    return r
end

--创建lable
function UITemplateToReplace:addLabel(name, paresentCtrl, text, fontsz, colorR, colorG, colorB, posx, posy, anchorx, anchory)
    local l = ccui.Text:create(text, ""Microsoft Yahei"", fontsz)
    l:setName(name)
    l:setPosition(posx, posy)
    l:setAnchorPoint(anchorx, anchory)
    l:setColor( cc.c3b( colorR, colorG, colorB ) )
    paresentCtrl:addChild(l)
    return l
end

--创建edit
function UITemplateToReplace:addEdit(name, paresentCtrl, fontsz, colorR, colorG, colorB, posx, posy, anchorx, anchory, sizex, sizey)
    local editText = CEditBoxEx:create();
	editText:InitEditBox( cc.size( sizex, sizey), ""common/common_input.png"");
	editText:setPosition( posx+(0.5-anchorx)*sizex, posy+(0.5-anchory)*sizey);
	editText:setFontName( ""Microsoft Yahei"" );
	editText:setFontSize( fontsz );
	editText:setFontColor( cc.c3b( colorR,colorG,colorB ) );
	editText:setMaxLength( 32 );
	paresentCtrl:addChild( editText);
    return editText
end

--创建scroll view
function UITemplateToReplace:addScrollView(name, paresentCtrl, posx, posy, anchorx, anchory, sx, sy, innersx, innersy)
    local ScrollView = ccui.ScrollView:create();
    ScrollView:setName(name)
    ScrollView:setPosition(posx, posy)
    ScrollView:setAnchorPoint(anchorx, anchory)
    ScrollView:setSize (cc.size(sx, sy) )
    ScrollView:setInnerContainerSize (cc.size(innersx, innersy) )
    ScrollView:setBounceEnabled(true);
    ScrollView:setTouchEnabled(true);
    paresentCtrl:addChild(ScrollView)
    return ScrollView
end


function UITemplateToReplace:resetLayout()
	

    --生成代码结束--
    
    --TODO bind here
end


";
        const string nodePrefix = "node_";
        const string spritePrefix = "sprite_";
        const string buttonPrefix = "button_";
        const string labelPrefix = "label_";
        const string editPrefix = "edit_";
        const string scrollPrefix = "scroll_";
        

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
            splitByString(str, "initScene()\r\n", out initPiece, out str);
            splitByString(str, "exitScene()\r\n", out releasePiece, out str);
            splitByString(str, "UITemplateToReplace:resetLayout()\r\n", out resetLayoutPiece, out endPiece);
        }

        public static string exportLua(ControlTree tree, string className)
        {
            split();
            string ret = "";
            ret += ctorPiece.Replace("UITemplateToReplace", className);
            ret += constructCor(tree);
            ret += preloadPiece.Replace("UITemplateToReplace", className);
            //preload piece
            ret += constructPreload(tree);
            ret += initPiece.Replace("UITemplateToReplace", className);
            ret += releasePiece.Replace("UITemplateToReplace", className);
            ret += constructRelease(tree);
            ret += resetLayoutPiece.Replace("UITemplateToReplace", className);
            ret += constructBuilder(tree);
            ret += endPiece.Replace("UITemplateToReplace", className);
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
                else if (info.typeName == "ScrollViewObjectData")//编辑框控件
                {
                    names.Add(scrollPrefix + info.name.ToLower());
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
                            ret += string.Format("    {0} = self:AddFrameSprite(\"{7}\", {1}, \"{2}\", {3}, {4}, {5}, {6})\n",
                                currentControll, paresentCtrl.Peek(), info.fpath, info.px, info.py, info.anchorX, info.anchorY, info.name.ToLower());
                        }
                        else
                        {
                            ret += string.Format("    {0} = self:AddSprite(\"{7}\", {1}, \"{2}\", {3}, {4}, {5}, {6})\n",
                                currentControll, paresentCtrl.Peek(), info.fpath, info.px, info.py, info.anchorX, info.anchorY, info.name.ToLower());
                        }
                    }
                    else if (info.typeName == "SingleNodeObjectData")//精灵控件
                    {
                        currentControll = "self."+ nodePrefix + info.name.ToLower();
                        
                            ret += string.Format("    {0} = self:addSingleNode(\"{4}\", {1}, {2}, {3})\n",
                                currentControll, paresentCtrl.Peek(), info.px, info.py, info.name.ToLower());
                        
                    }
                    else if (info.typeName == "ButtonObjectData")//按钮控件
                    {
                        currentControll = "self." + buttonPrefix + info.name.ToLower();
                        if (info.frame)
                        {
                            ret += string.Format("    {8} = self:addFrameButton(\"{9}\", {0}, \"{1}\", \"{2}\", \"{3}\", {4}, {5}, {6}, {7})\n",
                                paresentCtrl.Peek(), info.fpath, info.pressFpath, info.disableFpath, info.px, info.py, info.anchorX, info.anchorY, currentControll, info.name.ToLower());
                        }
                        else
                        {
                            ret += string.Format("    {8} = self:addButton(\"{9}\", {0}, \"{1}\", \"{2}\", \"{3}\", {4}, {5}, {6}, {7})\n",
                                paresentCtrl.Peek(), info.fpath, info.pressFpath, info.disableFpath, info.px, info.py, info.anchorX, info.anchorY, currentControll, info.name.ToLower());
                        }
                    }
                    else if (info.typeName == "TextObjectData")//字体控件
                    {
                        currentControll = "self." + labelPrefix + info.name.ToLower();
                        ret += string.Format("    {0} = self:addLabel(\"{11}\", {1}, \"{2}\", {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})\n",
                                currentControll, paresentCtrl.Peek(), info.text, info.fontSz, info.colorR, info.colorG, info.colorB, info.px, info.py, info.anchorX, info.anchorY, info.name.ToLower());
                        
                    }
                    else if (info.typeName == "TextFieldObjectData")//编辑框控件
                    {
                        currentControll = "self." + editPrefix + info.name.ToLower();
                        ret += string.Format("    {0} = self:addEdit(\"{12}\", {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})\n",
                                currentControll, paresentCtrl.Peek(), info.fontSz, info.colorR, info.colorG, info.colorB,
                                info.px, info.py, info.anchorX, info.anchorY, info.sx, info.sy, info.name.ToLower());

                    }
                    else if (info.typeName == "ScrollViewObjectData")//滚动控件
                    {
                        currentControll = "self." + scrollPrefix + info.name.ToLower();
                        ret += string.Format("    {0} = self:addScrollView(\"{10}\", {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})\n",
                                currentControll, paresentCtrl.Peek(), info.px, info.py, info.anchorX, info.anchorY, info.sx, info.sy,
                                info.innerSx, info.innerSy, info.name.ToLower());
                        if (info.ScrollDirectionType == scrollViewType.Horizontal)
                        {
                            ret += string.Format("    {0}:setDirection(2)\n", currentControll);
                        }
                        else if (info.ScrollDirectionType == scrollViewType.Vertical_Horizontal)
                        {
                            ret += string.Format("    {0}:setDirection(3)\n", currentControll);
                        }
                    }

                    if (ret != "")
                    {
                        if (!info.visible)
                        {
                            ret += string.Format("    {0}:setVisible(false)\n", currentControll);
                        }

                        if (info.scaleX != 1 || info.scaleY != 1)
                        {
                            ret += string.Format("    {0}:setScale({1}, {2})\n", currentControll, info.scaleX, info.scaleY);
                        }
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
