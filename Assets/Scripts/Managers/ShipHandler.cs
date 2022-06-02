using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipHandler : MonoBehaviour
{
    public static ShipHandler Instance;

    [SerializeField] private ShipPool shipPool;

    private List<ShipFacade> playerShips;
    private List<ShipFacade> enemyShips;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public bool TryDuelShips(int planetID)
    {
        ShipFacade playerShip = playerShips.DefaultIfEmpty(null).First(f => f.IsWithPlanet(planetID));
        ShipFacade enemyShip = enemyShips.DefaultIfEmpty(null).First(f => f.IsWithPlanet(planetID));

        if (playerShip != null && enemyShip != null)
        {
            return false;
        }

        enemyShips.Remove(enemyShip);
        playerShips.Remove(playerShip);

        playerShip.enabled = false;
        enemyShip.enabled = false;

        return true;
    }

    public void IncreaseShipCount(PlanetFacade planetFacade, int shipCount, ShipSide shipSide)
    {
        for (int i = 0; i < shipCount; i++)
        {
            ShipFacade ship = shipPool.SpawnShip().GetComponent<ShipFacade>();
            ship.Init(shipSide, planetFacade); 

            if (shipSide == ShipSide.Player)
            {
                playerShips.Add(ship);
            }
            else
            {
                enemyShips.Add(ship);
            }
        }
    }

    public void SendPlayerShips(int planetID, PlanetFacade planetFacade)
    {
        IEnumerable enumerator = playerShips.Where(w => w.IsWithPlanet(planetID));

        foreach (ShipFacade ship in enumerator)
        {
            ship.SendTo(planetFacade);
        }
    }
}
