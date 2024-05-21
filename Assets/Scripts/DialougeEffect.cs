using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Dialogue Effect", menuName = "DialogueEffect")]
public class DialogueEffect : ScriptableObject
{
    public Sprite portraitSprite = null;
    public string dialougeText = "";
    public float atkBoost = 0;
    public float speedBoost = 1;
    public float atkSpeedBoost = 0.25f;
    public float knockbackBoost = 0;
    public float duration = 3f;
}
