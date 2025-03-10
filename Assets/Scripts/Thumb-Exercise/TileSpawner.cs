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
        private GameObject        _previousTile;
        private void Start()
        {
            CurrentSpawnInterval = 2.0f;
        }

        public void SetMusic(AudioClip music)
        {
            _musicSegments.Clear();
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

            var totalDuration = _music.length; 
            var currentTime = 0f; 
            
            _musicSegments.Clear();

            while (currentTime < totalDuration)
            {
                float[] possibleDurations = { 1f, 2f, 3f };
                var segmentLength = possibleDurations[Random.Range(0, possibleDurations.Length)];

                if (currentTime + segmentLength > totalDuration)
                {
                    segmentLength = totalDuration - currentTime; 
                }

                var segmentSamples = Mathf.FloorToInt(segmentLength * _music.frequency);
                var startSample = Mathf.FloorToInt(currentTime * _music.frequency);

                var segmentData = new float[segmentSamples * _music.channels];

                for (var j = 0; j < segmentSamples * _music.channels; j++)
                {
                    segmentData[j] = audioData[startSample * _music.channels + j];
                }

                var segmentClip = AudioClip.Create(
                    $"Segment_{_musicSegments.Count + 1}",
                    segmentSamples,
                    _music.channels,
                    _music.frequency,
                    false
                );

                segmentClip.SetData(segmentData, 0);
                _musicSegments.Add(segmentClip);

                currentTime += segmentLength; 
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
            obj.GetComponent<BaseObstacle>().SetSpeed(CurrentSpeed);
            obj.GetComponent<Tile>().Audio = _musicSegments[_count];
            obj.GetComponent<Tile>().SetYScale(((DdlThumbData)CurrentDdl).size);
            CurrentSpawnInterval = _musicSegments[_count].length;
            var addition = 0.0f;
            while (_previousTile != null && 
                   _previousTile.GetComponent<Renderer>().bounds.Intersects
                       (new Bounds(spawnPosition[randomIndex].transform.position + addition * Vector3.up,
                           new Vector3(10.0f,obj.transform.localScale.y,1.0f))))
            {
                addition += 0.1f;
            }
            obj.transform.position = spawnPosition[randomIndex].transform.position + addition * Vector3.up;
            obj.SetActive(true);
            _previousTile = obj;
            _count++;
            if (_count == _musicSegments.Count)
                obj.GetComponent<Tile>().isLast = true;
        }
    }
}