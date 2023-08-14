using BookInfoExtraction.Interfaces;
using BookInfoExtraction.Models;
using System.Net;
using System.Text;
using System.Xml;

namespace BookInfoExtraction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string htmlFilePath = "D:\\Downloads\\A Fate Inked in Blood_ Book One of the Saga of the Unfated by Danielle L. Jensen, Hardcover _ Barnes & Noble®.html";
            BookService htmlFile = new BookService();
            htmlFile.Decode(htmlFilePath);
            ConsoleKeyInfo readKey;
            do
            {
                Console.WriteLine("Available atributes to modify : \t\n");
                Console.WriteLine($"{"NAME",-30} : CONTENT");
                Console.WriteLine("----------------------------------------------------------");
                htmlFile.htmlData.ForEach(x => x.PrintAtributesInfo());
                Console.WriteLine();
                Console.WriteLine("Enter atribute name you want to change ");
                string inputAtribute = Console.ReadLine().ToLower().Trim();
                Console.Clear();
                htmlFile.htmlData.Where(x => x.AtributeName != null && x.AtributeName.ToLower().Trim() == inputAtribute).First().PrintAtributesInfo();

                Console.WriteLine("Enter new content text");
                string inputAtributeContent = Console.ReadLine();

                htmlFile.htmlData.Where(x => x.AtributeName != null && x.AtributeName.ToLower().Trim() == inputAtribute).First().ChangeObject(inputAtributeContent);

                Console.WriteLine();
                Console.WriteLine("Press ESC to write changes");
                Console.WriteLine("Press ENTER to continue");
                readKey = Console.ReadKey();
            } while (readKey.Key != ConsoleKey.Escape);
            htmlFile.Encode();
        }
    }
}