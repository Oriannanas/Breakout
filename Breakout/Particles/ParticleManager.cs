using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class ParticleManager
    {
        /*private enum ManagerType
        {
            Burst,
            Stream
        }*/
        private Random rng = new Random();
        private Rectangle rect;
        private int particleCount;
        private int enabledCount;
        private Texture2D texture;
        private Color color;

        private InGameScreen game;

        private Particle[] particles;

        public ParticleManager(InGameScreen game, Rectangle rect, int particleCount, Texture2D texture, Color color)
        {
            this.game = game;
            this.particleCount = particleCount;
            this.rect = rect;
            this.color = color;
            this.texture = texture;
            this.particles = new Particle[particleCount];


            for (int i = 0; i < particleCount; i++)
            { 
                particles[i] = new Particle(i, rng.Next(rect.Left+ rect.Width / 4, rect.Right-rect.Width/4), rng.Next(rect.Top+ rect.Height / 4, rect.Bottom- rect.Height / 4), (int)rng.Next(3, 7), this.texture, this.color);
                particles[i].Acceleration = new Vector2(0, 0.2f);
                particles[i].Velocity = new Vector2((((particles[i].Position.X- rect.Center.X) / (rect.Width / 2))* ((float)rng.NextDouble()+1))*3, ((particles[i].Position.Y - rect.Center.Y) /(rect.Height/2)) * ((float)rng.NextDouble() + 1) * 2);
                enabledCount += 1;
            }
        }
        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].isEnabled)
                {
                    particles[i].Update(gameTime);
                    if (particles[i].Position.Y > game.viewportHeight)
                    {
                        particles[i].isEnabled = false;
                        enabledCount -= 1;
                    }
                }
            }
            if (enabledCount == 0)
            {
                game.RemoveParticleManager(this);
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Particle particle in particles)
            {
                if (particle.isEnabled)
                {
                    particle.Draw(spriteBatch);
                }
            }
        }
    }
}
