using System.Collections.Generic;

namespace API.Responses {
    public struct LangListResponse {
        public string[] dirs;
        public Dictionary<string,string> langs;
    }
}
