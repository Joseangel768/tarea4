using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading;

namespace SeleniumPrueba
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string reportPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SeleniumReport.html");
            HtmlReport report = new HtmlReport(reportPath); // Inicializamos el reporte.

            IWebDriver driver = new FirefoxDriver();
            try
            {
                driver.Navigate().GoToUrl("https://plataformavirtual.itla.edu.do/login/index.php");
                driver.Manage().Window.Maximize();

                report.AddStep("1", "Abrir la página de login", "Exitoso", CaptureScreenshot(driver, "Pagina de login"));

                IWebElement InputName = driver.FindElement(By.Id("username"));
                InputName.SendKeys("20230954");
                report.AddStep("2", "Ingresar nombre de usuario", "Exitoso", CaptureScreenshot(driver, "nombrescreen"));

                IWebElement InputLastname = driver.FindElement(By.Id("password"));
                InputLastname.SendKeys("Joseser12@");
                report.AddStep("3", "Ingresar contraseña", "Exitoso", CaptureScreenshot(driver, "passwordEntered"));

                IWebElement button = driver.FindElement(By.Id("loginbtn"));
                button.Click();
                Thread.Sleep(2000);
                report.AddStep("4", "Hacer clic en el botón de inicio de sesión", "Exitoso", CaptureScreenshot(driver, "areaPersonal"));

                IWebElement inputcursos = driver.FindElement(By.CssSelector(".dropdown-toggle.nav-link"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputcursos);
                Thread.Sleep(2000);
                report.AddStep("5", "Abrir menú de cursos", "Exitoso", CaptureScreenshot(driver, "misCursos"));

                driver.Navigate().GoToUrl("https://plataformavirtual.itla.edu.do/course/view.php?id=8791");
                report.AddStep("6", "Abrir una materia específica", "Exitoso", CaptureScreenshot(driver, "aula de una materia"));

                IWebElement dropdownIdiomas = driver.FindElement(By.CssSelector("a[title='Idioma']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdownIdiomas);
                Thread.Sleep(3000);
                report.AddStep("7", "Abrir menú de idiomas", "Exitoso", CaptureScreenshot(driver, "MenuIdiomasDesplegado"));

                IWebElement inputCalifica = driver.FindElement(By.XPath("//span[contains(@class, 'media-body') and text()='Calificaciones']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputCalifica);
                Thread.Sleep(3000);
                report.AddStep("8", "Abrir calificaciones", "Exitoso", CaptureScreenshot(driver, "Calificaciones"));

                IWebElement inputCalendario = driver.FindElement(By.XPath("//span[contains(@class, 'media-body') and text()='Calendario']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", inputCalendario);
                Thread.Sleep(3000);
                report.AddStep("9", "Abrir calendario", "Exitoso", CaptureScreenshot(driver, "Calendario"));

            }
            catch (Exception ex)
            {
                report.AddStep("Error", ex.Message, "Fallido", null);
            }
            finally
            {
                driver.Quit();
                report.FinishReport();
            }
        }

        static string CaptureScreenshot(IWebDriver driver, string screenshotName)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SeleniumScreenshots", $"{screenshotName}.png");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            screenshot.SaveAsFile(filePath);
            return filePath;
        }
    }

    public class HtmlReport
    {
        private StreamWriter writer;

        public HtmlReport(string filePath)
        {
            writer = new StreamWriter(filePath, false);
            writer.WriteLine("<html>");
            writer.WriteLine("<head><title>Reporte de Pruebas Automatizadas</title></head>");
            writer.WriteLine("<body>");
            writer.WriteLine("<h1>Resultados de la Prueba Automatizada</h1>");
            writer.WriteLine("<table border='1'>");
            writer.WriteLine("<tr><th>Paso</th><th>Descripción</th><th>Resultado</th><th>Captura</th></tr>");
        }

        public void AddStep(string step, string description, string result, string screenshotPath = null)
        {
            writer.WriteLine("<tr>");
            writer.WriteLine($"<td>{step}</td>");
            writer.WriteLine($"<td>{description}</td>");
            writer.WriteLine($"<td>{result}</td>");
            if (!string.IsNullOrEmpty(screenshotPath))
            {
                writer.WriteLine($"<td><img src='{screenshotPath}' width='200'/></td>");
            }
            else
            {
                writer.WriteLine("<td>Sin captura</td>");
            }
            writer.WriteLine("</tr>");
        }

        public void FinishReport()
        {
            writer.WriteLine("</table>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Close();
        }
    }
}
