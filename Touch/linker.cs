using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touch
{
    public class linker
    {
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

        public CNodeLink getLinker()
        {
            return null;
        }

        public CNodeLink _UI = null;
        public CNodeLink getUI()
        {
            if(_UI == null)
            {
                _UI = new CNodeLink();
                _UI.evtRemove += () =>
                {
                    var lnk1 = entityManager.ins.linkers.Where(lnk =>
                    {
                        var ui = lnk.getUI();
                        return ui == _UI;
                    }).First();
                    entityManager.ins.removeLinker(lnk1);
                };
            }
            return _UI;
        }
    }
}
