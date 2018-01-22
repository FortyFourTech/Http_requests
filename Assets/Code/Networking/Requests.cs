using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Assertions;

namespace API {
    public enum Request {
        authorize,
        playSound,
        getCurrent,
    }

    static public class RequestExtension {
        public static string url(this Request rq) {
            string result = NetworkService.BASE_URL;
            switch (rq) {
                case Request.authorize:
                    result += "/auth_login";
                    break;
                case Request.playSound:
                    result += "/quest_control";
                    break;
                case Request.getCurrent:
                    result += "";
                    break;
                default:
                    Assert.IsTrue(false, "url for request '" + rq.ToString() + "' is not defined");
                    break;
            }

            return result;
        }

        public static string method(this Request rq) {
            string result = "";
            switch (rq) {
                case Request.authorize:
                case Request.playSound:
                    result = UnityWebRequest.kHttpVerbPOST;
                    break;
                case Request.getCurrent:
                    result = UnityWebRequest.kHttpVerbGET;
                    break;
                default:
                    Assert.IsTrue(false, "method for request '" + rq.ToString() + "' is not defined");
                    break;
            }

            return result;
        }
    }
}
