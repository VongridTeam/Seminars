using Godot;

namespace Vongrid.ShipShooter
{
    public class Player : Area2D
    {
        [Export]
        private float speed = 200f;
        [Export]
        private float fireRate = 1;

        [Export]
        private PackedScene bullet = null!;

        private Timer fireTimer = null!;

        private float playerHalfWidth;
        private float viewportWidth;

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
            fireTimer = GetNode<Timer>("FireTimer");
            fireTimer.WaitTime = 2 / fireRate;
            Connect("area_entered", this, nameof(Hit));

            Sprite sprite = GetNode<Sprite>("Sprite");
            playerHalfWidth = sprite.Texture.GetWidth() * sprite.Transform.Scale.x / 2;
            viewportWidth = GetViewport().Size.x;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("Fire") && fireTimer.TimeLeft == 0)
            {
                fireTimer.Start();
                Fire();
            }
        }

        public override void _Process(float delta)
        {
            if (sceneManager.GameEnded)
            {
                return;
            }

            MovePlayer(delta);
        }

        private void MovePlayer(float delta)
        {
            Translate(GetVelocity() * delta);
            float x = Mathf.Clamp(Position.x, 0 + playerHalfWidth, GetViewport().Size.x - playerHalfWidth);
            Position = new Vector2(x, Position.y);
        }

        private void Fire()
        {
            Node2D bulletInstance = (Node2D)bullet.Instance();
            bulletInstance.Position = Transform.origin;

            GetNode("/root").AddChild(bulletInstance);
        }

        private void Hit(Node body)
        {
            if (body.IsInGroup("EnemyProjectile"))
            {
                sceneManager.LoseGame();
                body.QueueFree();
                QueueFree();
            }
        }

        private Vector2 GetVelocity()
        {
            Vector2 velocity = Vector2.Zero;

            if (Input.IsActionPressed("MoveLeft"))
            {
                velocity.x -= speed;
            }
            if (Input.IsActionPressed("MoveRight"))
            {
                velocity.x += speed;
            }

            return velocity;
        }
    }
}
