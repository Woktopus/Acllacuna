using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace Acllacuna
{
    public class SoundService : GameComponent
    {
        #region SoundStruct
        public struct SoundStruct
        {
            public string name { get; set; }
            public SoundEffect soundEffect { get; set; }
            public SoundEffectInstance soundEffectInstance { get; set; }
        }
        #endregion


        #region Fields

        Game game;

        public Dictionary<string,SoundStruct> Sounds { get; set; }

        #endregion

        #region SoundService
        public SoundService(Game game)
            : base(game)
        {
            ServiceHelper.Add<SoundService>(this);
            this.game = game;
            Sounds = new Dictionary<string, SoundStruct>();
        }

        public void AddSound(string assetName, string assetPath)
        {
            SoundStruct sound = new SoundStruct();
            sound.name = assetName;
            sound.soundEffect = game.Content.Load<SoundEffect>(assetPath);
            sound.soundEffectInstance = sound.soundEffect.CreateInstance();
            Sounds.Add(assetName, sound);
        }

        public void PlaySound(string assetName)
        {
            if (Sounds.Any(s => s.Key == assetName))
            {
                Sounds[assetName].soundEffect.Play();
            }
        }

        /// <summary>
        /// Décharge le contenu de la bibliothéque des sons 
        /// </summary>
        public void UnloadSounds()
        {
            Sounds.Clear();
        }

           

        #endregion
    }
}
