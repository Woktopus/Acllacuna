using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Acllacuna
{
    class SceneManager
    {
        #region Variables

        Scene currentScene;
        Scene newScene;

        /// <summary>
        /// Creating custom Content Manager
        /// </summary>
        ContentManager content;

        /// <summary>
        /// Screen Manager Instance
        /// </summary>
        private static SceneManager instance;

        /// <summary>
        /// Screen Stack
        /// </summary>
        /// 
        Stack<Scene> screenStack = new Stack<Scene>();

        /// <summary>
        /// Screen's width and height
        /// </summary>
        /// 
        Vector2 dimensions;

        bool transition;

        Transition fade = new Transition();

        Texture2D fadeTexture, nullImage;

        public GraphicsDevice graphicDevice;

        #endregion

        #region Properties

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();
                return instance;
            }
        }

        public ContentManager Content
        {
            get { return content; }
        }


        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }
        }
        #endregion

        #region Main Methods

        public void AddScene(Scene screen)
        {
            transition = true;
            newScene = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateValue = 1.0f;
        }

        public void AddScene(Scene screen, float alpha)
        {
            transition = true;
            newScene = screen;
            fade.IsActive = true;
            fade.ActivateValue = 1.0f;
            if (alpha != 1.0f)
                fade.Alpha = 1.0f - alpha;
            else
                fade.Alpha = alpha;

            fade.Increase = true;
        }

        public void Initialize()
        {
            currentScene = new MenuScene();
            fade = new Transition();
            //inputManager = new InputManager();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graph)
        {
            graphicDevice = graph;
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScene.LoadContent(content, graph);

            nullImage = this.content.Load<Texture2D>("Graphics/Transition/null");
            fadeTexture = this.content.Load<Texture2D>("Graphics/Transition/fade");
            fade.LoadContent(content, fadeTexture, Vector2.Zero);
            fade.Scale = Dimensions.X;
        }

        public void Update(GameTime gameTime, Game game)
        {
            if (!transition)
                currentScene.Update(gameTime,game);
            else
                Transition(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScene.Draw(spriteBatch);
            if (transition)
                fade.Draw(spriteBatch);
        }

        #endregion

        #region Private Methods

        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime);
            if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScene);
                currentScene.UnloadContent();
                currentScene = newScene;
                currentScene.LoadContent(content, graphicDevice);
            }
            else if (fade.Alpha == 0.0f)
            {
                transition = false;
                fade.IsActive = false;
            }
        }

        #endregion

    }
}
