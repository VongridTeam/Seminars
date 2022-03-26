using Godot;

namespace Vongrid.ShipShooter
{
    public class SceneManager : Node
    {
        [Export]
        private int numberOfEnemyColumns = 7;
        [Export]
        private int numberOfEnemyRows = 2;

        private int numberOfAliveEnemies;

        [Export]
        private NodePath playerSpawnPositionPath = null!;

        [Export]
        private PackedScene enemy = null!;

        [Export]
        private PackedScene player = null!;

        public bool GameEnded { get; private set; }

        public void UnitDied(Node node)
        {
            if (!node.IsInGroup("Enemy"))
            {
                return;
            }

            numberOfAliveEnemies--;
            if (numberOfAliveEnemies <= 0)
            {
                WinGame();
            }
        }

        public override void _Ready()
        {
            SpawnPlayer();
            SpawnEnemyWave();
        }

        public void LoseGame()
        {
            GetNode<Control>("/root/SceneRoot/FinalMessage/LossText").Visible = true;
            GameEnded = true;
        }

        private void WinGame()
        {
            GetNode<Control>("/root/SceneRoot/FinalMessage/WinText").Visible = true;
            GameEnded = true;
        }

        private void SpawnPlayer()
        {
            Node2D playerSpawnPosition = GetNode<Node2D>(playerSpawnPositionPath);
            Player playerInstance = (Player)player.Instance();

            playerInstance.Initialize(this);
            playerInstance.Position = playerSpawnPosition.Position;

            GetNode("/root").CallDeferred("add_child", playerInstance);
        }

        private void SpawnEnemyWave()
        {
            // Magic numbers bad (╯°□°）╯︵ ┻━┻
            Vector2 startPos = new Vector2(130, 120);
            Vector2 step = new Vector2(120, 70);
            const float ROW_OFFSET = 60;

            for (int rowIndex = 0; rowIndex < numberOfEnemyRows; rowIndex++)
            {
                float offset = ROW_OFFSET * (rowIndex % 2);
                for (int columnIndex = 0; columnIndex < numberOfEnemyColumns; columnIndex++)
                {
                    float x = startPos.x + (step.x * columnIndex) + offset;
                    float y = startPos.y + (step.y * rowIndex);
                    SpawnEnemy(x, y);
                }
            }
            numberOfAliveEnemies += numberOfEnemyColumns * numberOfEnemyRows;
        }

        private void SpawnEnemy(float x, float y)
        {
            Enemy enemyInstance = (Enemy)enemy.Instance();

            enemyInstance.Initialize(this);
            enemyInstance.Position = new Vector2(x, y);

            GetNode("/root").CallDeferred("add_child", enemyInstance);
        }
    }
}
