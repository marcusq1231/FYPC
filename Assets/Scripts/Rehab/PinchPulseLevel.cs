using UnityEngine;

namespace RehabVR
{
    public sealed class PinchPulseLevel : RehabLevelBase
    {
        [Header("Pinch Pulse")]
        [SerializeField] private float targetBpm = 40f;
        [SerializeField] private int requiredHits = 12;
        [SerializeField] private float hitWindowSeconds = 0.35f;

        private float _nextBeatTime;
        private int _hits;
        private bool _wasPressed;

        protected override void OnLevelStarted()
        {
            _hits = 0;
            _nextBeatTime = Time.time + (60f / Mathf.Max(1f, targetBpm));
            _wasPressed = false;
        }

        protected override LevelResult OnLevelTick(float deltaTime)
        {
            var trigger = Input.GetTrigger(Hand);
            var pressed = trigger > 0.75f;

            if (pressed && !_wasPressed)
            {
                var delta = Mathf.Abs(Time.time - _nextBeatTime);
                if (delta <= hitWindowSeconds)
                {
                    _hits++;
                    _nextBeatTime = Time.time + (60f / Mathf.Max(1f, targetBpm));
                }
            }

            _wasPressed = pressed;

            if (_hits >= requiredHits)
            {
                return LevelResult.Success;
            }

            return LevelResult.None;
        }
    }
}
