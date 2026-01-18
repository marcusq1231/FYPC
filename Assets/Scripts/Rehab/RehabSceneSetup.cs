using System.Collections.Generic;
using UnityEngine;

namespace RehabVR
{
    [ExecuteAlways]
    public sealed class RehabSceneSetup : MonoBehaviour
    {
        [Header("Scene Setup")]
        [SerializeField] private bool autoSetup = true;
        [SerializeField] private bool rebuildInPlayMode = false;
        [SerializeField] private Vector3 floorScale = new Vector3(2f, 1f, 2f);
        [SerializeField] private Vector3 tableScale = new Vector3(0.8f, 0.06f, 0.5f);
        [SerializeField] private Vector3 tablePosition = new Vector3(0f, 0.8f, 0.5f);

        private const string EnvironmentName = "RehabEnvironment";
        private const string SessionName = "RehabSession";
        private const string TargetsName = "ReachTargets";

        private void OnEnable()
        {
            if (!autoSetup)
            {
                return;
            }

            if (Application.isPlaying && !rebuildInPlayMode)
            {
                return;
            }

            EnsureScene();
        }

        private void EnsureScene()
        {
            var environment = FindOrCreate(EnvironmentName);
            var targetsParent = EnsureTargets(environment);
            EnsureFloor(environment);
            EnsureTable(environment);
            EnsureInstructionText(environment);

            var session = FindOrCreate(SessionName);
            var manager = session.GetComponent<RehabSessionManager>() ?? session.AddComponent<RehabSessionManager>();
            session.GetComponent<TrainingDataLogger>() ?? session.AddComponent<TrainingDataLogger>();

            var pinch = session.GetComponent<PinchPulseLevel>() ?? session.AddComponent<PinchPulseLevel>();
            var grip = session.GetComponent<GripHoldLevel>() ?? session.AddComponent<GripHoldLevel>();
            var rotation = session.GetComponent<RotationMatchLevel>() ?? session.AddComponent<RotationMatchLevel>();
            var sequence = session.GetComponent<ButtonSequenceLevel>() ?? session.AddComponent<ButtonSequenceLevel>();
            var reach = session.GetComponent<ReachTargetLevel>() ?? session.AddComponent<ReachTargetLevel>();

            var targetList = new List<Transform>();
            foreach (Transform child in targetsParent.transform)
            {
                targetList.Add(child);
            }

            reach.SetTargets(targetList);

            manager.AddLevel(pinch);
            manager.AddLevel(grip);
            manager.AddLevel(rotation);
            manager.AddLevel(sequence);
            manager.AddLevel(reach);
        }

        private GameObject FindOrCreate(string objectName)
        {
            var existing = GameObject.Find(objectName);
            if (existing != null)
            {
                return existing;
            }

            return new GameObject(objectName);
        }

        private GameObject EnsureTargets(GameObject environment)
        {
            var existing = GameObject.Find(TargetsName);
            if (existing != null)
            {
                return existing;
            }

            var parent = new GameObject(TargetsName);
            parent.transform.SetParent(environment.transform);
            parent.transform.localPosition = Vector3.zero;

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
                target.transform.SetParent(parent.transform);
                target.transform.position = positions[i];
                target.transform.localScale = Vector3.one * 0.08f;
            }

            return parent;
        }

        private void EnsureFloor(GameObject environment)
        {
            var floor = GameObject.Find("RehabFloor");
            if (floor == null)
            {
                floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
                floor.name = "RehabFloor";
                floor.transform.SetParent(environment.transform);
                floor.transform.position = Vector3.zero;
            }

            floor.transform.localScale = floorScale;
        }

        private void EnsureTable(GameObject environment)
        {
            var table = GameObject.Find("RehabTable");
            if (table == null)
            {
                table = GameObject.CreatePrimitive(PrimitiveType.Cube);
                table.name = "RehabTable";
                table.transform.SetParent(environment.transform);
            }

            table.transform.localPosition = tablePosition;
            table.transform.localScale = tableScale;
        }

        private void EnsureInstructionText(GameObject environment)
        {
            var text = GameObject.Find("RehabInstructions");
            if (text == null)
            {
                text = new GameObject("RehabInstructions");
                text.transform.SetParent(environment.transform);
                var mesh = text.AddComponent<TextMesh>();
                mesh.text = "VR Hand Rehab\nTrigger: Pinch Pulse\nGrip: Hold\nRotate: Match\nTrigger+Grip: Sequence\nReach spheres to finish";
                mesh.fontSize = 36;
                mesh.characterSize = 0.02f;
                mesh.alignment = TextAlignment.Center;
                mesh.anchor = TextAnchor.MiddleCenter;
            }

            text.transform.localPosition = new Vector3(0f, 1.4f, 0.9f);
            text.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
