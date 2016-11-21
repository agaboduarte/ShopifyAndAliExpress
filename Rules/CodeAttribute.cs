using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules
{
    [AttributeUsage(AttributeTargets.All)]
    public class CodeAttribute : Attribute
    {
        public CodeAttribute(string code)
        {
            Code = code;
        }

        public virtual string Code { get; set; }
    }
}
