using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform mainCamera;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main?.transform;

        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera が見つかりません。");
        }
    }

    // AudioClip を指定して再生できるようにする
    public void Play(AudioClip clip)
    {
        if (mainCamera == null || audioSource == null || clip == null) return;

        audioSource.clip = clip;

        float camX = mainCamera.position.x;
        float objX = transform.position.x;

        float relativeX = Mathf.Clamp(objX - camX, -17f, 17f);
        audioSource.panStereo = relativeX / 17f;
        audioSource.volume = Mathf.Lerp(1f, 0.5f, Mathf.Abs(relativeX) / 17f);

        audioSource.Play();
    }
}
