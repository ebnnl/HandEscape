
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ButtonTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    private bool pressedInProgress = false;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("ButtonActivator") && !pressedInProgress)
        {
            pressedInProgress = true;
            onButtonPressed?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("ButtonActivator"))
        {
            pressedInProgress = false;
            onButtonReleased?.Invoke();
        }
    }
}
