using Bhaldeas.Core.Classes;
using Bhaldeas.Core.IO;
using Bhaldeas.Core.Servants;
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
        public List<Servant> Servants { get; set; } = new List<Servant>();
        public List<Class> Classes { get; set; } = new List<Class>();
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();

        #region クラス
        /// <summary>
        /// クラス情報を取得
        /// </summary>
        /// <param name="importer">AtlasAcademyだとネットから情報を持ってくるしLocalFileだとローカルのJSONから情報を取得する</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task ImportClasses(IClassImporter importer)
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
        public async Task ExportClasses(IClassImporter exporter)
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
        public async Task ImportAttributes(IAttributeImporter importer)
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
        public async Task ExportAttributes(IAttributeImporter exporter)
        {
            if (exporter == null)
                throw new ArgumentNullException(nameof(exporter));

            await exporter.ExportAttributeAsync(Attributes);
        }
        #endregion
    }
}
