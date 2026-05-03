using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    public ComboState CurrCombo;

    [Range(1, 2)] public int Index;

    private void Start()
    {
    }

    public void ChainCombo()
    {

    }

    private void P1Combo()
    {
    }
    private void P2Combo()
    {
    }

    private void ResetCombo()
    {
        CurrCombo.isActive = false;
    }

    private void AdvanceCombo()
    {
        CurrCombo.comboCount++;
        CurrCombo.currentStep++;
    }
    private void StartCombo()
    {
        CurrCombo.isActive = true;
        CurrCombo.comboCount = 1;
        CurrCombo.currentStep = 0;
    }
}

public struct ComboState
{
    public int comboCount;
    public int currentStep;
    public float lastHitBeat;
    public bool isActive;
}
