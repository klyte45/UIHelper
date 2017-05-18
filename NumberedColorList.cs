using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ICities;
using ColossalFramework.UI;
using ColossalFramework;
using ColossalFramework.Plugins;
using System.Threading;
using System;
using System.Linq;

namespace Klyte.Extensions
{
	public class NumberedColorList
	{
		private UIPanel linesListPanel;
		private UIComponent m_parent;
		private List<Color32> m_colorList;
		private UIButton m_add;

		public List<Color32> colorList {
			get {
				return m_colorList;
			}
			set {
				m_colorList = value;
				redrawButtons ();
			}
		}

		public void Enable (){
			linesListPanel.enabled = true;
			m_add.enabled = true;
			redrawButtons ();
		}
		public void Disable(){
			foreach (Transform t in linesListPanel.transform) {
				GameObject.Destroy(t.gameObject);
			}
			m_add.enabled = false;
			linesListPanel.enabled = false;
		}

		public NumberedColorList(UIComponent parent, List<Color32> initialColorList, UIComponent addButtonContainer)
		{
			m_parent = parent;
			parent.width = 500;
			((UIPanel)parent).autoFitChildrenVertically = true;
			linesListPanel = m_parent.AttachUIComponent (UITemplateManager.GetAsGameObject (UIHelperExtension.kDropdownTemplate)) as UIPanel;
			linesListPanel.name = "NumberedColorList";
			linesListPanel.height = 40;
			linesListPanel.width = 500;
			linesListPanel.autoLayoutDirection = LayoutDirection.Horizontal;
			linesListPanel.autoLayoutStart = LayoutStart.TopLeft;
			linesListPanel.autoFitChildrenVertically = true;
			linesListPanel.wrapLayout = true;
			linesListPanel.autoLayoutPadding = new RectOffset (5, 5, 5, 5);

			foreach (Transform t in linesListPanel.transform) {
				GameObject.Destroy(t.gameObject);
			}

			m_add = addButtonContainer.GetComponentInChildren<UILabel>().AttachUIComponent (UITemplateManager.GetAsGameObject (UIHelperExtension.kButtonTemplate)) as UIButton;
			m_add.text = "+";
			m_add.autoSize = false;
			m_add.height = 27;
			m_add.width = 27;
			m_add.relativePosition = new Vector3(70f,0f,0);
			m_add.textPadding = new RectOffset(0,0,0,0);
			m_add.textHorizontalAlignment = UIHorizontalAlignment.Center;
			m_add.eventClick += delegate (UIComponent c, UIMouseEventParameter sel) {
				m_colorList.Add (Color.white);
				redrawButtons();
				if(eventOnAdd!=null){
					eventOnAdd();
				}
			}; 



			colorList = initialColorList;
		}

		private static  void initButton (UIButton button, string baseSprite)
		{
			string sprite = baseSprite;//"ButtonMenu";
			string spriteHov = baseSprite;
			button.normalBgSprite = sprite;
			button.disabledBgSprite = sprite;
			button.hoveredBgSprite = spriteHov;
			button.focusedBgSprite = spriteHov;
			button.pressedBgSprite = spriteHov;
			button.textColor = new Color32 (255, 255, 255, 255);
			button.pressedTextColor = Color.red;
			button.hoveredTextColor = Color.gray;
		}

		public void Redraw(){
			redrawButtons ();
		}

		private void redrawButtons ()
		{		
			foreach (Transform t in linesListPanel.transform) {
				GameObject.Destroy(t.gameObject);
			}
			for (int j = 0; j< colorList.Count; j++) {

				GameObject itemContainer = new GameObject ();			
			
				itemContainer.transform.parent = linesListPanel.transform;
				UIButtonWithId itemButton = itemContainer.AddComponent<UIButtonWithId> ();		

				itemButton.width = 35;
				itemButton.height = 35;

				initButton (itemButton,  "EmptySprite");
				itemButton.color = colorList [j];
				itemButton.hoveredColor = itemButton.color;
				itemButton.pressedColor = itemButton.color;
				itemButton.focusedColor = itemButton.color;
				itemButton.textColor = Color.white;
				itemButton.hoveredColor = itemButton.textColor;
				itemButton.id = j+1;
				itemButton.eventClick += (component, eventParam) => {
					if(eventOnClick!=null){
						eventOnClick(itemButton.id);
					}
				};
				setLineNumberMainListing (j+1, itemButton);	
				itemButton.name = "Color #" + (j+1);			
			}

		}

		private void setLineNumberMainListing (int num, UIButton button)
		{
			UILabel l = button.AddUIComponent<UILabel> ();
			l.autoSize = false;
			l.autoHeight = false;
			l.pivot = UIPivotPoint.TopLeft;
			l.verticalAlignment = UIVerticalAlignment.Middle;
			l.textAlignment = UIHorizontalAlignment.Center;
			l.relativePosition = new Vector3 (0, 0);
			l.width = button.width;
			l.height = button.height;
			l.useOutline = true;
			l.text = num.ToString();
			float ratio = l.width / 50;
			if (l.text.Length == 4) {
				l.textScale = ratio;	
				l.relativePosition = new Vector3 (0f, 1f);			
			} else if (l.text.Length == 3) {
				l.textScale = ratio * 1.25f;
				l.relativePosition = new Vector3 (0f, 1.5f);
			} else if (l.text.Length == 2) {
				l.textScale = ratio * 1.75f;
				l.relativePosition = new Vector3 (-0.5f, 0.5f);
			} else {
				l.textScale = ratio * 2.3f;
			}
		}

		private class UIButtonWithId : UIButton
		{
			public int id;
		}

		public event OnButtonSelect<int> eventOnClick;
		public event OnButtonClicked eventOnAdd;
	}
	public delegate void OnButtonSelect<T> (T idx);
}

