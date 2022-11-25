using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace bebaSpace.AlphabetMiniGame
{
    public class SoundManager : MonoBehaviour
    {

        #region Instance declaration
        private static SoundManager instance = null;

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (SoundManager)FindObjectOfType(typeof(SoundManager));
                    if (instance == null)
                    {
                        // Create gameObject and add component
                        instance = (new GameObject("SoundManager")).AddComponent<SoundManager>();
                    }
                }
                return instance;
            }
        }


        #endregion

        [SerializeField] private SoundEvent[] soundEvents;

        #region Awake
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }


            for (int i = 0; i < soundEvents.Length; i++)
            {
                if (soundEvents[i].AudioSource == null)
                {
                    GameObject audioSourceObject = new GameObject("SoundEvent_" + i + "_" + soundEvents[i].Name);
                    audioSourceObject.transform.SetParent(this.transform);
                    soundEvents[i].AudioSource = audioSourceObject.AddComponent<AudioSource>();
                }

                if (soundEvents[i].PlayOnAwake)
                {
                    soundEvents[i].Play();
                    return;
                }
            }

            //if (SaveLoad.SaveExist("GameState"))
            //{
            //    Load();
            //}
            //else
            //{
            //  
            //}

            //GameEvents.SaveInitiated += Save;
        }
        #endregion


        private void Update()
        {
            InitializeCoroutine();
        }

        private void InitializeCoroutine()
        {
            StartCoroutine(RepeatPlaySound());
        }

        public void PlaySound(string _name)
        {
            for (int i = 0; i < soundEvents.Length; i++)
            {
                if (soundEvents[i].Name == _name)
                {
                    if (soundEvents[i].AudioSource.isPlaying && GameManager.Instance.BGMPersist)
                    {
                        return;
                    }

                    soundEvents[i].Play();
                    return;
                }
            }

            Debug.LogWarning("SoundManager: Sound not found in SoundEvent list: " + _name);
        }

        public void PlaySoundUntilDone(string _name)
        {
            for (int i = 0; i < soundEvents.Length; i++)
            {
                if (soundEvents[i].Name == _name)
                {
                    if (soundEvents[i].AudioSource.isPlaying)
                    {
                        return;
                    }

                    soundEvents[i].Play();
                    return;
                }
            }

            Debug.LogWarning("SoundManager: Sound not found in SoundEvent list: " + _name);
        }

            public void StopSound(string _name)
        {
            for (int i = 0; i < soundEvents.Length; i++)
            {
                if (soundEvents[i].Name == _name)
                {
                    soundEvents[i].Stop();
                    return;
                }
            }

            Debug.LogWarning("SoundManager: Sound not found in SoundEvent list: " + _name);
        }

        IEnumerator RepeatPlaySound()
        {
            for (int i = 0; i < soundEvents.Length; i++)
            {
                if (soundEvents[i].PlayCalled == true && soundEvents[i].RandomizeLoop == true && soundEvents[i].AvoidRepeat == true)
                {
                    if (soundEvents[i].Delay == false)
                    {
                        yield return new WaitForSeconds(soundEvents[i].AudioSource.clip.length);
                    }

                    if (soundEvents[i].Delay == true)
                    {
                        float delay = Random.Range(soundEvents[i].DelayTime - soundEvents[i].RandomizeDelay, soundEvents[i].DelayTime + soundEvents[i].RandomizeDelay);
                        yield return new WaitForSeconds(soundEvents[i].AudioSource.clip.length + delay);
                    }

                    if (!soundEvents[i].AudioSource.isPlaying)
                    {
                        PlaySound(soundEvents[i].Name);
                    }
                }
            }
        }

    }

    [System.Serializable]
    public class SoundEvent
    {
        public string Name;
        public AudioSource AudioSource;
        public AudioClip[] AudioClips;
        public AudioMixerGroup Output;

        [Range(0f, 1f)]
        public float MinVolume = 1f;

        [Range(0f, 1f)]
        public float MaxVolume = 1f;

        [Range(0f, 3f)]
        public float MinPitch = 1f;

        [Range(0f, 3f)]
        public float MaxPitch = 1f;

        [Range(0f, 1f)]
        public float StereoPan = 0f;

        [Range(0f, 5f)]
        public float DelayTime = 0f;

        [Range(0f, 5f)]
        public float RandomizeDelay = 0f;

        public bool AvoidRepeat = false;
        public bool Delay = false;
        public bool Loop = false;
        public bool RandomizeLoop = false;
        public bool PlayOnAwake = false;
        public bool Mute = false;

        [HideInInspector]
        public bool PlayCalled = false;


        public void Play()
        {
            PlayCalled = true;

            float randomVolume = Random.Range(MinVolume, MaxVolume);
            float randomPitch = Random.Range(MinPitch, MaxPitch);

            AudioSource.volume = randomVolume;
            AudioSource.pitch = randomPitch;
            AudioSource.panStereo = StereoPan;
            AudioSource.loop = Loop;
            AudioSource.mute = Mute;
            AudioSource.outputAudioMixerGroup = Output;


            if (Delay == false && AvoidRepeat == false)
            {
                AudioSource.clip = AudioClips[Random.Range(0, AudioClips.Length)];
                AudioSource.Play();
            }

            if (AvoidRepeat == true && Delay == false)
            {
                int r = Random.Range(1, AudioClips.Length);
                AudioSource.clip = AudioClips[r];
                AudioSource.Play();
                AudioClips[r] = AudioClips[0];
                AudioClips[0] = AudioSource.clip;
            }

            if (Delay == true && AvoidRepeat == true)
            {
                float delay = Random.Range(DelayTime - RandomizeDelay, DelayTime + RandomizeDelay);
                int r = Random.Range(1, AudioClips.Length);
                AudioSource.clip = AudioClips[r];
                AudioSource.PlayDelayed(delay);
                AudioClips[r] = AudioClips[0];
                AudioClips[0] = AudioSource.clip;
            }

            if (Delay == true && AvoidRepeat == false)
            {
                float delay = Random.Range(DelayTime - RandomizeDelay, DelayTime + RandomizeDelay);
                AudioSource.clip = AudioClips[Random.Range(0, AudioClips.Length)];
                AudioSource.PlayDelayed(delay);
            }
        }

        public void Stop()
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
        }

    }
}