using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touch
{
    public class linker
    {
        private linker()
        {

        }

        public static linker create()
        {
            var ins = new linker();
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

        public static linker getIns()
        {
            return mIns;
        }
        static linker mIns = null;

        private entity _left;
        private entity _right;
        public entity left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                if(_left == null)
                {
                    getUI().leftNode = null;
                    return;
                }
                getUI().leftNode = _left.getUI();
                getUI().updateView();
            }
        }

        public entity right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
                if (_right == null)
                {
                    getUI().rightNode = null;
                    return;
                }
                getUI().rightNode = _right.getUI();
                getUI().updateView();
            }
        }



        public CNodeLink _UI = null;
        public CNodeLink getUI()
        {
            if(_UI == null)
            {
                _UI = new CNodeLink();
            }
            return _UI;
        }
    }
}
