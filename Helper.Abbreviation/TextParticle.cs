using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public class TextParticle
    {
        public TextParticle(string text)
        {
            var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(text);

            this.Default = text;
            this.DefaultNormalize = Encoding.UTF8.GetString(bytes).ToUpper();
            this.Edited = text;
            this.Visible = true;
        }

        public string Default { get; protected set; }

        public string DefaultNormalize { get; protected set; }

        public string Edited { get; set; }

        public string EditedNormalize
        {
            get
            {
                var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(Edited);
                return Encoding.UTF8.GetString(bytes).ToUpper();
            }
        }

        public bool Visible { get; set; }

        public override string ToString()
        {
            return string.Format("Default: {0}, DefaultNormalize: {1}, Edited: {2}, EditedNormalize: {3}, Visible: {4}",
                this.Default, this.DefaultNormalize, this.Edited, this.EditedNormalize, this.Visible);
        }
    }
}
