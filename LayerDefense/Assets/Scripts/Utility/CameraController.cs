using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _shakeCamera;

    private CinemachineBasicMultiChannelPerlin _channelPerlin = null;
    private CancellationTokenSource _tokenSource = null;

    public void Shake(float amplitude, float second)
    {
        if (_channelPerlin == null)
            return;

        if (_tokenSource != null)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = null;
        }

        _tokenSource = new CancellationTokenSource();
        var targetTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_tokenSource.Token);

        DelayedShake(amplitude, second, targetTokenSource).Forget();
    }

    #region :   Unity Message

    private void Awake()
    {
        _channelPerlin = _shakeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        if (_tokenSource != null)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = null;
        }
    }

    #endregion

    protected virtual async UniTaskVoid DelayedShake(float amplitude, float second, CancellationTokenSource tokenSource)
    {
        var t = second;
        while (t > 0)
        {
            _channelPerlin.m_AmplitudeGain = Mathf.Lerp(0, amplitude, t / second);

            t -= Time.deltaTime;

            await UniTask.Yield(tokenSource.Token);
        }

        _channelPerlin.m_AmplitudeGain = 0;
    }
}
