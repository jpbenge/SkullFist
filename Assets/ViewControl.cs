using UnityEngine;
using System.Collections;





//[ExecuteInEditMode]
public class ViewControl : MonoBehaviour {



    private Quaternion worldRotation;
    public float rotationPosition;
    public float cameraHieght;
    public float rotation;
    public float factor;
    public float speed;
    public float heightscale;
    public float rotscale;
    public Vector3 basePos;


	// Use this for initialization
	void Start () {
        basePos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        factor = rotation * speed;

       // yRotation += Input.GetAxis("Horizontal");

        cameraHieght = -404 + (factor * heightscale);
        rotationPosition = factor * rotscale;
        transform.eulerAngles = new Vector3(0, 0, rotationPosition);
        transform.position = new Vector3(transform.position.x, cameraHieght, transform.position.z);
        //worldRotation.eulerAngles.Set(0f,rotationPosition,0f);
       // transform.rotation..eulerAngles.Set(0,rotationPosition,0);
       // transform.Rotate(rotationPosition, 0, 0);
	}
}
