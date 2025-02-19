using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    // Gán Panel thông báo trong Inspector
    public GameObject notificationPanel;
    public CanvasGroup notificationCanvasGroup;
    // Thời gian hiển thị thông báo (giây)
    public float displayDuration = 2.0f;
    public float fadeDuration = 0.5f;
    
    // Nếu dùng Animator cho fade in/out
    //public Animator animator;

    // Hàm gọi để hiển thị thông báo
    public void ShowNotification()
    {
        // Kích hoạt Panel nếu chưa hiển thị
        notificationPanel.SetActive(true);
        notificationCanvasGroup.alpha = 1f;
        Debug.Log("OK");
        // Nếu sử dụng Animator, kích hoạt trigger xuất hiện
        //if (animator != null)
        //{
        //    animator.SetTrigger("Show");
        //}




        // Sau displayDuration, ẩn thông báo
        //StartCoroutine(HideNotificationAfterDelay(displayDuration));
        StartCoroutine(FadeOutNotification());
    }
    private IEnumerator FadeOutNotification()
    {
        // Chờ displayDuration giây
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Giảm dần alpha từ 1 đến 0
            notificationCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Đảm bảo alpha = 0 và ẩn panel
        notificationCanvasGroup.alpha = 0f;
        notificationPanel.SetActive(false);
    }

    //IEnumerator HideNotificationAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    if (animator != null)
    //    {
    //        animator.SetTrigger("Hide");
    //        // Đợi thời gian animation hide hoàn thành (ví dụ: 0.5 giây)
    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    notificationPanel.SetActive(false);
    //}
}
