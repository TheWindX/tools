using System;
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
    public partial class listItem : UserControl
    {
        public listItem()
        {
            InitializeComponent();
            isPick = false;
            mName.MouseDown += MName_MouseDown;
            mBG.MouseDown += MName_MouseDown;
            mExpand.MouseDown += MExpand_MouseDown;
        }

        private void MExpand_MouseDown(object sender, MouseButtonEventArgs e)
        {
            expand = !expand;
            if (evtOnExpand != null) evtOnExpand(expand);
        }

        private void MName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //isPick = true;
            if (evtOnPick != null) evtOnPick();
        }

        public System.Action<bool> evtOnExpand;
        public System.Action evtOnPick;

        public EditObject editObject
        {
            get;
            set;
        }
        public string name
        {
            get
            {
                return mName.Text;
            }
            set
            {
                mName.Text = value;
            }
        }

        public bool expand
        {
            get
            {
                return mExpand.Text == "-";
            }
            set
            {
                if(value)
                {
                    mExpand.Text = "-";
                }
                else
                {
                    mExpand.Text = "+";
                }
            }
        }

        private int mLevel = 0;
        public int level
        {
            get
            {
                return mLevel;
            }
            set
            {
                mLevel = value;
                this.Margin = new Thickness(20 * mLevel, 0, 0, 0);
            }
        }

        public static SolidColorBrush pickbrush = new SolidColorBrush(Colors.Black);
        public static SolidColorBrush unpickbrush = new SolidColorBrush(Color.FromArgb(0,0,0,0));
        public bool mIsPick = false;
        public bool isPick
        {
            get
            {
                return mIsPick;
            }
            set
            {
                if (value) mBG.Fill = pickbrush;
                else mBG.Fill = unpickbrush;

                if(mIsPick == false)
                {
                    if(value)
                    {
                        foreach (var com in editObject.components)
                        {
                            try
                            {
                                com.editorInit();
                            }
                            catch(Exception ex)
                            {
                                MLogger.error(ex.ToString());
                            }
                        }
                        MRuntime.evtFrame += MRuntime_evtFrame;
                    }
                }
                else
                {
                    if(!value)
                    {
                        foreach (var com in editObject.components)
                        {
                            try
                            {
                                com.editorExit();
                            }
                            catch (Exception ex)
                            {
                                MLogger.error(ex.ToString());
                            }
                        }
                        MRuntime.evtFrame -= MRuntime_evtFrame;
                    }
                }
                mIsPick = value;
            }
        }

        private void MRuntime_evtFrame()
        {
            foreach (var com in editObject.components)
            {
                try
                {
                    com.editorUpdate();
                }
                catch (Exception ex)
                {
                    MLogger.error(ex.ToString());
                }
            }
        }
    }
}
