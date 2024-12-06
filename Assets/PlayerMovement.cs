using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController characterController;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public Transform cameraMain; 
    float turnSmoothVelocity;

    [Header("Beat Stats")]
    public float beatRateInSeconds = 1.0f;
    [Range(0f, 10f)]
    public float beatRateMin = 0.2f;
    [Range(0f, 10f)]
    public float beatRateMax = 1f;

    int beatTrack = 1;

    private AttachmentSystem coreAttachment;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        coreAttachment = GetComponent<AttachmentSystem>();
        StartCoroutine("beat");
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraMain.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }


        // Pulse
        if (transform.localScale.x > 1 && transform.localScale.y > 1 && transform.localScale.z > 1)
        {
            transform.localScale = new Vector3(transform.localScale.x - 1f * Time.deltaTime, transform.localScale.y - 1f * Time.deltaTime, transform.localScale.z - 1f * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    IEnumerator beat()
    {
        yield return new WaitForSeconds(beatRateInSeconds);

        //// Play sound main
        coreAttachment.Step(1);

        //// Play every other beat
        if(beatTrack % 2 == 0) coreAttachment.Step(2);

        //// Play every fourth beat
        if (beatTrack % 4 == 0) coreAttachment.Step(0);

        beatTrack += 1;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        StartCoroutine("beat");
    }
}
