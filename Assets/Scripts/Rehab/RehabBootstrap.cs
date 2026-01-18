using System.Collections.Generic;
using UnityEngine;

namespace RehabVR
{
    public sealed class RehabBootstrap : MonoBehaviour
    {
        [Header("Auto Setup")]
        [SerializeField] private bool autoCreateOnPlay = true;

        private void Awake()
        {
            if (!autoCreateOnPlay)
            {
                return;
            }

            var sessionObject = new GameObject("RehabSession");
            var manager = sessionObject.AddComponent<RehabSessionManager>();
            sessionObject.AddComponent<TrainingDataLogger>();

            var pinch = sessionObject.AddComponent<PinchPulseLevel>();
            var grip = sessionObject.AddComponent<GripHoldLevel>();
            var rotation = sessionObject.AddComponent<RotationMatchLevel>();
            var sequence = sessionObject.AddComponent<ButtonSequenceLevel>();
            var reach = sessionObject.AddComponent<ReachTargetLevel>();

            var targets = CreateReachTargets();
            reach.SetTargets(targets);

            manager.AddLevel(pinch);
            manager.AddLevel(grip);
            manager.AddLevel(rotation);
            manager.AddLevel(sequence);
            manager.AddLevel(reach);
        }

        private List<Transform> CreateReachTargets()
        {
            var targets = new List<Transform>();
            var parent = new GameObject("ReachTargets").transform;
            parent.position = Vector3.zero;

            var positions = new[]
            {
                new Vector3(0.2f, 1.2f, 0.4f),
                new Vector3(-0.2f, 1.1f, 0.5f),
                new Vector3(0.25f, 1.3f, 0.6f)
            };

            for (var i = 0; i < positions.Length; i++)
            {
                var target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                target.name = $"ReachTarget_{i + 1}";
                target.transform.SetParent(parent);
                target.transform.position = positions[i];
                target.transform.localScale = Vector3.one * 0.08f;
                targets.Add(target.transform);
            }

            return targets;
        }
    }
}
