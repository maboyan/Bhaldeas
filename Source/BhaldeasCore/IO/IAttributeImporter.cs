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
        Task<IEnumerable<Attribute>> ImportAttributeAsync();
        Task ExportAttributeAsync(IEnumerable<Attribute> attributes);
    }
}
