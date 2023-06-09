﻿using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections;
using API;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Testing
{
    public partial class EnumTests
    {

        // [Test]
        // public void TestRequestEnum()
        // {
        //     TestExtensionMethods(typeof(ERequest));
        // }

        static void TestExtensionMethods(Type enumeration)
        {
            var values = Enum.GetValues(enumeration);
            var methods = GetExtensionMethods(enumeration);

            foreach (var val in values)
            {
                foreach (var method in methods)
                {
                    method.Invoke(null, new object[] { val });
                }
            }
        }

        static IEnumerable<MethodInfo> GetExtensionMethods(Type extendedType)
        {
            var assembly = extendedType.Assembly;
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                            | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == extendedType
                        select method;
            return query;
        }
    }
}
