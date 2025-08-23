using Bhaldeas.Core.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaldeas.Core.IO
{
    public interface ITraitImporter
    {
        /// <summary>
        /// 特性情報を読み取りBhaldeasの形式に変換する
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Trait>> ImportTraitAsync();

        /// <summary>
        /// Bhaldeasが持っている特性情報を出力する
        /// </summary>
        /// <param name="traits"></param>
        /// <returns></returns>
        Task ExportTraitAsync(IEnumerable<Trait> traits);
    }
}
