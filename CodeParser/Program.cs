using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeParser
{
    class Program
    {
        static void test1()
        {
            var c = new CodeSource();
            c.setSoruce(
@"asdf
1asdf
1234
1234asdf
333388881111
");
            c.getchar();
            c.markPos();
            c.pushPos();

            for (int i = 0; i < 4; i++ )
            {
                c.getchar();
                c.pushPos();
                c.markPos();
            }
            var l = c.getLineMarked();
                Console.WriteLine(l);
                c.popPos();
                c.popPos();
                c.popPos();
                c.markPos();
                l = c.getLineMarked();
                Console.WriteLine(l);
        }


        static void test2()
        {
            SymbolFactory sf = new SymbolFactory();
            sf.pushLeft();
            sf.pushEpsilon();
            sf.pushEpsilon();
            sf.pushStringSymbol("123");
            sf.pushLeft();
            sf.pushEpsilon();
            sf.pushEpsilon();
            sf.pushStringSymbol("456");
            sf.pushOr("t");
            sf.pushAnd("r");
            var symb = sf.popSymbol();
            var rs = (symb as compoundExp).getAllRules();
            var ls = rs.getList();

            ls.Reverse();
            ls.ForEach(r =>
                {
                    Console.WriteLine(r.getRuleRep());
                });
            //Console.WriteLine(symb.getRep());
        }

        static void Main(string[] args)
        {
            test2();
        }
    }
}
