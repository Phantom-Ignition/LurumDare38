﻿using LudumDare38.Managers;
using LudumDare38.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace LudumDare38.Objects.Guns
{
    public enum GunType
    {
        Basic,
        Shield,
        LaserGun
    }

    abstract class GameGunBase
    {
        protected GunType _gunType;
        public GunType GunType => _gunType;

        protected int _orbitLevel;
        protected float _angle;
        protected CharacterSprite _sprite;
        public CharacterSprite Sprite => _sprite;
        
        //--------------------------------------------------
        // Cooldown

        protected float _cooldown;
        protected float _currentCooldown;
        public float CurrentCooldown => _currentCooldown;

        //--------------------------------------------------
        // Static

        public bool Static { get; set; }

        //----------------------//------------------------//

        public GameGunBase(int orbitLevel, GunType gunType, float angle)
        {
            _orbitLevel = orbitLevel;
            _gunType = gunType;
            _angle = angle;
            _currentCooldown = 0;
            CreateSprite();
        }

        protected abstract void CreateSprite();
        public virtual bool Shot(out GameProjectile projectile)
        {
            projectile = null;
            _currentCooldown = _cooldown;
            return false;
        }

        public virtual void Update(GameTime gameTime, float rotation, float floating)
        {
            var floatingMultiplier = (_orbitLevel) * 1.1f;
            var center = SceneManager.Instance.VirtualSize / 2;
            rotation = (3 - _orbitLevel) * rotation;
            rotation += _angle;
            var orbitDistance = 17 + _orbitLevel * 30;
            var position = center + new Vector2(orbitDistance * (float)Math.Cos(rotation), orbitDistance * (float)Math.Sin(rotation)) +
                floating * floatingMultiplier * Vector2.UnitY;

            _sprite.Rotation = rotation + (float)Math.PI / 2;
            _sprite.Position = position;
            _sprite.Update(gameTime);

            if (_currentCooldown > 0)
            {
                _currentCooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_currentCooldown < 0)
                    _currentCooldown = 0.0f;
            }
        }

        public virtual void PreDraw(SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            spriteBatch.Begin(transformMatrix: viewportAdapter.GetScaleMatrix(), samplerState: SamplerState.PointClamp);
        }

        public virtual void Draw(SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            PreDraw(spriteBatch, viewportAdapter);
            _sprite.Draw(spriteBatch, _sprite.Position);
            spriteBatch.End();
        }
    }
}
