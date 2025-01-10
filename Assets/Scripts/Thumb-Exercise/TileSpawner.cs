using System.Collections.Generic;
using DDL;
using UnityEngine;
using Util;

namespace Thumb_Exercise
{
    public class TileSpawner : BaseSpawner
    {
        public List<GameObject>   spawnPosition;
        public AudioClip          music;
        public float              segmentLength;
        
        private List<AudioClip>   _musicSegments = new List<AudioClip>();
        private int               _previous = -1;
        private int               _count;
        private void Start()
        {
            CurrentSpawnInterval = 2.0f;
            SplitAudio();
        }
        protected override void Setup()
        {
            CurrentSpeed = CurrentDdl.baseSpeed;
        }

        protected override void SetupDdl(DdlBase d)
        {
            CurrentSpeed = d.baseSpeed;
        }
        private void SplitAudio()
        {
            if (music == null)
            {
                Debug.LogError("Original clip not assigned!");
                return;
            }

            var audioData = new float[music.samples * music.channels];
            music.GetData(audioData, 0);

            var segmentSamples = Mathf.FloorToInt(segmentLength * music.frequency);

            var totalSegments = Mathf.CeilToInt((float)music.samples / segmentSamples);

            for (var i = 0; i < totalSegments; i++)
            {
                var startSample = i * segmentSamples;
                var currentSegmentSamples = Mathf.Min(segmentSamples, music.samples - startSample);

                var segmentData = new float[currentSegmentSamples * music.channels];

                for (var j = 0; j < currentSegmentSamples * music.channels; j++)
                {
                    segmentData[j] = audioData[startSample * music.channels + j];
                }

                var segmentClip = AudioClip.Create(
                    $"Segment_{i + 1}",
                    currentSegmentSamples,
                    music.channels,
                    music.frequency,
                    false
                );

                segmentClip.SetData(segmentData, 0);

                _musicSegments.Add(segmentClip);
            }
        }
        protected override void Spawn()
        {
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
        }
    }
}