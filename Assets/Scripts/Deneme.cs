using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme :  MonoBehaviour, IAliasEntity
{

    [SerializeField] private string _alias;
    [SerializeField] private AliasEntityTag _tag;

    public string Alias { get => _alias; set => _alias = value; }
    public AliasEntityTag Tag { get => _tag; set => _tag = value; }



}
