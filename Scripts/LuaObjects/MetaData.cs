using System.Collections.Generic;

namespace ApophisSoftware.LuaObjects {
	public class MetaData {
		public Dictionary<string, string> metaData = new Dictionary<string, string>();

		public void setstring(string key, string value) {
			// stack:get_meta():set_string("description", "My worn out pick")

			if (metaData.ContainsKey(key)) {
				metaData[key] = value;
			} else {
				metaData.Add(key, value);
			}
		}

		public string getstring(string key, string defaultval) {
			if (metaData.ContainsKey(key)) {
				return metaData[key];
			} else {
				return defaultval;
			}
		}
	}
}