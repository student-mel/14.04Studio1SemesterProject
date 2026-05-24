using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text tutorialText;

    private TutorialStep currentStep;

    private void OnEnable()
    {
        EventBus.Subscribe("p1_dirinput_vector", OnMove);
        EventBus.Subscribe("p1_jump", OnJump);
        EventBus.Subscribe("tutorial_crouch", OnCrouch);

        EventBus.Subscribe("p1_attack", OnAttack);

        EventBus.Subscribe("p1_block", OnBlock);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("p1_dirinput_vector", OnMove);
        EventBus.Unsubscribe("p1_jump", OnJump);
        EventBus.Unsubscribe("tutorial_crouch", OnCrouch);

        EventBus.Unsubscribe("p1_attack", OnAttack);

        EventBus.Unsubscribe("p1_block_end", OnBlock);
    }

    private void Start()
    {
        currentStep = TutorialStep.Move;
        UpdateTutorialText();
    }

    void OnMove(object obj)
    {
        if (currentStep != TutorialStep.Move)
            return;

        Vector2 input = (Vector2)obj;

        if (Mathf.Abs(input.x) > 0.1f)
        {
            NextStep();
        }
    }

    void OnJump(object obj)
    {
        if (currentStep != TutorialStep.Jump)
            return;

        NextStep();
    }

    void OnCrouch(object obj)
    {
        if (currentStep != TutorialStep.Crouch)
            return;

        NextStep();
    }

    void OnAttack(object obj)
    {
        Moveset move = obj as Moveset;

        if (move == null)
            return;

        switch (currentStep)
        {
            case TutorialStep.LightAttack:
                if (move.Name == "Light Attack")
                    NextStep();
                break;

            case TutorialStep.MediumAttack:
                if (move.Name == "Medium Attack")
                    NextStep();
                break;

            case TutorialStep.HeavyAttack:
                if (move.Name == "Heavy Attack")
                    NextStep();
                break;

            case TutorialStep.JumpLightAttack:
                if (move.Name == "Light Attack")
                    //if (move.Name == "Jump Light Attack")
                    NextStep();
                break;

            case TutorialStep.JumpMediumAttack:
                if (move.Name == "Medium Attack")
                    //if (move.Name == "Jump Medium Attack")
                    NextStep();
                break;

            case TutorialStep.JumpHeavyAttack:
                if (move.Name == "Heavy Attack")
                    //if (move.Name == "Jump Heavy Attack")
                    NextStep();
                break;

            case TutorialStep.CrouchLightAttack:
                if (move.Name == "Light Attack")
                    //if (move.Name == "Crouch Light Attack")
                    NextStep();
                break;

            case TutorialStep.CrouchMediumAttack:
                if (move.Name == "Medium Attack")
                    //if (move.Name == "Crouch Medium Attack")
                    NextStep();
                break;

            case TutorialStep.CrouchHeavyAttack:
                if (move.Name == "Heavy Attack")
                    //if (move.Name == "Crouch Heavy Attack")
                    NextStep();
                break;
        }
    }

    void OnBlock(object obj)
    {
        if (currentStep != TutorialStep.Block)
            return;

        NextStep();
    }

    void NextStep()
    {
        currentStep++;

        if (currentStep > TutorialStep.Complete)
            currentStep = TutorialStep.Complete;

        UpdateTutorialText();
    }

    void UpdateTutorialText()
    {
        switch (currentStep)
        {
            case TutorialStep.Move:
                tutorialText.text = "Move Left or Right";
                break;

            case TutorialStep.Jump:
                tutorialText.text = "Jump";
                break;

            case TutorialStep.Crouch:
                tutorialText.text = "Crouch";
                break;

            case TutorialStep.LightAttack:
                tutorialText.text = "Use a Light Attack";
                break;

            case TutorialStep.MediumAttack:
                tutorialText.text = "Use a Medium Attack";
                break;

            case TutorialStep.HeavyAttack:
                tutorialText.text = "Use a Heavy Attack";
                break;

            case TutorialStep.Block:
                tutorialText.text = "Block";
                break;

            case TutorialStep.JumpLightAttack:
                tutorialText.text = "Perform a Jump Light Attack";
                break;

            case TutorialStep.JumpMediumAttack:
                tutorialText.text = "Perform a Jump Medium Attack";
                break;

            case TutorialStep.JumpHeavyAttack:
                tutorialText.text = "Perform a Jump Heavy Attack";
                break;

            case TutorialStep.CrouchLightAttack:
                tutorialText.text = "Perform a Crouch Light Attack";
                break;

            case TutorialStep.CrouchMediumAttack:
                tutorialText.text = "Perform a Crouch Medium Attack";
                break;

            case TutorialStep.CrouchHeavyAttack:
                tutorialText.text = "Perform a Crouch Heavy Attack";
                break;

            case TutorialStep.Complete:
                tutorialText.text = "Tutorial Complete!";
                break;
        }
    }
}
