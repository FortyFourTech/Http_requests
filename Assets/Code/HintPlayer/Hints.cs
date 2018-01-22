using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum HintEnum {
    one,
    two,
    three,
    four,
    five,
}

public static class HintExtension {
    public static string requestParameterValue(this HintEnum hint) {
        string result = "";
        switch (hint) {
            case HintEnum.one:
                result = "1401";
                break;
            case HintEnum.two:
                result = "1402";
                break;
            case HintEnum.three:
                result = "1403";
                break;
            case HintEnum.four:
                result = "1404";
                break;
            case HintEnum.five:
                result = "1405";
                break;
            default:
                Assert.IsTrue(false, "request parameter value for hint '" + hint.ToString() + "' is not defined");
                break;
        }

        return result;
    }
}
