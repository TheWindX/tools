﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public void addEditorItem(EditObject item)
        {
            addItem(item.getComponent<editorUICOM>().getMenuItem());
            foreach(var sub in item.children)
            {
                addEditorItem(sub);
            }
        }

        private EditObject getLast(EditObject item)
        {
            var items = item.children.ToArray();
            if(items.Count() == 0)
            {
                return item;
            }
            return getLast(items.Last());
        }

        public void insertEditorItem(EditObject item)
        {
            if(itemPicked == null)
            {
                addItem(item.getComponent<editorUICOM>().getMenuItem());
            }
            else
            {
                item.parent = itemPicked.editObject.parent;
                addItemAfter(getLast(itemPicked.editObject).getComponent<editorUICOM>().getMenuItem(),
                        item.getComponent<editorUICOM>().getMenuItem());
            }
        }

        public void addItem(listItem item)
        {
            if (item == null) return;
            m_object_list.Children.Add(item);
            item.evtOnPick = () => onPick(item);
            item.evtOnExpand = isExpand => expand(item, isExpand);
        }

        public void showItem(listItem itemToShow, bool isShow)
        {
            var eo = itemToShow.editObject;
            var ui = eo.getComponent<editorUICOM>().getMenuItem();
            if (isShow)
            {
                ui.Visibility = Visibility.Visible;

                if (ui.expand)
                {
                    foreach (var sub in eo.children)
                    {
                        var subUI = sub.getComponent<editorUICOM>().getMenuItem();
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
                        var subUI = sub.getComponent<editorUICOM>().getMenuItem();
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
                var subUI = sub.getComponent<editorUICOM>().getMenuItem();
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
            item.evtOnPick = () => onPick(item);
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
            item.evtOnPick = () => onPick(item);
            item.evtOnExpand = isExpand => expand(item, isExpand);
        }

        listItem itemPicked = null;
        public void onPick(listItem item)
        {
            if(item == itemPicked)
            {
                return;
            }
            if(itemPicked != null)
            {
                itemPicked.isPick = false;
            }
            item.isPick = true;
            itemPicked = item;
        }
    }
}
