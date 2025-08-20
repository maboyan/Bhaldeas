using Bhaldeas.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaldeas.Core.IO
{
    public interface IClassImporter
    {
        /// <summary>
        /// クラス情報を読み取りBhaldeasの形式に変換する
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Class>> ImportClassAsync();

        /// <summary>
        /// Bhaldeasが持っているクラス情報を出力する
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        Task ExportClassAsync(IEnumerable<Class> classes);
    }
}
