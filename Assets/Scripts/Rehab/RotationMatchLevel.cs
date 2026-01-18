using UnityEngine;

namespace RehabVR
{
    public sealed class RotationMatchLevel : RehabLevelBase
    {
        [Header("Rotation Match")]
        [SerializeField] private float targetYawDegrees = 45f;
        [SerializeField] private float toleranceDegrees = 10f;
        [SerializeField] private float holdSeconds = 2f;

        private float _holdTimer;

        protected override void OnLevelStarted()
        {
            _holdTimer = 0f;
        }

        protected override LevelResult OnLevelTick(float deltaTime)
        {
            var rotation = Input.GetRotation(Hand);
            var yaw = rotation.eulerAngles.y;
            var delta = Mathf.DeltaAngle(yaw, targetYawDegrees);

            if (Mathf.Abs(delta) <= toleranceDegrees)
            {
                _holdTimer += deltaTime;
                if (_holdTimer >= holdSeconds)
                {
                    return LevelResult.Success;
                }
            }
            else
            {
                _holdTimer = 0f;
            }

            return LevelResult.None;
        }
    }
}
