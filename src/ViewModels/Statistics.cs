﻿using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace SourceGit.ViewModels
{
	public class Statistics : ObservableObject
	{
		public bool IsLoading
		{
			get => _isLoading;
			private set => SetProperty(ref _isLoading, value);
		}

		public int SelectedIndex
		{
			get => _selectedIndex;
			set
			{
				if (SetProperty(ref _selectedIndex, value))
					RefreshReport();
			}
		}

		public Models.StatisticsReport SelectedReport
		{
			get => _selectedReport;
			private set => SetProperty(ref _selectedReport, value);
		}

		public Statistics(string repo)
		{
			_repo = repo;

			Task.Run(() =>
			{
				var result = new Commands.Statistics(_repo).Result();
				Dispatcher.UIThread.Invoke(() =>
				{
					_data = result;
					RefreshReport();
					IsLoading = false;
				});
			});
		}

		private void RefreshReport()
		{
			if (_data == null)
				return;

			switch (_selectedIndex)
			{
				case 0:
					SelectedReport = _data.Year;
					break;
				case 1:
					SelectedReport = _data.Month;
					break;
				default:
					SelectedReport = _data.Week;
					break;
			}
		}

		private string _repo = string.Empty;
		private bool _isLoading = true;
		private Models.Statistics _data = null;
		private Models.StatisticsReport _selectedReport = null;
		private int _selectedIndex = 0;
	}
}
