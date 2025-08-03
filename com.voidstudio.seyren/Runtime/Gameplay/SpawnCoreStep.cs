using System;
using System.Collections.Generic;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Gameplay
{
    /// <summary>
    /// Core step that spawns enemies into the game context.
    /// </summary>
    public class SpawnCoreStep : ICoreStep
    {
        private readonly int _spawnCount;
        private bool _hasSpawned = false;
        private List<IUnit> _enemyPool = new List<IUnit>();
        private List<Vector3> _enemyPositions = new List<Vector3>();
        private int _batchSize;

        public SpawnCoreStep(List<IUnit> enemyPool, List<Vector3> enemyPositions, int spawnBatchSize = 5)
        {
            _enemyPool = enemyPool;
            _enemyPositions = enemyPositions;
            _batchSize = spawnBatchSize;
        }

        public void Execute(GameContext ctx)
        {
            if (_hasSpawned) return;

            int count = Math.Min(_spawnCount + _batchSize, _enemyPool.Count);
            for (int i = _spawnCount; i < count; i++)
            {
                IUnit enemy = _enemyPool[i];
                enemy.Move(_enemyPositions[i]);
                ctx.Enemies.Add(enemy);
            }
        }

        public bool IsComplete(GameContext ctx)
        {
            return _spawnCount == _enemyPool.Count;
        }

        public bool CanStart(GameContext ctx)
        {
            // can start when game is started for 5 seconds
            return ctx.Time.CurrentTime >= 5f;
        }
    }
}
