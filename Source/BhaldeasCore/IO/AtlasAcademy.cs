using Bhaldeas.Core.Classes;
using Bhaldeas.Core.IO;
using Bhaldeas.Core.Traits;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core.Servants.DatabaseIO
{
    /// <summary>
    /// https://api.atlasacademy.io/docs
    /// からAPIを叩いて情報を取得するクラス
    /// </summary>
    public class AtlasAcademy
        : AtlasAcademyBase
    {
        private static HttpClient client = new HttpClient();

        private static readonly string BASE_URL = @"https://api.atlasacademy.io/export";

        #region IClassImporter
        public override async Task<IEnumerable<Class>> ImportClassAsync()
        {
            // クラス相性を作成
            IEnumerable<Class> result = null;
            {
                var url = $"{BASE_URL}/JP/NiceClassRelation.json";
                var res = await client.GetAsync(url);
                res.EnsureSuccessStatusCode();

                using var content = res.Content;
                using var stream = await content.ReadAsStreamAsync();
                result = await ReadClassAffinityAsync(stream);
            }

            // クラス攻撃力補正を作成
            {
                var url = $"{BASE_URL}/JP/NiceClassAttackRate.json";
                var res = await client.GetAsync(url);
                res.EnsureSuccessStatusCode();

                using var content = res.Content;
                using var stream = await content.ReadAsStreamAsync();
                await ReadClassAttackRateAsync(stream, result);
            }

            return result;
        }

        /// <summary>
        /// サーバーに対してエクスポートは出来ないため未実装
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public override Task ExportClassAsync(IEnumerable<Class> classes)
        {
            throw new InvalidOperationException("AtlasAcademyでExportは実装できません");
        }
        #endregion

        #region IAttributeImporter
        public override async Task<IEnumerable<Attribute>> ImportAttributeAsync()
        {

            var url = $"{BASE_URL}/JP/NiceAttributeRelation.json";
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            using var content = res.Content;
            using var stream = await content.ReadAsStreamAsync();
            var result = await ReadAttributeAsync(stream);

            return result;
        }

        /// <summary>
        /// サーバーに対してエクスポートは出来ないため未実装
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public override Task ExportAttributeAsync(IEnumerable<Attribute> attributes)
        {
            throw new InvalidOperationException("AtlasAcademyでExportは実装できません");
        }

        #endregion

        #region IServantImporter
        public override async Task<IEnumerable<Servant>> ImportServantAsync(IEnumerable<Class> allClasses, IEnumerable<Attribute> allAttributes, IEnumerable<Trait> allTraits)
        {
            var url = $"{BASE_URL}/JP/nice_servant.json";
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            using var content = res.Content;
            using var stream = await content.ReadAsStreamAsync();
            var result = await ReadServantAsync(stream, allClasses, allAttributes, allTraits);

            return result;
        }

        /// <summary>
        /// サーバーに対してエクスポートは出来ないため未実装
        /// </summary>
        /// <param name="servants"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task ExportServantAsync(IEnumerable<Servant> servants)
        {
            throw new NotImplementedException();
        }
        #endregion // IServantImporter

        #region ITraitImporter
        public override async Task<IEnumerable<Trait>> ImportTraitAsync()
        {
            var url = $"{BASE_URL}/JP/nice_trait.json";
            var res = await client.GetAsync(url);
            res.EnsureSuccessStatusCode();

            using var content = res.Content;
            using var stream = await content.ReadAsStreamAsync();
            var result = await ReadTraitAsync(stream);

            return result;
        }

        /// <summary>
        /// サーバーに対してエクスポートは出来ないため未実装
        /// </summary>
        /// <param name="traits"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task ExportTraitAsync(IEnumerable<Trait> traits)
        {
            throw new NotImplementedException();
        }
        #endregion // ITraitImporter
    }
}
