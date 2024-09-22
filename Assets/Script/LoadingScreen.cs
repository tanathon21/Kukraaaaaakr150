using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar; // ตัว Slider ที่ใช้แสดงสถานะการโหลด
    public Text progressText; // ตัว Text ที่ใช้แสดงเปอร์เซ็นต์การโหลด
    public CanvasGroup imageCanvasGroup; // CanvasGroup สำหรับรูปภาพที่ต้องการให้ fade
    public float baseLoadingTime = 3.0f; // เวลาหน่วงพื้นฐานในการโหลด (หน่วยเป็นวินาที)

    private float fakeLoadingTime; // เวลาหน่วงที่ปรับได้

    void Start()
    {
        // คำนวณเวลาหน่วงการโหลดตามสมรรถภาพของอุปกรณ์
        CalculateFakeLoadingTime();

        // เริ่มโหลด Scene
        StartCoroutine(LoadAsyncOperation());
    }

    void CalculateFakeLoadingTime()
    {
        // กำหนดเวลาหน่วงการโหลดตามขนาด RAM
        if (SystemInfo.systemMemorySize >= 8192) // 8 GB = 8192 MB
        {
            fakeLoadingTime = baseLoadingTime; // สำหรับเครื่องที่มี RAM อย่างน้อย 8 GB
        }
        else
        {
            // หาก RAM ต่ำกว่า 8 GB ลดเวลาหน่วง (หรือปรับตามที่คุณต้องการ)
            fakeLoadingTime = baseLoadingTime * 2.0f; // เพิ่มเวลาหน่วง 2.0 เท่า
        }

        // จำกัดค่าระหว่าง 1 และ 10 วินาที
        fakeLoadingTime = Mathf.Clamp(fakeLoadingTime, 1.0f, 10.0f);
    }

    IEnumerator LoadAsyncOperation()
    {
        // โหลด Scene แบบ Asynchronously
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync("MainScene");
        gameLevel.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < fakeLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            // อัปเดต Progress Bar และ Text
            progressBar.value = Mathf.Clamp01(elapsedTime / fakeLoadingTime);
            progressText.text = Mathf.RoundToInt(progressBar.value * 100) + "%";

            // ค่อยๆ เพิ่มค่าความโปร่งใสของรูปภาพ
            imageCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fakeLoadingTime);

            // รอให้ถึงเฟรมต่อไป
            yield return null;
        }

        // หลังจากเวลาหน่วงผ่านไป ให้โหลด Scene จริง
        progressBar.value = 1;
        progressText.text = "100%";
        imageCanvasGroup.alpha = 1;
        gameLevel.allowSceneActivation = true;
    }
}
