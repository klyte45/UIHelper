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
	public class CrudDropDownComponent
	{
		private UIDropDown m_dropDown;
		private UIDropDown m_deleteButton;
		private UIDropDown m_createButton;
		private UILabel m_title;
		private UIComponent m_parent;
		private static string[] options;

		public String selectedItem {
			get {
				return m_dropDown.selectedValue;
			}
			set {
				m_dropDown.selectedValue =value;
			}
		}

		public UIComponent parent {
			get {
				return m_parent;
			}
		}

		public CrudDropDownComponent (UIComponent parent, string title)
		{
			m_parent = parent;
			UIPanel uIPanel = m_parent.AttachUIComponent (UITemplateManager.GetAsGameObject (UIHelperExtension.kDropdownTemplate)) as UIPanel;
			uIPanel.name = "DropDownColorSelector";
			uIPanel.height = 40;
			uIPanel.width = 280;
			uIPanel.autoLayoutDirection = LayoutDirection.Horizontal;
			uIPanel.autoLayoutStart = LayoutStart.TopLeft;


			m_title = uIPanel.Find<UILabel> ("Label");
			m_title.autoSize = false;
			m_title.name = "Title";
			m_title.text = title;
			m_title.height = 28;
			m_title.width = 140;
			m_title.textAlignment = UIHorizontalAlignment.Left;
			m_title.textColor = Color.white;
			m_title.padding = new RectOffset (5, 5, 5, 5);

			m_dropDown = uIPanel.Find<UIDropDown> ("Dropdown");
			initializeDropDown (ref m_dropDown);


		}

		private void initializeDropDown (ref UIDropDown dropDown)
		{
			if (options == null) {
				List<string> optionsList = new List<string> ();
				for (int i =0; i<=64; i++) {
					optionsList.Add (String.Format ("{0:X2}", i==0?0:(i*4)-1));
				}
				options = optionsList.ToArray ();
			}
			dropDown.items = options;

			dropDown.useOutline = true;
			dropDown.outlineColor = Color.black;
			dropDown.textColor = Color.white;
			dropDown.autoSize = true;
			dropDown.itemPadding = new RectOffset (4, 4, 0, 0);
			dropDown.textFieldPadding = new RectOffset (5, 10, 5, 5);
		}

		public event SelectionChangeHandler eventSelectionChange;
		public event CreateOptionHandler eventCreateOption;
		
		public delegate void SelectionChangeHandler (String value);
		public delegate void CreateOptionHandler (String value);
	}

}

