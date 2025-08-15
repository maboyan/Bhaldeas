using Bhaldeas.Core.Classes;

namespace Bhaldeas.Core.Servants
{
    public class Servant
    {
        public static readonly int LEVEL_MAX = 120;

        public int Id { get; set; }
        public string FullName { get; set; }
        public string FullYomi { get; set; }
        public string Name { get; set; }

        public int Rarity { get; set; }
        public int Cost { get; set; }

        public Class Class { get; set; } = null;

        public int[] Hp { get; set; } = new int[LEVEL_MAX];
        public int[] Attack { get; set; } = new int[LEVEL_MAX];

    }
}
