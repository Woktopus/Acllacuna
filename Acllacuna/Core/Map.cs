using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    class Map
    {
        int[,] map;

        public Dictionary<int, string> mondico { get; set; }

        List<Block> listBlock;
        String pathMap;
        Parser pars;

        public Map(String path)
        {
            listBlock = new List<Block>();
            mondico = new Dictionary<int, string>();
            mondico.Add(1, "Graphics/cube1");
            mondico.Add(2, "Graphics/cube2");
            pathMap = path;
            pars = new Parser("Map/Map1.txt");
            
        }

        public void LoadContent(ContentManager Content,World world)
        {
            pars.LoadContent(Content);
            map = pars.tabMap();
            for (int i = 0; i <= map.GetLength(0)-1; i++)
            {
                for (int j = 0; j <= map.GetLength(1)-1; j++)
                {
                    if (map[i,j] < 0)
                    {
                        Block b = new Block();

                        b.LoadContent(world, new Vector2(2, 2), new Vector2((i * 2) + 1, (j * 2) + 1), Content,mondico[Math.Abs(map[i,j])], false);
                        listBlock.Add(b);
                    } else if (map[i, j] > 0) { 
                        Block b = new Block();
                        b.LoadContent(world, new Vector2(2, 2), new Vector2((i * 2)+1, (j * 2) + 1), Content, mondico[Math.Abs(map[i, j])], true);
                        listBlock.Add(b);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Block b in listBlock)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}
