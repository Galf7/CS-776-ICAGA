using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {

    private AudioSource source;
    // Use this for initialization
    void Start () {
   
}
    void Awake()
    {

        source = GetComponent<AudioSource>();

    }
    public static class Globals
    {
        public static int coll = 0; // Modifiable in Code
        
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collieded");
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        
        Globals.coll = 1;
        float v = Input.GetAxis("Horizontal");
        float h = -Input.GetAxis("Vertical");
        Vector3 bar = -Camera.main.transform.forward * v;
        bar.y = 0;
        //bar.x = 0;
        Vector3 hor = -Camera.main.transform.right * h;
        hor.y = 0;
        //hor.z = 0;
        
        //rigidbody.AddForce(-Camera.main.transform.forward * 100.0f);
        GameObject.Find("MCSMaleLite").transform.position += bar *0.1f;
        GameObject.Find("MCSMaleLite").transform.position += hor * 0.1f;
    }
    void OnCollisionStay(Collision col)
    {
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
        Debug.Log("Collision stay");
        float v = Input.GetAxis("Horizontal");
        float h = -Input.GetAxis("Vertical");
        Vector3 bar = -Camera.main.transform.forward * v;
        bar.y = 0;
        //bar.x = 0;
        Vector3 hor = -Camera.main.transform.right * h;
        hor.y = 0;
        //hor.z = 0;

        //rigidbody.AddForce(-Camera.main.transform.forward * 100.0f);
       // GameObject.Find("MCSMaleLite").transform.position += bar * 0.3f;
       // GameObject.Find("MCSMaleLite").transform.position += hor * 0.3f;
    }
    void OnCollisionExit(Collision col)
    {
       // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
        Debug.Log("Collision exit");
        Globals.coll = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float v = Input.GetAxis("Horizontal");
        float h = -Input.GetAxis("Vertical");


        Vector3 bar = Camera.main.transform.forward * v * 0.1f;
        bar.y = 0;
        //bar.x = 0;
        Vector3 hor = Camera.main.transform.right * h * 0.1f;
        hor.y = 0;
        //hor.z = 0;
        if (Globals.coll == 1)
        {
           // GameObject.Find("MCSMaleLite").transform.position += bar * -10.0f;
           // GameObject.Find("MCSMaleLite").transform.position += hor * -10.0f;
        }
        else
        {

            

            GameObject.Find("MCSMaleLite").transform.position += bar;
            GameObject.Find("MCSMaleLite").transform.position += hor;
            //transform.localPosition += new Vector3(-h * 0.1f, 0f, -v * 0.1f);
        }
    }
}
