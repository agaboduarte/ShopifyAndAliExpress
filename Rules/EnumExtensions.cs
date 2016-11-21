using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules
{
    public static class EnumExtensions
    {
        public static string GetCodeAttribute(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(CodeAttribute)) as CodeAttribute;

            return attribute == null ? value.ToString() : attribute.Code;
        }
    }
}
