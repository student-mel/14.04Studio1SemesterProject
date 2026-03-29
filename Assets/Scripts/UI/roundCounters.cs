using UnityEngine;

public class roundCounters : MonoBehaviour
{
    public GameObject[] roundIndicators;
    private bool[] roundsWon = { false, false };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject indicator in roundIndicators)
        {
            indicator.SetActive(false);
        }
    }

    public void WinRound()
    {
        if (roundsWon[0] == false)
        {
            roundIndicators[0].SetActive(true);
            roundsWon[0] = true;
        }

        else
        {
            roundIndicators[1].SetActive(true);
            roundsWon[1] = true;
            //You can put your game win call here or just call this from another script that handles it
        }

    }
}
