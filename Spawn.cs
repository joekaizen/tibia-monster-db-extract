using System;
using System.Collections.Generic;
using System.Text;

namespace monster_db_extract
{
    class Spawn
    {
        // Sector File, Amount, Radius and Regen.
        public string SectorFile { get; set; }
        public int Amount { get; set; }
        public int Radius { get; set; }
        public int Regen { get; set; }

        // Monster Spawn - Center
        public int StartingPointX { get; set; }
        public int StartingPointY { get; set; }
        public int StartingPointZ { get; set; }

        // Monster Spawn - Offset
        public int OffsetPointX { get; set; }
        public int OffsetPointY { get; set; }

        // Monster
        public int MonsterId { get; set; }
        public string MonsterName { get; set; }
    }
}
