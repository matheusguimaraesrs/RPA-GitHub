using BLD.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;
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
        static void Main()
        {
            string login;
            string password;

            Console.Write("Insira seu login do GitHub:");
            login = Console.ReadLine();
            password = ConsolePassword.ReadPassword();

            //pré configurações
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            Versionador v = new Versionador();
            var chromeDriverService = ChromeDriverService.CreateDefaultService(v.versaoChromeDriver());
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(chromeDriverService, options: options);

            //execução de login
            driver.Navigate().GoToUrl("https://github.com/login");
            IWebElement Web = driver.FindElement(By.Name("login"));
            Web.Clear();
            Web.SendKeys(login);

            Web = driver.FindElement(By.Name("password"));
            Web.Clear();
            Web.SendKeys(password);
            driver.FindElement(By.Name("commit")).Click();
            driver.FindElement(By.XPath("/html/body/div[1]/div[3]/main/div/div[2]/div[2]/div[3]/button")).Click();
            driver.FindElement(By.XPath("//*[@id='two-factor-alternatives-body']/li[1]/a")).Click();
            driver.FindElement(By.CssSelector("body > div.logged-out.env-production.page-responsive.page-two-factor-auth.session-authentication > div.application-main > main > div > div.authentication-body > div.authentication-body-buttons > form > button")).Click();

            WaitForLoad(driver);
            //Thread.Sleep(TimeSpan.FromSeconds(50000));
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