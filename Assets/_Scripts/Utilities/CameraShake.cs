using UnityEngine;
using Cinemachine;

namespace FinleyConway.Utilities
{
    public class CameraShake : Singleton<CameraShake>
    {
        private float _shakeTimer;
        private float _startingShakeTimer;
        private float _startingIntensity;

        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        protected override void Awake()
        {
            base.Awake();
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            // set intensity to 0 after set period of time
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                        _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0, (1 - (_shakeTimer / _startingShakeTimer)));
            }
        }

        public void ShakeCamera(float intensity, float time)
        {
            // shakes cinemachine camera
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            _shakeTimer = time;
            _startingShakeTimer = time;
            _startingIntensity = intensity;
        }
    }
}
