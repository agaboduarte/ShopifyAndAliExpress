using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public class StreetCollection : BaseCollection<NameParticle>
    {
        public StreetCollection(string streetType, string streetTitle, string street, string number, string complement)
        {
            if (streetType != null)
                this.StreetType = new NameParticle(streetType);
            if (streetTitle != null)
                this.StreetTitle = new NameParticle(streetTitle);
            if (street != null)
                this.Street = new NameCollection(street);
            if (number != null)
                this.Number = new NameParticle(number);
            if (complement != null)
                this.Complement = new NameParticle(complement);
        }

        public NameParticle StreetType { get; set; }

        public NameParticle StreetTitle { get; set; }

        public NameCollection Street { get; set; }

        public NameParticle Number { get; set; }

        public NameParticle Complement { get; set; }

        public new string Edited
        {
            get
            {
                var build = new HashSet<string>();

                if (this.StreetType != null && this.StreetType.Visible)
                    build.Add(this.StreetType.Edited);

                if (this.StreetTitle != null && this.StreetTitle.Visible)
                    build.Add(this.StreetTitle.Edited);

                if (this.Street != null)
                    build.Add(this.Street.Edited);

                if (this.Number != null && this.Number.Visible)
                    build.Add(this.Number.Edited);

                if (this.Complement != null && this.Complement.Visible)
                    build.Add(this.Complement.Edited);

                return string.Join(" ", build.ToArray());
            }
        }

        public new string EditedNormalize
        {
            get
            {
                var build = new HashSet<string>();

                if (this.StreetType != null && this.StreetType.Visible)
                    build.Add(this.StreetType.EditedNormalize);

                if (this.StreetTitle != null && this.StreetTitle.Visible)
                    build.Add(this.StreetTitle.EditedNormalize);

                if (this.Street != null && this.Street.Any())
                    build.Add(this.Street.EditedNormalize);

                if (this.Number != null && this.Number.Visible)
                    build.Add(this.Number.EditedNormalize);

                if (this.Complement != null && this.Complement.Visible)
                    build.Add(this.Complement.EditedNormalize);

                return string.Join(" ", build.ToArray());
            }
        }

        public new string Default
        {
            get
            {
                var build = new HashSet<string>();

                if (this.StreetType != null)
                    build.Add(this.StreetType.Edited);

                if (this.StreetTitle != null)
                    build.Add(this.StreetTitle.Edited);

                if (this.Street != null)
                    build.Add(this.Street.Edited);

                if (this.Number != null)
                    build.Add(this.Number.Edited);

                if (this.Complement != null)
                    build.Add(this.Complement.Edited);

                return string.Join(" ", build.ToArray());
            }
        }

        public new string DefaultNormalize
        {
            get
            {
                var build = new HashSet<string>();

                if (this.StreetType != null)
                    build.Add(this.StreetType.EditedNormalize);

                if (this.StreetTitle != null)
                    build.Add(this.StreetTitle.EditedNormalize);

                if (this.Street != null)
                    build.Add(this.Street.EditedNormalize);

                if (this.Number != null)
                    build.Add(this.Number.EditedNormalize);

                if (this.Complement != null)
                    build.Add(this.Complement.EditedNormalize);

                return string.Join(" ", build.ToArray());
            }
        }

        public override BaseCollection<NameParticle> Short(int maxLength)
        {
            var clone = new StreetCollection(
                this.StreetType != null ? this.StreetType.Default : null,
                this.StreetTitle != null ? this.StreetTitle.Default : null,
                this.Street != null ? this.Street.Default : null,
                this.Number != null ? this.Number.Default : null,
                this.Complement != null ? this.Complement.Default : null);

            if (clone.Edited.Length > maxLength)
            {
                var forDisabled = new HashSet<NameParticle>();
                var streetNewLength = default(int);
                var minStreetLength = 3;

                if (this.StreetTitle != null)
                    forDisabled.Add(clone.StreetTitle);
                if (this.StreetType != null)
                    forDisabled.Add(clone.StreetType);

                var enumerableDisabled = forDisabled.GetEnumerator();
                enumerableDisabled.MoveNext();

                do
                {
                    if (enumerableDisabled.Current != null)
                        enumerableDisabled.Current.Visible = false;

                    if (clone.Edited.Length > maxLength)
                    {
                        streetNewLength = clone.Street.Edited.Length - (clone.Edited.Length - maxLength);

                        if (streetNewLength >= minStreetLength)
                            clone.Street = (NameCollection)clone.Street.Short(streetNewLength);
                    }
                }
                while (clone.Edited.Length > maxLength && enumerableDisabled.MoveNext());

                if (clone.Edited.Length == 0 || clone.Edited.Length > maxLength)
                    return null;
            }

            return clone;
        }
    }
}
