using Bhaldeas.Core.Classes;
using Bhaldeas.Core.IO;
using Bhaldeas.Core.Servants;
using Bhaldeas.Core.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core
{
    /// <summary>
    /// 最終的にこれへの参照を持っておけば色々と情報が得られるクラス
    /// </summary>
    public class Database
    {
        public List<Servant> Servants { get; set; } = [];
        public List<Class> Classes { get; set; } = [];
        public List<Attribute> Attributes { get; set; } = [];
        public List<Trait> Traits { get; set; } = [];

        #region クラス
        /// <summary>
        /// クラス情報を取得
        /// </summary>
        /// <param name="importer">AtlasAcademyだとネットから情報を持ってくるしLocalFileだとローカルのJSONから情報を取得する</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ImportClassesAsync(IClassImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException(nameof(importer));

            var classes = await importer.ImportClassAsync();
            Classes.Clear();
            Classes.AddRange(classes);
        }

        /// <summary>
        /// クラス情報を出力
        /// </summary>
        /// <param name="exporter">実質LocalFileしか指定されないはず</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ExportClassesAsync(IClassImporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException(nameof(exporter));

            await exporter.ExportClassAsync(Classes);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 属性情報を取得
        /// </summary>
        /// <param name="importer">AtlasAcademyだとネットから情報を持ってくるしLocalFileだとローカルのJSONから情報を取得する</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ImportAttributesAsync(IAttributeImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException(nameof(importer));

            var attributes = await importer.ImportAttributeAsync();
            Attributes.Clear();
            Attributes.AddRange(attributes);
        }

        /// <summary>
        /// 属性情報を出力
        /// </summary>
        /// <param name="exporter">実質LocalFileしか指定されないはず</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ExportAttributesAsync(IAttributeImporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException(nameof(exporter));

            await exporter.ExportAttributeAsync(Attributes);
        }
        #endregion

        #region 特性
        /// <summary>
        /// 特性情報を取得
        /// </summary>
        /// <param name="importer">AtlasAcademyだとネットから情報を持ってくるしLocalFileだとローカルのJSONから情報を取得する</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ImportTraitAsync(ITraitImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException(nameof(importer));

            var traits = await importer.ImportTraitAsync();
            Traits.Clear();
            Traits.AddRange(traits);
        }

        /// <summary>
        /// 特性情報を出力
        /// </summary>
        /// <param name="exporter">実質LocalFileしか指定されないはず</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ExportTraitAsync(ITraitImporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException(nameof(exporter));

            await exporter.ExportTraitAsync(Traits);
        }
        #endregion

        #region サーヴァント
        /// <summary>
        /// サーヴァント情報を取得
        /// </summary>
        /// <param name="importer">AtlasAcademyだとネットから情報を持ってくるしLocalFileだとローカルのJSONから情報を取得する</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ImportServantsAsync(IServantImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException(nameof(importer));

            var servants = await importer.ImportServantAsync(Classes, Attributes, Traits);
            Servants.Clear();
            Servants.AddRange(servants);
        }

        /// <summary>
        /// サーヴァント情報を出力
        /// </summary>
        /// <param name="exporter">実質LocalFileしか指定されないはず</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ExportServantsAsync(IServantImporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException(nameof(exporter));

            await exporter.ExportServantAsync(Servants);
        }
        #endregion
    }
}
