using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Timers;
using System.Windows.Forms;
using System.Net.Mail;

namespace ResQEzMode
{
    class Program
    {
        static void Main(string[] args)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments("-headless");
            var driver = new FirefoxDriver(options);
            Console.Clear(); //Clear initialization stuff from console
            driver.Navigate().GoToUrl("http://resq-club.com/app"); //Navigate to correct URL
            FetchFood(driver);
            while (true)
            {
                FetchFood(driver);
                Thread.Sleep(60000); //Search once/minute
            }       
        }       
        public static void FetchFood( FirefoxDriver driver)
        {
            Console.Clear(); //Clear initialization stuff from console
            Console.ResetColor();
            driver.Navigate().Refresh();
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("return activeMap.setCenter({lat: 60.454510, lng: 22.264824})"); //Move GoogleMaps to Turku
            js.ExecuteScript("return activeMap.setZoom(12)"); //Zoom closer to reveal all foods
            Thread.Sleep(3000); //Wait for JS to execute

            driver.Navigate().Refresh();
            IList<IWebElement> all = driver.FindElements(By.ClassName("offerRowName")); //Look for offer elements
            IList<IWebElement> suppliers = driver.FindElements(By.ClassName("offerRowProviderName")); //Look for offer elements

            String[] allText = new String[all.Count];
            String[] supplierName = new String[suppliers.Count];
            String[] combinedText = new String[suppliers.Count];

            int i = 0;
            int j = 0;
            foreach (IWebElement element in all) //Add all text content from offers to string array
            {
                allText[i++] = element.Text;
            }
            foreach (IWebElement element in suppliers) //Add all text content from offers to string array
            {
                supplierName[j++] = element.Text;
                Console.WriteLine(element);

            }
            for (int k = 0; k < suppliers.Count; k++)
            {
                if (allText[k].Contains("kassi") & !allText[k].Contains("leipä") & !allText[k].Contains("Leipä") && !allText[k].Contains("Viili") && !supplierName[k].Contains("Ruokatori"))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                Console.WriteLine(allText[k] + "\t" + supplierName[k]);
            }
            driver.Close();
            driver.Quit();
        }       
    }
}
