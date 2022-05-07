using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BackgroundImages { Home, Reward };    

public class BackgroundManager : MonoBehaviour
{
    public Image Background;

    public Sprite HomeImage;
    public Sprite RewardImage;

    public void SetImage(BackgroundImages whichImage)
    {
        if(whichImage == BackgroundImages.Home) {
            Background.sprite = HomeImage;
        } else {
            Background.sprite = RewardImage;
        }
    }

}
