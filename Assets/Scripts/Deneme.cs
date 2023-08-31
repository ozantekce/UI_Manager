using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deneme :  MonoBehaviour, IAliasEntity
{

    [SerializeField] private string _alias;

    public string Alias { get => _alias; set => _alias = value; }





    private void Start()
    {

    }

}
