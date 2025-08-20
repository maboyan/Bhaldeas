using Bhaldeas.Core.Classes;
using Bhaldeas.Core.IO;
using Bhaldeas.Core.Servants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core.DatabaseIO
{
    public class LocalFile
        : IClassImporter, IAttributeImporter, IServantImporter
    {
        private static readonly JsonSerializerOptions s_serializeOption = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };


        #region IClassImporter
        /// <summary>
        /// クラス情報ImportExport用ファイルパス
        /// </summary>
        public string ClassFilePath { get; set; } = string.Empty;
        
        public async Task<IEnumerable<Class>> ImportClassAsync()
        {
            using var stream = new FileStream(ClassFilePath, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.DeserializeAsync<List<Class>>(stream, s_serializeOption);
            
            // この時点では相性リストは名前しか入っていないのでクラスへの参照を追加する
            foreach(var klass in result)
            {
                klass.UpdateAttackClass(result);
            }
            return result;
        }

        public async Task ExportClassAsync(IEnumerable<Class> classes)
        {
            using var stream = new FileStream(ClassFilePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, classes, s_serializeOption);
        }
        #endregion // IClassImporter

        #region IAttributeImporter
        /// <summary>
        /// 属性情報ImportExport用ファイルパス
        /// </summary>
        public string AttributeFilePath { get; set; } = string.Empty;

        public async Task<IEnumerable<Attribute>> ImportAttributeAsync()
        {
            using var stream = new FileStream(AttributeFilePath, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.DeserializeAsync<List<Attribute>>(stream, s_serializeOption);

            // この時点では相性リストは名前しか入っていないのでクラスへの参照を追加する
            foreach (var attr in result)
            {
                attr.UpdateAttackAttribute(result);
            }
            return result;
        }

        public async Task ExportAttributeAsync(IEnumerable<Attribute> attributes)
        {
            using var stream = new FileStream(AttributeFilePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, attributes, s_serializeOption);
        }
        #endregion // IAttributeImporter

        #region IServantImporter
        /// <summary>
        /// サーヴァント情報ImportExport用ファイルパス
        /// </summary>
        public string ServantFilePath { get; set; } = string.Empty;

        public async Task<IEnumerable<Servant>> ImportServantAsync(IEnumerable<Class> allClasses, IEnumerable<Attribute> allAttributes)
        {
            using var stream = new FileStream(ServantFilePath, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.DeserializeAsync<List<Servant>>(stream, s_serializeOption);

            result.ForEach(a => a.UpdateClassReference(allClasses));
            result.ForEach(a => a.UpdateAttributeReference(allAttributes));

            return result;
        }

        public async Task ExportServantAsync(IEnumerable<Servant> servants)
        {
            using var stream = new FileStream(ServantFilePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, servants, s_serializeOption);
        }
        #endregion
    }
}
