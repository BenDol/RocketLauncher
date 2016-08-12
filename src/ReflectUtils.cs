/*
 * Copyright (c) 2014 RocketLauncher <https://github.com/BenDol/RocketLauncher>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Policy;

namespace Launcher {

    class ReflectUtils {

        public static Assembly ReflectionOnlyLoadWithPartialName(string partialName) {
            return ReflectionOnlyLoadWithPartialName(partialName, null);
        }

        public static Assembly ReflectionOnlyLoadWithPartialName(string partialName, Evidence securityEvidence) {
            if (securityEvidence != null)
                new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();

            AssemblyName fileName = new AssemblyName(partialName);

            var assembly = nLoad(fileName, null, securityEvidence, null, null, false, true);

            if (assembly != null)
                return assembly;

            var assemblyRef = EnumerateCache(fileName);

            if (assemblyRef != null)
                return InternalLoad(assemblyRef, securityEvidence, null, true);

            return assembly;
        }

        private static Assembly nLoad(params object[] args) {
            return (Assembly)typeof(Assembly)
                .GetMethod("nLoad", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, args);
        }

        private static AssemblyName EnumerateCache(params object[] args) {
            return (AssemblyName)typeof(Assembly)
                .GetMethod("EnumerateCache", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, args);
        }

        private static Assembly InternalLoad(params object[] args) {
            // Easiest to query because the StackCrawlMark type is internal
            return (Assembly)
                typeof(Assembly).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .First(m => m.Name == "InternalLoad" && m.GetParameters()[0].ParameterType == typeof(AssemblyName))
                .Invoke(null, args);
        }
    }
}
