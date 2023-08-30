using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������HMD��ǰ����Main camera�ķ���Ͷ������.
public class HMDForwordRay : MonoBehaviour
{
    Transform trackedHMD;
    // Start is called before the first frame update
    void Start()
    {
        trackedHMD = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��FixedUpdate��Ͷ������.
    private void FixedUpdate()
    {
        Ray hmdray = new Ray(trackedHMD.position, trackedHMD.forward);
        Physics.Raycast(hmdray, 100);  //Ĭ�ϻᴥ��trigger.
    }
}
