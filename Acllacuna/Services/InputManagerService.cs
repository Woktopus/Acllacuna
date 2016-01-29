using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Acllacuna
{
    public class InputManagerService : GameComponent
    {
        
        #region gamePad
        
        public struct gamePad
        {
            GamePadState[] currentState;    //Contains current gamepad states 
            GamePadState[] prevState;       //Contains Previous gamepad states 
            bool[] isActive;                //Stores what controllers should be active 
            bool[] isConnected;             //Stores what controllers are active 

            public void Initialize()
            {
                currentState = new GamePadState[4];
                prevState = new GamePadState[4];
                isActive = new bool[4];
                isConnected = new bool[4];

                for (int i = 0; i < 4; i++)
                {
                    isActive[i] = false;
                    isConnected[i] = false;
                }
            }

            public GamePadState GetState(PlayerIndex index)
            {
                return currentState[(int)index];
            }

            public GamePadState GetPrevState(PlayerIndex index)
            {
                return prevState[(int)index];
            }

            public void Update()
            {
                int i;
                for (i = 0; i < 4; i++)         //Update Controller States 
                {
                    prevState[i] = currentState[i];
                    currentState[i] = Microsoft.Xna.Framework.Input.GamePad.GetState((PlayerIndex)i);
                }
            }

        }

        #endregion

        #region keyboard

        public struct keyboard
        {
            KeyboardState currentState; //Contains current keyboard state 
            KeyboardState prevState;    //Contains previous keyboard state 

            public void Initialize()
            {
                currentState = new KeyboardState();
            }

            public KeyboardState GetState()
            {
                return currentState;
            }

            public KeyboardState GetPrevState()
            {
                return prevState;
            }

            public void Update()
            {
                prevState = currentState;
                currentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }
        }

        #endregion

        #region mouse

        public struct mouse
        {

            MouseState currentState;   //Contains current mouse state 
            MouseState prevState;      //Contains previous mouse state 

            public void Initialize()
            {
                currentState = new MouseState();
            }

            public MouseState GetState()
            {
                return currentState;
            }

            public MouseState GetPrevState()
            {
                return prevState;
            }

            public void Update()
            {
                prevState = currentState;
                currentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            } 
        }

        #endregion

        #region InputManager
         Game game;                      //Reference to game 
 
        public gamePad GamePad;
        public keyboard Keyboard; 
        public mouse Mouse; 
        #endregion 

        #region Methods 
        public InputManagerService(Game game) //Constructor 
            : base(game) 
        { 
            GamePad = new gamePad(); 
            Keyboard = new keyboard(); 
            Mouse = new mouse();
            ServiceHelper.Add<InputManagerService>(this);
            this.game = game; 
        } 
 
        public override void Initialize() 
        { 
            GamePad.Initialize(); 
            Keyboard.Initialize(); 
            Mouse.Initialize(); 
            base.Initialize(); 
        } 
 
        public override void Update(GameTime gameTime) 
        { 
            GamePad.Update(); 
            Keyboard.Update(); 
            Mouse.Update(); 
            base.Update(gameTime); 
        } 
        #endregion


    }
}
