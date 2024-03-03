﻿using System;
using System.Collections.Generic;

namespace SourceGit.Commands {
	public static class Discard {
		public static void All(string repo) {
			new Reset(repo, "HEAD", "--hard").Exec();
			new Clean(repo).Exec();
		}

		public static void ChangesInWorkTree(string repo, List<Models.Change> changes) {
			var needClean = new List<string>();
			var needCheckout = new List<string>();

			foreach (var c in changes) {
				if (c.WorkTree == Models.ChangeState.Untracked || c.WorkTree == Models.ChangeState.Added) {
					needClean.Add(c.Path);
				} else {
					needCheckout.Add(c.Path);
				}
			}

			for (int i = 0; i < needClean.Count; i += 10) {
				var count = Math.Min(10, needClean.Count - i);
				new Clean(repo, needClean.GetRange(i, count)).Exec();
			}

			for (int i = 0; i < needCheckout.Count; i += 10) {
				var count = Math.Min(10, needCheckout.Count - i);
				new Checkout(repo).Files(needCheckout.GetRange(i, count));
			}
		}

		public static void ChangesInStaged(string repo, List<Models.Change> changes) {
			for (int i = 0; i < changes.Count; i += 10) {
				var count = Math.Min(10, changes.Count - i);
				var files = new List<string>();
				for (int j = 0; j < count; j++)
					files.Add(changes[i + j].Path);
				new Restore(repo, files, "--staged --worktree").Exec();
			}
		}
	}
}