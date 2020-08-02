using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Previewer : MonoBehaviour
{
    public static Previewer instance;

    public GameObject[] previewBlocks;

    GameObject currentActive;

    private void Awake()
    {
        instance = this;
    }

    public void ShowPreview(int index)
    {
        Destroy(currentActive);

        currentActive = Instantiate(previewBlocks[index], transform.position, Quaternion.identity) as GameObject;
    }
}
