using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCloudyExpressions : MonoBehaviour
{
    [SerializeField] private Animator CloudyAn;
    [SerializeField] private Image Cloudy;
    [SerializeField] private Sprite CloudyNewExpression;

    void OnEnable()
    {
        Cloudy.sprite = CloudyNewExpression;
        CloudyAn.Play("Cloudy Pop", 0, 0);
    }
}
