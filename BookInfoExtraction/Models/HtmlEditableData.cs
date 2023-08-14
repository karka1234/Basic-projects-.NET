using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInfoExtraction.Models
{
    internal class HtmlEditableData
    {
        public string OrginalDataLine { get; set; }               
        public string ModifiedDataLine { get; set; }               
        public string Content { get; set; }
        public string AtributeName { get; set; }
        public string AtributeContent { get; set; }
        public HtmlEditableData(string fullDataLineOrginal)
        {
            OrginalDataLine = fullDataLineOrginal;
        }
        public HtmlEditableData(string fullDataLineOrginal, string content) : this(fullDataLineOrginal) 
        { 
            Content = content;
        }
        public HtmlEditableData(string fullDataLineOrginal, string atributeName, string atributeContent) : this(fullDataLineOrginal)
        {
            AtributeName = atributeName;
            AtributeContent = atributeContent;
        }
        public void ChangeObject(string content)
        {
            ModifiedDataLine = OrginalDataLine.Replace(AtributeContent, content);
            AtributeContent = content;
        }
        public void PrintAtributesInfo()
        {
            Console.WriteLine($"{AtributeName,-30} : {AtributeContent}");
        }
    }
}
