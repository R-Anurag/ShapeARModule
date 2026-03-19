using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShapeSelector : MonoBehaviour
{
    [Header("Shape Toggles")]
    public Toggle sphereToggle;
    public Toggle cubeToggle;
    public Toggle cylinderToggle;
    public Toggle pyramidToggle;

    [Header("Navigation")]
    public Button continueButton;

    void Start()
    {
        // Full reset only on fresh entry (no shape chosen yet)
        if (string.IsNullOrEmpty(ShapeModuleCache.data.shapeName))
        {
            ShapeModuleCache.Reset();
            // Seed cache with whichever toggle is on by default in the scene
            if (sphereToggle.isOn)   ShapeModuleCache.data.shapeName = "ShapeSphere";
            else if (cubeToggle.isOn) ShapeModuleCache.data.shapeName = "ShapeCube";
            else if (cylinderToggle.isOn) ShapeModuleCache.data.shapeName = "ShapeCylinder";
            else if (pyramidToggle.isOn)  ShapeModuleCache.data.shapeName = "ShapePyramid";
        }

        RestoreToggleState();

        sphereToggle.onValueChanged.AddListener(on   => { if (on) ShapeModuleCache.data.shapeName = "ShapeSphere"; });
        cubeToggle.onValueChanged.AddListener(on     => { if (on) ShapeModuleCache.data.shapeName = "ShapeCube"; });
        cylinderToggle.onValueChanged.AddListener(on => { if (on) ShapeModuleCache.data.shapeName = "ShapeCylinder"; });
        pyramidToggle.onValueChanged.AddListener(on  => { if (on) ShapeModuleCache.data.shapeName = "ShapePyramid"; });

        continueButton.onClick.AddListener(OnContinue);
    }

    void RestoreToggleState()
    {
        string saved = ShapeModuleCache.data.shapeName;
        if (string.IsNullOrEmpty(saved)) return;

        sphereToggle.SetIsOnWithoutNotify(saved == "ShapeSphere");
        cubeToggle.SetIsOnWithoutNotify(saved == "ShapeCube");
        cylinderToggle.SetIsOnWithoutNotify(saved == "ShapeCylinder");
        pyramidToggle.SetIsOnWithoutNotify(saved == "ShapePyramid");

        sphereToggle.GetComponent<ToggleSpriteSwap>()?.ForceRefresh(saved == "ShapeSphere");
        cubeToggle.GetComponent<ToggleSpriteSwap>()?.ForceRefresh(saved == "ShapeCube");
        cylinderToggle.GetComponent<ToggleSpriteSwap>()?.ForceRefresh(saved == "ShapeCylinder");
        pyramidToggle.GetComponent<ToggleSpriteSwap>()?.ForceRefresh(saved == "ShapePyramid");
    }

    void OnContinue()
    {
        if (string.IsNullOrEmpty(ShapeModuleCache.data.shapeName))
        {
            Debug.LogWarning("No shape selected.");
            return;
        }
        SceneManager.LoadScene("BehaviorSelectScene");
    }
}
