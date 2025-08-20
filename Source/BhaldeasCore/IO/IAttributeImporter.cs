using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core.IO
{
    public interface IAttributeImporter
    {
        /// <summary>
        /// 属性情報を読み取りBhaldeasの形式に変換する
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Attribute>> ImportAttributeAsync();

        /// <summary>
        /// Bhaldeasが持っている属性情報を出力する
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        Task ExportAttributeAsync(IEnumerable<Attribute> attributes);
    }
}
