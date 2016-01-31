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
    public class Map
    {
        int[,] map;

        public Dictionary<int, string> mondico { get; set; }

        List<Block> listBlock;
        List<Spike> listSpike;
        String pathMap;
		Parser pars;

		public float GetRealWidth()
		{
			return (float)(pars.width * 2);
		}

		public float GetRealHeight()
		{
			return (float)(pars.height * 2);
		}

        public Map(String path)
        {
            listBlock = new List<Block>();
            listSpike = new List<Spike>();
            mondico = new Dictionary<int, string>();
            mondico.Add(1, "Graphics/TileMap/cactus_bottom");
            mondico.Add(2, "Graphics/TileMap/stoneSol");
            mondico.Add(3, "Graphics/TileMap/spike");
            mondico.Add(4, "Graphics/TileMap/spike_bas");
            mondico.Add(5, "Graphics/TileMap/spike_droite");
            mondico.Add(6, "Graphics/TileMap/spike_gauche");
            mondico.Add(7, "Graphics/TileMap/cactus_top");
            mondico.Add(8, "Graphics/TileMap/torchStone");
            mondico.Add(9, "Graphics/TileMap/stonebrick");
            mondico.Add(10, "Graphics/TileMap/furnace");
            mondico.Add(11, "Graphics/TileMap/furnace2");
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
                            b.LoadContent(world, new Vector2(2, 2), new Vector2((i * 2) + 1, (j * 2) + 1), Content, mondico[Math.Abs(map[i, j])], false);
                            listBlock.Add(b);
                    } else if (map[i, j] > 0) {
                        if (map[i, j] >=3 && map[i, j]<=6)
                        {
                            Spike s = new Spike();
                            s.LoadContent(Content, world, new Vector2(1.50f, 1.50f), new Vector2((i * 2) + 1, (j * 2) + 1), mondico[Math.Abs(map[i, j])]);
                            listSpike.Add(s);
                        }
                        else
                        {
                            Block b = new Block();
                            b.LoadContent(world, new Vector2(2, 2), new Vector2((i * 2) + 1, (j * 2) + 1), Content, mondico[Math.Abs(map[i, j])], true);
                            listBlock.Add(b);
                        }
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

            foreach (Spike b in listSpike)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}
