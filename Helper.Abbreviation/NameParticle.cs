using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public class NameParticle : TextParticle
    {
        public NameParticle(string text)
            : base(text)
        {
        }

        public bool Suffix
        {
            get
            {
                return NameCollection.SuffixNames.Any(i => i.Key == this.DefaultNormalize || i.Value == this.DefaultNormalize);
            }
        }

        public bool Connective
        {
            get
            {
                return NameCollection.ConnectivesNames.Contains(this.DefaultNormalize);
            }
        }

        public override string ToString()
        {
            return string.Format("Default: {0}, DefaultNormalize: {1}, Edited: {2}, EditedNormalize: {6}, Suffix: {4}, Connective: {5}, Visible: {3}",
                this.Default, this.DefaultNormalize, this.Edited, this.Visible, this.Suffix, this.Connective, this.EditedNormalize);
        }
    }
}
