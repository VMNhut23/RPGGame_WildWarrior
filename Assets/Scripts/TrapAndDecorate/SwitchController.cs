using UnityEngine;
using Cinemachine;
using System.Collections;

public class SwitchController : MonoBehaviour
{
    public GameObject door;
    public Animator switchAnimator;
    public CinemachineVirtualCamera doorCamera;
    public CinemachineVirtualCamera playerCamera;
    public float switchAnimationDelay = 1f; 
    public float cameraFocusTime = 2f;  
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            StartCoroutine(ActivateSwitchSequence());
        }
    }

    private IEnumerator ActivateSwitchSequence()
    {
        if (switchAnimator != null)
        {
            yield return new WaitForSeconds(switchAnimationDelay);
            switchAnimator.SetTrigger("Activate");
        }
        yield return FocusOnDoor();
    }

    private System.Collections.IEnumerator FocusOnDoor()
    {
        doorCamera.Priority = 11;
        playerCamera.Priority = 9;

        OpenDoor();

        yield return new WaitForSeconds(cameraFocusTime); 

        playerCamera.Priority = 11;
        doorCamera.Priority = 9;
    }

    private void OpenDoor()
    {
        if (door != null)
        {
            Animator doorAnimator = door.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Open");
                AudioManager.instance.PlaySFX(37, null);
            }
            else
            {
                Destroy(door);
            }
        }
    }
}
