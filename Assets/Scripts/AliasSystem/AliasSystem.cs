using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public sealed class AliasSystem : MonoBehaviour
{


    private static AliasSystem s_Instance;

    [SerializeField, InterfaceType(typeof(IAliasEntity))]
    private MonoBehaviour[] _aliasEntities;

    private Dictionary<string, IAliasEntity> _aliasToEntity = new Dictionary<string, IAliasEntity>();

    private Dictionary<AliasEntityTag, List<IAliasEntity>> _tagToEntities = new Dictionary<AliasEntityTag, List<IAliasEntity>>();


    private void Awake()
    {
        if (s_Instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        MakeSingleton();

        for (int i = 0; i < _aliasEntities.Length; i++)
        {
            IAliasEntity temp = _aliasEntities[i].GetComponent<IAliasEntity>();
            AddEntity(temp);
        }

    }



    public void FindAllEntities()
    {

        Scene currentScene = SceneManager.GetActiveScene();

        List<MonoBehaviour> allEntities = new List<MonoBehaviour>();
        foreach (GameObject rootGameObject in currentScene.GetRootGameObjects())
        {
            MonoBehaviour[] subs = rootGameObject.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (MonoBehaviour entity in subs)
            {
                if(entity is IAliasEntity)
                {
                    allEntities.Add(entity);
                }
            }
        }

        _aliasEntities = allEntities.ToArray();

    }


    public void AddEntity(IAliasEntity entity)
    {
        if(_aliasToEntity.ContainsKey(entity.Alias))
        {
            Debug.LogError("Alias should be unique !!! "+" alias : "+entity.Alias);
            return;
        }
        _aliasToEntity.Add(entity.Alias, entity);

        List<IAliasEntity> list = _tagToEntities.GetValueOrDefault(entity.Tag, new List<IAliasEntity>());
        list.Add(entity);
        _tagToEntities[entity.Tag] = list;

    }

    public void RemoveEntity(IAliasEntity entity)
    {
        RemoveEntity(entity.Alias);
    }

    public void RemoveEntity(string alias)
    {
        _aliasToEntity.Remove(alias);
    }


    public E GetEntity<E>(string alias) where E : IAliasEntity
    {
        if( _aliasToEntity.ContainsKey(alias)) return (E)_aliasToEntity[alias];
        Debug.LogError("Alias " + alias + " cannot found !!!");
        return default;
    }


    private void MakeSingleton() { s_Instance = this; }

    public static AliasSystem Instance { get => s_Instance; }


}


public static class AliasSystemExtensions
{

    public static void AddToAliasSystem(this IAliasEntity entity)
    {
        AliasSystem.Instance.AddEntity(entity);
    }


    public static void RemoveFromAliasSystem(this IAliasEntity entity)
    {
        AliasSystem.Instance.RemoveEntity(entity);
    }

    public static void RemoveFromAliasSystem(this string alias)
    {
        AliasSystem.Instance.RemoveEntity(alias);
    }



    public static E GetEntity<E>(this string alias) where E : IAliasEntity
    { 
        return AliasSystem.Instance.GetEntity<E>(alias); 
    }

}
