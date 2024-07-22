using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Garden/Flower")]
public class FlowerSO : ScriptableObject
{
    [ColorUsage(true, true)] public List<Color> colorList = new List<Color>();
    public List<Sprite> flowerSprites = new List<Sprite>();
}
