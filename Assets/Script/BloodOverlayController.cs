using UnityEngine;
using UnityEngine.UI;

public class BloodOverlayController : MonoBehaviour
{
    private Image bloodOverlay; // Tham chiếu đến Image bao phủ
    public float flashSpeed = 1f; // Tốc độ nhấp nháy
    public float lowHealthThreshold = 0.3f; // Ngưỡng máu thấp (30%)

    private bool isLowHealth = false;
    private float currentAlpha = 0f;

    void Start()
    {
        // Tự động tìm BloodOverlay trong Canvas
        if (bloodOverlay == null)
        {
            GameObject overlayObject = GameObject.Find("BloodOverlay"); // Tìm đối tượng BloodOverlay theo tên
            if (overlayObject != null)
            {
                bloodOverlay = overlayObject.GetComponent<Image>();
            }
            else
            {
                Debug.LogError("BloodOverlay not found in the scene!");
            }
        }
    }
    void Update()
    {
        // Giả sử bạn có một biến `health` và `maxHealth` để theo dõi máu của người chơi
        //float health = GetComponent<PlayerHealth>().currentHealth; // Thay thế bằng cách lấy máu thực tế của người chơi
        //float maxHealth = GetComponent<PlayerHealth>().maxHealth; // Thay thế bằng cách lấy máu tối đa của người chơi

        // Kiểm tra nếu máu dưới 30%
        //if (health / maxHealth < lowHealthThreshold)
        //{
        //    isLowHealth = true;
        //}
        //else
        //{
        //    isLowHealth = false;
        //    bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, 0); // Đặt alpha về 0 khi máu trên 30%
        //}

        //// Nhấp nháy khi máu thấp
        //if (isLowHealth)
        //{
        //    currentAlpha = Mathf.PingPong(Time.time * flashSpeed, 0.5f); // Điều chỉnh alpha từ 0 đến 0.5
        //    bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, currentAlpha);
        //}
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            currentAlpha = Mathf.PingPong(Time.time * flashSpeed, 0.1f);
            Debug.Log(currentAlpha);

            bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, currentAlpha);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, 0);
        }
    }
}