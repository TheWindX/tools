/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
namespace MiniEditor
{
    [CustomComponent(path = "TEST", name = "test1")]
    class COMTest1 : MComponent
    {
        public bool boolValue { get; set; }
        public int intValue { get; set; }
        public double doubleValue { get; set; }
        public string stringValue { get; set; }

        [Description]
        public string descripValue { get { return "description from test1"; } }
    }
}
