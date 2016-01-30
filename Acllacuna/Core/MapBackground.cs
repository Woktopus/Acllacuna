using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Acllacuna
{
    public class MapBackground
    {
        String path;
        Parser parse;
        String[,] map;
        List<Image> listImage;

        public MapBackground(String path)
        {
            this.path = path;
            parse = new Parser("Map/Map1Background.txt");
            listImage = new List<Image>();

        }

        public void LoadContent(ContentManager Content)
        {
            parse.LoadContent(Content);
            map = parse.backgroundMap();

            for (int i = 0; i <= map.GetLength(0) - 1; i++)
            {
                Image pic = new Image();
                pic.LoadContent(Content, map[i, 4], Color.White, new Vector2(int.Parse(map[i, 0]), int.Parse(map[i, 1])));
                pic.scale = new Vector2(float.Parse(map[i,2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(map[i, 3], CultureInfo.InvariantCulture.NumberFormat));
                listImage.Add(pic);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Image im in listImage)
            {
                im.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach (Image im in listImage)
            {
                im.Draw(sprite);
            }
        }
    }
}

