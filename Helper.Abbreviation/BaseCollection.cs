using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Helper.Abbreviation
{
    public abstract class BaseCollection<TTextParticle> : HashSet<TTextParticle>
         where TTextParticle : TextParticle
    {
        public static readonly char[] Vowel;

        public static readonly string[] ConnectivesNames;

        public static readonly Dictionary<string, string> SuffixNames;

        static BaseCollection()
        {
            Vowel = new char[] { 'A', 'E', 'I', 'O', 'U' };

            ConnectivesNames = new string[] 
            {
                "COM","DA","DAS","DE","DEL","DES","DI","DIS","DO","DOS","DU","DUS","PARA"
            };

            SuffixNames = new Dictionary<string, string>();

            SuffixNames.Add("FILHO", "FL");
            SuffixNames.Add("SOBRINHO", "SOB");
            SuffixNames.Add("JUNIOR", "JR");
            SuffixNames.Add("NETO", "NT");
        }

        public BaseCollection()
            : base()
        {
        }

        public BaseCollection(IEnumerable<TTextParticle> collection)
            : base(collection)
        {
        }

        public abstract BaseCollection<TTextParticle> Short(int maxLength);

        public string Edited
        {
            get
            {
                return string.Join(" ", this.Where(i => i.Visible).Select(i => i.Edited));
            }
        }

        public string EditedNormalize
        {
            get
            {
                return string.Join(" ", this.Where(i => i.Visible).Select(i => i.EditedNormalize));
            }
        }

        public string Default
        {
            get
            {
                return string.Join(" ", this.Select(i => i.Default));
            }
        }

        public string DefaultNormalize
        {
            get
            {
                return string.Join(" ", this.Select(i => i.DefaultNormalize));
            }
        }

        protected string Cutting(string text, int? cuttingLength)
        {
            if (cuttingLength != null)
                return text.Length > cuttingLength.Value ? text.Substring(0, cuttingLength.Value) : text;
            else
            {
                var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(text);
                var textBase = Encoding.UTF8.GetString(bytes).ToUpper();
                var isBeforeVowel = false;
                var count = 0;

                for (var index = 0; index < text.Length; index++)
                {
                    if (Vowel.Contains(textBase[index]))
                        count++;

                    if (count >= 2 && !isBeforeVowel)
                        return text.Substring(0, index);

                    isBeforeVowel = Vowel.Contains(textBase[index]);
                }
            }

            return text;
        }
    }
}
