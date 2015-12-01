using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeParser
{
    class Program
    {
        static void Main(string[] args)
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
                c.markPos();
            }
            var l = c.getLineMarked();
                Console.WriteLine(l);
                c.popPos();
                c.markPos();
                l = c.getLineMarked();
                Console.WriteLine(l);
        }
    }
}
