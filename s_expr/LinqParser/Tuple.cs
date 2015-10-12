using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Tuple<T>
{
    public T _0;
}

public class Tuple<T, T1>
{
    public T _0;
    public T1 _1;
}

public class Tuple<T, T1, T2>
{
    public T _0;
    public T1 _1;
    public T2 _2;
    public static Tuple<T, T1, T2> make(T t, T1 t1, T2 t2)
    {
        return new Tuple<T, T1, T2> { _0 = t, _1 = t1, _2 = t2 };
    }
}

public class Tuple<T, T1, T2, T3>
{
    public T _0;
    public T1 _1;
    public T2 _2;
    public T3 _3;
    public static Tuple<T, T1, T2, T3> make(T t, T1 t1, T2 t2, T3 t3)
    {
        return new Tuple<T, T1, T2, T3> { _0 = t, _1 = t1, _2 = t2, _3=t3 };
    }
}


public class Tuple<T, T1, T2, T3, T4>
{
    public T _0;
    public T1 _1;
    public T2 _2;
    public T3 _3;
    public T4 _4;
    public static Tuple<T, T1, T2, T3, T4> make(T t, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        return new Tuple<T, T1, T2, T3, T4> { _0 = t, _1 = t1, _2 = t2, _3 = t3 ,_4 = t4};
    }
}


public class Tuple<T, T1, T2, T3, T4, T5>
{
    public T _0;
    public T1 _1;
    public T2 _2;
    public T3 _3;
    public T4 _4;
    public T5 _5;
    public static Tuple<T, T1, T2, T3, T4, T5> make(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        return new Tuple<T, T1, T2, T3, T4, T5> { _0 = t, _1 = t1, _2 = t2, _3 = t3, _4 = t4, _5=t5};
    }
}


public class Tuple<T, T1, T2, T3, T4, T5, T6>
{
    public T _0;
    public T1 _1;
    public T2 _2;
    public T3 _3;
    public T4 _4;
    public T5 _5;
    public T6 _6;
    public static Tuple<T, T1, T2, T3, T4, T5, T6> make(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
        return new Tuple<T, T1, T2, T3, T4, T5, T6> { _0 = t, _1 = t1, _2 = t2, _3 = t3, _4 = t4, _5 = t5, _6 = t6};
    }
}


public class Tuple
{
    public static Tuple<T> make<T>(T t)
    {
        return new Tuple<T> { _0 = t };
    }

    public static Tuple<T, T1> make<T, T1>(T t, T1 t1)
    {
        return new Tuple<T, T1> { _0 = t, _1 = t1 };
    }

    public static Tuple<T, T1, T2> make<T, T1, T2>(T t, T1 t1, T2 t2)
    {
        return new Tuple<T, T1, T2> { _0 = t, _1 = t1, _2 = t2 };
    }

    public static Tuple<T, T1, T2, T3> make<T, T1, T2, T3>(T t, T1 t1, T2 t2, T3 t3)
    {
        return new Tuple<T, T1, T2, T3> { _0 = t, _1 = t1, _2 = t2, _3 = t3 };
    }
    public static Tuple<T, T1, T2, T3, T4> make<T, T1, T2, T3, T4>(T t, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        return new Tuple<T, T1, T2, T3, T4> { _0 = t, _1 = t1, _2 = t2, _3 = t3, _4 = t4 };
    }
    public static Tuple<T, T1, T2, T3, T4, T5> make<T, T1, T2, T3, T4, T5>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        return new Tuple<T, T1, T2, T3, T4, T5> { _0 = t, _1 = t1, _2 = t2, _3 = t3, _4 = t4, _5 = t5 };
    }

    public static Tuple<T, T1, T2, T3, T4, T5, T6> make<T, T1, T2, T3, T4, T5, T6>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
        return new Tuple<T, T1, T2, T3, T4, T5, T6> { _0 = t, _1 = t1, _2 = t2, _3 = t3, _4 = t4, _5 = t5, _6=t6 };
    }
}