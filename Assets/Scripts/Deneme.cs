using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme :  MonoBehaviour, IAliasEntity
{

    [SerializeField] private string _alias;

    public string Alias { get => _alias; set => _alias = value; }



}
