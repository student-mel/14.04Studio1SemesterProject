using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    public ComboState CurrCombo;

    [Range(1, 2)] public int Index;

    private void Start()
    {
        CombatResolver.i.OnCombatResolved += ChainCombo;
    }

    public void ChainCombo(CombatResult result)
    {

        switch (Index)
        {
            case 1:
                P1Combo(result);
                Debug.Log($"Player 1 Combo: {CurrCombo.comboCount}");
                break;
            case 2:
                P2Combo(result);
                Debug.Log($"Player 2 Combo: {CurrCombo.comboCount}");
                break;
            default:
                break;
        }

    }

    private void P1Combo(CombatResult result)
    {
        if (!result.p2Hit)
        {
            ResetCombo();
            return;
        }
        if (result.p1Hit)
        {
            ResetCombo();
            return;
        }
        if (IsComboValid(result))
        {
            AdvanceCombo();
        }
        else
        {
            StartCombo();
        }
    }
    private void P2Combo(CombatResult result)
    {
        if (!result.p1Hit)
        {
            ResetCombo();
            return;
        }
        if (result.p2Hit)
        {
            ResetCombo();
            return;
        }
        if (IsComboValid(result))
        {
            AdvanceCombo();
        }
        else
        {
            StartCombo();
        }
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

    private bool IsComboValid(CombatResult result)
    {
        return false;
    }
}

public struct ComboState
{
    public int comboCount;
    public int currentStep;
    public float lastHitBeat;
    public bool isActive;
}
