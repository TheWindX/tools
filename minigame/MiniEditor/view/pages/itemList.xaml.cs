using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private void addEditorItem(MObject item)
        {
            addItem(item.getComponent<COMEditorObject>().getMenuItem());
            foreach(var sub in item.children)
            {
                addEditorItem(sub);
            }
        }

        private MObject getLast(MObject item)
        {
            var items = item.children.ToArray();
            if(items.Count() == 0)
            {
                return item;
            }
            return getLast(items.Last());
        }

        public void insertEditorObject(MObject item)
        {
            if(currentItem == null)
            {
                addItem(item.getComponent<COMEditorObject>().getMenuItem());
            }
            else
            {
                //item.parent = currentItem.mObject.parent;
                addItemAfter(item.parent.getComponent<COMEditorObject>().getMenuItem(), item.getComponent<COMEditorObject>().getMenuItem());
                //addItemAfter(getLast(currentItem.mObject).getComponent<COMEditorObject>().getMenuItem(),
                //        item.getComponent<COMEditorObject>().getMenuItem());
            }
        }

        public void addItem(listItem item)
        {
            if (item == null) return;
            m_object_list.Children.Add(item);
            item.evtOnPick = () => pickUI(item);
            item.evtOnExpand = isExpand => expand(item, isExpand);
        }
        
        public void removeEditorObject(MObject obj)
        {
            var uiItem = obj.getComponent<COMEditorObject>().getMenuItem();
            removeItem(uiItem);
        }

        public void removeEditorObjectChildren(MObject obj)
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
                var eo = item.mObject;
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
            var eo = item.mObject;
            foreach (var sub in eo.children.ToArray())
            {
                var subUI = sub.getComponent<COMEditorObject>().getMenuItem();
                removeItem(subUI);
            }
        }

        public void showItem(listItem itemToShow, bool isShow)
        {
            var eo = itemToShow.mObject;
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

            var eo = itemToExpand.mObject;
            
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
                var uimapObject = currentItem.mObject.getComponent<COMMapObject>();
                if (uimapObject != null)
                {
                    uimapObject.getMapUIItem().isPicked = false;
                }
            }
            item.isPick = true;
            currentItem = item;

            EditorFuncs.getComponentPage().reflush();
            var uimapObject1 = currentItem.mObject.getComponent<COMMapObject>();
            if(uimapObject1 != null)
            {
                uimapObject1.getMapUIItem().isPicked = true;
            }
        }

        public void pickEditObject(MObject obj)
        {
            var item = obj.getComponent<COMEditorObject>().getMenuItem();
            pickUI(item);
        }

        //todo, no in UI
        public MObject getCurrentObj()
        {
            if(currentItem == null)
            {
                return null;
            }

            return currentItem.mObject;
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            PreviewKeyDown += ItemList_KeyDown;
        }

        private void ItemList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up)
            {
                var obj = getCurrentObj();
                if (obj == null) return;
                pickEditObject(obj.next);
            }
            else if(e.Key == Key.Down)
            {
                var obj = getCurrentObj();
                if (obj == null) return;
                pickEditObject(obj.preview);
            }
            else if (e.Key == Key.Left)
            {
                var obj = getCurrentObj();
                if (obj == null) return;
                if (obj.parent == null) return;
                pickEditObject(obj.parent);
            }
            else if (e.Key == Key.Right)
            {
                var obj = getCurrentObj();
                if (obj == null) return;
                foreach (var c in obj.children)
                {
                    pickEditObject(c);
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
        }
    }
}
