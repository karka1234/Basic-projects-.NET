using BookInfoExtraction.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BookInfoExtraction.Models
{
    internal class BookService : IBookHtmlService
    {
        private string metaTag = "<meta";
        private string startTitleTag = "<title>";
        private string endTitleTag = "</title>";
        public List<HtmlEditableData> htmlData = new List<HtmlEditableData>();//sukurt klase reikttu su atitinkamais kintamaisiasi
        private string DataSeed = string.Empty;
        private string DataOut = string.Empty;

        public void Decode(string dataSeed)//ienumerable galetu but
        {
            try
            {
                DataSeed = dataSeed;
                using (StreamReader sr = new StreamReader(DataSeed))
                {                    
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if(line.Contains(startTitleTag))
                            htmlData.Add(new HtmlEditableData(line, line.Replace(startTitleTag, "").Replace(endTitleTag, "")  ));
                        if (line.Contains(metaTag))
                        {                
                            FindMetaAtributesNameAndContentAndAdd(line);
                        }                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void FindMetaAtributesNameAndContentAndAdd(string line)
        {
            Regex metaTag = new Regex(@"\s+(?:name|property)=""([^""]+)""\s+content=""([^""]+)""\s*\/?>");
            foreach (Match match in metaTag.Matches(line))
            {
                htmlData.Add(new HtmlEditableData(line, match.Groups[1].Value, match.Groups[2].Value));
            }
        }

        public void Encode()
        {
            try
            {
                DataOut = DataSeed.Replace(".html", "NEW.html");
                using (StreamWriter sw = new StreamWriter(DataOut))
                using (StreamReader sr = new StreamReader(DataSeed))                
                {
                    string line;
                    while (((line = sr.ReadLine()) != null) )
                    {
                        string newLine = string.Empty;
                        foreach (var item in htmlData.Where(x => x.ModifiedDataLine != null))
                        {
                            if (item.OrginalDataLine == line)
                            { 
                                newLine = item.ModifiedDataLine;
                            }
                        }
                        if(newLine != string.Empty)
                            sw.WriteLine(newLine);
                        else sw.WriteLine(line);                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
