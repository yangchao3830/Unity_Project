using System;
using TMPro;
using UnityEngine.UI;

public class InquriePanelController : PanelController
{
    [Header("Inquire Panel")]
    [SerializeField] private TMP_Text npcNanmeText;
    [SerializeField] private Image npcAvata;
    [SerializeField] private TMP_Text messageTitlleText;
    [SerializeField] private TMP_Text messageContentText;

    private InquireAction _currentAction;
    private int _currentIndex = -1;

    [Header("Buttons")]
    [SerializeField] private Button confirmButton;

    public override Type PanelActionType => typeof(InquireAction);

    public override void SetupPanel(ActionBase actionBase)
    {
        base.SetupPanel(actionBase);
        FirstButton = confirmButton;
        BindButtons();
        SetDefaultSelection();

        _currentAction = (InquireAction)actionBase;
        ApplyMessage(_currentAction.PickRandomMessageIndex());

    }

    private void ApplyMessage(int messageIndex)
    {
        _currentAction.GetInquireActionData(messageIndex, out var inquireActionData);
        _currentIndex = messageIndex;
        npcNanmeText.text = inquireActionData.personName;
        npcAvata.sprite = inquireActionData.portraitOverride;
        messageTitlleText.text = inquireActionData.title;
        messageContentText.text = inquireActionData.message;
    }

    private void BindButtons()
    {
        RebindButtons(confirmButton, OnCancel);
    }
}
