using System.IO;
using System.Text;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
