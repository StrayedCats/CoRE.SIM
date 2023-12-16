using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideGameUI : MonoBehaviour
{
    public Image[] images;

    public void hideImage(bool isHide)
    {
        foreach (var image in images)
        {
            if (image == null) { continue; }
            if (isHide)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.1f);
            }
            else
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
        }
    }
}
