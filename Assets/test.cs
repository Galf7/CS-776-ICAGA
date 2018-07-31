using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public float thrust;
    public Rigidbody rb;
    float h;
    float v;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
         h = Input.GetAxis("Horizontal");
         v = Input.GetAxis("Vertical");
       // Vector3 ver = Camera.main.transform.forward * v;
       // rb.AddForce(Camera.main.transform.forward * v * thrust);
       // rb.AddForce(Camera.main.transform.forward * h * thrust);

        rb.AddForce(0, 0, -thrust*v, ForceMode.Impulse);
        rb.AddForce(-thrust * h, 0, 0, ForceMode.Force);
    }
}
