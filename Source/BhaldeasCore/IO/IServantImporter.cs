using Bhaldeas.Core.Servants;
using Bhaldeas.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = Bhaldeas.Core.Attributes.Attribute;

namespace Bhaldeas.Core.IO
{
    public interface IServantImporter
    {
        /// <summary>
        /// サーバンドの情報を読み取りBhaldeasの形式に変換する
        /// </summary>
        /// <param name="allClasses"></param>
        /// <param name="allAttributes"></param>
        /// <returns></returns>
        Task<IEnumerable<Servant>> ImportServantAsync(IEnumerable<Class> allClasses, IEnumerable<Attribute> allAttributes);

        /// <summary>
        /// Bhaldeasが持っているサーヴァント情報を出力する
        /// </summary>
        /// <param name="servants"></param>
        /// <returns></returns>
        Task ExportServantAsync(IEnumerable<Servant> servants);


    }
}
