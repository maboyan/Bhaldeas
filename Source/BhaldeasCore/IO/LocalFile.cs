using Bhaldeas.Core.Classes;
using Bhaldeas.Core.IO;
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
        : IClassImporter, IAttributeImporter
    {
        private static readonly JsonSerializerOptions s_serializeOption = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };

        public string FilePath { get; set; } = string.Empty;

        #region IClassImporter
        /// <summary>
        /// ローカルファイルからクラス情報を読み取る
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Class>> ImportClassAsync()
        {
            using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.DeserializeAsync<List<Class>>(stream, s_serializeOption);
            
            // この時点では相性リストは名前しか入っていないのでクラスへの参照を追加する
            foreach(var klass in result)
            {
                klass.UpdateAttackClass(result);
            }
            return result;
        }

        /// <summary>
        /// ローカルファイルにクラス情報を出力する
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public async Task ExportClassAsync(IEnumerable<Class> classes)
        {
            using var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, classes, s_serializeOption);
        }
        #endregion // IClassImporter

        #region IAttributeImporter
        /// <summary>
        /// ローカルファイルから属性情報を読み取る
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Attribute>> ImportAttributeAsync()
        {
            using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var result = await JsonSerializer.DeserializeAsync<List<Attribute>>(stream, s_serializeOption);

            // この時点では相性リストは名前しか入っていないのでクラスへの参照を追加する
            foreach (var attr in result)
            {
                attr.UpdateAttackAttribute(result);
            }
            return result;
        }

        /// <summary>
        /// ローカルファイルに属性情報を出力する
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public async Task ExportAttributeAsync(IEnumerable<Attribute> attributes)
        {
            using var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, attributes, s_serializeOption);
        }
        #endregion // IAttributeImporter
    }
}
