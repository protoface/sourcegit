using System.Collections.Generic;

namespace SourceGit.Models {
	public class User {
		public static User Invalid = new User();
		public static Dictionary<string, User> Caches = new Dictionary<string, User>();

		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;

		public override bool Equals(object obj) {
			if (obj == null || !(obj is User))
				return false;

			var other = obj as User;
			return Name == other.Name && Email == other.Email;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public static User FindOrAdd(string data) {
			if (Caches.TryGetValue(data, out User user)) {
				return user;
			} else {
				var nameEndIdx = data.IndexOf('<');
				var name = nameEndIdx >= 2 ? data.Substring(0, nameEndIdx - 1) : string.Empty;
				var email = data.Substring(nameEndIdx + 1);

				user = new User() { Name = name, Email = email };
				Caches.TryAdd(data, user);
				return user;
			}
		}
	}
}