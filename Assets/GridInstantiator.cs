using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstantiator : MonoBehaviour
{
    public GameObject smallLinePrefab;
    public GameObject largeLinePrefab;
    public Canvas canvas;
    public int numberOfLines = 10;
    public float smallLineSpacing = 1.0f;
    public float largeLineSpacing = 3.0f;
    public float yOffset = 0.0f;

    void Start()
    {
        float totalWidth = 0f;
        bool isSmallLine = true;

        for (int i = 0; i < numberOfLines; i++)
        {
            GameObject linePrefab = isSmallLine ? smallLinePrefab : largeLinePrefab;
            float spacing = isSmallLine ? smallLineSpacing : largeLineSpacing;


            GameObject line = Instantiate(linePrefab, new Vector3(totalWidth, yOffset, 0f), Quaternion.identity, canvas.transform);


            totalWidth += spacing;


            if (isSmallLine)
            {
                if ((i + 1) % 4 == 0)
                {
                    isSmallLine = false;
                }
            }
            else
            {

                isSmallLine = true;
            }
        }

        canvas.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-33.69f, yOffset);
    }
}