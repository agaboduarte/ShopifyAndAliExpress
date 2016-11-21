using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public class WordCollection : BaseCollection<TextParticle>
    {
        public WordCollection(string text)
            : base()
        {
            var particles = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in particles)
                this.Add(new TextParticle(item));
        }

        public WordCollection(IEnumerable<TextParticle> collection)
            : base(collection)
        {
        }

        public override BaseCollection<TextParticle> Short(int maxLength)
        {
            var clone = new WordCollection(this.Default);
            var cuttingLength = default(int?);

            if (clone.Edited.Length > maxLength)
            {
                // remove os conectivos.
                for (var index = clone.Count - 1; index >= 0 && clone.Edited.Length > maxLength; index--)
                {
                    var item = clone.ElementAt(index);

                    if (ConnectivesNames.Contains(item.DefaultNormalize) ||
                        item.DefaultNormalize.Length == 1 && Vowel.Contains(item.DefaultNormalize[0]))
                    {
                        item.Visible = false;
                    }
                }

                var middle = clone.Count / 2;
                var before = middle - 1;
                var after = middle + 1;

                if (middle >= 0)
                    clone.ElementAt(middle).Edited = this.Cutting(clone.ElementAt(middle).Edited, cuttingLength);

                while (clone.Edited.Length > maxLength && (before >= 0 || after < clone.Count))
                {
                    if (before >= 0)
                        clone.ElementAt(before).Edited = this.Cutting(clone.ElementAt(before--).Edited, cuttingLength);

                    if (clone.Edited.Length > maxLength && after < clone.Count)
                        clone.ElementAt(after).Edited = this.Cutting(clone.ElementAt(after++).Edited, cuttingLength);
                }

                if (clone.Edited.Length > maxLength)
                    return null;
            }

            return clone;
        }
    }
}
