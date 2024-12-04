using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Attachable : MonoBehaviour
{
    public UnityAction stepAction;

    void Start()
    {   
    }

    void Update()
    {
    }
}
