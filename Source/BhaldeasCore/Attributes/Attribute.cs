using Bhaldeas.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bhaldeas.Core.Attributes
{
    public class Attribute
    {
        /// <summary>
        /// 属性名
        /// これがユニークなIDも兼務している
        /// </summary>
        [JsonInclude]
        public string Name { get; init; }

        /// <summary>
        /// 属性名をenumにしたもの
        /// </summary>
        [JsonInclude]
        public AttributeType DisplayAttribute { get; private set; } = AttributeType.Human;

        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が0.9倍になる属性の名前リスト
        /// JSONに保存されるのはこちらなのでJSONを読み込んだときはAttack09Listを作り直す必要がある
        /// </summary>
        [JsonInclude]
        public List<string> Attack09NameList { get; init; } = new List<string>();
        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.1倍になるクラスリスト
        /// JSONには保存されない（循環参照などの関係で処理が面倒）
        /// </summary>
        [JsonIgnore]
        public List<Attribute> Attack09List { get; } = new List<Attribute>();

        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.1倍になる属性の名前リスト
        /// JSONに保存されるのはこちらなのでJSONを読み込んだときはAttack11Listを作り直す必要がある
        /// </summary>
        [JsonInclude]
        public List<string> Attack11NameList { get; init; } = new List<string>();
        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.1倍になるクラスリスト
        /// JSONには保存されない（循環参照などの関係で処理が面倒）
        /// </summary>
        [JsonIgnore]
        public List<Attribute> Attack11List { get; } = new List<Attribute>();

        public Attribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            setDisplayAttribute(name);
        }

        private void setDisplayAttribute(string name)
        {
            var lower = Name.ToLower();
            // 雰囲気で振り分ける
            if (lower.Contains("saber"))
                DisplayClass = ClassType.Saber;
            else if (lower.Contains("archer"))
                DisplayClass = ClassType.Archer;
            else if (lower.Contains("lancer"))
                DisplayClass = ClassType.Lancer;
            else if (lower.Contains("rider"))
                DisplayClass = ClassType.Rider;
            else if (lower.Contains("caster"))
                DisplayClass = ClassType.Caster;
            else if (lower.Contains("assassin"))
                DisplayClass = ClassType.Assassin;
            else if (lower.Contains("berserker"))
                DisplayClass = ClassType.Berserker;
            else if (lower.Contains("shielder"))
                DisplayClass = ClassType.Shielder;
            else if (lower.Contains("ruler"))
                DisplayClass = ClassType.Ruler;
            else if (lower.Contains("alterego"))
                DisplayClass = ClassType.Alteriego;
            else if (lower.Contains("avenger"))
                DisplayClass = ClassType.Avenger;
            else if (lower.Contains("mooncancer"))
                DisplayClass = ClassType.MoonCancer;
            else if (lower.Contains("foreigner"))
                DisplayClass = ClassType.Foreigner;
            else if (lower.Contains("pretender"))
                DisplayClass = ClassType.Pretender;
            else if (lower.Contains("beast"))
                DisplayClass = ClassType.Beast;
            else if (lower.Contains("demongodpillar"))
                DisplayClass = ClassType.Beast;
            else if (lower.Contains("uolgamarie"))
                DisplayClass = ClassType.Beast;
            else
            {
                Console.WriteLine($"setDisplayClass: {Name} unknown class name");
                DisplayClass = ClassType.Unknown;
            }
        }
    }
}
