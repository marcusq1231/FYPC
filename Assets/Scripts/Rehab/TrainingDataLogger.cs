using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace RehabVR
{
    public sealed class TrainingDataLogger : MonoBehaviour
    {
        private readonly List<string> _rows = new List<string>();
        private string _sessionId;
        private float _sessionStart;

        private void Awake()
        {
            _sessionId = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            _sessionStart = Time.time;
            _rows.Add("session_id,level_name,result,elapsed_seconds,timestamp");
        }

        public void LogLevelStart(RehabLevelBase level)
        {
            Debug.Log($"Starting level: {level.LevelName}");
        }

        public void LogLevelResult(RehabLevelBase level, LevelResult result)
        {
            var elapsed = level.ElapsedTime;
            var timestamp = DateTime.Now.ToString("O", CultureInfo.InvariantCulture);
            _rows.Add($"{_sessionId},{level.LevelName},{result},{elapsed:F2},{timestamp}");
        }

        public void CompleteSession()
        {
            var duration = Time.time - _sessionStart;
            Debug.Log($"Session complete in {duration:F2}s");
            SaveToDisk();
        }

        private void SaveToDisk()
        {
            var directory = Path.Combine(Application.persistentDataPath, "RehabLogs");
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, $"rehab_session_{_sessionId}.csv");
            File.WriteAllLines(path, _rows);
            Debug.Log($"Rehab session log saved to {path}");
        }
    }
}
