﻿using System.Threading.Tasks;

namespace SourceGit.ViewModels
{
	public class DeleteRepositoryNode : Popup
	{
		public RepositoryNode Node
		{
			get => _node;
			set => SetProperty(ref _node, value);
		}

		public DeleteRepositoryNode(RepositoryNode node)
		{
			_node = node;
			View = new Views.DeleteRepositoryNode() { DataContext = this };
		}

		public override Task<bool> Sure()
		{
			Preference.RemoveNode(_node);
			return null;
		}

		private RepositoryNode _node = null;
	}
}
