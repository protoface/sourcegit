﻿using System;

namespace SourceGit.Commands
{
	public class GC : Command
	{
		public GC(string repo, Action<string> outputHandler)
		{
			_outputHandler = outputHandler;
			WorkingDirectory = repo;
			Context = repo;
			TraitErrorAsOutput = true;
			Args = "gc";
		}

		protected override void OnReadline(string line)
		{
			_outputHandler?.Invoke(line);
		}

		private Action<string> _outputHandler;
	}
}
