using UnityEngine;
using UnityEngine.UI;

public class SwingBar : MonoBehaviour
{
    public GolfBallController ballController;

    private Image swingBar;

    // Start is called before the first frame update
    void Start()
    {
        swingBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        swingBar.enabled = ballController.prepSwing;
        swingBar.fillAmount = ballController.swingStrength;
    }
}
