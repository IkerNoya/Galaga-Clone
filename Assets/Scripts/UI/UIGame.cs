using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGame : MonoBehaviour
{
    public TextMeshProUGUI score;
    GameManager manager;
    private void Start()
    {
        manager = GameManager.instance;
    }
    void Update()
    {
        score.text = "SCORE: " + manager.GetScore().ToString();
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
