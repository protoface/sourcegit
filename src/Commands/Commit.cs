﻿using System.IO;

namespace SourceGit.Commands
{
	public class Commit : Command
	{
		public Commit(string repo, string message, bool amend, bool allowEmpty = false)
		{
			var file = Path.GetTempFileName();
			File.WriteAllText(file, message);

			WorkingDirectory = repo;
			Context = repo;
			Args = $"commit --file=\"{file}\"";
			if (amend)
				Args += " --amend --no-edit";
			if (allowEmpty)
				Args += " --allow-empty";
		}
	}
}
