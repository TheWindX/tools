using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Touch
{
    public class entity
    {
        private entity()
        {

        }

        public static entity create()
        {
            var ins = new entity();
            mIns = ins;
            ins.focus();
            return ins;
        }

        public void focus()
        {
            if (mIns != null) unfocus();
            mIns = this;
        }

        public void unfocus()
        {
        }

        public static entity getIns()
        {
            return mIns;
        }
        static entity mIns = null;

        public string name;

        public float x;
        public float y;

        CNode mUI = null;

        public CNode getUI()
        {
            if(mUI == null)
            {
                mUI = new CNode();
                mUI.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                mUI.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                
                mUI.evtOnRightDown += pt =>
                {
                    if (entityManager.ins.pickSt == entityManager.e_createLineSt.e_init)
                    {
                        entityManager.ins.pickSt = entityManager.e_createLineSt.e_pickOnePoint;
                        entityManager.ins.curLink = new CNodeLink();
                        entityManager.ins.curLink.IsEnabled = false;
                        entityManager.ins.curLink.IsHitTestVisible = false;
                        (mUI.Parent as Grid).Children.Add(entityManager.ins.curLink);
                        entityManager.ins.curLink.leftNode = mUI;
                        entityManager.ins.curLink.begin = pt;
                    }
                };

                mUI.evtOnLeftUp += pt =>
                {
                    if (entityManager.ins.pickSt == entityManager.e_createLineSt.e_pickOnePoint)
                    {
                        entityManager.ins.pickSt = entityManager.e_createLineSt.e_init;
                        entityManager.ins.curLink.end = pt;
                        entityManager.ins.curLink.rightNode = mUI;

                        var ent1 = entityManager.ins.mDatas.data.ents.Where(ent =>
                       {
                           return ent.getUI() == entityManager.ins.curLink.leftNode;
                       }).First();


                        var ent2 = entityManager.ins.mDatas.data.ents.Where(ent =>
                        {
                            return ent.getUI() == entityManager.ins.curLink.rightNode;
                        }).First();
                        entityManager.ins.getUI().Children.Remove(entityManager.ins.curLink);
                        entityManager.ins.addLinker(ent1, ent2);
                    }
                };
            }
            return mUI;
        }


        public void updateView()
        {
            var ui = getUI();
            ui.text = name;
            ui.Margin = new System.Windows.Thickness(x, y, 0, 0);
        }

        public void updateData()
        {
            var ui = getUI();
            name = ui.text;
            x = (float)ui.Margin.Left;
            y = (float)ui.Margin.Top;
        }
    }
}
