using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace MapDisplay
{
    class Test
    {
        static void Main(string[] args)
        {
            string saveLocation = @"C:\Users\KlauseKarlson\Documents\MapMakerSave\ExampleMap.map";
            Map m;
            using(FileStream f= new FileStream(saveLocation, FileMode.Open))
            {
                using(ZipArchive save =new ZipArchive(f,ZipArchiveMode.Read))
                {
                    m = Map.loadMap(save);
                }
            }
            int width = m.getWidth();
            int height = m.getHeight();
            Console.WriteLine(width+ ","+ height);
            for (int row=0;row<height;row++)
            {
                
                for (int col=0;col<width;col++)
                {
                    Console.Write("[" + m.getTile(col, row, 0).getName() + "]");
                }
                Console.WriteLine("");
            }
            Console.ReadLine();
        }//end main
    }
}//end namespace
