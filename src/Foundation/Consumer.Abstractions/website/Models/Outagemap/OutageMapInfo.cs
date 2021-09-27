using System;

namespace SYMB2C.Foundation.Consumer.Abstractions.Models.Outagemap
{
    public class OutageMapInfo
    {
        public OutageMapInfo()
        {
            this.MapList = new OutageMapItem[] { };
        }

        public float FileSize { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public string UpdateDateTimeFormatted { get; set; }

        public OutageMapItem[] MapList { get; set; }
    }
}
