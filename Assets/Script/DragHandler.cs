using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    private GameObject draggingObject;

    public GameObject modelPrefab; // Prefab สำหรับโมเดลที่จะลาก

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;

        if (modelPrefab != null)
        {
            draggingObject = Instantiate(modelPrefab, transform.root);
            draggingObject.transform.SetAsLastSibling();

            CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false; // ทำให้ dragging object ไม่บล็อก raycasts
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingObject != null)
        {
            draggingObject.transform.position = Input.mousePosition; // ปรับตำแหน่งตามตำแหน่งเมาส์
        }
    }

   public void OnEndDrag(PointerEventData eventData)
{
    if (draggingObject != null)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject targetObject = hit.collider.gameObject;

            // ตรวจสอบว่า targetObject เป็นโมเดลที่สามารถแทนที่ได้หรือไม่
            if (targetObject.CompareTag("ReplaceableModel"))
            {
                // ลบโมเดลเดิมออก
                Destroy(targetObject); 

                // วางโมเดลใหม่ที่ตำแหน่งของ hit
                draggingObject.transform.position = hit.point; // ใช้ตำแหน่งของ hit
                draggingObject.transform.localScale = Vector3.one; // ตั้งขนาดให้คงที่ (1, 1, 1)
            }
            else
            {
                Destroy(draggingObject); // หากไม่สามารถแทนที่ได้ ให้ทำลายโมเดลที่ลาก
            }
        }
        else
        {
            Destroy(draggingObject); // หากไม่ชนอะไรให้ทำลายโมเดลที่ลาก
        }

        draggingObject = null; // รีเซ็ต draggingObject
    }
}

}
