using UnityEngine;

namespace Project_2
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField] private int maxPitchAtScore;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlayPerfectSound(int perfectStreakCount)
        {
            _audioSource.pitch = Mathf.Lerp(1, 3, (float)perfectStreakCount / maxPitchAtScore);
            _audioSource.Play();
        }
    }
}