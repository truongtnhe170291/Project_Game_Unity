using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Button volumeButton;
    public Sprite muteSprite;  // Icon tắt âm thanh
    public Sprite unmuteSprite; // Icon bật âm thanh

    private Image buttonImage;
    private float lastVolume = 1f; // Lưu lại giá trị âm thanh trước khi mute

    private void Start()
    {
        buttonImage = volumeButton.GetComponent<Image>();

        // Load âm lượng từ PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Kiểm tra xem có đang tắt âm hay không
        UpdateButtonSprite(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeButton.onClick.AddListener(ToggleMute);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();

        // Nếu kéo slider về 0, đổi nút thành mute
        UpdateButtonSprite(volume);
    }

    private void ToggleMute()
    {
        if (AudioListener.volume > 0)
        {
            lastVolume = AudioListener.volume; // Lưu lại giá trị trước khi tắt
            SetVolume(0);
            volumeSlider.value = 0;
        }
        else
        {
            SetVolume(lastVolume > 0 ? lastVolume : 1f);
            volumeSlider.value = lastVolume;
        }
    }

    private void UpdateButtonSprite(float volume)
    {
        if (volume <= 0)
        {
            buttonImage.sprite = muteSprite;
        }
        else
        {
            buttonImage.sprite = unmuteSprite;
        }
    }
}
