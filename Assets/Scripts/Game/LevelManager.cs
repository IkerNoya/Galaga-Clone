using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] float PlanetOffset;
    [Space]
    [SerializeField] GameObject bluePlanet;
    [SerializeField] GameObject pinkPlanet;
    [Space]
    public List<Transform> Spawners;

    GameObject player;
    GameObject cam;
    int enemyCount;
    Vector3 camHeight;
    void Start()
    {
        cam = Camera.main.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, cam.transform.position.y + camHeight.y + offset, transform.position.z);

        PlanetParallax();
    }
    void PlanetParallax()
    {
        if (bluePlanet != null && pinkPlanet != null && player != null)
        {
            int blueIndex = Random.Range(0, Spawners.Count);
            int pinkIndex = Random.Range(0, Spawners.Count);
            if (!bluePlanet.GetComponent<Renderer>().isVisible && player.transform.position.y > bluePlanet.transform.position.y)
            {
                if (blueIndex == pinkIndex)
                    blueIndex = Random.Range(0, Spawners.Count);

                bluePlanet.transform.position = new Vector3(Spawners[blueIndex].position.x, Spawners[blueIndex].position.y + Random.Range(0, PlanetOffset), Spawners[blueIndex].position.z);
            }
            if (!pinkPlanet.GetComponent<Renderer>().isVisible && player.transform.position.y > pinkPlanet.transform.position.y)
            {
                if (pinkIndex == blueIndex)
                    pinkIndex = Random.Range(0, Spawners.Count);

                pinkPlanet.transform.position = new Vector3(Spawners[pinkIndex].position.x, Spawners[pinkIndex].position.y + Random.Range(0, PlanetOffset), Spawners[pinkIndex].position.z);
            }
        }
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }
    
}
