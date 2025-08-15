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
            // 振り分ける
            switch (lower)
            {
                case "sky":
                    DisplayAttribute = AttributeType.Sky;
                    break;
                case "earth":
                    DisplayAttribute = AttributeType.Earth;
                    break;
                case "human":
                    DisplayAttribute = AttributeType.Human;
                    break;

                case "star":
                    DisplayAttribute = AttributeType.Star;
                    break;
                case "beast":
                    DisplayAttribute = AttributeType.Beast;
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"知らない属性({name})です");

            }
        }

        /// <summary>
        /// インポートで取得した際は名前リストしかない状態のものを更新して属性リストにする
        /// </summary>
        /// <param name="allAttributes">属性情報が全て入ったリスト</param>
        public void UpdateAttackAttribute(IEnumerable<Attribute> allAttributes)
        {
            updateAttackAttributeUnit(allAttributes, Attack09NameList, Attack09List);
            updateAttackAttributeUnit(allAttributes, Attack11NameList, Attack11List);
        }

        private void updateAttackAttributeUnit(IEnumerable<Attribute> allAttributes, List<string> names, List<Attribute> target)
        {
            target.Clear();

            foreach (string name in names)
            {
                var attr = allAttributes.FirstOrDefault(c => c.Name == name);
                if (attr == null)
                    throw new InvalidOperationException($"updateAttackAttributeUnit: {name}は知らない属性です");

                target.Add(attr);
            }
        }
    }
}
