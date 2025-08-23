using Bhaldeas.Core.Classes;
using Bhaldeas.Core.Servants;
using Bhaldeas.Core.Traits;
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
        /// <param name="allTraits"></param>
        /// <returns></returns>
        Task<IEnumerable<Servant>> ImportServantAsync(IEnumerable<Class> allClasses, IEnumerable<Attribute> allAttributes, IEnumerable<Trait> allTraits);

        /// <summary>
        /// Bhaldeasが持っているサーヴァント情報を出力する
        /// </summary>
        /// <param name="servants"></param>
        /// <returns></returns>
        Task ExportServantAsync(IEnumerable<Servant> servants);


    }
}
