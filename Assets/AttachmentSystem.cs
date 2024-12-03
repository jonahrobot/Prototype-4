using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttachmentSystem : MonoBehaviour
{
    public CinemachineRotationComposer rotationComposer;
    public Transform[] attachmentPoints;

    private Attachable[] attachments;
    private bool isSelecting = false;
    private Attachable selectedObject = null;

    void Start()
    {
        attachments = new Attachable[attachmentPoints.Length];
    }

    int GetBestAttachmentPoint(Vector3 position)
    {
        int bestAttachmentPoint = -1;
        float bestSqDistance = 1;
        for (int i = 0; i < attachmentPoints.Length; i++)
        {
            if (attachments[i] == null)
            {
                float sqDist = (attachmentPoints[i].transform.position - position).sqrMagnitude;
                if (sqDist < bestSqDistance)
                {
                    bestSqDistance = sqDist;
                    bestAttachmentPoint = i;
                }
            }
        }
        return bestAttachmentPoint;
    }

    void Detatch(Attachable attachable)
    {
        for (int i = 0; i < attachments.Length; i++)
        {
            if(attachments[i] == attachable)
            {
                attachments[i] = null;
                return;
            }
        }
    }

    void Attach(int index, Attachable attachable)
    {
        attachments[index] = attachable;
    }

    void Update()
    {
        for(int i = 0; i < attachmentPoints.Length; i++)
        {
            if (attachments[i] != null)
            {
                attachments[i].transform.position = attachmentPoints[i].transform.position;
            }
        }

        if(isSelecting)
        {
            if (Input.GetMouseButton(0))
            {
                if(selectedObject == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        selectedObject = hit.transform.GetComponent<Attachable>();
                        Detatch(selectedObject);
                    }
                }
                else
                {
                    float dist = (Camera.main.transform.position - transform.position).magnitude;
                    Vector3 goalPoint = Camera.main.transform.position + Camera.main.transform.forward * dist;
                    int bestAttachmentPoint = GetBestAttachmentPoint(goalPoint);
                    if(bestAttachmentPoint >= 0)
                    {
                        goalPoint = attachmentPoints[bestAttachmentPoint].position;
                    }
                    selectedObject.transform.position = goalPoint;
                }
            }
            else if (selectedObject != null)
            {
                int bestAttachmentPoint = GetBestAttachmentPoint(selectedObject.transform.position);
                if (bestAttachmentPoint >= 0)
                {
                    Attach(bestAttachmentPoint, selectedObject);
                }
                selectedObject = null;
                isSelecting = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSelecting = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isSelecting = true;
            }
        }
        rotationComposer.enabled = !isSelecting;
    }
}
