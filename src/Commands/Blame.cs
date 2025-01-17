﻿using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SourceGit.Commands {
	public class Blame : Command {
		private static readonly Regex REG_FORMAT = new Regex(@"^\^?([0-9a-f]+)\s+.*\((.*)\s+(\d+)\s+[\-\+]?\d+\s+\d+\) (.*)");
		private static readonly DateTime UTC_START = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

		public Blame(string repo, string file, string revision) {
			WorkingDirectory = repo;
			Context = repo;
			Args = $"blame -t {revision} -- \"{file}\"";
			RaiseError = false;

			_result.File = file;
		}

		public Models.BlameData Result() {
			var succ = Exec();
			if (!succ) {
				return new Models.BlameData();
			}

			if (_needUnifyCommitSHA) {
				foreach (var line in _result.LineInfos) {
					if (line.CommitSHA.Length > _minSHALen) {
						line.CommitSHA = line.CommitSHA.Substring(0, _minSHALen);
					}
				}
			}

			_result.Content = _content.ToString();
			return _result;
		}

		protected override void OnReadline(string line) {
			if (_result.IsBinary)
				return;
			if (string.IsNullOrEmpty(line))
				return;

			if (line.IndexOf('\0') >= 0) {
				_result.IsBinary = true;
				_result.LineInfos.Clear();
				return;
			}

			var match = REG_FORMAT.Match(line);
			if (!match.Success)
				return;

			_content.AppendLine(match.Groups[4].Value);

			var commit = match.Groups[1].Value;
			var author = match.Groups[2].Value;
			var timestamp = int.Parse(match.Groups[3].Value);
			var when = UTC_START.AddSeconds(timestamp).ToString("yyyy/MM/dd");

			var info = new Models.BlameLineInfo() {
				IsFirstInGroup = commit != _lastSHA,
				CommitSHA = commit,
				Author = author,
				Time = when,
			};

			_result.LineInfos.Add(info);
			_lastSHA = commit;

			if (line[0] == '^') {
				_needUnifyCommitSHA = true;
				_minSHALen = Math.Min(_minSHALen, commit.Length);
			}
		}

		private Models.BlameData _result = new Models.BlameData();
		private StringBuilder _content = new StringBuilder();
		private string _lastSHA = string.Empty;
		private bool _needUnifyCommitSHA = false;
		private int _minSHALen = 64;
	}
}