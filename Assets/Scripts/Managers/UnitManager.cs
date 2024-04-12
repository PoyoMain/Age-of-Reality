using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    /// <summary>
    /// Hero positions on grid
    /// </summary>
    private readonly Vector2[] _HeroPositions = new Vector2[]
    {
        new Vector2(0, 2),
        new Vector2(0, 4),
        new Vector2(0, 0),
        new Vector2(0, 6),
        new Vector2(0, 4),
    };

    /// <summary>
    /// Enemy positions on grid
    /// </summary>
    private readonly Vector2[] _EnemyPositions = new Vector2[]
    {
        new Vector2(8, 2),
        new Vector2(8, 4),
        new Vector2(8, 0),
        new Vector2(8, 6),
        new Vector2(8, 4),
    };

    /// <summary>
    /// Spawn Heroes
    /// </summary>
    /// <param name="heroes">The types of heroes to spawn</param>
    /// <returns>A list of the spawned heroes</returns>
    public List<HeroUnitBase> SpawnHeroes(List<ScriptableHero> heroes)
    {
        List<HeroUnitBase> heroUnits = new();

        for (int i = 0; i < heroes.Count; i++)
        {
            Vector2Int gridPos = Vector2Int.RoundToInt(_HeroPositions[i]);
            heroUnits.Add(SpawnHeroUnit(heroes[i].name, GridInfo.FindGridSpot(gridPos)));
        }
        
        return heroUnits;
    }

    /// <summary>
    /// Spawn Enemies
    /// </summary>
    /// <param name="heroes">The types of enemies to spawn</param>
    /// <returns>A list of the spawned enemies</returns>
    public List<EnemyUnitBase> SpawnEnemies(List<ScriptableEnemy> enemies)
    {
        List<EnemyUnitBase> enemyUnits = new();

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector2Int gridPos = Vector2Int.RoundToInt(_EnemyPositions[i]);
            enemyUnits.Add(SpawnEnemyUnit(enemies[i].name, GridInfo.FindGridSpot(gridPos)));
        }

        return enemyUnits;
    }

    private HeroUnitBase SpawnHeroUnit(string name, GridSpot gridSpot)
    {
        ScriptableHero heroScriptable = ResourceStorage.Instance.GetHero(name);

        Vector3 pos = new(gridSpot.WorldPosition.x, gridSpot.WorldPosition.y, 0);
        HeroUnitBase heroUnit = Instantiate(heroScriptable.Prefab, pos, Quaternion.identity, transform) as HeroUnitBase;

        heroUnit.data = heroScriptable;
        HeroStats stats = heroScriptable.BaseStats;
        heroUnit.InitStats(stats);
        

        return heroUnit;
    }

    private EnemyUnitBase SpawnEnemyUnit(string name, GridSpot gridSpot)
    {
        ScriptableEnemy enemyScriptable = ResourceStorage.Instance.GetEnemy(name);

        Vector3 pos = new(gridSpot.WorldPosition.x, gridSpot.WorldPosition.y, 0);
        EnemyUnitBase enemyUnit = Instantiate(enemyScriptable.Prefab, pos, Quaternion.identity, transform) as EnemyUnitBase;

        EnemyStats stats = enemyScriptable.BaseStats;
        enemyUnit.InitStats(stats);
        enemyUnit.data = enemyScriptable;

        return enemyUnit;
    }

}
