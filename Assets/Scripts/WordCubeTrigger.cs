using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �����Ǵ�����������һ���������ڵ���cube�㼶�Ŀ��������б����ʿ����Ϣ...
public class WordCubeTrigger : MonoBehaviour
{
    public int index;
    public Color selectedColor = new Color(255, 255, 0, 30);  //��ѡ�еĸ�����ɫ.
    WordCubes wordCubes;
    Color oldColor;
    MeshRenderer meshRenderer;
    TextMeshProUGUI selfWord;
    // Start is called before the first frame update
    void Start()
    {
        wordCubes = transform.parent.GetComponent<WordCubes>();
        index = gameObject.name[gameObject.name.Length - 1] - '1';
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        oldColor = meshRenderer.material.color;
        selfWord = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void highlight(bool on){
        // �������û�е��ʣ�������β�����!
        if(on && selfWord.text.Length > 0)
            meshRenderer.material.color = selectedColor;
        else
            meshRenderer.material.color = oldColor;
    }



    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.LogWarning("WordCube Triggered!");
    //     if(wordCubes.getSelectedIndex() != index)  
    //         wordCubes.setSelectedIndex(index);
    //     // ����
    //     //if(selfWord.text.Length>0)
    //         meshRenderer.material.color = selectedColor;
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //     Debug.LogWarning("WordCube not Triggered!");
    //     if(wordCubes.getSelectedIndex() == index)
    //         wordCubes.setSelectedIndex(-1);
    //     // ȡ������.
    //     //if(meshRenderer.material.color != oldColor)
    //         meshRenderer.material.color = oldColor;
    // }
}
