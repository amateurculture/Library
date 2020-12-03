using UnityEngine;

public class FacingSign : MonoBehaviour
{
	void FixedUpdate ()
	{
        if (Time.frameCount % 3 == 0) {
            transform?.LookAt(Camera.main?.transform);
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
        }
    }
}
