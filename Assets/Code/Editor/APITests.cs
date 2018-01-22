using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using API;

namespace Testing {
    public class APITests {

        [Test]
        public void TestAuthorization() {
            var enumerator = NetworkService.DoAuthorization((result, error) => {
                Assert.IsTrue(result, error);
            });

            Action<IEnumerator> goThroughCoroutine = null;
            goThroughCoroutine = input => {
                while (input.MoveNext()) {
                    //yield return null;
                    if (input.Current is AsyncOperation)
                        while (!(input.Current as AsyncOperation).isDone) { }
                    //yield return null;
                    else if (input.Current is IEnumerator)
                        goThroughCoroutine(input.Current as IEnumerator);
                }
            };
            goThroughCoroutine(enumerator);
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        //[UnityTest]
        //public IEnumerator EnumTestsWithEnumeratorPasses() {
        //	// Use the Assert class to test conditions.
        //	// yield to skip a frame
        //	yield return null;
        //}
    }
}
