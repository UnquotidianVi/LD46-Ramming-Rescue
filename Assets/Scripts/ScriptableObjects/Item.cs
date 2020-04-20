using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject 
{
    public Sprite sprite;
    public string description;

    public bool consumableItem;
    public float effectOnBloofLoss;
    public float effectOnAwareneess;
    public float effectOnBlood;

    public bool isExplosive;
    public float explosionTime;

    public string effectText;
    public float requiredMetersFromHospital;
}
