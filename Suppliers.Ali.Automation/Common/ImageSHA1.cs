using System.Collections.Generic;

namespace ISAA.Suppliers.Ali.Automation.Common
{
    public class ImageSHA1
    {
        public string NewUrl { get; set; }

        public List<string> OriginalUrl { get; set; } = new List<string>();

        public string SHA1 { get; set; }
    }
}