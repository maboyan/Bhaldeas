using Bhaldeas.Core.Classes;
using Bhaldeas.Core.IO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core.Servants.DatabaseIO
{
    /// <summary>
    /// https://api.atlasacademy.io/docs
    /// からAPIを叩いて情報を取得するクラス
    /// </summary>
    public class AtlasAcademy
        : IClassImporter, IAttributeImporter
    {
        private static HttpClient client = new HttpClient();

        private static readonly string BASE_URL = @"https://api.atlasacademy.io/export";

        #region IClassImporter
        /// <summary>
        /// サーバンドのクラス情報を取得してクラスの相性リストと攻撃倍率情報を取得する
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Class>> ImportClassAsync()
        {
            // クラス相性を作成
            var result = await loadClassAffinitAsync();

            // クラス攻撃力補正を作成
            await loadClassAttackRateAsync(result);

            return result;
        }

        private async Task<IEnumerable<Class>> loadClassAffinitAsync()
        {
            var result = new List<Class>();

            /*
             * {
             *   "saber": {
             *     "saber": 1000,
             *     "archer": 500,
             *     ...
             *   },
             *   "archer": {
             *     "saber": 2000,
             *     "archer": 1000,
             *     ...
             *   },
             *   ...
             * }
             * 
             * のようなJSONを想定
             */

            var url = $"{BASE_URL}/JP/NiceClassRelation.json";
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            using var content = res.Content;
            using var stream = await content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);
            var root = document.RootElement;

            // 1. クラス相性を文字列ベースで作成
            foreach (var property in root.EnumerateObject())
            {
                var sourceName = property.Name;
                var sourceClass = new Class(sourceName);

                var affinity = property.Value;
                foreach(var target in  affinity.EnumerateObject())
                {
                    var targetName = target.Name;
                    var milliValue = target.Value.GetInt32();

                    switch(milliValue)
                    {
                        case 500:
                            sourceClass.Attack05NameList.Add(targetName);
                            break;
                        case 1000:
                            break;
                        case 1200:
                            sourceClass.Attack12NameList.Add(targetName);
                            break;
                        case 1500:
                            sourceClass.Attack15NameList.Add(targetName);
                            break;
                        case 2000:
                            sourceClass.Attack20NameList.Add(targetName);
                            break;

                        default:
                            throw new InvalidDataException($"{milliValue}は知らないクラス相性数値です");
                    }
                }

                var duplication = result.Any(a => a.Name == sourceName);
                if (duplication)
                    throw new InvalidDataException($"{sourceName}のクラスが複数存在します");
                
                result.Add(sourceClass);
            }

            // 2. 全てのクラスの相性表をインスタンスに変換
            foreach (var klass in result)
            {
                klass.UpdateAttackClass(result);
            }

            return result;
        }

        private async Task loadClassAttackRateAsync(IEnumerable<Class> classes)
        {
            /*
             * {
             *   "saber": 1000,
             *   "archer": 950,
             *   ...
             * }
             * 
             * のようなJSONを想定
             */

            var url = $"{BASE_URL}/JP/NiceClassAttackRate.json";
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            using var content = res.Content;
            using var stream = await content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);
            var root = document.RootElement;

            foreach (var property in root.EnumerateObject())
            {
                var name = property.Name;
                var value = property.Value.GetInt32();

                var klass = classes.FirstOrDefault(a => a.Name == name);

                // たまに意味のわからないクラスが入っているので無視
                if (klass == null)
                {
                    Console.WriteLine($"downloadClassAttackRateAsync: {name} class does not exist");
                    continue;
                }

                klass.AttackMilliRate = value;
            }
        }

        /// <summary>
        /// サーヴァントのクラス情報を出力することはできないので未実装
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task ExportClassAsync(IEnumerable<Class> classes)
        {
            throw new InvalidOperationException("AtlasAcademyでExportは実装できません");
        }
        #endregion

        #region IAttributeImporter
        public async Task<IEnumerable<Attribute>> ImportAttributeAsync()
        {
            var result = new List<Attribute>();

            /*
             * {
             *   "human": {
             *     "human": 1000,
             *     "sky": 1100,
             *     ...
             *   },
             *   "sky": {
             *     "human": 900,
             *     "sky": 1000,
             *     ...
             *   },
             *   ...
             * }
             * 
             * のようなJSONを想定
             */

            var url = $"{BASE_URL}/JP/NiceAttributeRelation.json";
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            using var content = res.Content;
            using var stream = await content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);
            var root = document.RootElement;

            // 1. 属性相性を文字列ベースで作成
            foreach (var property in root.EnumerateObject())
            {
                var sourceName = property.Name;
                var sourceAttr = new Attribute(sourceName);

                var affinity = property.Value;
                foreach (var target in affinity.EnumerateObject())
                {
                    var targetName = target.Name;
                    var milliValue = target.Value.GetInt32();

                    switch (milliValue)
                    {
                        case 900:
                            sourceAttr.Attack09NameList.Add(targetName);
                            break;
                        case 1000:
                            break;
                        case 1100:
                            sourceAttr.Attack11NameList.Add(targetName);
                            break;

                        default:
                            throw new InvalidDataException($"{milliValue}は知らない属性相性数値です");
                    }
                }

                var duplication = result.Any(a => a.Name == sourceName);
                if (duplication)
                    throw new InvalidDataException($"{sourceName}の属性が複数存在します");

                result.Add(sourceAttr);
            }

            // 2. 全ての属性相性表をインスタンスに変換
            foreach (var attr in result)
            {
                attr.UpdateAttackAttribute(result);
            }

            return result;
        }

        public Task ExportAttributeAsync(IEnumerable<Attribute> attributes)
        {
            throw new InvalidOperationException("AtlasAcademyでExportは実装できません");
        }
        #endregion
    }
}
