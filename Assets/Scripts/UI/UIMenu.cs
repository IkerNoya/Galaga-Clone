using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField] GameObject levelSelector;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject levelsText;
    public void OnClickPlay()
    {
        if(levelSelector!=null) levelSelector.SetActive(true);
        if(playButton != null) playButton.SetActive(false);
        if(levelsText != null) levelsText.SetActive(true);
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
