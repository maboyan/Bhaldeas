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

namespace Bhaldeas.Core.DatabaseIO
{
    public class LocalFile
        : IClassImporter
    {
        private static readonly JsonSerializerOptions s_serializeOption = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };

        public string ClassFilePath { get; set; } = string.Empty;

        /// <summary>
        /// ローカルファイルからクラス情報を読み取る
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// ローカルファイルに暮らす情報を出力する
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        public async Task ExportClassAsync(IEnumerable<Class> classes)
        {
            using var stream = new FileStream(ClassFilePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, classes, s_serializeOption);
        }
    }
}
