using CommandLine;
using System.Threading.Tasks;

namespace BhaldeasCUI
{
    internal class Program
    {
        static int Main(string[] args)
        {
            var ret = Parser.Default.ParseArguments<Args>(args);
            switch(ret.Tag)
            {
                case ParserResultType.Parsed:
                    break;
                case ParserResultType.NotParsed:
                default:
                    return 1;
            }

            // 引数解析成功したっぽい
            
            var parsed = ret as Parsed<Args>;
            var parsedArgs = parsed.Value;
            var result = main(parsedArgs).Result;
            return result;
        }

        static async Task<int> main(Args args)
        {
            switch(args.Mode)
            {
                case Mode.Import:
                    var importer = new Importer(args.ImportMode);
                    var path = args.OutputPath;
                    if (string.IsNullOrWhiteSpace(path))
                        throw new ArgumentException("-oオプションが指定されていません");
                    return await importer.ImportAsync(path);

                default:
                    throw new NotImplementedException(args.Mode.ToString());
            }
        }

        #region CommandLine
        enum Mode
        {
            Import,
        }

        /// <summary>
        /// コマンドライン引数
        /// </summary>
        class Args
        {
            // モード
            [Option('m', "mode", Required = true, HelpText = "モード")]
            public Mode Mode { get; set; }

            [Option('d', "import", Required = false, HelpText = "インポート用モードを指定します")]
            public ImportMode ImportMode { get; set; } = ImportMode.None;

            [Option('o', "output", Required = false, HelpText = "インポート時保存ファイルパス")]
            public string OutputPath { get; set; } = string.Empty;


            // その他
            [Value(1, MetaName = "Others")]
            public IEnumerable<string> Others { get; set; }
        }
        #endregion
    }
}
