using Bhaldeas.Core.Classes;
using Bhaldeas.Core.Traits;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core.Servants
{
    /// <summary>
    /// サーヴァントを表すクラス
    /// </summary>
    [DebuggerDisplay("{Id} {Name} {OriginalName} {ClassName}")]
    public class Servant
    {
        public static readonly int LEVEL_MAX = 120;

        [JsonInclude]
        public int Id { get; init; }
        
        [JsonInclude]
        public string Name { get; init; }
        
        [JsonInclude]
        public string Yomi { get; init; }
        
        [JsonInclude]
        public string OriginalName { get; init; }

        [JsonInclude]
        public string BattleName { get; init; }

        [JsonInclude]
        public int Rarity { get; init; }
        
        [JsonInclude]
        public int Cost { get; init; }

        [JsonInclude]
        public string ClassName { get; init; }
        [JsonIgnore]
        public Class Class { get; set; } = null;

        [JsonInclude]
        public string AttributeName { get; init; }
        [JsonIgnore]
        public Attribute Attribute { get; set; } = null;

        /// <summary>
        /// サーヴァント情報に記録されているIDと名前の組
        /// </summary>
        [JsonInclude]
        public KeyValuePair<int, string>[] Traits { get; init; }
        /// <summary>
        /// AtlasAcademyのTrait mappingに載っていたIDから割り出した特性
        /// </summary>
        [JsonIgnore]
        public Trait[] KnownTraits { get; set; } = null;
        /// <summary>
        /// AtlasAcademyのTrait mappingに載っていなかった特性
        /// </summary>
        [JsonIgnore]
        public KeyValuePair<int, string>[] UnknownTraits { get; set; } = null;

        [JsonInclude]
        public int[] Hp { get; init; } = new int[LEVEL_MAX];
        
        [JsonInclude]
        public int[] Attack { get; init; } = new int[LEVEL_MAX];

        /// <summary>
        /// ClassNameからクラスインスタンスをする
        /// </summary>
        /// <param name="allClasses"></param>
        public void UpdateClassReference(IEnumerable<Class> allClasses)
        {
            if (string.IsNullOrWhiteSpace(ClassName))
                return;

            Class = allClasses.FirstOrDefault(a => a.Name == ClassName);
        }

        /// <summary>
        /// AttributeNameから属性インスタンスを設定する
        /// </summary>
        /// <param name="allAttributes"></param>
        public void UpdateAttributeReference(IEnumerable<Attribute> allAttributes)
        {
            if (string.IsNullOrWhiteSpace(AttributeName))
                return;

            Attribute = allAttributes.FirstOrDefault(a => a.Name == AttributeName);
        }

        /// <summary>
        /// TraitIdsから特性インスタンスを設定する
        /// </summary>
        /// <param name="allTraits"></param>
        public void UpdateTraitsReference(IEnumerable<Trait> allTraits)
        {
            var result = new List<Trait>();
            var unknownList = new List<KeyValuePair<int, string>>();
            foreach(var trait in Traits)
            {
                var t = allTraits.FirstOrDefault(a => a.Id == trait.Key);
                if (t == null)
                {
                    Console.WriteLine($"UpdateTraitsReference: <{trait.Key}, {trait.Value}> unknown trait id");
                    unknownList.Add(trait);
                    continue;
                }

                result.Add(t);
            }

            KnownTraits = result.ToArray();
            UnknownTraits = unknownList.ToArray();
        }
    }
}
