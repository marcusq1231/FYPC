using UnityEngine;

namespace RehabVR
{
    public sealed class GripHoldLevel : RehabLevelBase
    {
        [Header("Grip Hold")]
        [SerializeField] private float holdSeconds = 3f;
        [SerializeField] private int requiredReps = 5;
        [SerializeField] private float gripThreshold = 0.7f;

        private float _holdTimer;
        private int _reps;

        protected override void OnLevelStarted()
        {
            _holdTimer = 0f;
            _reps = 0;
        }

        protected override LevelResult OnLevelTick(float deltaTime)
        {
            var grip = Input.GetGrip(Hand);
            if (grip >= gripThreshold)
            {
                _holdTimer += deltaTime;
                if (_holdTimer >= holdSeconds)
                {
                    _reps++;
                    _holdTimer = 0f;
                }
            }
            else
            {
                _holdTimer = 0f;
            }

            if (_reps >= requiredReps)
            {
                return LevelResult.Success;
            }

            return LevelResult.None;
        }
    }
}
