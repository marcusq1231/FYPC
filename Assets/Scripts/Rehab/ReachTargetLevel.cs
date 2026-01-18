using System.Collections.Generic;
using UnityEngine;

namespace RehabVR
{
    public sealed class ReachTargetLevel : RehabLevelBase
    {
        [Header("Reach Targets")]
        [SerializeField] private float targetRadius = 0.1f;
        [SerializeField] private float staySeconds = 0.5f;
        [SerializeField] private List<Transform> targets = new List<Transform>();

        private int _currentTargetIndex;
        private float _stayTimer;

        protected override void OnLevelStarted()
        {
            _currentTargetIndex = 0;
            _stayTimer = 0f;
        }

        protected override LevelResult OnLevelTick(float deltaTime)
        {
            if (targets.Count == 0)
            {
                return LevelResult.Failed;
            }

            var handPosition = Input.GetPosition(Hand);
            var target = targets[_currentTargetIndex];
            if (target == null)
            {
                return LevelResult.Failed;
            }

            var distance = Vector3.Distance(handPosition, target.position);
            if (distance <= targetRadius)
            {
                _stayTimer += deltaTime;
                if (_stayTimer >= staySeconds)
                {
                    _currentTargetIndex++;
                    _stayTimer = 0f;
                    if (_currentTargetIndex >= targets.Count)
                    {
                        return LevelResult.Success;
                    }
                }
            }
            else
            {
                _stayTimer = 0f;
            }

            return LevelResult.None;
        }

        public void SetTargets(List<Transform> newTargets)
        {
            targets = newTargets;
        }
    }
}
