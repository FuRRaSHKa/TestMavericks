using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlanetGenerator : MonoBehaviour
{
    [SerializeField] private PlanetInput planetInput;

    [SerializeField] private LevelData levelData;
    [SerializeField] private PlanetFacade[] planetPrefabs;

    private List<PlanetFacade> planets;

    private Bounds screenBounds;

    private void Awake()
    {
        InitBounds();
        SpawnPlanets();

        planetInput.SetPlanets(planets);
    }

    private void InitBounds()
    {
        screenBounds = new Bounds(transform.position, new Vector3(10, 10, 10));
    }

    private void SpawnPlanets()
    {
        int count = levelData.PlayerPlanetCount + levelData.PlayerPlanetCount + levelData.EnemyPlanetCount;
        planets = new List<PlanetFacade>(count);

        for (int i = 0; i < count; i++)
        {
            PlanetFacade planet = Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], transform);

            if (PlacePlanet(planet))
            {
                planets.Add(planet);
            }
            else
            {
                Destroy(planet);
            }
        }
    }

    private bool PlacePlanet(PlanetFacade planetFacade, int tryCount = 5)
    {
        Vector3 pos = Vector3.zero;
        float planetRadius = planetFacade.PlanetRadius;

        for (int i = 0; i < tryCount; i++)
        {
            pos.x = Random.Range(screenBounds.min.x, screenBounds.max.x);
            pos.z = Random.Range(screenBounds.min.z, screenBounds.max.z);

            if (CheckPlanetCollusion(pos, planetRadius))
            {
                planetFacade.transform.localPosition = pos;
                return true;
            }    
        }

        return false;
    }

    private bool CheckPlanetCollusion(Vector3 pos, float radius)
    {
        for (int i = 0; i < planets.Count; i++)
        {
            if (radius + planets[i].PlanetRadius > (pos - planets[i].transform.localPosition).magnitude)
            {
                return false;
            }
        }

        return true;
    }
}
