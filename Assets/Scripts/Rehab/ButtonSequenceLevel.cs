using UnityEngine;

namespace RehabVR
{
    public sealed class ButtonSequenceLevel : RehabLevelBase
    {
        [Header("Button Sequence")]
        [SerializeField] private int requiredSequences = 6;
        [SerializeField] private float triggerThreshold = 0.75f;
        [SerializeField] private float gripThreshold = 0.7f;

        private int _sequenceIndex;
        private int _completed;
        private bool _triggerLatched;
        private bool _gripLatched;

        protected override void OnLevelStarted()
        {
            _sequenceIndex = 0;
            _completed = 0;
            _triggerLatched = false;
            _gripLatched = false;
        }

        protected override LevelResult OnLevelTick(float deltaTime)
        {
            var trigger = Input.GetTrigger(Hand) >= triggerThreshold;
            var grip = Input.GetGrip(Hand) >= gripThreshold;

            if (_sequenceIndex == 0)
            {
                if (trigger && !_triggerLatched)
                {
                    _sequenceIndex = 1;
                }
            }
            else
            {
                if (grip && !_gripLatched)
                {
                    _sequenceIndex = 0;
                    _completed++;
                }
            }

            _triggerLatched = trigger;
            _gripLatched = grip;

            if (_completed >= requiredSequences)
            {
                return LevelResult.Success;
            }

            return LevelResult.None;
        }
    }
}
