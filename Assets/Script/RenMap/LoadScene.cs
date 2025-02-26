using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public RectTransform rectTransform; // Thanh tiến trình
    public TextMeshProUGUI progressText; // Văn bản hiển thị phần trăm tải

    private void Start()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    private IEnumerator LoadNextSceneAsync()
    {
        string nextSceneName = PlayerPrefs.GetString("NextScene");
        Debug.Log($"Load Scene: {nextSceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        // Không cho phép scene tự động chuyển khi tải xong
        asyncLoad.allowSceneActivation = false;
        float originalWidth = rectTransform.rect.width;
        // Đợi cho đến khi scene được tải xong
        while (!asyncLoad.isDone)
        {
            // Tính toán tiến trình tải (từ 0 đến 0.9)
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Cập nhật thanh tiến trình và văn bản
            rectTransform.sizeDelta = new Vector2 (progress* originalWidth, rectTransform.sizeDelta.y);
            progressText.text = $"{progress*100}%";


            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
                PlayerPrefs.DeleteKey("NextScene");
            }

            yield return null;
        }
    }
}
