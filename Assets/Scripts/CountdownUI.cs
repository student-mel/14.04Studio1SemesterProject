using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    public TMP_Text countdownText;

    public IEnumerator PlayCountdown()
    {
        countdownText.gameObject.SetActive(true);

        countdownText.text = "Ready";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        /*yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "FIGHT!";*/
        yield return new WaitForSeconds(0.5f);

        countdownText.gameObject.SetActive(false);
    }
}
