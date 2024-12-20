using System;
using System.Xml.Linq;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class ConsoleLogger : ILogger
    {


        string filePath = "log.xml";

        public ConsoleLogger() 
        {
            if (!File.Exists(filePath) || !IsValidXml(filePath))
            {
                var root = new XElement("logs");
                root.Save(filePath);
            }
        }
        public void LogWarning(string message, params object[] args)
        {
            Console.WriteLine(string.Concat("WARN: ", message), args);
            LogToXml("WARN", message);

        }

        public void LogInfo(string message, params object[] args)
        {
            Console.WriteLine(string.Concat("INFO: ", message), args);
            LogToXml("INFO", message);

        }


        //private void LogMessage(string type, string message, params object[] args)
        //{
        //    Console.WriteLine(type + ": " + message, args);
        //    using (StreamWriter logfile = File.AppendText(filePath))
        //    {
        //        logfile.WriteLine("<log><type>" + type + "</type><message>" + message + "</message></log> ", args);
        //    }
        //}


        private void LogToXml(string type, string message)
        {
            try
            {
                var logEntry = new XElement("log",
                    new XElement("type", type),
                    new XElement("message", message)
                );

                // Load the existing XML file
                var doc = XDocument.Load(filePath);

                // Add the new log entry to the root element
                doc.Root.Add(logEntry);

                // Save the updated XML document
                doc.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Unable to write to XML log file. {ex.Message}");
            }
        }


        private bool IsValidXml(string filePath)
        {
            try
            {
                XDocument.Load(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
    
}
