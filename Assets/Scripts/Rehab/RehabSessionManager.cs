using System.Collections.Generic;
using UnityEngine;

namespace RehabVR
{
    public sealed class RehabSessionManager : MonoBehaviour
    {
        [SerializeField] private List<RehabLevelBase> levels = new List<RehabLevelBase>();
        [SerializeField] private bool autoStart = true;

        private readonly XRHandInput _input = new XRHandInput();
        private int _currentLevelIndex;
        private RehabLevelBase _currentLevel;
        private TrainingDataLogger _logger;

        public IReadOnlyList<RehabLevelBase> Levels => levels;

        private void Awake()
        {
            _logger = GetComponent<TrainingDataLogger>();
            if (_logger == null)
            {
                _logger = gameObject.AddComponent<TrainingDataLogger>();
            }

            foreach (var level in levels)
            {
                level.Initialize(_input);
            }
        }

        private void Start()
        {
            if (autoStart)
            {
                StartSession();
            }
        }

        public void StartSession()
        {
            if (levels.Count == 0)
            {
                Debug.LogWarning("RehabSessionManager has no levels assigned.");
                return;
            }

            _currentLevelIndex = 0;
            StartLevel(_currentLevelIndex);
        }

        private void Update()
        {
            if (_currentLevel == null)
            {
                return;
            }

            var result = _currentLevel.Tick(Time.deltaTime);
            if (result != LevelResult.None)
            {
                _logger.LogLevelResult(_currentLevel, result);
                AdvanceLevel();
            }
        }

        private void StartLevel(int index)
        {
            if (index < 0 || index >= levels.Count)
            {
                _currentLevel = null;
                _logger.CompleteSession();
                return;
            }

            _currentLevel = levels[index];
            _logger.LogLevelStart(_currentLevel);
            _currentLevel.StartLevel();
        }

        private void AdvanceLevel()
        {
            _currentLevelIndex++;
            StartLevel(_currentLevelIndex);
        }

        public void AddLevel(RehabLevelBase level)
        {
            if (!levels.Contains(level))
            {
                levels.Add(level);
                level.Initialize(_input);
            }
        }
    }
}
