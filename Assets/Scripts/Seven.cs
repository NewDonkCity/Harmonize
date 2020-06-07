using UnityEngine;
using System.Collections;

public class Seven : MonoBehaviour
{
    [SerializeField] private Five _metronome;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField, Range(0f, 2f)] private double _attackTime;
    [SerializeField, Range(0f, 2f)] private double _sustainTime;
    [SerializeField, Range(0f, 2f)] private double _releaseTime;
    [SerializeField, Range(1, 8)] private int _numVoices = 2;
    [SerializeField] private Six _samplerVoicePrefab;

    private Six[] _samplerVoices;
    private int _nextVoiceIndex;

    private void Awake()
    {
        _samplerVoices = new Six[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            Six samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            _samplerVoices[i] = samplerVoice;
        }
    }

    private void OnEnable()
    {
        if (_metronome != null)
        {
            _metronome.Ticked += HandleTicked;
        }
    }

    private void OnDisable()
    {
        if (_metronome != null)
        {
            _metronome.Ticked -= HandleTicked;
        }
    }

    private void HandleTicked(double tickTime)
    {
        _samplerVoices[_nextVoiceIndex].Play(_audioClip, tickTime, _attackTime, _sustainTime, _releaseTime);

        _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
    }
}