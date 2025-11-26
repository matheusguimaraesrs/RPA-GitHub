using BLD.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string login;
            string password;

            Console.Write("Insira seu login do GitHub:");
            login = Console.ReadLine();
            password = ConsolePassword.ReadPassword();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            Versionador v = new Versionador();
            var chromeDriverService = ChromeDriverService.CreateDefaultService(v.versaoChromeDriver());
            chromeDriverService.HideCommandPromptWindow = true;

            using (ChromeDriver driver = new ChromeDriver(chromeDriverService, options: options))
            {
                driver.Navigate().GoToUrl("https://github.com/login");
                WaitForLoad(driver);
                IWebElement Web = driver.FindElement(By.Name("login"));
                Web.SendKeys(login);
                Web = driver.FindElement(By.Name("password"));
                Web.SendKeys(password);
                WaitForLoad(driver);

            }
        }

        static void WaitForLoad(ChromeDriver driver)
        {
            Thread.Sleep(1000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(wd => driver.ExecuteScript("return document.readyState").ToString() == "complete");
        }
    }
    public static class ConsolePassword
    {
        public static string ReadPassword()
        {
            Console.Write("Digite a senha: ");

            StringBuilder passwordBuilder = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); 

                if (key.Key == ConsoleKey.Enter)
                    break;
                if (key.Key == ConsoleKey.Backspace && passwordBuilder.Length > 0)
                {
                    passwordBuilder.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    passwordBuilder.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return passwordBuilder.ToString();
        }
    }
}