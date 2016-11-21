using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifySharp
{
    /// <summary>
    /// A class for filtering lists and counts. Note for ShopifySharp contributors: this class will 
    /// be obsolete in v2.0 in favor of the <see cref="ShopifyListFilter"/> or 
    /// <see cref="ShopifyCountFilter"/>. Please use those instead. 
    /// </summary>
    public class ShopifyFilterOptions: Parameterizable
    {
        /// <summary>
        /// Limit the amount of results. Default is 50, max is 250.
        /// </summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Page of results to be returned. Default is 1.
        /// </summary>
        [JsonProperty("page")]
        public int? Page { get; set; }

        /// <summary>
        /// An optional, comma-separated list of fields to include in the response.
        /// </summary>
        [JsonProperty("fields")]
        public string Fields { get; set; }
    }
}
