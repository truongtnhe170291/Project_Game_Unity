using UnityEngine;
using UnityEngine.UI; // Dùng nếu không dùng TextMeshPro
using TMPro;         // Dùng nếu sử dụng TextMeshPro

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel;  // Tham chiếu đến Panel của popup
    public Button yesButton;       // Nút Đồng ý
    public Button noButton;        // Nút Không
    public TextMeshProUGUI popupText;  // Văn bản của popup, nếu dùng TextMeshPro
    //public Text popupText;        // Dùng Text thay vì TextMeshPro nếu cần

    private void Start()
    {
        // Ẩn popup khi bắt đầu
        popupPanel.SetActive(false);

        // Gán sự kiện khi bấm nút
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    // Hàm để hiện popup
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f;  // Tạm dừng game
    }

    // Hàm được gọi khi người chơi nhấn "Đồng ý"
    private void OnYesButtonClicked()
    {
        // Thực hiện hành động khi người chơi đồng ý
        Debug.Log("Người chơi đã đồng ý!");

        popupPanel.SetActive(false);
        Time.timeScale = 1f;  // Tiếp tục game
        int levelNumber = PlayerPrefs.GetInt("levelNumber");
        GameManager.Instance.StartLevel(levelNumber);
    }

    // Hàm được gọi khi người chơi nhấn "Không"
    private void OnNoButtonClicked()
    {
        // Thực hiện hành động khi người chơi từ chối
        Debug.Log("Người chơi đã từ chối!");

        popupPanel.SetActive(false);
        Time.timeScale = 1f;  // Tiếp tục game
    }
}
