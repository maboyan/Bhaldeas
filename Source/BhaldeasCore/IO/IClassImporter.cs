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
        Task<IEnumerable<Class>> ImportClassAsync();
        Task ExportClassAsync(IEnumerable<Class> classes);
    }
}
