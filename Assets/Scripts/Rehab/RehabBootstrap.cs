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

            var sessionObject = GameObject.Find("RehabSession") ?? new GameObject("RehabSession");
            var manager = sessionObject.GetComponent<RehabSessionManager>() ?? sessionObject.AddComponent<RehabSessionManager>();
            sessionObject.GetComponent<TrainingDataLogger>() ?? sessionObject.AddComponent<TrainingDataLogger>();

            var pinch = sessionObject.GetComponent<PinchPulseLevel>() ?? sessionObject.AddComponent<PinchPulseLevel>();
            var grip = sessionObject.GetComponent<GripHoldLevel>() ?? sessionObject.AddComponent<GripHoldLevel>();
            var rotation = sessionObject.GetComponent<RotationMatchLevel>() ?? sessionObject.AddComponent<RotationMatchLevel>();
            var sequence = sessionObject.GetComponent<ButtonSequenceLevel>() ?? sessionObject.AddComponent<ButtonSequenceLevel>();
            var reach = sessionObject.GetComponent<ReachTargetLevel>() ?? sessionObject.AddComponent<ReachTargetLevel>();

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
            var parentObject = GameObject.Find("ReachTargets") ?? new GameObject("ReachTargets");
            var parent = parentObject.transform;
            parent.position = Vector3.zero;

            if (parent.childCount > 0)
            {
                foreach (Transform child in parent)
                {
                    targets.Add(child);
                }

                return targets;
            }

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
