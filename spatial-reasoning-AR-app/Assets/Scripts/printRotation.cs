using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printRotation : MonoBehaviour
{
  Quaternion previousRotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != previousRotation) {
          Debug.Log(transform.rotation.eulerAngles);
          previousRotation = transform.rotation;
        }
    }
}
