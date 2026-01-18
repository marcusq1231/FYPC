using UnityEngine;
using UnityEngine.XR;

namespace RehabVR
{
    public enum RehabHand
    {
        Left,
        Right
    }

    public sealed class XRHandInput
    {
        private InputDevice _leftDevice;
        private InputDevice _rightDevice;

        public XRHandInput()
        {
            RefreshDevices();
        }

        public void RefreshDevices()
        {
            _leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            _rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        public float GetTrigger(RehabHand hand)
        {
            var device = GetDevice(hand);
            if (!device.isValid)
            {
                RefreshDevices();
                device = GetDevice(hand);
            }

            if (device.TryGetFeatureValue(CommonUsages.trigger, out var trigger))
            {
                return trigger;
            }

            return 0f;
        }

        public float GetGrip(RehabHand hand)
        {
            var device = GetDevice(hand);
            if (!device.isValid)
            {
                RefreshDevices();
                device = GetDevice(hand);
            }

            if (device.TryGetFeatureValue(CommonUsages.grip, out var grip))
            {
                return grip;
            }

            return 0f;
        }

        public Vector3 GetPosition(RehabHand hand)
        {
            var device = GetDevice(hand);
            if (!device.isValid)
            {
                RefreshDevices();
                device = GetDevice(hand);
            }

            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out var position))
            {
                return position;
            }

            return Vector3.zero;
        }

        public Quaternion GetRotation(RehabHand hand)
        {
            var device = GetDevice(hand);
            if (!device.isValid)
            {
                RefreshDevices();
                device = GetDevice(hand);
            }

            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation))
            {
                return rotation;
            }

            return Quaternion.identity;
        }

        private InputDevice GetDevice(RehabHand hand)
        {
            return hand == RehabHand.Left ? _leftDevice : _rightDevice;
        }
    }
}
