using UnityEngine;

public class GarmentSwitcher : MonoBehaviour
{
    [Header("Garment Objects in Scene")]
    public GameObject[] garments;

    [Header("Demo Settings")]
    public bool showHUD = true;

    private int currentIndex = 0;
    private float deltaTime = 0.0f;

    void Start()
    {
        if (garments == null || garments.Length == 0)
        {
            Debug.LogWarning("No garments assigned to GarmentSwitcher.");
            return;
        }

        ShowGarment(0);
        Debug.Log("GarmentSwitcher started. Garment count: " + garments.Length);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextGarment();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousGarment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowGarment(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowGarment(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowGarment(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ShowGarment(3);
        }
    }

    public void NextGarment()
    {
        if (!HasGarments()) return;

        currentIndex = (currentIndex + 1) % garments.Length;
        ShowGarment(currentIndex);
    }

    public void PreviousGarment()
    {
        if (!HasGarments()) return;

        currentIndex = (currentIndex - 1 + garments.Length) % garments.Length;
        ShowGarment(currentIndex);
    }

    public void ShowGarment(int index)
    {
        if (!HasGarments()) return;

        if (index < 0 || index >= garments.Length)
        {
            Debug.LogWarning("Invalid garment index: " + index);
            return;
        }

        currentIndex = index;

        for (int i = 0; i < garments.Length; i++)
        {
            if (garments[i] != null)
            {
                garments[i].SetActive(i == currentIndex);
            }
        }

        Debug.Log("Current garment: " + garments[currentIndex].name);
    }

    private bool HasGarments()
    {
        if (garments == null || garments.Length == 0)
        {
            Debug.LogWarning("No garments assigned.");
            return false;
        }

        return true;
    }

    void OnGUI()
    {
        if (!showHUD || !HasGarments()) return;

        float fps = 1.0f / deltaTime;

        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        string garmentName = garments[currentIndex] != null
            ? garments[currentIndex].name
            : "None";

        GUI.Label(
            new Rect(20, 20, 600, 120),
            "XR Virtual Fitting MVP\n" +
            "Current Garment: " + garmentName + "\n" +
            "FPS: " + Mathf.Ceil(fps).ToString() + "\n" +
            "Controls: ← / → or 1~4",
            style
        );
    }
}