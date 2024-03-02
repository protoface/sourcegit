﻿namespace SourceGit.Models {
	public enum ChangeViewMode {
		List,
		Grid,
		Tree,
	}

	public enum ChangeState {
		None,
		Modified,
		Added,
		Deleted,
		Renamed,
		Copied,
		Unmerged,
		Untracked
	}

	public class Change {
		public ChangeState Index { get; set; }
		public ChangeState WorkTree { get; set; } = ChangeState.None;
		public string Path { get; set; } = "";
		public string OriginalPath { get; set; } = "";

		public bool IsConflit {
			get {
				if (Index == ChangeState.Unmerged || WorkTree == ChangeState.Unmerged)
					return true;
				if (Index == ChangeState.Added && WorkTree == ChangeState.Added)
					return true;
				if (Index == ChangeState.Deleted && WorkTree == ChangeState.Deleted)
					return true;
				return false;
			}
		}

		public void Set(ChangeState index, ChangeState workTree = ChangeState.None) {
			Index = index;
			WorkTree = workTree;

			if (index == ChangeState.Renamed || workTree == ChangeState.Renamed) {
				var idx = Path.IndexOf('\t');
				if (idx >= 0) {
					OriginalPath = Path.Substring(0, idx);
					Path = Path.Substring(idx + 1);
				}
				else {
					idx = Path.IndexOf(" -> ");
					if (idx > 0) {
						OriginalPath = Path.Substring(0, idx);
						Path = Path.Substring(idx + 4);
					}
				}
			}

			if (Path[0] == '"')
				Path = Path.Substring(1, Path.Length - 2);
			if (!string.IsNullOrEmpty(OriginalPath) && OriginalPath[0] == '"')
				OriginalPath = OriginalPath.Substring(1, OriginalPath.Length - 2);
		}
	}
}