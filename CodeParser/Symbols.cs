using CodeParser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeParser
{
    class Symbol
    {
        public virtual string getRep(){return null;}
        protected Symbol() { }
    }


    class Epsilon : Symbol
    {
        public override string getRep() { return "Epsilon"; }
        public static Epsilon create()
        {
            return new Epsilon();
        }
    }

    class StringSymbol : Symbol
    {
        public string content;
        public override string getRep() { return string.Format("\"{0}\"", content); }
        public static StringSymbol create(string c)
        {
            var r = new StringSymbol();
            r.content = c;
            return r;
        }
    }

    class compoundExp : Symbol
    {
        protected List<Symbol> mSymbols = new List<Symbol>();

        string mKey = null;

        private void collectRules(StkList<compoundExp> allRules)
        {
            var k = getKey();
            if (k != null)
            {
                var idx = allRules.find(o=>o == this);
                if(idx == -1)
                {
                    allRules.push_back(this);

                    for (int i = 0; i < mSymbols.Count; ++i)
                    {
                        var s = mSymbols[i];
                        if (s is compoundExp)
                        {
                            var cs = (s as compoundExp);
                            cs.collectRules(allRules);
                        }
                    }
                }
            }
        }

        public StkList<compoundExp> getAllRules()
        {
            StkList<compoundExp> r = new StkList<compoundExp>();
            collectRules(r);
            return r;
        }

        //产生式
        public string getRuleRep()
        {
            var k = getKey();
            var v = getRep();
            return k + " = " + v;
        }

        public virtual int priority()//组合优先级
        {
            return 0;
        }
        public void setKey(string title)//key for rule
        {
            mKey = title;
        }
        public string getKey()
        {
            return mKey;
        }

        protected string _getRep(string splitter)
        {
            string r = "";
            int len = mSymbols.Count;
            for (int i = 0; i < len; ++i)
            {
                var s = mSymbols[i];

                if (s is compoundExp)
                {
                    var s1 = (s as compoundExp);
                    var k = s1.getKey();
                    if (k != null)
                    {
                        r += k;
                    }
                    else
                    {
                        var rep = s1.getRep();
                        if (s1.priority() <= this.priority())
                            r += "(" + rep + ")";
                        else
                            r += rep;
                    }
                }
                else
                {
                    r += s.getRep();
                }

                if (i != len - 1)
                {
                    r += splitter;
                }
            }
            return r;
        }
    }

    class AndExp : compoundExp
    {
        public static AndExp create()
        {
            var r = new AndExp();
            return r;
        }

        public AndExp ap(Symbol s)
        {
            mSymbols.Add(s);
            return this;
        }

        public override int priority()//组合优先级
        {
            return 1;
        }
        public override string getRep()
        {
            return _getRep(" + ");
        }
    }

    class OrExp : compoundExp
    {
        public static OrExp create()
        {
            var r = new OrExp();
            return r;
        }

        public OrExp ap(Symbol s)
        {
            mSymbols.Add(s);
            return this;
        }

        public override int priority()//组合优先级
        {
            return 0;
        }
        public override string getRep()
        {
            return _getRep(" | ");
        }
    }

    class StkObj
    {

    }

    class StkObjSymbol : StkObj
    {
        public Symbol symbol;
        private StkObjSymbol() { }
        public static StkObjSymbol create(Symbol s)
        {
            var obj = new StkObjSymbol();
            obj.symbol = s;
            return obj;
        }
    }

    class StkObjLeft : StkObj
    {
        public static StkObjLeft create()
        {
            var obj = new StkObjLeft();
            return obj;
        }
    }

    class SymbolFactory
    {
        StkList<StkObj> mStk = new StkList<StkObj>();
        public void pushEpsilon()
        {
            var e = Epsilon.create();
            var stko = StkObjSymbol.create(e);
            mStk.push_back(stko);
        }

        public void pushStringSymbol(string s)
        {
            var e = StringSymbol.create(s);
            var stko = StkObjSymbol.create(e);
            mStk.push_back(stko);
        }

        public void pushLeft()
        {
            var stko = StkObjLeft.create();
            mStk.push_back(stko);
        }

        public void pushSymbol(Symbol s)
        {
            mStk.push_back(StkObjSymbol.create(s));
        }

        public Symbol popSymbol()
        {
            var s = (mStk.peek() as StkObjSymbol).symbol;
            mStk.pop_back();
            return s;
        }

        public void pushAnd(string key = null)
        {
            var ando = AndExp.create();
            int leftIdx = mStk.rfind(o => o is StkObjLeft);
            if(leftIdx != -1)
            {
                for(int i = leftIdx+1; i<mStk.len(); ++i)
                {
                    var o = mStk.at(i);
                    var s = (o as StkObjSymbol).symbol;
                    ando.ap(s);
                }
                mStk.pop_to(leftIdx);
                ando.setKey(key);
                mStk.push_back(StkObjSymbol.create(ando));
                return;
            }
            throw new Exception("not left bracket");
        }

        public void pushOr(string key = null)
        {
            var oro = OrExp.create();
            int leftIdx = mStk.rfind(o => o is StkObjLeft);
            if (leftIdx != -1)
            {
                for (int i = leftIdx+1; i < mStk.len(); ++i)
                {
                    var o = mStk.at(i);
                    var s = (o as StkObjSymbol).symbol;
                    oro.ap(s);
                }
                mStk.pop_to(leftIdx);
                oro.setKey(key);
                mStk.push_back(StkObjSymbol.create(oro));
                return;
            }
            throw new Exception("not left bracket");
        }
        
        public Symbol peekSymbol()
        {
            var s = (mStk.peek() as StkObjSymbol).symbol;
            return s;
        }

        public StkObj peek()
        {
            return mStk.peek();
        }
    }
}
