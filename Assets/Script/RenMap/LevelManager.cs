using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Helper;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public Button button;           // Reference đến button
        public Text levelText;          // Text hiển thị số level
        public Image starsImage;        // Image hiển thị số sao
        //public Image buttonImage;       // Image của button (để thay đổi locked/unlocked)
    }

    public Sprite highlightedSprite; 
    public Sprite pressedSprite; 
    public Sprite selectedSprite;

    [Header("Level Buttons")]
    [SerializeField] private LevelButton[] levelButtons;  // Array chứa các button level

    [Header("Button Sprites")]
    [SerializeField] private Sprite lockedButtonSprite;   // Sprite cho button bị khóa
    [SerializeField] private Sprite unlockedButtonSprite; // Sprite cho button đã mở khóa

    [Header("Star Rating Sprites")]
    [SerializeField] private Sprite[] starSprites;        // Array các sprite hiển thị sao (0,1,2,3 sao)

    private void Start()
    {
        InitializeLevelButtons();
    }
    private void InitializeLevelButtons()
    {
        // Khởi tạo cho từng button
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            LevelButton levelBtn = levelButtons[i];

            // Set text số level
            if (levelBtn.levelText != null)
            {
                levelBtn.levelText.text = levelIndex.ToString();
            }

            // Load trạng thái mở khóa của level
            bool isUnlocked = IsLevelUnlocked(levelIndex);

            int stars = DoorData.StatusDoors[levelIndex-1];

            // Cập nhật visual của button
            UpdateButtonVisual(levelBtn, isUnlocked, stars, levelIndex);

            // Thêm listener cho button
            levelBtn.button.onClick.AddListener(() => OnLevelButtonClicked(levelIndex));
        }
    }

    private void UpdateButtonVisual(LevelButton levelBtn, bool isUnlocked, int stars, int level)
    {
        if(isUnlocked == false)
        {
            levelBtn.button.image.sprite = lockedButtonSprite;
            levelBtn.levelText.text = "";
            levelBtn.starsImage.enabled = false;
        }
        else if (stars == 0)
        {
            levelBtn.button.transition = Selectable.Transition.SpriteSwap;

            SpriteState spriteState = new SpriteState();
            spriteState.highlightedSprite = highlightedSprite;
            spriteState.pressedSprite = pressedSprite;
            spriteState.selectedSprite = selectedSprite;
            levelBtn.button.spriteState = spriteState;


            levelBtn.button.image.sprite = unlockedButtonSprite;
            levelBtn.starsImage.sprite = starSprites[0];

            levelBtn.levelText.text = $"{level}";
            levelBtn.starsImage.enabled = false;
        }

        else
        {
            levelBtn.starsImage.sprite = starSprites[stars-1];
            levelBtn.starsImage.enabled = isUnlocked; // Ẩn sao nếu level bị khóa
        }
    }

    private void OnLevelButtonClicked(int levelNumber)
    {
        Debug.Log($"Click level: {levelNumber}");
        PlayerPrefs.SetInt("levelNumber", levelNumber);
        if (DoorData.StatusDoors[levelNumber-1] != -1)  // Biến bool kiểm tra trạng thái khóa của màn chơi
        {
            FindObjectOfType<PopupController>().ShowPopup();
        }
        else
        {
            FindObjectOfType<NotificationManager>().ShowNotification();
        }
    }


    private bool IsLevelUnlocked(int levelNumber)
    {
        if (DoorData.StatusDoors[levelNumber-1] == -1) return false;

        return true;
    }

    // Lấy số sao đã đạt được của level
    private int GetLevelStars(int levelNumber)
    {
        return PlayerPrefs.GetInt($"Level_{levelNumber}_Stars", 0);
    }

    // Gọi hàm này khi người chơi hoàn thành level
    //public void OnLevelComplete(int levelNumber, int starsEarned)
    //{
    //    // Lưu số sao đạt được
    //    PlayerPrefs.SetInt($"Level_{levelNumber}_Stars", starsEarned);

    //    // Mở khóa level tiếp theo
    //    if (levelNumber < levelButtons.Length)
    //    {
    //        PlayerPrefs.SetInt($"Level_{levelNumber + 1}_Unlocked", 1);
    //    }

    //    PlayerPrefs.Save();

    //    // Cập nhật lại visual của các button
    //    int nextLevel = levelNumber + 1;
    //    if (nextLevel <= levelButtons.Length)
    //    {
    //        UpdateButtonVisual(levelButtons[nextLevel - 1], true, 0);
    //    }
    //}

    // Reset tất cả tiến độ (dùng cho testing)
    public void ResetAllProgress()
    {
        for (int i = 2; i <= levelButtons.Length; i++)
        {
            PlayerPrefs.DeleteKey($"Level_{i}_Unlocked");
            PlayerPrefs.DeleteKey($"Level_{i}_Stars");
        }
        PlayerPrefs.Save();
        InitializeLevelButtons();
    }
}