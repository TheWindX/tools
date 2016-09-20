using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace MiniEditor
{
    /// <summary>
    /// Interaction logic for itemList.xaml
    /// </summary>
    public partial class itemList : UserControl
    {
        public itemList()
        {
            InitializeComponent();
        }

        //增加到最后
        private void addEditorItem(EditorObject item)
        {
            addItem(item.getComponent<COMEditorObject>().getMenuItem());
            foreach(var sub in item.children)
            {
                addEditorItem(sub);
            }
        }

        private EditorObject getLast(EditorObject item)
        {
            var items = item.children.ToArray();
            if(items.Count() == 0)
            {
                return item;
            }
            return getLast(items.Last());
        }

        public void insertEditorObject(EditorObject item)
        {
            if(currentItem == null)
            {
                addItem(item.getComponent<COMEditorObject>().getMenuItem());
            }
            else
            {
                item.parent = currentItem.editObject;
                //reflushEditorObject();
                //addItemAfter(item.parent.getComponent<COMEditorObject>().getMenuItem(), item.getComponent<COMEditorObject>().getMenuItem());
                //addItemAfter(getLast(currentItem.editObject).getComponent<COMEditorObject>().getMenuItem(),
                //        item.getComponent<COMEditorObject>().getMenuItem());
            }

            reflushEditorObject();
        }

        public void removeEditorObject(EditorObject item)
        {
            item.parent = null;
            reflushEditorObject();
        }

        public void upperEditorObject(EditorObject item)
        {
            item.upper();
            reflushEditorObject();
        }

        public void lowerEditorObject(EditorObject item)
        {
            item.lower();
            reflushEditorObject();
        }

        public void levelUpEditorObject(EditorObject item)
        {
            item.levelUp();
            reflushEditorObject();
        }

        public void levelDownEditorObject(EditorObject item)
        {
            item.levelDown();
            reflushEditorObject();
        }

        public void addItem(listItem item)
        {
            if (item == null) return;
            m_object_list.Children.Add(item);
            item.evtOnPick = () => pickUI(item);
            item.evtOnExpand = isExpand => expand(item, isExpand);
        }
        
        //public void removeEditorObject(EditorObject obj)
        //{
        //    var uiItem = obj.getComponent<COMEditorObject>().getMenuItem();
        //    removeItem(uiItem);
        //}

        public void removeEditorObjectChildren(EditorObject obj)
        {
            var uiItem = obj.getComponent<COMEditorObject>().getMenuItem();
            removeChildren(uiItem);
        }

        public void removeItem(listItem item)
        {
            if (item == null) return;
            m_object_list.Children.Remove(item);
            item.evtOnPick = null;
            item.evtOnExpand = null;

            //删除所有component
            try
            {
                var eo = item.editObject;
                eo.parent = null;
                EditorWorld.removeObject(eo);
                foreach (var com in eo.components.ToArray())
                {
                    if (com.GetType() == typeof(COMEditorObject)) continue;
                    eo.removeComponent(com);
                }
            }
            catch(Exception ex)
            {
                MLogger.error(ex.ToString());
            }

            removeChildren(item);
        }
        
        public void removeChildren(listItem item)
        {
            var eo = item.editObject;
            foreach (var sub in eo.children.ToArray())
            {
                var subUI = sub.getComponent<COMEditorObject>().getMenuItem();
                removeItem(subUI);
            }
        }

        public void showItem(listItem itemToShow, bool isShow)
        {
            var eo = itemToShow.editObject;
            var ui = eo.getComponent<COMEditorObject>().getMenuItem();
            if (isShow)
            {
                ui.Visibility = Visibility.Visible;

                if (ui.expand)
                {
                    foreach (var sub in eo.children)
                    {
                        var subUI = sub.getComponent<COMEditorObject>().getMenuItem();
                        showItem(subUI, isShow);
                    }
                }
            }
            else
            {
                ui.Visibility = Visibility.Collapsed;
                if (ui.expand)
                {
                    foreach (var sub in eo.children)
                    {
                        var subUI = sub.getComponent<COMEditorObject>().getMenuItem();
                        showItem(subUI, isShow);
                    }
                }
            }
        }

        public void expand(listItem itemToExpand, bool isExpand)
        {
            itemToExpand.expand = isExpand;

            var eo = itemToExpand.editObject;
            
            foreach (var sub in eo.children)
            {
                var subUI = sub.getComponent<COMEditorObject>().getMenuItem();
                showItem(subUI, isExpand);
            }
        }

        public void addItemAfter(listItem itemBefore, listItem item)
        {
            if (item == null) return;
            var i = 0;
            var count = m_object_list.Children.Count;
            for (; i<count; ++i)
            {
                if(m_object_list.Children[i] == itemBefore)
                {
                    break;
                }
            }
            i = i + 1;
            m_object_list.Children.Insert(i, item);
            item.evtOnPick = () => pickUI(item);
            item.evtOnExpand = isExpand => expand(item, isExpand);
        }

        public void addItemBefore(listItem itemAfter, listItem item)
        {
            if (item == null) return;
            var i = 0;
            for (; i < m_object_list.Children.Count; ++i)
            {
                if (m_object_list.Children[i] == itemAfter)
                {
                    break;
                }
            }
            m_object_list.Children.Insert(i, item);
            item.evtOnPick = () => pickUI(item);
            item.evtOnExpand = isExpand => expand(item, isExpand);
        }

        listItem currentItem = null;
        public void pickUI(listItem item)
        {
            if(item == currentItem)
            {
                return;
            }
            if(currentItem != null)
            {
                currentItem.isPick = false;
            }
            item.isPick = true;
            currentItem = item;

            EditorFuncs.getComponentPage().reflush();
        }

        public void pickEditObject(EditorObject obj)
        {
            var item = obj.getComponent<COMEditorObject>().getMenuItem();
            pickUI(item);
        }

        //todo, no in UI
        public EditorObject getCurrentObj()
        {
            if(currentItem == null)
            {
                return null;
            }

            return currentItem.editObject;
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            PreviewKeyDown += ItemList_KeyDown;
        }

        public static string xmlData = null;
        private void ItemList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    xmlData = obj.toString();
                }
            }
            else if (e.Key == Key.V)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    if (xmlData == null) return;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlData);
                    var eo = MComponentExtender.editorObjectFromXML(getCurrentObj(), doc.DocumentElement);
                    EditorFuncs.getItemListPage().reflushEditorObject();
                }
            }
            else if (e.Key == Key.X)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    if (EditorWorld.getRootEditorObject() == obj) return;
                    xmlData = obj.toString();
                    getCurrentObj().parent = null;
                    pickEditObject(EditorWorld.getRootEditorObject());
                    EditorFuncs.getItemListPage().reflushEditorObject();
                }
            }
            else if (e.Key == Key.Delete)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    if (EditorWorld.getRootEditorObject() == obj) return;
                    removeEditorObject(obj);
                }
            }
            else if (e.Key == Key.Up)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    if (EditorWorld.getRootEditorObject() == obj) return;
                    if (obj == null) return;
                    upperEditorObject(obj);
                }
                else
                {
                    var obj = getCurrentObj();
                    if (obj == null) return;
                    pickEditObject(obj.next);
                }

            }
            else if (e.Key == Key.Down)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    if (EditorWorld.getRootEditorObject() == obj) return;
                    if (obj == null) return;
                    lowerEditorObject(obj);
                }
                else
                {
                    var obj = getCurrentObj();
                    if (obj == null) return;
                    pickEditObject(obj.preview);
                }
            }
            else if (e.Key == Key.Left)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    if (EditorWorld.getRootEditorObject() == obj) return;
                    if (obj == null) return;
                    levelUpEditorObject(obj);
                }
                else
                {
                    var obj = getCurrentObj();
                    if (obj == null) return;
                    if (obj.parent == null) return;
                    pickEditObject(obj.parent);
                }
            }
            else if (e.Key == Key.Right)
            {
                if (EditorFuncs.isLeftControlPressed())
                {
                    var obj = getCurrentObj();
                    if (EditorWorld.getRootEditorObject() == obj) return;
                    if (obj == null) return;
                    levelDownEditorObject(obj);
                }
                else
                {
                    var obj = getCurrentObj();
                    if (obj == null) return;
                    foreach (var c in obj.children)
                    {
                        pickEditObject(c);
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var obj = EditorWorld.getRootEditorObject();
            addEditorItem(obj);
            pickEditObject(obj);
        }

        public void reflushEditorObject()
        {
            m_object_list.Children.Clear();
            var obj = EditorWorld.getRootEditorObject();
            addEditorItem(obj);
            EditorFuncs.getComponentPage().reflush();
        }
    }
}
