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
        Attribute,
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

                case ImportMode.Attribute:
                    return await importAttributeAsync(path);

                case ImportMode.Servant:
                    return await importServantsAsync(path);
            }

            throw new NotImplementedException(Mode.ToString());
        }

        private async Task<int> importClassAsync(string path)
        {
            var db = new Database();
            try
            {
                var aa = new AtlasAcademyLocal()
                {
                    ClassAffinityFilePath = @"cache\NiceClassRelation.json",
                    ClassAttackRateFilePath = @"cache\NiceClassAttackRate.json",
                };
                await db.ImportClassesAsync(aa);
                
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

        private async Task<int> importAttributeAsync(string path)
        {
            var db = new Database();
            try
            {
                var aa = new AtlasAcademyLocal()
                {
                    AttributeFilePath = @"cache\NiceAttributeRelation.json",
                };
                await db.ImportAttributesAsync(aa);

                // ImportしたものをJSONで保存
                var local = new LocalFile()
                {
                    AttributeFilePath = path
                };
                await local.ExportAttributeAsync(db.Attributes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }

            return 0;
        }

        private async Task<int> importServantsAsync(string path)
        {
            var db = new Database();
            try
            {
                var aa = new AtlasAcademyLocal()
                {
                    ClassAffinityFilePath = @"cache\NiceClassRelation.json",
                    ClassAttackRateFilePath = @"cache\NiceClassAttackRate.json",
                    AttributeFilePath = @"cache\NiceAttributeRelation.json",
                    ServantFilePath = @"cache\nice_servant.json",
                };
                await db.ImportClassesAsync(aa);
                await db.ImportAttributesAsync(aa);
                await db.ImportServantsAsync(aa);

                // ImportしたものをJSONで保存
                var local = new LocalFile()
                {
                    ServantFilePath = path
                };
                await local.ExportServantAsync(db.Servants);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 1;
            }

            return 0;
        }
    }
}
