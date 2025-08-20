using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bhaldeas.Core.Classes
{
    /// <summary>
    /// サーヴァントのクラスを表すクラス
    /// </summary>
    [DebuggerDisplay("{Name} {DisplayClass} {AttackMilliRate}")]
    public class Class
    {
        /// <summary>
        /// サーヴァント名
        /// これがユニークなIDも兼務している
        /// </summary>
        [JsonInclude]
        public string Name { get; init; }

        /// <summary>
        /// ゲーム中どうやって表示されるか？を示したクラス
        /// Bhaldeas側で考えているので間違っているかもしれない
        /// </summary>
        [JsonInclude]
        public ClassType DisplayClass { get; private set; } = ClassType.Unknown;

        /// <summary>
        /// クラスに与えられている攻撃力補正値
        /// </summary>
        [JsonInclude]
        public int AttackMilliRate { get; set; } = 1000;

        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が0.5倍になるクラスの名前リスト
        /// JSONに保存されるのはこちらなのでJSONを読み込んだときはAttack05Listを作り直す必要がある
        /// </summary>
        [JsonInclude]
        public List<string> Attack05NameList { get; init; } = [];
        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が0.5倍になるクラスリスト
        /// JSONには保存されない（循環参照などの関係で処理が面倒）
        /// </summary>
        [JsonIgnore]
        public List<Class> Attack05List { get; } = [];

        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.2倍になるクラスの名前リスト
        /// JSONに保存されるのはこちらなのでJSONを読み込んだときはAttack12Listを作り直す必要がある
        /// </summary>
        [JsonInclude]
        public List<string> Attack12NameList { get; init; } = [];
        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.2倍になるクラスリスト
        /// JSONには保存されない（循環参照などの関係で処理が面倒）
        /// </summary>
        [JsonIgnore]
        public List<Class> Attack12List { get; } = [];

        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.5倍になるクラスの名前リスト
        /// JSONに保存されるのはこちらなのでJSONを読み込んだときはAttack15Listを作り直す必要がある
        /// </summary>
        [JsonInclude]
        public List<string> Attack15NameList { get; init; } = [];
        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が1.5倍になるクラスリスト
        /// JSONには保存されない（循環参照などの関係で処理が面倒）
        /// </summary>
        [JsonIgnore]
        public List<Class> Attack15List { get; } = [];

        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が2.0倍になるクラスの名前リスト
        /// JSONに保存されるのはこちらなのでJSONを読み込んだときはAttack20Listを作り直す必要がある
        /// </summary>
        [JsonInclude]
        public List<string> Attack20NameList { get; init;  } = [];
        /// <summary>
        /// 自分が攻撃したときに攻撃倍率が2.0倍になるクラスリスト
        /// JSONには保存されない（循環参照などの関係で処理が面倒）
        /// </summary>
        [JsonIgnore]
        public List<Class> Attack20List { get; } = [];

        public Class(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            setDisplayClass(name);
        }

        /// <summary>
        /// 雰囲気でクラスの振り分けをする
        /// </summary>
        /// <param name="name"></param>
        private void setDisplayClass(string name)
        {
            var lower = name.ToLower();
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

        /// <summary>
        /// インポートで取得した際は名前リストしかない状態のものを更新してクラスリストにする
        /// </summary>
        /// <param name="allClasses">クラス情報が全て入ったリスト</param>
        public void UpdateAttackClass(IEnumerable<Class> allClasses)
        {
            updateAttackClassUnit(allClasses, Attack05NameList, Attack05List);
            updateAttackClassUnit(allClasses, Attack12NameList, Attack12List);
            updateAttackClassUnit(allClasses, Attack15NameList, Attack15List);
            updateAttackClassUnit(allClasses, Attack20NameList, Attack20List);
        }

        private void updateAttackClassUnit(IEnumerable<Class> allClasses, List<string> names, List<Class> target)
        {
            target.Clear();

            foreach (string name in names)
            {
                var klass = allClasses.FirstOrDefault(c => c.Name == name);
                if (klass == null)
                {
                    Console.WriteLine($"updateAttackClassUnit: {name} unknown class");
                    continue;
                }

                target.Add(klass);
            }
        }
    }
}
