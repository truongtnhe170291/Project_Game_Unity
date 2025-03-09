using Assets.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs; // Danh sách Prefab nhân vật
    public Image characterDisplay;        // UI hiển thị hình ảnh nhân vật
    public Button nextButton, prevButton, selectButton;

    private int currentCharacterIndex = 0;

    void Start()
    {
        UpdateCharacterDisplay();

        // Thêm sự kiện cho Button
        nextButton.onClick.AddListener(NextCharacter);
        prevButton.onClick.AddListener(PreviousCharacter);
        selectButton.onClick.AddListener(SelectCharacter);
    }

    void UpdateCharacterDisplay()
    {
        // Tạo một instance tạm thời từ Prefab nhưng không hiển thị
        GameObject tempCharacter = Instantiate(characterPrefabs[currentCharacterIndex]);

        // Tìm GameObject "Character" trong Prefab
        Transform characterTransform = tempCharacter.transform.Find("Character");

        if (characterTransform != null)
        {
            SpriteRenderer spriteRenderer = characterTransform.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                characterDisplay.sprite = spriteRenderer.sprite;
            }
        }

        // Xóa object tạm sau khi lấy sprite
        Destroy(tempCharacter);
    }


    void NextCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characterPrefabs.Length;
        UpdateCharacterDisplay();
    }

    void PreviousCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
        UpdateCharacterDisplay();
    }

    void SelectCharacter()
    {
        // Lưu nhân vật đã chọn vào PlayerPrefs
        PlayerPrefs.SetInt(PlayerPrefsHelper.SelectSkin, currentCharacterIndex);
        PlayerPrefs.Save();
        int levelNumber = PlayerPrefs.GetInt("levelNumber");
        GameManager.Instance.StartLevel(levelNumber);
    }
}
