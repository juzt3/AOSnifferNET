using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSnifferNET
{
    public class evMobChangeState
    {
        public int mobID;
        public short enchantment;

        public evMobChangeState(int mobID, short enchantment)
        {
            this.mobID = mobID;
            this.enchantment = enchantment;
        }
    }
}
