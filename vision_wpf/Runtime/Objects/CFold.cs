﻿using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ns_vision
{
    public partial class CFold : Component
    {
        public override void inherit()
        {
            addComponent<CModuleTree>();

            //override of CModuleItem._HandleDrawIcon
            var mi = getComponent<CModuleItem>();
            mi._HandleDrawIcon = () =>
            {
                mICon.runtimeObject = this.getComponent<CModuleItem>();
                mICon.setTitle(getComponent<CNamed>().name);
                return mICon;
            };

            //override of CModuleItem._HandleSelect
            mi._HandleSelect = b =>
            {
                mICon.setSelect(b);
            };
        }

        public UI_ICON_Folder mICon = new UI_ICON_Folder();
    }

}