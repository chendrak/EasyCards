using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace EasyCards.UI;

public class CardPanel : UniverseLib.UI.Panels.PanelBase
{
    public static CardPanel Instance { get; private set; }
    
    public static RectTransform NavBarRect { get; private set; }
    
    public CardPanel(UIBase owner) : base(owner)
    {
        Instance = this;
    }

    public override string Name => "Easy Cards - Cards";
    public override int MinWidth => 640;
    public override int MinHeight => 480;
    
    public override Vector2 DefaultAnchorMin => new(0.5f, 1f);
    public override Vector2 DefaultAnchorMax => new(0.5f, 1f);
    
    public override Vector2 DefaultPosition => new(0 - MinWidth / 2, Screen.height);
    
    public override bool CanDragAndResize => false;
    
    public ButtonRef CloseBtn { get; private set; }

    protected override void ConstructPanelContent()
    {
        GameObject navbarPanel = UIFactory.CreateUIObject("MainNavbar", UIRoot);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(navbarPanel, false, false, true, true, 5, 4, 4, 4, 4, TextAnchor.MiddleCenter);
        navbarPanel.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
        NavBarRect = navbarPanel.GetComponent<RectTransform>();
        NavBarRect.pivot = new Vector2(0.5f, 1f);
        
        CloseBtn = UIFactory.CreateButton(navbarPanel, "CloseBtn", "Close");
        UIFactory.SetLayoutElement(CloseBtn.Component.gameObject, minWidth: 160, minHeight: 40, preferredWidth: 160, preferredHeight: 40, flexibleWidth: 0, flexibleHeight: 0);

        CloseBtn.OnClick += UiManager.HideCardPanel;
    }
}