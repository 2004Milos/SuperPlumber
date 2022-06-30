using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform PrviSlojZemlje;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.WorldToViewportPoint(PrviSlojZemlje.position).y > 0.64)
        {
            cam.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            FindObjectOfType<Movement>().enabled = true;
            this.GetComponent<CameraMovement>().enabled = false;
        }
    }
}
