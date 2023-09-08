using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������HMD��ǰ����Main camera�ķ���Ͷ������. ʶ���������ĸ����ʿ鲢�ҵ���WordCubes�Ŀ��ƺ���.
public class HMDForwordRay : MonoBehaviour
{
    Transform trackedHMD;
    public WordCubes wordCubes;
    // Start is called before the first frame update
    void Start()
    {
        trackedHMD = Camera.main.transform;
        wordCubes = GameObject.Find("wordcubes").GetComponent<WordCubes>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��FixedUpdate��Ͷ������.
    private void FixedUpdate()
    {
        Ray hmdray = new Ray(trackedHMD.position, trackedHMD.forward);
        RaycastHit raycasthit;
        //Debug.DrawRay(trackedHMD.position, trackedHMD.forward * 100, Color.red);
        if(Physics.Raycast(hmdray, out raycasthit, 100)){   //Ĭ�ϻᴥ��trigger.
            if(raycasthit.collider.name.Substring(0, raycasthit.collider.name.Length-1)=="wordcube"){
                //Debug.LogWarning("hit " + raycasthit.collider.name);
                int index = raycasthit.collider.name[raycasthit.collider.name.Length-1] - '1';
                //Debug.LogWarning("index = " + index.ToString());
                wordCubes.selectbyRay(index);
            }
        }
        else{
            wordCubes.selectbyRay(-1);
        }
    }
}
