using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public class TitleCollection : BaseCollection<TextParticle>
    {
        public TitleCollection(string text)
            : base()
        {
            var particles = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in particles)
                this.Add(new TextParticle(item));
        }

        public TitleCollection(IEnumerable<TextParticle> collection)
            : base(collection)
        {
        }

        public override BaseCollection<TextParticle> Short(int maxLength)
        {
            var clone = new TitleCollection(this.Default);

            if (clone.Edited.Length > maxLength)
            {
                var last = clone.Count - 1;

                while (clone.Edited.Length > maxLength)
                {
                    clone.ElementAt(last--).Visible = false;
                }

                if (clone.Edited.Length > maxLength)
                    throw new NotSupportedException();
            }

            return clone;
        }
    }
}
