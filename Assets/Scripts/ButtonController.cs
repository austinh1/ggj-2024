using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private Transform button;
    private Vector3 pressedPos;
    private const float pushSpeed = 1f;
    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        button = transform.Find("Button");
        pressedPos = button.position + new Vector3(0f, -0.18f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed && !button.transform.position.Equals(pressedPos))
        {
            float step = pushSpeed * Time.deltaTime;
            button.transform.position = Vector3.MoveTowards(button.transform.position, pressedPos, step);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!pressed && other.tag == "Player")
        {
            pressed = true;
            var spawner = GetComponent<Spawner>();
            if (spawner != null)
            {
                spawner.active = true;
            }

            var buttonObjective = ObjectiveController.Instance().GetObjective(ObjectiveType.PressButton);
            buttonObjective.Increment();
        }
    }
}
