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
                mUI.PreviewKeyDown += MUI_PreviewKeyDown;

                mUI.evtOnRightDown += pt =>
                {
                    if (entityManager.pickSt == entityManager.e_createLineSt.e_init)
                    {
                        entityManager.pickSt = entityManager.e_createLineSt.e_pickOnePoint;
                        entityManager.curLink = new CNodeLink();
                        entityManager.curLink.IsEnabled = false;
                        entityManager.curLink.IsHitTestVisible = false;
                        (mUI.Parent as Grid).Children.Add(entityManager.curLink);
                        entityManager.curLink.leftNode = mUI;
                        entityManager.curLink.begin = pt;
                    }
                };

                mUI.evtOnLeftUp += pt =>
                {
                    if (entityManager.pickSt == entityManager.e_createLineSt.e_pickOnePoint)
                    {
                        entityManager.pickSt = entityManager.e_createLineSt.e_init;
                        entityManager.curLink.end = pt;
                        entityManager.curLink.rightNode = mUI;
                    }
                };
            }
            return mUI;
        }




        private void MUI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                StringEnter.ShowOnTop(str =>
                {
                    mUI.text = str;
                });
            }
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
