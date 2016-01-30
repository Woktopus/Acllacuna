using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    class Parser
    {
        String path;
        List<string> list = new List<string>();
        char[] separator = { ';' };
        public int width { get; set; }
        public int height { get; set; }


        public Parser(string path)
        {
            
            this.path = path;
        }

        public void LoadContent(ContentManager Content)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory,path)))
            {
                try
                {
                    FileStream file = File.Open(path, FileMode.Open);
                    StreamReader fileReader = new StreamReader(file);
                    string line;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        list.Add(line); // Add to list.
                        //Console.WriteLine(line); // Write to console.
                    }
                }
                catch(Exception e)
                {

                }
            }
            else
            {
                Console.WriteLine("INTROUVEBLA");
            }
        }

        public int[,] tabMap()
        {
            
            String[] taille = list[0].Split(separator);
            height = int.Parse(taille[0]);
            width = int.Parse(taille[1]);
            int[,] map = new int[int.Parse(taille[1]), int.Parse(taille[0])];
            list.RemoveAt(0);
            int i = 0,j=0;
            foreach (String ligne in list)
            {
                i = 0;
                String[] tile = ligne.Split(separator);
                foreach(String a in tile)
                {
                    map[i, j] = int.Parse(a);
                        i++;
                }
                j++;
            }
            /*for (int a = 0; a <= 9; a++)
            {
                for (int b = 0; b <= 9; b++)
                {
                    Console.WriteLine(map[b, a]);
                }
            }*/

                    return map;
        }
    }
}
