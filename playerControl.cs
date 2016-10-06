using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

    public float speed;
    public Camera mainCamera;
    private Vector3 cameraFront;
    private Vector3 cameraRight;
    public float rotationSpeed;

    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        cameraRight = mainCamera.transform.TransformDirection(Vector3.right);
        cameraFront = mainCamera.transform.TransformDirection(Vector3.forward);
        cameraRight.y = 0;
        cameraFront.y = 0;
        cameraRight.Normalize();
        cameraFront.Normalize();        
        Vector3 move = -1 * transform.right * Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        transform.position = transform.position + ( speed * move * Time.deltaTime);
        
    }
}
