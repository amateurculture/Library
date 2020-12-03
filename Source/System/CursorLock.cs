using UnityEngine;

public class CursorLock : MonoBehaviour
{
    public bool isLocked;

    void Start()
    {
        Cursor.lockState = (isLocked) ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
