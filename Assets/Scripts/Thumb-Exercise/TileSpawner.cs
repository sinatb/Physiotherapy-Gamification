using System.Collections;
using System.Collections.Generic;
using DDL;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class TileSpawner : BaseSpawner
    {
        public List<GameObject>   spawnPosition;
        public float              segmentLength;
        
        private List<AudioClip>   _musicSegments = new List<AudioClip>();
        private int               _previous = -1;
        private int               _count;
        private AudioClip         _music;
        private void Start()
        {
            CurrentSpawnInterval = 2.0f;
        }

        public void SetMusic(AudioClip music)
        {
            _music = music;
            StartCoroutine(SplitAudio());
        }
        protected override void Setup()
        {
            CurrentSpeed = CurrentDdl.baseSpeed;
            _count = 0;
        }

        protected override void SetupDdl(DdlBase d)
        {
            CurrentSpeed = d.baseSpeed;
        }
        private IEnumerator SplitAudio()
        {
            if (_music == null)
            {
                Debug.LogError("Original clip not assigned!");
                yield break;
            }

            while (_music.loadState != AudioDataLoadState.Loaded)
            {
                Debug.LogError(_music.loadState);
                yield return null; 
            }

            var audioData = new float[_music.samples * _music.channels];
            _music.GetData(audioData, 0);

            var segmentSamples = Mathf.FloorToInt(segmentLength * _music.frequency);

            var totalSegments = Mathf.CeilToInt((float)_music.samples / segmentSamples);

            for (var i = 0; i < totalSegments; i++)
            {
                var startSample = i * segmentSamples;
                var currentSegmentSamples = Mathf.Min(segmentSamples, _music.samples - startSample);

                var segmentData = new float[currentSegmentSamples * _music.channels];

                for (var j = 0; j < currentSegmentSamples * _music.channels; j++)
                {
                    segmentData[j] = audioData[startSample * _music.channels + j];
                }

                var segmentClip = AudioClip.Create(
                    $"Segment_{i + 1}",
                    currentSegmentSamples,
                    _music.channels,
                    _music.frequency,
                    false
                );

                segmentClip.SetData(segmentData, 0);

                _musicSegments.Add(segmentClip);
            }
        }
        protected override void Spawn()
        {
            if (_count >= _musicSegments.Count)
                return;
            var randomIndex = Random.Range(0, spawnPosition.Count);
            while (randomIndex == _previous)
            {
                randomIndex = Random.Range(0, spawnPosition.Count);
            }
            _previous = randomIndex;
            var obj = pool.GetPooledObject();
            obj.transform.position = spawnPosition[randomIndex].transform.position;
            obj.SetActive(true);
            obj.GetComponent<BaseObstacle>().SetSpeed(CurrentSpeed);
            obj.GetComponent<Tile>().SetYScale(((DdlThumbData)CurrentDdl).size);
            obj.GetComponent<Tile>().Audio = _musicSegments[_count];
            _count++;
            if (_count == _musicSegments.Count)
                obj.GetComponent<Tile>().isLast = true;

        }
    }
}