using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.position += Time.deltaTime * Vector3.forward * 10f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.position += Time.deltaTime * Vector3.back * 10f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position += Time.deltaTime * Vector3.left * 10f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position += Time.deltaTime * Vector3.right * 10f;
        }
    }
}
