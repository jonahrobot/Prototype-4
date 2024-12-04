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
            attachments[index] = null;
        }
    }

    void Attach(int index, Attachable attachable)
    {
        if (index >= 0 && index < attachments.Length)
        {
            attachments[index] = attachable;
        }
    }

    void Step(int index)
    {
        if(index >= 0 && index < attachments.Length && attachments[index] != null)
        {
            attachments[index].stepAction();
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
            }
        }

        Attachable[] attachables = FindObjectsByType<Attachable>(FindObjectsSortMode.None);
        foreach(Attachable attachable in attachables)
        {
            if(!attachments.Contains(attachable))
            {
                if((attachable.transform.position - transform.position).sqrMagnitude < 4)
                {
                    Attach(GetNextAttachmentPoint(false), attachable);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Dettach(GetNextAttachmentPoint(true));
        }
    }
}
