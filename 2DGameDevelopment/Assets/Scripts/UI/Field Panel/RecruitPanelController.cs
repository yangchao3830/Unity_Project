using System;
using TMPro;
using UnityEngine.UI;

public class RecruitPanelController : PanelController
{
    [Header("Recruit Panel")]
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public override Type PanelActionType => typeof(RecruitAction);

    public override void SetupPanel(ActionBase actionBase)
    {
        base.SetupPanel(actionBase);
        RecruitAction recruitAction = (RecruitAction)actionBase;
        npcNameText.text = recruitAction.CurrentCharacter.name;
        LevelText.text = recruitAction.CurrentCharacter.BaseLevel.ToString();
        characterImage.sprite = recruitAction.CurrentCharacter.Portrait;

        BindButtons();
        SetDefaultSelection();
    }

    private void BindButtons()
    {
        RebindButtons(confirmButton, OnCancel);
        RebindButtons(cancelButton, OnCancel);
    }
}
