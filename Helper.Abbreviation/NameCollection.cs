using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public class NameCollection : BaseCollection<NameParticle>
    {
        public NameCollection(string fullName)
        {
            var particles = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in particles)
                this.Add(new NameParticle(item));
        }

        public NameCollection(IEnumerable<NameParticle> collection)
            : base(collection)
        {
        }

        public NameParticle FirstName
        {
            get
            {
                return this.FirstOrDefault(i => !i.Suffix && i.Visible);
            }
        }

        public NameParticle SecondName
        {
            get
            {
                var enumerator = this.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == this.FirstName)
                    {
                        while (enumerator.MoveNext())
                        {
                            if (!enumerator.Current.Connective && !enumerator.Current.Suffix && enumerator.Current != this.LastName)
                                return enumerator.Current;
                        }
                    }
                }

                return null;
            }
        }

        public IEnumerable<NameParticle> ComposedName
        {
            get
            {
                if (this.FirstName != null && this.SecondName != null)
                {
                    var enumerator = this.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current == this.FirstName)
                        {
                            if (enumerator.MoveNext() && !enumerator.Current.Connective)
                                return new HashSet<NameParticle> { this.FirstName, this.SecondName };
                            else
                                return null;
                        }
                    }
                }

                return null;
            }
        }

        public IEnumerable<NameParticle> Middle
        {
            get
            {
                var enumerator = this.GetEnumerator();
                var middle = new HashSet<NameParticle>();
                var isMiddle = false;

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == this.SecondName)
                    {
                        isMiddle = true;
                        enumerator.MoveNext();
                    }

                    if (enumerator.Current == this.LastName)
                    {
                        isMiddle = false;
                        enumerator.MoveNext();
                    }

                    if (isMiddle && !enumerator.Current.Connective)
                        middle.Add(enumerator.Current);
                }

                if (middle.Any())
                    return middle;

                return null;
            }
        }

        public NameParticle LastName
        {
            get
            {
                return this.LastOrDefault(i => !i.Suffix && i.Visible);
            }
        }

        public NameParticle SuffixName
        {
            get
            {
                var suffix = this.LastOrDefault(i => i.Suffix && i.Visible);
                var last = this.LastOrDefault(i => i.Visible);

                if (last == suffix)
                    return suffix;

                return null;
            }
        }

        public IEnumerable<NameParticle> Connectives
        {
            get
            {
                return new HashSet<NameParticle>(this.Where(i => i.Connective));
            }
        }

        public override BaseCollection<NameParticle> Short(int maxLength)
        {
            int cuttingLength = 1;
            var clone = new NameCollection(this.Default);

            if (clone.Edited.Length > maxLength)
            {
                // replace para o sufixo curto.
                for (var index = clone.Count - 1; index >= 0 && clone.Edited.Length > maxLength; index--)
                {
                    var value = SuffixNames.FirstOrDefault(i => i.Key == clone.ElementAt(index).DefaultNormalize);

                    if (value.Value != null)
                        clone.ElementAt(index).Edited = value.Value;
                }

                var skippedLastName = false;

                // para os nomes entre primeiro nome e útimo nome faz a abreviação caso não seja um conectivo.
                for (var index = clone.Count - 1; index >= 1 && clone.Edited.Length > maxLength; index--)
                {
                    var isSuffix = SuffixNames.Any(i => i.Key == clone.ElementAt(index).DefaultNormalize || i.Value == clone.ElementAt(index).DefaultNormalize);

                    if (isSuffix)
                        continue;

                    if (!isSuffix && !skippedLastName)
                    {
                        skippedLastName = true;
                        continue;
                    }

                    if (!ConnectivesNames.Contains(clone.ElementAt(index).DefaultNormalize))
                        clone.ElementAt(index).Edited = this.Cutting(clone.ElementAt(index).Edited, cuttingLength);
                }

                // remove os conectivos.
                for (var index = clone.Count - 1; index >= 0 && clone.Edited.Length > maxLength; index--)
                {
                    if (ConnectivesNames.Contains(clone.ElementAt(index).DefaultNormalize))
                        clone.ElementAt(index).Visible = false;
                }

                var firstAndLastName = new NameCollection(clone.Default);

                for (int i = 0; i < firstAndLastName.Count; i++)
                {
                    firstAndLastName.ElementAt(i).Edited = clone.ElementAt(i).Edited;
                    firstAndLastName.ElementAt(i).Visible = clone.ElementAt(i).Visible;
                }

                var lastSuffix = true;
                skippedLastName = false;

                // resultado deixando o primeiro e último nome + cortando todo o meio do nome + deixando apenas o último sufixo.
                for (var index = firstAndLastName.Count - 1; index >= 1 && firstAndLastName.Edited.Length > maxLength; index--)
                {
                    var isSuffix = SuffixNames.Any(i => i.Key == firstAndLastName.ElementAt(index).DefaultNormalize || i.Value == firstAndLastName.ElementAt(index).DefaultNormalize);

                    if (isSuffix && lastSuffix)
                    {
                        lastSuffix = false;
                        continue;
                    }

                    if (!isSuffix && !skippedLastName)
                    {
                        skippedLastName = true;
                        continue;
                    }

                    firstAndLastName.ElementAt(index).Visible = false;
                }

                if (firstAndLastName.Edited.Length <= maxLength)
                    return firstAndLastName;

                // abrevia o primeiro nome
                clone.ElementAt(0).Edited = this.Cutting(clone.ElementAt(0).Edited, cuttingLength);

                skippedLastName = false;
                lastSuffix = true;

                // resultado abreviando todo o restante menos o último nome e removendo todos sufixos exceto o último.
                for (var index = clone.Count - 1; index >= 0 && clone.Edited.Length > maxLength; index--)
                {
                    var isSuffix = SuffixNames.Any(i => i.Key == clone.ElementAt(index).DefaultNormalize || i.Value == clone.ElementAt(index).DefaultNormalize);

                    if (isSuffix && lastSuffix)
                    {
                        lastSuffix = false;
                        continue;
                    }

                    if (!isSuffix && !skippedLastName)
                    {
                        skippedLastName = true;
                        continue;
                    }

                    if (isSuffix)
                        clone.ElementAt(index).Visible = false;
                    else
                        clone.ElementAt(index).Edited = this.Cutting(clone.ElementAt(index).Edited, cuttingLength);
                }

                skippedLastName = false;

                // corta todos os nomes entre primeiro e último
                for (var index = clone.Count - 1; index >= 1 && clone.Edited.Length > maxLength; index--)
                {
                    var isSuffix = SuffixNames.Any(i => i.Key == clone.ElementAt(index).DefaultNormalize || i.Value == clone.ElementAt(index).DefaultNormalize);

                    if (isSuffix)
                        continue;

                    if (!skippedLastName)
                    {
                        skippedLastName = true;
                        continue;
                    }

                    clone.ElementAt(index).Visible = false;
                }

                if (clone.Edited.Length > maxLength && clone.LastName != null)
                    clone.LastName.Edited = this.Cutting(clone.LastName.Edited, cuttingLength);

                if (clone.Edited.Length > maxLength)
                    return null;
            }

            return clone;
        }
    }
}
