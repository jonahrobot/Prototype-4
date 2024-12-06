using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttachmentSystem : MonoBehaviour
{
    public CinemachineRotationComposer rotationComposer;
    public Transform[] attachmentPoints;

    private Attachable[] attachments;

    void Start()
    {
        attachments = new Attachable[attachmentPoints.Length];
    }

    int GetNextAttachmentPoint(bool full)
    {
        for (int i = 0; i < attachmentPoints.Length; i++)
        {
            if ((attachments[i] == null) ^ full)
            {
                return i;
            }
        }
        return -1;
    }

    void Dettach(Attachable attachable)
    {
        for (int i = 0; i < attachments.Length; i++)
        {
            if(attachments[i] == attachable)
            {
                Dettach(i);
                return;
            }
        }
    }

    void Dettach(int index)
    {
        if (index >= 0 && index < attachments.Length)
        {
            Rigidbody rb = attachments[index].GetComponent<Rigidbody>();
            rb.linearVelocity = attachments[index].transform.rotation * new Vector3(0, 5, 0);
            rb.angularVelocity = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
            attachments[index] = null;
        }
    }

    void Attach(int index, Attachable attachable)
    {
        if (index >= 0 && index < attachments.Length && !attachments.Contains(attachable))
        {
            attachments[index] = attachable;
        }
    }

    public void Step(int index)
    {
        if(index >= 0 && index < attachments.Length && attachments[index] != null)
        {
            attachments[index].stepAction.Invoke();
        }
    }

    void Update()
    {
        for(int i = 0; i < attachmentPoints.Length; i++)
        {
            if (attachments[i] != null)
            {
                attachments[i].transform.position = attachmentPoints[i].transform.position;
                attachments[i].transform.rotation = attachmentPoints[i].transform.rotation;
                Rigidbody rb = attachments[i].GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Dettach(GetNextAttachmentPoint(true));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Attachable attachable = other.gameObject.GetComponent<Attachable>();
        if (attachable != null)
        {
            Attach(GetNextAttachmentPoint(false), attachable);
        }
    }
}
