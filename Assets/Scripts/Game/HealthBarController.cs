using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] GameObject[] lifes;
    [SerializeField] PlayerController player;
    void Update()
    {
        if(player!=null)
        {
            for (int i = 0; i < player.GetMaxLives(); i++)
            {
                if (i <= player.GetLives() - 1 && lifes[i] != null)
                    lifes[i].SetActive(true);
                else if (i > player.GetLives() - 1 && lifes[i] != null)
                    lifes[i].SetActive(false);
            }
        }
    }
}