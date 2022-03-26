using Godot;

namespace Vongrid.ShipShooter
{
    public class Enemy : Area2D
    {
        [Export]
        private float speed = 200f;
        [Export]
        private float maxfireRate = 5;
        [Export]
        private float minfireRate = 1;

        [Export]
        private float maxMoveDistance = 4;

        [Export]
        private PackedScene bullet = null!;

        [Export]
        private NodePath timerPath = null!;
        private Timer fireTimer = null!;

        private float timeAlive;

        private RandomNumberGenerator rng = new RandomNumberGenerator();

        public SceneManager sceneManager = null!;

        public void Initialize(SceneManager sceneManager)
        {
            if (this.sceneManager != null)
            {
                GD.PushWarning($"Scene manager already initialized on object {this}");
                return;
            }
            this.sceneManager = sceneManager;
        }

        public override void _Ready()
        {
            fireTimer = GetNode<Timer>(timerPath);
            rng.Randomize();
            // Give the player between 2 and 3 seconds to spawn so the bullets don't instantly kill them
            fireTimer.Start(rng.RandfRange(2, 3));

            Connect("area_entered", this, nameof(Hit));
        }

        public override void _Process(float delta)
        {
            if (sceneManager.GameEnded)
            {
                SetProcess(false);
                return;
            }

            if (fireTimer.TimeLeft == 0)
            {
                fireTimer.WaitTime = 2 / rng.RandfRange(minfireRate, maxfireRate);
                fireTimer.Start();
                Fire();
            }

            Translate(GetVelocity() * delta);

            timeAlive += delta;
        }

        private void Hit(Node body)
        {
            if (body.IsInGroup("PlayerProjectile"))
            {
                sceneManager.UnitDied(this);
                body.QueueFree();
                QueueFree();
            }
        }

        private void Fire()
        {
            Node2D bulletInstance = (Node2D)bullet.Instance();
            bulletInstance.Position = Transform.origin;

            GetNode("/root").AddChild(bulletInstance);
        }

        private Vector2 GetVelocity()
        {
            Vector2 velocity = Vector2.Zero;

            velocity.x = (PingPong(timeAlive, maxMoveDistance) - (maxMoveDistance / 2)) * speed;

            return velocity;
        }

        private float PingPong(float selfIncrementingValue, float maxValue)
        {
            float currentValue = selfIncrementingValue % (maxValue * 2);

            if (currentValue >= 0 && currentValue < maxValue)
            {
                return currentValue;
            }
            else
            {
                return (maxValue * 2) - currentValue;
            }
        }
    }
}
