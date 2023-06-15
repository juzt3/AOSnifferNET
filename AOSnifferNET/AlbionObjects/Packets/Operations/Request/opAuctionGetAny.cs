using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSnifferNET
{
    // {"0":8,"1":"accessories","2":"bag","3":"5","4":0,"5":"8","6":'query text', "7":0,"8":"3","9":50,"11":0,"12":true,"253":76} (id, category, subcategory, quality, unknown, tier, unknown, enhancement...
    internal class opAuctionGetAny
    {
        public int id;
        public string queryText;
        public string category;
        public string subcategory;
        public int quality;
        public int tier;
        public int enchantment;

        public opAuctionGetAny(int id,string queryText, string category, string subcategory, int quality, int tier, int enchantment)
        {
            this.id = id;
            this.queryText = queryText;
            this.category = category;
            this.subcategory = subcategory;
            this.quality = quality;
            this.tier = tier;
            this.enchantment = enchantment;
        }
    }
}
