using Godot;

namespace Vongrid.ShipShooter
{
    public class Bullet : Area2D
    {
        [Export]
        private float speed = 200f;

        [Export]
        private NodePath visibilityNotifierPath = null!;
        private VisibilityNotifier2D visibilityNotifier = null!;

        private Vector2 velocity;

        public override void _Ready()
        {
            velocity = new Vector2(0, -speed);
            visibilityNotifier = GetNode<VisibilityNotifier2D>(visibilityNotifierPath);
        }

        public override void _Process(float delta)
        {
            if (!visibilityNotifier.IsOnScreen())
            {
                QueueFree();
                return;
            }

            Translate(velocity * delta);
        }
    }
}
