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
    public class ProjectileFactory
    {
        public PhysicsScene scene { get; set; }
        public ContentManager Content { get; set; }

        public ProjectileFactory()
        {
        }

        public void LoadContent(PhysicsScene scene , ContentManager Content)
        {
            this.scene = scene;
            this.Content = Content;
        }

        public void LaunchProjectile(DirectionEnum direction, Vector2 size, Vector2 origine, string texturePath, float speed)
        {
            Projectile projectile = new Projectile();
            projectile.LoadContent(scene.world, size, origine, Content, texturePath, direction, speed);
            scene.projectiles.Add(projectile);
        }

        
    }
}
