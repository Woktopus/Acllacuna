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
    public class DynamicMap
    {
        String path;
        Parser parse;
        public Dictionary<int, string> mondico { get; set; }
        int[,] map;
        List<MovingPlatforme> listMov;
        public List<Enemy> listEnnemy;
        World world;
        public Boss boss;


        public DynamicMap(String path)
        {
            this.path = path;
            mondico = new Dictionary<int, string>();
            mondico.Add(1, "Graphics/TileMap/cactus_bottom");
            mondico.Add(2, "Graphics/TileMap/stonebrick");
            parse = new Parser("Map/Map1Dyn.txt");
            listMov = new List<MovingPlatforme>();
            listEnnemy = new List<Enemy>();
            boss = new Boss();

        }

        public void LoadContent(ContentManager Content, World world,PhysicsScene phy)
        {
            this.world = world;
            parse.LoadContent(Content);
            map = parse.dynMap();

            for (int i = 0; i <= map.GetLength(0) - 1; i++)
            {
                if (map[i,0] == 1) //PLATEFORME
                {
                    MovingPlatforme mov = new MovingPlatforme();
                    mov.LoadContent(world, new Vector2(map[i,3], map[i, 4]), new Vector2(map[i, 1], map[i, 2]),Content,mondico[map[i,8]],(PlatformeDirection)map[i,5], map[i, 6], map[i,7]);
                    listMov.Add(mov);
                }else if (map[i, 0] == 2){ //ENEMIE
                    Enemy en = new Enemy();
                    en.LoadContent(world, Content, new Vector2(map[i, 1], map[i, 2]),phy);
                    listEnnemy.Add(en);
                }
            else if (map[i, 0] == 3)
            { //BOSS
              boss.LoadContent(Content, world, new Vector2(map[i, 1], map[i, 2]), new Vector2(map[i, 3], map[i, 4]));
            }

        }
        }

        public void Update(GameTime gameTime)
        {
            foreach (MovingPlatforme mov in listMov)
            {
                mov.Update(gameTime);
            }
            foreach (Enemy en in listEnnemy)
            {
                en.Update(gameTime,world);
            }
            boss.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatforme mov in listMov)
            {
                mov.Draw(spriteBatch);
            }

            foreach (Enemy en in listEnnemy)
            {
                en.Draw(spriteBatch);
            }
            boss.Draw(spriteBatch);
        }
    }
}
