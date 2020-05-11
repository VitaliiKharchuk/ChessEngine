using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScript : MonoBehaviour
{
    public void SwitchToWhiteVsEngineScene()
    {
        SceneManager.LoadScene("WhiteVsEngineScene");
    }

    public void SwitchToBlackVsEngineScene()
    {
        SceneManager.LoadScene("BlackVsEngineScene");
    }

    public void SwitchToTwoPlayersScene()
    {
        SceneManager.LoadScene("TwoPlayersScene");
    }
}
