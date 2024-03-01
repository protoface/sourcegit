﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace SourceGit.ViewModels
{
	public class PopupHost : ObservableObject
	{
		public static PopupHost Active
		{
			get;
			set;
		} = null;

		public Popup Popup
		{
			get => _popup;
			set => SetProperty(ref _popup, value);
		}

		public static bool CanCreatePopup()
		{
			return Active != null && (Active._popup == null || !Active._popup.InProgress);
		}

		public static void ShowPopup(Popup popup)
		{
			popup.HostPageId = Active.GetId();
			Active.Popup = popup;
		}

		public static async void ShowAndStartPopup(Popup popup)
		{
			popup.HostPageId = Active.GetId();
			Active.Popup = popup;

			if (!popup.Check())
				return;

			popup.InProgress = true;
			var task = popup.Sure();
			if (task != null)
			{
				var finished = await task;
				if (finished)
				{
					Active.Popup = null;
				}
				else
				{
					popup.InProgress = false;
				}
			}
			else
			{
				Active.Popup = null;
			}
		}

		public virtual string GetId()
		{
			return string.Empty;
		}

		public async void ProcessPopup()
		{
			if (_popup != null)
			{
				if (!_popup.Check())
					return;

				_popup.InProgress = true;
				var task = _popup.Sure();
				if (task != null)
				{
					var finished = await task;
					if (finished)
					{
						Popup = null;
					}
					else
					{
						_popup.InProgress = false;
					}
				}
				else
				{
					Popup = null;
				}
			}
		}

		public void CancelPopup()
		{
			if (_popup == null)
				return;
			if (_popup.InProgress)
				return;
			Popup = null;
		}

		private Popup _popup = null;
	}
}
