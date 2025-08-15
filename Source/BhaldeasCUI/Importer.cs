using Bhaldeas.Core;
using Bhaldeas.Core.DatabaseIO;
using Bhaldeas.Core.Servants.DatabaseIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhaldeasCUI
{
    internal enum ImportMode
    {
        None,

        Class,
        Servant,
    }

    /// <summary>
    /// Importモード時のクラス
    /// </summary>
    internal class Importer
    {
        public ImportMode Mode { get; } = ImportMode.None;

        public Importer(ImportMode mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// ネットから情報を取得するクラス
        /// </summary>
        /// <param name="path">取得したファイルを保存する先(JSON)</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> ImportAsync(string path)
        {
            switch(Mode)
            {
                case ImportMode.None:
                    return 0;

                case ImportMode.Class:
                    return await importClassAsync(path);

                case ImportMode.Servant:
                    throw new NotImplementedException();
            }

            throw new NotImplementedException(Mode.ToString());
        }

        private async Task<int> importClassAsync(string path)
        {
            var db = new Database();
            try
            {
                var aa = new AtlasAcademy();
                await db.ImportClasses(aa);
                
                // ImportしたものをJSONで保存
                var local = new LocalFile()
                {
                    ClassFilePath = path
                };
                await local.ExportClassAsync(db.Classes);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }

            return 0;

        }
    }
}
