using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API.Responses {
    public struct LangListResponse {
        public string[] dirs;
        public Dictionary<string,string> langs;
    }
}
