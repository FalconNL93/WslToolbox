using System.Reflection;

namespace WslToolbox.Core
{
    public class GenericClass
    {
        public static Assembly Assembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly();
        }
    }
}