using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SkdRefSiteAPI.DAO
{
    public class Logger
    {
        private readonly string APPLICATION_NAME;
        private readonly int STACK_FRAME;

        public Logger(string applicationName, int stackFrame = 2)
        {
            APPLICATION_NAME = applicationName;
            STACK_FRAME = stackFrame;
        }

        public void Log(Exception ex, params object[] parameters)
        {
            var serializedParameters = GetSerializedParameters(parameters);
            PerformLog("Error", serializedParameters, ex);
        }

        public void Log(string message, params object[] parameters)
        {
            var serializedParameters = GetSerializedParameters(parameters);
            PerformLog(message, serializedParameters);
        }

        private void PerformLog(string message, string parameters, Exception exception = null)
        {
            try
            {
                var time = DateTime.Now;
                var source = GetSource();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error logging: " + ex.ToString());
            }
        }

        private string GetSerializedParameters(params object[] parameters)
        {
            var serializedParameters = new List<string>();
            foreach (var parameter in parameters)
            {
                // for some reason i can't get the json library package working so doing it the stupid way
                var ms = new MemoryStream();
                var ser = new DataContractJsonSerializer(typeof(object));
                ser.WriteObject(ms, parameter);
                byte[] json = ms.ToArray();
                ms.Close();
                serializedParameters.Add(Encoding.UTF8.GetString(json, 0, json.Length));
            }

            var allParameters = String.Join("; ", serializedParameters);
            return allParameters;
        }

        [MethodImpl(MethodImplOptions.NoInlining)] // This prevents the compiler from inlining the function to optimize things. Doing so would alter our stack trace so we might not get the right frame.
        private string GetSource()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(STACK_FRAME); // 0 would be this function, 1 is the one that called it (eg, Log), and 2 should be whatever called log.
            var method = frame.GetMethod();
            var methodName = method.Name;
            var className = method.ReflectedType.Name;
            var source = String.Format("{0} {1}.{2}", APPLICATION_NAME, className, methodName);
            return source;
        }
    }
}
