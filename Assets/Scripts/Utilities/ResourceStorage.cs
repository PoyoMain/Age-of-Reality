using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceStorage : Singleton<ResourceStorage>
{
    // Stores reference to all heroes
    public List<ScriptableHero> Heroes {  get; private set; }
    private Dictionary<string, ScriptableHero> _HeroesDict;

    // Stores reference to all enemies
    public List<ScriptableEnemy> Enemies { get; private set; }
    private Dictionary<string, ScriptableEnemy> _EnemyDict;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    /// <summary>
    /// Loads all scriptable assets into respective dictionaries
    /// </summary>
    private void AssembleResources()
    {
        Heroes = Resources.LoadAll<ScriptableHero>("Heroes").ToList();
        _HeroesDict = Heroes.ToDictionary(x => x.name, x => x);

        Enemies = Resources.LoadAll<ScriptableEnemy>("Enemies").ToList();
        _EnemyDict = Enemies.ToDictionary(x => x.name, x => x);
    }

    /// <summary>
    /// Grabs a specified hero from the dictionary
    /// </summary>
    /// <param name="name">The name of the hero to search for</param>
    /// <returns>The hero searched for</returns>
    public ScriptableHero GetHero(string name)
    {
        return _HeroesDict[name];
    }

    /// <summary>
    /// Grabs a specified enemy from the dictionary
    /// </summary>
    /// <param name="name">The name of the enemy to search for</param>
    /// <returns>The enemy searched for</returns>
    public ScriptableEnemy GetEnemy(string name)
    {
        return _EnemyDict[name];
    }
}