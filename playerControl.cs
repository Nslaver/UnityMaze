using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    public float speed;
    public Camera mainCamera;
    private Vector3 cameraFront;
    private Vector3 cameraRight;
    public float rotationSpeed;
    private int health;
    private int objectives;
    public Rigidbody rb;
    void Start()
    {
        health = 100;
        objectives = 0;
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

    void OnGUI()
    {
        GUI.Box(new Rect(5, 5, 80, 40), "H:" + health + "/100 \n"+
            "O: " + objectives + "/3");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "vidas")
        {
            Destroy(col.gameObject);
            objectives++;
            if (objectives == 3){
                SceneManager.LoadScene("Ganaste", LoadSceneMode.Single);
            }
        }
    }
}
