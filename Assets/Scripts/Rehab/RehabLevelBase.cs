using UnityEngine;

namespace RehabVR
{
    public abstract class RehabLevelBase : MonoBehaviour
    {
        [Header("Level Info")]
        [SerializeField] private string levelName;
        [SerializeField] private float timeLimitSeconds = 60f;
        [SerializeField] private RehabHand hand = RehabHand.Right;

        public string LevelName => string.IsNullOrWhiteSpace(levelName) ? name : levelName;
        public float TimeLimitSeconds => timeLimitSeconds;
        public RehabHand Hand => hand;
        public bool IsRunning { get; private set; }
        public float ElapsedTime { get; private set; }

        protected XRHandInput Input { get; private set; }

        public virtual void Initialize(XRHandInput input)
        {
            Input = input;
        }

        public void StartLevel()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            ElapsedTime = 0f;
            OnLevelStarted();
        }

        public LevelResult Tick(float deltaTime)
        {
            if (!IsRunning)
            {
                return LevelResult.None;
            }

            ElapsedTime += deltaTime;
            var result = OnLevelTick(deltaTime);
            if (result != LevelResult.None)
            {
                IsRunning = false;
                OnLevelEnded(result);
            }
            else if (ElapsedTime >= timeLimitSeconds)
            {
                IsRunning = false;
                OnLevelEnded(LevelResult.Failed);
                return LevelResult.Failed;
            }

            return result;
        }

        protected virtual void OnLevelStarted() { }
        protected virtual LevelResult OnLevelTick(float deltaTime) => LevelResult.None;
        protected virtual void OnLevelEnded(LevelResult result) { }
    }

    public enum LevelResult
    {
        None,
        Success,
        Failed
    }
}
