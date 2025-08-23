using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bhaldeas.Core.Traits
{
    [DebuggerDisplay("{Id} {Name}")]
    public class Trait
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonInclude]
        public int Id { get; init; }

        /// <summary>
        /// 英語属性名
        /// </summary>
        [JsonInclude]
        public string Name { get; init; }
    }
}
