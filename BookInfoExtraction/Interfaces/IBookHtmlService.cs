using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInfoExtraction.Interfaces
{
    internal interface IBookHtmlService
    {
        public void Decode(string dataSeed);
        public void Encode();
    }
}
