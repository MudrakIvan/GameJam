using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgorundLoop : MonoBehaviour
{
    public GameObject[] levels;

    public float choke;

    private Camera mainCamera;

    private Vector2 screenBounds;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        foreach (var obj in levels)
        {
            LoadChildObjects(obj);
        }
    }

    private void LoadChildObjects(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth) + 1; // at least 2 objects are needed
        GameObject clone = Instantiate(obj);

        for (int i = 0; i < childNeeded; i++)
        {
            GameObject child = Instantiate(clone);
            Destroy(child.GetComponent<SpriteMover>());
            child.transform.SetParent(obj.transform);
            child.transform.position = new Vector3(objectWidth * i, 0, 0) + obj.transform.position;
            child.name = obj.name + i;
        }

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void LateUpdate()
    {
        foreach (var obj in levels)
        {
            RepositionChildObjects(obj);
        }
    }

    void RepositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();

        if (children.Length <= 1)
        {
            return;
        }

        GameObject firstChild = children[1].gameObject; // index 0 is parent
        GameObject lastChild = children[children.Length - 1].gameObject;

        float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;

        if (transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth)
        {
            firstChild.transform.SetAsLastSibling();
            firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            return;
        }
        if (transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjectWidth)
        {
            lastChild.transform.SetAsFirstSibling();
            lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
        }
    }

}
