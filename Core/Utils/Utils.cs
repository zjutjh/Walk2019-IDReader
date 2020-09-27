using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Core
{
    public static class CoreUtils
    {
        public static string ConvertJsonString(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            JsonSerializer serializer = new JsonSerializer();

            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();
        }


        public static void RunBat(string batPath)
        {
            Process process = new Process();
            FileInfo file = new FileInfo(batPath);
            process.StartInfo.WorkingDirectory = file.Directory.FullName;
            process.StartInfo.FileName = batPath;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
        }
    }
}
