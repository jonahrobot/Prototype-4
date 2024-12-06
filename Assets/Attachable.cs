using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Attachable : MonoBehaviour
{
    [SerializeField]
    public UnityEvent stepAction;

    void Start()
    {   
    }

    void Update()
    {
    }
}
