using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaKeysMapper
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CorrespondingKeyCodeAttribute : System.Attribute
    {
        public int Code { get; internal set; }

        public CorrespondingKeyCodeAttribute(int code)
        {
            Code = code;
        }
    }
}
