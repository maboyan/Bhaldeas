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
    /// から取得したJSONのキャッシュファイルを扱うクラス
    /// </summary>
    public class AtlasAcademyLocal
        : AtlasAcademyBase
    {
        #region IClassImporter
        public string ClassAffinityFilePath { get; set; }
        public string ClassAttackRateFilePath { get; set; }

        public override async Task<IEnumerable<Class>> ImportClassAsync()
        {
            // クラス相性を作成
            using var affinityStream = new FileStream(ClassAffinityFilePath, FileMode.Open);
            var result = await ReadClassAffinityAsync(affinityStream);

            // クラス攻撃力補正を作成
            using var attackRateStream = new FileStream(ClassAttackRateFilePath, FileMode.Open);
            await ReadClassAttackRateAsync(attackRateStream, result);

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
        public string AttributeFilePath { get; set; }

        public override async Task<IEnumerable<Attribute>> ImportAttributeAsync()
        {
            using var stream = new FileStream(AttributeFilePath, FileMode.Open);
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
        public string ServantFilePath { get; set; }
        
        public override async Task<IEnumerable<Servant>> ImportServantAsync(IEnumerable<Class> allClasses, IEnumerable<Attribute> allAttributes)
        {
            using var stream = new FileStream(ServantFilePath, FileMode.Open);
            var result = await ReadServantAsync(stream, allClasses, allAttributes);
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
    }
}
