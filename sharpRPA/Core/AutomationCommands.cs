﻿//Copyright (c) 2017 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Data;
using MSHTML;
using System.Windows.Automation;
using System.Net.Mail;
using sharpRPA.Core;
using System.Net;
using System.IO;
using System.Drawing;

namespace sharpRPA.Core.AutomationCommands
{
    #region Base Command

    [XmlInclude(typeof(SendKeysCommand))]
    [XmlInclude(typeof(SendMouseMoveCommand))]
    [XmlInclude(typeof(PauseCommand))]
    [XmlInclude(typeof(ActivateWindowCommand))]
    [XmlInclude(typeof(MoveWindowCommand))]
    [XmlInclude(typeof(CommentCommand))]
    [XmlInclude(typeof(ThickAppClickItemCommand))]
    [XmlInclude(typeof(ThickAppGetTextCommand))]
    [XmlInclude(typeof(ResizeWindowCommand))]
    [XmlInclude(typeof(WaitForWindowCommand))]
    [XmlInclude(typeof(MessageBoxCommand))]
    [XmlInclude(typeof(StopProcessCommand))]
    [XmlInclude(typeof(StartProcessCommand))]
    [XmlInclude(typeof(VariableCommand))]
    [XmlInclude(typeof(RunScriptCommand))]
    [XmlInclude(typeof(CloseWindowCommand))]
    [XmlInclude(typeof(IEBrowserCreateCommand))]
    [XmlInclude(typeof(IEBrowserNavigateCommand))]
    [XmlInclude(typeof(IEBrowserCloseCommand))]
    [XmlInclude(typeof(IEBrowserElementCommand))]
    [XmlInclude(typeof(IEBrowserFindBrowserCommand))]
    [XmlInclude(typeof(SetWindowStateCommand))]
    [XmlInclude(typeof(BeginLoopCommand))]
    [XmlInclude(typeof(EndLoopCommand))]
    [XmlInclude(typeof(ClipboardGetTextCommand))]
    [XmlInclude(typeof(ScreenshotCommand))]
    [XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelAddWorkbookCommand))]
    [XmlInclude(typeof(ExcelGoToCellCommand))]
    [XmlInclude(typeof(ExcelSetCellCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelRunMacroCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SMTPSendEmailCommand))]
    [XmlInclude(typeof(ErrorHandlingCommand))]
    [XmlInclude(typeof(StringSubstringCommand))]
    [XmlInclude(typeof(StringSplitCommand))]
    [XmlInclude(typeof(BeginIfCommand))]
    [XmlInclude(typeof(EndIfCommand))]
    [XmlInclude(typeof(ElseCommand))]
    [XmlInclude(typeof(OCRCommand))]
    [XmlInclude(typeof(HTTPRequestCommand))]
    [XmlInclude(typeof(HTTPQueryResultCommand))]
    [XmlInclude(typeof(ImageRecognitionCommand))]
    [Serializable]
    public abstract class ScriptCommand
    {
        [XmlAttribute]
        public string CommandName { get; set; }
        [XmlAttribute]
        public bool IsCommented { get; set; }
        [XmlAttribute]
        public string SelectionName { get; set; }
        [XmlAttribute]
        public int DefaultPause { get; set; }
        [XmlAttribute]
        public int LineNumber { get; set; }
        [XmlAttribute]
        public bool PauseBeforeExeucution { get; set; }
        [XmlIgnore]
        public System.Drawing.Color DisplayForeColor { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Comment Field (Optional)")]
        public string v_Comment { get; set; }
        [XmlAttribute]
        public bool CommandEnabled { get; set; }

        public ScriptCommand()
        {
            this.DisplayForeColor = System.Drawing.Color.SteelBlue;
            this.CommandEnabled = false;
            this.DefaultPause = 250;
            this.IsCommented = false;
        }

        public virtual void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        public virtual void RunCommand(object sender, Core.Script.ScriptAction command, System.ComponentModel.BackgroundWorker bgw)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }

        public virtual string GetDisplayValue()
        {
            return SelectionName;
        }
    }

    #endregion Base Command

    #region Legacy IE Web Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IEBrowserCreateCommand()
        {
            this.CommandName = "IEBrowserCreateCommand";
            this.SelectionName = "Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var newBrowserSession = new SHDocVw.InternetExplorer();
            newBrowserSession.Visible = true;
            sendingInstance.appInstances.Add(v_InstanceName, newBrowserSession);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to find and attach to an existing IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserFindBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Enter the Browser Name")]
        public string v_IEBrowserName { get; set; }

        public IEBrowserFindBrowserCommand()
        {
            this.CommandName = "IEBrowserFindBrowserCommand";
            this.SelectionName = "Find Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            bool browserFound = false;
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if ((shellWindow.Document is MSHTML.HTMLDocument) && (shellWindow.Document.Title == v_IEBrowserName))
                {
                    sendingInstance.appInstances.Add(v_InstanceName, shellWindow.Application);
                    browserFound = true;
                    break;
                }
            }

            //try partial match
            if (!browserFound)
            {
                foreach (IWebBrowser2 shellWindow in shellWindows)
                {
                    if ((shellWindow.Document is MSHTML.HTMLDocument) && (shellWindow.Document.Title.Contains(v_IEBrowserName)))
                    {
                        sendingInstance.appInstances.Add(v_InstanceName, shellWindow.Application);
                        browserFound = true;
                        break;
                    }
                }
            }

            if (!browserFound)
            {
                throw new Exception("Browser was not found!");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Browser Name: '" + v_IEBrowserName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate the associated IE web browser to a URL.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserNavigateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IEBrowserNavigateCommand()
        {
            this.CommandName = "WebBrowserNavigateCommand";
            this.SelectionName = "Navigate";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;
                browserInstance.Navigate(v_URL.ConvertToUserVariable(sender));
                WaitForReadyState(browserInstance);
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close the associated IE web browser")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IEBrowserCloseCommand()
        {
            this.CommandName = "IEBrowserCloseCommand";
            this.SelectionName = "Close Browser";
            this.CommandEnabled = true;
            this.v_InstanceName = "default";
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;
                browserInstance.Quit();
                sendingInstance.appInstances.Remove(v_InstanceName);
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to manipulate (get or set) elements within the HTML document of the associated IE web browser.  Features an assisting element capture form")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll and MSHTML.dll to achieve automation.")]
    public class IEBrowserElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or capture element search parameters")]
        public System.Data.DataTable v_WebSearchTable { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select an action")]
        public string v_WebAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Action Parameters")]
        public System.Data.DataTable v_WebActionParameterTable { get; set; }

        public IEBrowserElementCommand()
        {
            this.CommandName = "IEBrowserElementCommand";
            this.SelectionName = "Element Action";
            this.CommandEnabled = true;
            this.v_InstanceName = "default";

            this.v_WebSearchTable = new System.Data.DataTable();
            this.v_WebSearchTable.TableName = DateTime.Now.ToString("WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebSearchTable.Columns.Add("Enabled");
            this.v_WebSearchTable.Columns.Add("Property Name");
            this.v_WebSearchTable.Columns.Add("Property Value");

            this.v_WebActionParameterTable = new System.Data.DataTable();
            this.v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;

            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;

                DataTable searchTable = Core.Common.Clone<DataTable>(v_WebSearchTable);

                DataColumn matchFoundColumn = new DataColumn();
                matchFoundColumn.ColumnName = "Match Found";
                matchFoundColumn.DefaultValue = false;
                searchTable.Columns.Add(matchFoundColumn);

                var elementSearchProperties = from rws in searchTable.AsEnumerable()
                                              where rws.Field<string>("Enabled") == "True"
                                              select rws;

                bool qualifyingElementFound = false;

                foreach (IHTMLElement element in browserInstance.Document.All)
                {
                    qualifyingElementFound = FindQualifyingElement(elementSearchProperties, element);

                    if ((qualifyingElementFound) && (v_WebAction == "Invoke Click"))
                    {
                        element.click();
                        WaitForReadyState(browserInstance);
                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Left Click") || (qualifyingElementFound) && (v_WebAction == "Middle Click") || (qualifyingElementFound) && (v_WebAction == "Right Click"))
                    {
                        int elementXposition = FindElementXPosition(element);
                        int elementYposition = FindElementYPosition(element);

                        //inputs need to be validated

                        int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());

                        int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());

                        var ieClientLocation = User32Functions.GetWindowPosition(new IntPtr(browserInstance.HWND));

                        SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                        newMouseMove.v_XMousePosition = (elementXposition + ieClientLocation.left + 10) + userXAdjust; // + 10 gives extra padding
                        newMouseMove.v_YMousePosition = (elementYposition + ieClientLocation.top + 90) + userYAdjust; // +90 accounts for title bar height
                        newMouseMove.v_MouseClick = v_WebAction;
                        newMouseMove.RunCommand(sender);

                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Set Attribute"))
                    {
                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string valueToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Value To Set"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        valueToSet = valueToSet.ConvertToUserVariable(sender);

                        element.setAttribute(attributeName, valueToSet);
                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Get Attribute"))
                    {
                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string convertedAttribute = Convert.ToString(element.getAttribute(attributeName));

                        convertedAttribute.StoreInUserVariable(sender, variableName);

                        break;
                    }
                }

                if (!qualifyingElementFound)
                {
                    throw new Exception("Could not find the element!");
                }
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        private bool FindQualifyingElement(EnumerableRowCollection<DataRow> elementSearchProperties, IHTMLElement element)
        {
            foreach (DataRow seachCriteria in elementSearchProperties)
            {
                string searchPropertyName = seachCriteria.Field<string>("Property Name");
                string searchPropertyValue = seachCriteria.Field<string>("Property Value");
                string searchPropertyFound = seachCriteria.Field<string>("Match Found");

                searchPropertyFound = "False";

                if (element.GetType().GetProperty(searchPropertyName) == null)
                {
                    return false;
                }

                int searchValue;
                if (int.TryParse(searchPropertyValue, out searchValue))
                {
                    int elementValue = (int)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                    if (elementValue == searchValue)
                    {
                        seachCriteria.SetField<string>("Match Found", "True");
                    }
                    else
                    {
                        seachCriteria.SetField<string>("Match Found", "False");
                    }
                }
                else
                {
                    string elementValue = (string)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                    if ((elementValue != null) && (elementValue == searchPropertyValue))
                    {
                        seachCriteria.SetField<string>("Match Found", "True");
                    }
                    else
                    {
                        seachCriteria.SetField<string>("Match Found", "False");
                    }
                }
            }

            foreach (var seachCriteria in elementSearchProperties)
            {
                Console.WriteLine(seachCriteria.Field<string>("Property Value"));
            }

            return elementSearchProperties.Where(seachCriteria => seachCriteria.Field<string>("Match Found") == "True").Count() == elementSearchProperties.Count();
        }

        private int FindElementXPosition(MSHTML.IHTMLElement obj)
        {
            int curleft = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curleft += obj.offsetLeft;
                    obj = obj.offsetParent;
                }
            }

            return curleft;
        }

        public int FindElementYPosition(MSHTML.IHTMLElement obj)
        {
            int curtop = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curtop += obj.offsetTop;
                    obj = obj.offsetParent;
                }
            }

            return curtop;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: '" + v_WebAction + "', Instance Name: '" + v_InstanceName + "']";
        }

        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }

    #endregion Legacy IE Web Commands

    #region Web Selenium

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Normal")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]

        public string v_BrowserWindowOption { get; set; }

        public SeleniumBrowserCreateCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources");
            OpenQA.Selenium.Chrome.ChromeDriverService driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
            //driverService.HideCommandPromptWindow = true;

            var newSeleniumSession = new OpenQA.Selenium.Chrome.ChromeDriver(driverService, new OpenQA.Selenium.Chrome.ChromeOptions());

            if (sendingInstance.appInstances.ContainsKey(v_InstanceName))
            {
                //need to figure out how to handle multiple potential session names
                sendingInstance.appInstances.Remove(v_InstanceName);
            }

            sendingInstance.appInstances.Add(v_InstanceName, newSeleniumSession);

            //handle window type on startup - https://github.com/saucepleez/sharpRPA/issues/22
            switch (v_BrowserWindowOption)
            {
                case "Maximize":
                    newSeleniumSession.Manage().Window.Maximize();
                    break;
                case "Normal":
                case "":
                default:
                    break;
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public SeleniumBrowserNavigateURLCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateURLCommand";
            this.SelectionName = "Navigate to URL";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().GoToUrl(v_URL.ConvertToUserVariable(sender));
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate forward a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateForwardCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateForwardCommand()
        {
            this.CommandName = "WebBrowserNavigateCommand";
            this.SelectionName = "Navigate Forward";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Forward();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate backwards in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateBackCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateBackCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateBackCommand";
            this.SelectionName = "Navigate Back";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Back();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to refresh a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserRefreshCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserRefreshCommand()
        {
            this.CommandName = "SeleniumBrowserRefreshCommand";
            this.SelectionName = "Refresh";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Refresh();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserCloseCommand()
        {
            this.CommandName = "SeleniumBrowserCloseCommand";
            this.SelectionName = "Close Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Quit();
                seleniumInstance.Dispose();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Class Name")]
        public string v_SeleniumSearchType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Parameter")]
        public string v_SeleniumSearchParameter { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Element Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Invoke Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Attribute")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Wait For Element To Exist")]
        public string v_SeleniumElementAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        public DataTable v_WebActionParameterTable { get; set; }
        public SeleniumBrowserElementActionCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Element Action";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;

            this.v_WebActionParameterTable = new System.Data.DataTable();
            this.v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            //convert to user variable -- https://github.com/saucepleez/sharpRPA/issues/22
            v_SeleniumSearchParameter = v_SeleniumSearchParameter.ConvertToUserVariable(sender);

            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;

                OpenQA.Selenium.IWebElement element = null;

                if (v_SeleniumElementAction == "Wait For Element To Exist")
                {
                    int timeOut = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    var timeToEnd = DateTime.Now.AddSeconds(timeOut);

                    while (timeToEnd >= DateTime.Now)
                    {
                        try
                        {
                            element = FindElement(seleniumInstance);
                            break;
                        }
                        catch (Exception)
                        {
                            sendingInstance.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    if (element == null)
                    {
                        throw new Exception("Element Not Found");
                    }

                    return;
                }
                else
                {

                    element = FindElement(seleniumInstance);
                }



                switch (v_SeleniumSearchType)
                {
                    case "Find Element By XPath":
                        element = seleniumInstance.FindElementByXPath(v_SeleniumSearchParameter);
                        break;

                    case "Find Element By ID":
                        element = seleniumInstance.FindElementById(v_SeleniumSearchParameter);
                        break;

                    case "Find Element By Name":
                        element = seleniumInstance.FindElementByName(v_SeleniumSearchParameter);
                        break;

                    case "Find Element By Tag Name":
                        element = seleniumInstance.FindElementByTagName(v_SeleniumSearchParameter);
                        break;

                    case "Find Element By Class Name":
                        element = seleniumInstance.FindElementByClassName(v_SeleniumSearchParameter);
                        break;

                    default:
                        throw new Exception("Search Type was not found");
                }

                switch (v_SeleniumElementAction)
                {
                    case "Invoke Click":
                        element.Click();
                        break;

                    case "Left Click":
                    case "Right Click":
                    case "Middle Click":

                        int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());

                        int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());

                        var elementLocation = element.Location;
                        SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                        var seleniumWindowPosition = seleniumInstance.Manage().Window.Position;
                        newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X + 30 + userXAdjust); // added 30 for offset
                        newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y + 130 + userYAdjust); //added 130 for offset
                        newMouseMove.v_MouseClick = v_SeleniumElementAction;
                        newMouseMove.RunCommand(sender);
                        break;

                    case "Set Text":

                        string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Text To Set"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string[] potentialKeyPresses = textToSet.Split('{', '}');

                        Type seleniumKeys = typeof(OpenQA.Selenium.Keys);
                        System.Reflection.FieldInfo[] fields = seleniumKeys.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                        //check if chunked string contains a key press command like {ENTER}
                        foreach (string chunkedString in potentialKeyPresses)
                        {
                            if (chunkedString == "")
                                continue;

                            if (fields.Any(f => f.Name == chunkedString))
                            {
                                string keyPress = (string)fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
                                seleniumInstance.Keyboard.PressKey(keyPress);
                            }
                            else
                            {
                                //convert to user variable - https://github.com/saucepleez/sharpRPA/issues/22
                                var convertedChunk = chunkedString.ConvertToUserVariable(sender);
                                element.SendKeys(convertedChunk);
                            }
                        }

                        break;

                    case "Get Text":
                    case "Get Attribute":

                        string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string elementValue;
                        if (v_SeleniumElementAction == "Get Text")
                        {
                            elementValue = element.Text;
                        }
                        else
                        {
                            elementValue = element.GetAttribute(attributeName);
                        }

                        elementValue.StoreInUserVariable(sender, variableName);

                        break;

                    default:
                        throw new Exception("Element Action was not found");
                }
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        private OpenQA.Selenium.IWebElement FindElement(OpenQA.Selenium.Chrome.ChromeDriver seleniumInstance)
        {
            OpenQA.Selenium.IWebElement element = null;

            switch (v_SeleniumSearchType)
            {
                case "Find Element By XPath":
                    element = seleniumInstance.FindElementByXPath(v_SeleniumSearchParameter);
                    break;

                case "Find Element By ID":
                    element = seleniumInstance.FindElementById(v_SeleniumSearchParameter);
                    break;

                case "Find Element By Name":
                    element = seleniumInstance.FindElementByName(v_SeleniumSearchParameter);
                    break;

                case "Find Element By Tag Name":
                    element = seleniumInstance.FindElementByTagName(v_SeleniumSearchParameter);
                    break;

                case "Find Element By Class Name":
                    element = seleniumInstance.FindElementByClassName(v_SeleniumSearchParameter);
                    break;

                default:
                    throw new Exception("Search Type was not found");
            }

            return element;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }
    }

    #endregion Web Selenium

    #region Misc Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class PauseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Amount of time to pause for (in milliseconds).")]
        public int v_PauseLength { get; set; }

        public PauseCommand()
        {
            this.CommandName = "PauseCommand";
            this.SelectionName = "Pause Script";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(v_PauseLength);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Wait for " + v_PauseLength + "ms]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Action On Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Stop Processing")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue Processing")]
        public string v_ErrorHandlingAction { get; set; }

        public ErrorHandlingCommand()
        {
            this.CommandName = "ErrorHandlingCommand";
            this.SelectionName = "Error Handling";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            UI.Forms.frmScriptEngine engineForm = (UI.Forms.frmScriptEngine)sender;
            engineForm.errorHandling = this;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_ErrorHandlingAction + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the configuration.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    public class CommentCommand : ScriptCommand
    {
        public CommentCommand()
        {
            this.CommandName = "CommentCommand";
            this.SelectionName = "Add Code Comment";
            this.DisplayForeColor = System.Drawing.Color.ForestGreen;
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "// Comment: " + this.v_Comment;
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a MessageBox and supports variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class MessageBoxCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the message to be displayed.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Message { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        public int v_AutoCloseAfter { get; set; }
        public MessageBoxCommand()
        {
            this.CommandName = "MessageBoxCommand";
            this.SelectionName = "Show Message";
            this.CommandEnabled = true;
            this.v_AutoCloseAfter = 0;
        }

        public override void RunCommand(object sender)
        {
            UI.Forms.frmScriptEngine engineForm = (UI.Forms.frmScriptEngine)sender;
           string variableMessage = v_Message.ConvertToUserVariable(sender);

            var result = engineForm.Invoke(new Action(() =>
            {
                engineForm.ShowMessage(variableMessage, "MessageBox Command", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, v_AutoCloseAfter);
            }

            ));
            //System.Windows.Forms.MessageBox.Show(ConvertToUserVariabledText, "Message Box Command", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Message: " + v_Message + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command activates a window and brings it to the front.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetForegroundWindow', 'ShowWindow' from user32.dll to achieve automation.")]
    public class ActivateWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }

        public ActivateWindowCommand()
        {
            this.CommandName = "ActivateWindowCommand";
            this.SelectionName = "Activate Window";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);
            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowState(hWnd, User32Functions.WindowState.SW_SHOWNORMAL);
                User32Functions.SetForegroundWindow(hWnd);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the X position to move the window to.")]
        public int v_XWindowPosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Y position to move the window to.")]
        public int v_YWindowPosition { get; set; }

        public MoveWindowCommand()
        {
            this.CommandName = "MoveWindowCommand";
            this.SelectionName = "Move Window";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowPosition(hWnd, v_XWindowPosition, v_YWindowPosition);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window width")]
        public int v_XWindowSize { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window height")]
        public int v_YWindowSize { get; set; }

        public ResizeWindowCommand()
        {
            this.CommandName = "ResizeWindowCommand";
            this.SelectionName = "Resize Window";

            //not working
            //this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowSize(hWnd, v_XWindowSize, v_YWindowSize);
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Size (" + v_XWindowSize + "," + v_YWindowSize + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SendMessage' from user32.dll to achieve automation.")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }

        public CloseWindowCommand()
        {
            this.CommandName = "CloseWindowCommand";
            this.SelectionName = "Close Window";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero)
            {
                User32Functions.CloseWindow(hWnd);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command sets a target windows state (minimize, maximize, restore)")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Minimize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Restore")]
        public string v_WindowState { get; set; }

        public SetWindowStateCommand()
        {
            this.CommandName = "SetWindowStateCommand";
            this.SelectionName = "Set Window State";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            IntPtr hWnd = User32Functions.FindWindow(v_WindowName);

            if (hWnd != IntPtr.Zero) //If found
            {
                User32Functions.WindowState WINDOW_STATE = User32Functions.WindowState.SW_SHOWNORMAL;
                switch (v_WindowState)
                {
                    case "Maximize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MAXIMIZE;
                        break;

                    case "Minimize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MINIMIZE;
                        break;

                    case "Restore":
                        WINDOW_STATE = User32Functions.WindowState.SW_RESTORE;
                        break;

                    default:
                        break;
                }

                User32Functions.SetWindowState(hWnd, WINDOW_STATE);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Window State: " + v_WindowState + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command waits for a window to exist")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class WaitForWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Seconds To Wait")]
        public string v_LengthToWait { get; set; }

        public WaitForWindowCommand()
        {
            this.CommandName = "WaitForWindowCommand";
            this.SelectionName = "Wait For Window To Exist";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            var waitUntil = int.Parse(v_LengthToWait.ConvertToUserVariable(sender));
            var endDateTime = DateTime.Now.AddSeconds(waitUntil);

            IntPtr hWnd = IntPtr.Zero;

            while (DateTime.Now < endDateTime)
            {
                hWnd = User32Functions.FindWindow(v_WindowName);

                if (hWnd != IntPtr.Zero) //If found
                    break;

                System.Threading.Thread.Sleep(1000);

            }

            if (hWnd == IntPtr.Zero)
            {
                throw new Exception("Window was not found in the allowed time!");
            }




        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_WindowName + "', Wait Up To " + v_LengthToWait + " seconds]";
        }

    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process. You can use short names 'chrome.exe' or fully qualified names 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the name or path to the program (ex. notepad, calc)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ProgramName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter any arguments (if applicable)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ProgramArgs { get; set; }

        public StartProcessCommand()
        {
            this.CommandName = "StartProcessCommand";
            this.SelectionName = "Start Process";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            v_ProgramName = v_ProgramName.ConvertToUserVariable(sender);
            v_ProgramArgs = v_ProgramArgs.ConvertToUserVariable(sender);

            if (v_ProgramArgs == "")
            {
                System.Diagnostics.Process.Start(v_ProgramName);
            }
            else
            {
                System.Diagnostics.Process.Start(v_ProgramName, v_ProgramArgs);
            }

            System.Threading.Thread.Sleep(2000);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process. You can use the name of the process 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class StopProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the process name to be stopped (calc, notepad)")]
        public string v_ProgramShortName { get; set; }

        public StopProcessCommand()
        {
            this.CommandName = "StopProgramCommand";
            this.SelectionName = "Stop Process";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(v_ProgramShortName);

            foreach (var prc in processes)
                prc.CloseMainWindow();
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramShortName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the script")]
        public string v_ScriptPath { get; set; }

        public RunScriptCommand()
        {
            this.CommandName = "RunScriptCommand";
            this.SelectionName = "Run Script";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            {
                System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
                scriptProc.StartInfo.FileName = v_ScriptPath;
                scriptProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                scriptProc.Start();
                scriptProc.WaitForExit();

                scriptProc.Close();
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Script Path: " + v_ScriptPath + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class VariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input to be set to above variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Input { get; set; }
        public VariableCommand()
        {
            this.CommandName = "VariableCommand";
            this.SelectionName = "Set Variable";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var requiredVariable = sendingInstance.variableList.Where(var => var.variableName == v_userVariableName).FirstOrDefault();



            if (requiredVariable != null)
            {
                requiredVariable.variableValue = v_Input.ConvertToUserVariable(sender);
            }
            else
            {
                throw new Exception("Variable was not found. Enclose variables within brackets, ex. [vVariable]");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_Input + "' to Variable '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Clipboard Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get text from the clipboard.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    public class ClipboardGetTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to set clipboard contents")]
        public string v_userVariableName { get; set; }

        public ClipboardGetTextCommand()
        {
            this.CommandName = "ClipboardCommand";
            this.SelectionName = "Get Text";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            User32Functions.GetClipboardText().StoreInUserVariable(sender, v_userVariableName);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Text From Clipboard and Apply to Variable: " + v_userVariableName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to send email using SMTP.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the System.Net Namespace to achieve automation")]
    public class SMTPSendEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Host Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPHost { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Port")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public int v_SMTPPort { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Username")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPUserName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPPassword { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("From Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPFromEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("To Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPToEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Subject")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPSubject { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Body")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPBody { get; set; }
        public SMTPSendEmailCommand()
        {
            this.CommandName = "SMTPCommand";
            this.SelectionName = "Send SMTP Email";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            string varSMTPHost = v_SMTPHost.ConvertToUserVariable(sender);
            string varSMTPPort = v_SMTPPort.ToString().ConvertToUserVariable(sender);
            string varSMTPUserName = v_SMTPUserName.ConvertToUserVariable(sender);
            string varSMTPPassword = v_SMTPPassword.ConvertToUserVariable(sender);

            string varSMTPFromEmail = v_SMTPFromEmail.ConvertToUserVariable(sender);
            string varSMTPToEmail = v_SMTPToEmail.ConvertToUserVariable(sender);
            string varSMTPSubject = v_SMTPSubject.ConvertToUserVariable(sender);
            string varSMTPBody = v_SMTPBody.ConvertToUserVariable(sender);

            var client = new SmtpClient(varSMTPHost, int.Parse(varSMTPPort));
            client.Credentials = new System.Net.NetworkCredential(varSMTPUserName, varSMTPPassword);
            client.EnableSsl = true;

            client.Send(varSMTPFromEmail, varSMTPToEmail, varSMTPSubject, varSMTPBody);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [To Address: '" + v_SMTPToEmail + "']";
        }
    }

    #endregion Misc Commands

    #region Input Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to send key strokes to the current or a targeted window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    public class SendKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to send")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSend { get; set; }

        public SendKeysCommand()
        {
            this.CommandName = "SendKeysCommand";
            this.SelectionName = "Send Keystrokes";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            if (v_WindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand();
                activateWindow.v_WindowName = v_WindowName;
                activateWindow.RunCommand(sender);
            }

            v_TextToSend = v_TextToSend.ConvertToUserVariable(sender);
            System.Windows.Forms.SendKeys.SendWait(v_TextToSend);

            System.Threading.Thread.Sleep(500);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_TextToSend + "' to '" + v_WindowName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to simulate mouse movement and click the mouse on coordinates.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the X position to move the mouse to")]
        public int v_XMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Y position to move the mouse to")]
        public int v_YMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            this.CommandName = "SendMouseMoveCommand";
            this.SelectionName = "Send Mouse Move";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            User32Functions.SetCursorPosition(v_XMousePosition, v_YMousePosition);
            User32Functions.SendMouseClick(v_MouseClick, v_XMousePosition, v_YMousePosition);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command clicks an item in a Thick Application window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a SendMouseMove Command to click and achieve automation")]
    public class ThickAppClickItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        public string v_AutomationWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        public string v_AutomationHandleName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        public string v_MouseClick { get; set; }

        public ThickAppClickItemCommand()
        {
            this.CommandName = "ThickAppClickItemCommand";
            this.SelectionName = "Click UI Item";
            this.CommandEnabled = true;
            this.DefaultPause = 3000;
        }

        public override void RunCommand(object sender)
        {
            var searchItem = AutomationElement.RootElement.FindFirst
            (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
            v_AutomationWindowName));

            if (searchItem == null)
            {
                throw new Exception("Window not found");
            }

            var requiredItem = searchItem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, v_AutomationHandleName));

            var newActivateWindow = new ActivateWindowCommand();
            newActivateWindow.v_WindowName = v_AutomationWindowName;
            newActivateWindow.RunCommand(sender);

            //get newpoint for now
            var newPoint = requiredItem.GetClickablePoint();

            //send mousemove command
            var newMouseMove = new SendMouseMoveCommand();
            newMouseMove.v_XMousePosition = (int)newPoint.X;
            newMouseMove.v_YMousePosition = (int)newPoint.Y;
            newMouseMove.v_MouseClick = v_MouseClick;
            newMouseMove.RunCommand(sender);
        }

        public List<string> FindHandleObjects(string windowTitle)
        {
            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            var searchItems = automationElement.FindAll(TreeScope.Descendants, PropertyCondition.TrueCondition);

            List<String> handleList = new List<String>();
            foreach (AutomationElement item in searchItems)
            {
                if (item.Current.Name.Trim() != string.Empty)
                    handleList.Add(item.Current.Name);
            }
            // handleList = handleList.OrderBy(x => x).ToList();

            return handleList;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Perform " + v_MouseClick + " on '" + v_AutomationHandleName + "' in Window '" + v_AutomationWindowName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a Thick Application window and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    public class ThickAppGetTextCommand : ScriptCommand
    {
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        public string v_AutomationWindowName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        public string v_AutomationHandleDisplayName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Automation ID of the Item")]
        public string v_AutomationID { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        public string v_userVariableName { get; set; }

        public ThickAppGetTextCommand()
        {
            this.CommandName = "ThickAppGetTextCommand";
            this.SelectionName = "Get UI Item";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var searchItem = AutomationElement.RootElement.FindFirst
            (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
            v_AutomationWindowName));

            if (searchItem == null)
            {
                throw new Exception("Window not found");
            }

            var requiredItem = searchItem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, v_AutomationID));

            var newVariableCommand = new Core.AutomationCommands.VariableCommand();
            newVariableCommand.v_userVariableName = v_userVariableName;
            newVariableCommand.v_Input = requiredItem.Current.Name;
            newVariableCommand.RunCommand(sender);
        }

        public string FindHandleID(string windowTitle, string nameProperty)
        {
            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            var requiredItem = automationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, nameProperty));

            return requiredItem.Current.AutomationId;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Variable [" + v_userVariableName + "] From ID " + v_AutomationID + " (" + v_AutomationHandleDisplayName + ") in Window '" + v_AutomationWindowName + "']";
        }
    }

    #endregion Input Commands

    #region Loop Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select style of loop to perform")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Loop Number Of Times")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Loop Through List")]
        public string v_LoopType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Loop Parameter (Number or Variable List Name)")]
        public string v_LoopParameter { get; set; }

        public BeginLoopCommand()
        {
            this.CommandName = "BeginLoopCommand";
            this.SelectionName = "Begin Loop";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand, System.ComponentModel.BackgroundWorker bgw)
        {
            Core.AutomationCommands.BeginLoopCommand loopCommand = (Core.AutomationCommands.BeginLoopCommand)parentCommand.ScriptCommand;
            var engineForm = (UI.Forms.frmScriptEngine)sender;

            int loopTimes;
            Script.ScriptVariable complexVarible = null;
            if (v_LoopType == "Loop Number Of Times")
            {
                loopTimes = int.Parse(loopCommand.v_LoopParameter);
            }
            else
            {
                complexVarible = engineForm.variableList.Where(x => x.variableName == v_LoopParameter).FirstOrDefault();
                var listToLoop = (List<string>)complexVarible.variableValue;
                loopTimes = listToLoop.Count();
            }

            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVarible != null)
                    complexVarible.currentPosition = i;

                bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber });

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (bgw.CancellationPending)
                        return;
                    engineForm.ExecuteCommand(cmd, bgw);
                }

                bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Finished Loop From Line " + loopCommand.LineNumber });
            }
        }

        public override string GetDisplayValue()
        {
            if (v_LoopType == "Loop Number Of Times")
            {
                return "Loop " + v_LoopParameter + " Times";
            }
            else
            {
                return "Loop List Variable '" + v_LoopParameter + "'";
            }
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the exit point of looped (repeated) actions.  Required for all loops.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the serializer to signify the end point of a loop.")]
    public class EndLoopCommand : ScriptCommand
    {
        public EndLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "EndLoopCommand";
            this.SelectionName = "End Loop";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "End Loop";
        }
    }

    #endregion Loop Commands

    #region Excel Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to open the Excel Application.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public ExcelCreateApplicationCommand()
        {
            this.CommandName = "ExcelOpenApplicationCommand";
            this.SelectionName = "Create Excel Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            var newExcelSession = new Microsoft.Office.Interop.Excel.Application();
            newExcelSession.Visible = true;
            sendingInstance.appInstances.Add(v_InstanceName, newExcelSession);
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        public ExcelOpenWorkbookCommand()
        {
            this.CommandName = "ExcelOpenWorkbookCommand";
            this.SelectionName = "Open Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Open(v_FilePath);
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add a new Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelAddWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public ExcelAddWorkbookCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Add Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Add();
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to move to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGoToCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        public string v_CellLocation { get; set; }
        public ExcelGoToCellCommand()
        {
            this.CommandName = "ExcelGoToCellCommand";
            this.SelectionName = "Go To Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[v_CellLocation].Select();
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Go To: '" + v_CellLocation + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set the value of a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ExcelCellAddress { get; set; }
        public ExcelSetCellCommand()
        {
            this.CommandName = "ExcelSetCellCommand";
            this.SelectionName = "Set Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                v_ExcelCellAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
                v_TextToSet = v_TextToSet.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[v_ExcelCellAddress].Value = v_TextToSet;
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Cell '" + v_ExcelCellAddress + "' to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelGetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ExcelCellAddress { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        public string v_userVariableName { get; set; }

        public ExcelGetCellCommand()
        {
            this.CommandName = "ExcelGetCellCommand";
            this.SelectionName = "Get Cell";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                v_ExcelCellAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                var cellValue = (string)excelSheet.Range[v_ExcelCellAddress].Value;
                cellValue.StoreInUserVariable(sender, v_userVariableName);
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Value From '" + v_ExcelCellAddress + "' and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a macro in an Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelRunMacroCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the macro name")]
        public string v_MacroName { get; set; }
        public ExcelRunMacroCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Run Macro";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Run(v_MacroName);
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if the Workbook should be saved")]
        public bool v_ExcelSaveOnExit { get; set; }
        public ExcelCloseApplicationCommand()
        {
            this.CommandName = "ExcelCloseApplicationCommand";
            this.SelectionName = "Close Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            object excelObject;
            var sendingInstance = (UI.Forms.frmScriptEngine)sender;
            if (sendingInstance.appInstances.TryGetValue(v_InstanceName, out excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.ActiveWorkbook.Close(v_ExcelSaveOnExit);
                excelInstance.Quit();
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save On Close: " + v_ExcelSaveOnExit + ", Instance Name: '" + v_InstanceName + "']";
        }
    }

    #endregion Excel Commands

    #region String Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("String Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to trim a string")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringSubstringCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Start from Position")]
        public int v_startIndex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Length (-1 to keep remainder)")]
        public int v_stringLength { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the changes")]
        public string v_applyToVariableName { get; set; }
        public StringSubstringCommand()
        {
            this.CommandName = "StringSubstringCommand";
            this.SelectionName = "Substring";
            this.CommandEnabled = true;
            v_stringLength = -1;
        }
        public override void RunCommand(object sender)
        {
            v_userVariableName = v_userVariableName.ConvertToUserVariable(sender);

            //apply substring
            if (v_stringLength >= 0)
            {
                v_userVariableName = v_userVariableName.Substring(v_startIndex, v_stringLength);
            }
            else
            {
                v_userVariableName = v_userVariableName.Substring(v_startIndex);
            }

            v_userVariableName.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Substring to '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("String Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to split a string")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    public class StringSplitCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to split")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Input Delimiter")]
        public string v_splitCharacter { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the list variable which will contain the results")]
        public string v_applyConvertToUserVariableName { get; set; }
        public StringSplitCommand()
        {
            this.CommandName = "StringSplitCommand";
            this.SelectionName = "Split";
            this.v_applyConvertToUserVariableName = "default";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var stringVariable = v_userVariableName.ConvertToUserVariable(sender);

            List<string> splitString;
            if (v_splitCharacter == "[crLF]")
            {
                splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
            else
            {
                splitString = stringVariable.Split(new string[] { v_splitCharacter }, StringSplitOptions.None).ToList();
            }

            var sendingInstance = (UI.Forms.frmScriptEngine)sender;

            //get complex variable from engine and assign
            var requiredComplexVariable = sendingInstance.variableList.Where(x => x.variableName == v_applyConvertToUserVariableName).FirstOrDefault();
            requiredComplexVariable.variableValue = splitString;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Split '" + v_userVariableName + "' by '" + v_splitCharacter + "' and apply to '" + v_applyConvertToUserVariableName + "']";
        }
    }

    #endregion String Commands

    #region If Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true.  Any 'BeginIf' command must have a following 'EndIf' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    public class BeginIfCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of If Command")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Value")]
        public string v_IfActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        public DataTable v_IfActionParameterTable { get; set; }

        public BeginIfCommand()
        {
            this.CommandName = "BeginIfCommand";
            this.SelectionName = "Begin If";
            this.CommandEnabled = true;

            //define parameter table
            this.v_IfActionParameterTable = new System.Data.DataTable();
            this.v_IfActionParameterTable.TableName = DateTime.Now.ToString("IfActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_IfActionParameterTable.Columns.Add("Parameter Name");
            this.v_IfActionParameterTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand, System.ComponentModel.BackgroundWorker bgw)
        {
            var engineForm = (UI.Forms.frmScriptEngine)sender;

            if (v_IfActionType == "Value")
            {
                string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                value1 = value1.ConvertToUserVariable(sender);
                value2 = value2.ConvertToUserVariable(sender);

                bool ifResult = false;

                decimal cdecValue1, cdecValue2;

                switch (operand)
                {
                    case "is equal to":
                        ifResult = (value1 == value2);
                        break;

                    case "is not equal to":
                        ifResult = (value1 != value2);
                        break;

                    case "is greater than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 > cdecValue2);
                        break;

                    case "is greater than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 >= cdecValue2);
                        break;

                    case "is less than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 < cdecValue2);
                        break;

                    case "is less than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 <= cdecValue2);
                        break;
                }

                int startIndex, endIndex, elseIndex;
                if (parentCommand.AdditionalScriptCommands.Any(item => item.ScriptCommand is Core.AutomationCommands.ElseCommand))
                {
                    elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is Core.AutomationCommands.ElseCommand);

                    if (ifResult)
                    {
                        startIndex = 0;
                        endIndex = elseIndex;
                    }
                    else
                    {
                        startIndex = elseIndex + 1;
                        endIndex = parentCommand.AdditionalScriptCommands.Count;
                    }
                }
                else if (ifResult)
                {
                    startIndex = 0;
                    endIndex = parentCommand.AdditionalScriptCommands.Count;
                }
                else
                {
                    return;
                }

                for (int i = startIndex; i < endIndex; i++)
                {
                    if (bgw.CancellationPending)
                        return;
                    engineForm.ExecuteCommand(parentCommand.AdditionalScriptCommands[i], bgw);
                }
            }
        }

        public override string GetDisplayValue()
        {
            switch (v_IfActionType)
            {
                case "Value":

                    string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value1"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Operand"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value2"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (" + value1 + " " + operand + " " + value2 + ")";

                default:
                    break;
            }

            return "";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the exit point of If actions.  Required for all Begin Ifs.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the serializer to signify the end point of an if.")]
    public class EndIfCommand : ScriptCommand
    {
        public EndIfCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "EndIfCommand";
            this.SelectionName = "End If";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "End If";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("TBD")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class ElseCommand : ScriptCommand
    {
        public ElseCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "ElseCommand";
            this.SelectionName = "Else";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "Else";
        }
    }

    #endregion If Commands

    #region OCR and Image Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to covert an image file into text for parsing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command has a dependency on and implements OneNote OCR to achieve automation.")]
    public class OCRCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Image to OCR")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply OCR Result To Variable")]
        public string v_userVariableName { get; set; }
        public OCRCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "OCRCommand";
            this.SelectionName = "Perform OCR";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engineForm = (UI.Forms.frmScriptEngine)sender;

            var ocrEngine = new OneNoteOCRDll.OneNoteOCR();
            var arr = ocrEngine.OcrTexts(v_FilePath.ConvertToUserVariable(engineForm)).ToArray();

            string endResult = "";
            foreach (var text in arr)
            {
                endResult += text.Text;
            }

            //apply to user variable
            endResult.StoreInUserVariable(sender, v_userVariableName);
        }

        public override string GetDisplayValue()
        {
            return "OCR '" + v_FilePath + "' and apply result to '" + v_userVariableName + "'";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command takes a screenshot and saves it to a location")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    public class ScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        public string v_ScreenshotWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to save the image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        public ScreenshotCommand()
        {
            this.CommandName = "ScreenshotCommand";
            this.SelectionName = "Take Screenshot";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var image = User32Functions.CaptureWindow(v_ScreenshotWindowName);
            string ConvertToUserVariabledString = v_FilePath.ConvertToUserVariable(sender);
            image.Save(ConvertToUserVariabledString);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_ScreenshotWindowName + "', File Path: '" + v_FilePath + "]";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command attempts to find an existing image on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class ImageRecognitionCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Capture the search image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper)]
        public string v_ImageCapture { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Offset X Coordinate - Optional")]
        public int v_xOffsetAdjustment { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Offset Y Coordinate - Optional")]
        public int v_YOffsetAdjustment { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        public string v_MouseClick { get; set; }
        public ImageRecognitionCommand()
        {
            this.CommandName = "ImageRecognitionCommand";
            this.SelectionName = "Image - Image Recognition";
            this.CommandEnabled = true;

            v_xOffsetAdjustment = 0;
            v_YOffsetAdjustment = 0;
        }
        public override void RunCommand(object sender)
        {
            bool testMode = false;
            if (sender is UI.Forms.frmCommandEditor)
            {
                testMode = true;
            }


            //user image to bitmap
            Bitmap userImage = new Bitmap(Core.Common.Base64ToImage(v_ImageCapture));

            //take screenshot
            Size shotSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            Point upperScreenPoint = new Point(0, 0);
            Point upperDestinationPoint = new Point(0, 0);
            Bitmap desktopImage = new Bitmap(shotSize.Width, shotSize.Height);
            Graphics graphics = Graphics.FromImage(desktopImage);
            graphics.CopyFromScreen(upperScreenPoint, upperDestinationPoint, shotSize);

            //create desktopOutput file
            Bitmap desktopOutput = new Bitmap(desktopImage);

            //get graphics for drawing on output file
            Graphics screenShotUpdate = Graphics.FromImage(desktopOutput);

            //declare maximum boundaries
            int userImageMaxWidth = userImage.Width - 1;
            int userImageMaxHeight = userImage.Height - 1;
            int desktopImageMaxWidth = desktopImage.Width - 1;
            int desktopImageMaxHeight = desktopImage.Height - 1;


            //get corners and center of user image to develop 'fingerprint'
            List<ImageRecognitionFingerPrint> userImageFingerprint = new List<ImageRecognitionFingerPrint>();
            //top left
            userImageFingerprint.Add(new ImageRecognitionFingerPrint() { xLocation = 0, yLocation = 0 });
            //top right
            userImageFingerprint.Add(new ImageRecognitionFingerPrint() { xLocation = userImageMaxWidth, yLocation = 0 });
            //bottom left
            userImageFingerprint.Add(new ImageRecognitionFingerPrint() { xLocation = 0, yLocation = userImageMaxHeight });
            //bottomright
            userImageFingerprint.Add(new ImageRecognitionFingerPrint() { xLocation = userImageMaxWidth, yLocation = userImageMaxHeight });
            //center
            userImageFingerprint.Add(new ImageRecognitionFingerPrint() { xLocation = (userImageMaxWidth / 2), yLocation = (userImageMaxHeight / 2) });



            //find source colors of each 'fingerprint'
            foreach (var fingerPrint in userImageFingerprint)
            {
                fingerPrint.PixelColor = userImage.GetPixel(fingerPrint.xLocation, fingerPrint.yLocation);
            }

            bool imageFound = false;

            //for each row on the screen
            for (int rowPixel = 0; rowPixel < desktopImageMaxWidth; rowPixel++)
            {

                //for each column on screen
                for (int columnPixel = 0; columnPixel < desktopImageMaxHeight; columnPixel++)
                {

                    try
                    {




                    //get the current pixel from current row and column
                    // userImageFingerPrint.First() for now will always be from top left (0,0)
                    var currentPixel = desktopImage.GetPixel(rowPixel + userImageFingerprint.First().xLocation, columnPixel + userImageFingerprint.First().yLocation);

                    //compare to see if desktop pixel matches top left pixel from user image
                    if (currentPixel == userImageFingerprint.First().PixelColor)
                    {

                            //look through each item in the fingerprint to see if offset pixel colors match
                            int matchCount = 0;
                        for (int item = 0; item < userImageFingerprint.Count; item++)
                        {
                            //find pixel color from offset X,Y relative to current position of row and column
                            currentPixel = desktopImage.GetPixel(rowPixel + userImageFingerprint[item].xLocation, columnPixel + userImageFingerprint[item].yLocation);

                            //if color matches
                            if (userImageFingerprint[item].PixelColor == currentPixel)
                            {
                                    matchCount++;

                                //draw on output to demonstrate finding
                                if (testMode)
                                    screenShotUpdate.DrawRectangle(Pens.Blue, rowPixel + userImageFingerprint[item].xLocation, columnPixel + userImageFingerprint[item].yLocation, 5, 5);
                            }
                            else
                            {
                                //mismatch in the pixel series, not a series of matching coordinate
                                //?add threshold %?
                                imageFound = false;

                                //draw on output to demonstrate finding
                                if (testMode)
                                    screenShotUpdate.DrawRectangle(Pens.OrangeRed, rowPixel + userImageFingerprint[item].xLocation, columnPixel + userImageFingerprint[item].yLocation, 5, 5);
                            }

                        }

                        if (matchCount == userImageFingerprint.Count())
                        {
                                imageFound = true;

                                var topLeftX = rowPixel + userImageFingerprint.First().xLocation;
                                var topLeftY = columnPixel + userImageFingerprint.First().yLocation;

                                if (testMode)
                                {
                                    //draw on output to demonstrate finding
                                    var Rectangle = new Rectangle(topLeftX, topLeftY, userImageMaxWidth, userImageMaxHeight);
                                    Brush brush = new SolidBrush(Color.ForestGreen);
                                    screenShotUpdate.FillRectangle(brush, Rectangle);
                                }

                                //move mouse to position
                                var mouseMove = new SendMouseMoveCommand();
                                mouseMove.v_XMousePosition = topLeftX + (v_xOffsetAdjustment);
                                mouseMove.v_YMousePosition = topLeftY + (v_xOffsetAdjustment);
                                mouseMove.v_MouseClick = v_MouseClick;

                                mouseMove.RunCommand(sender);


                        }



                    }


                        if (imageFound)
                            break;

                    }
                    catch (Exception ex)
                    {
                     //continue
                    }
                }


                if (imageFound)
                    break;
            }

            //write output


            if (!imageFound)
            {
                throw new Exception("Image was not found on screen");
            }




            if (testMode)
            {
                //screenShotUpdate.FillRectangle(Brushes.White, 5, 20, 275, 105);
                //screenShotUpdate.DrawString("Blue = Matching Point", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 20);
                //screenShotUpdate.DrawString("OrangeRed = Mismatched Point", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 60);
                //screenShotUpdate.DrawString("Green Rectangle = Match Area", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 100);

                UI.Forms.Supplement_Forms.frmImageCapture captureOutput = new UI.Forms.Supplement_Forms.frmImageCapture();
                captureOutput.pictureBox1.Image = desktopOutput;
                captureOutput.Show();
                captureOutput.TopMost = true;
                //captureOutput.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }

            graphics.Dispose();
            userImage.Dispose();
            desktopImage.Dispose();
            screenShotUpdate.Dispose();


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Find On Screen]";
        }
    }



    #endregion OCR and Image Commands

    #region HTTP Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("WebAPI Commands")]
    [Attributes.ClassAttributes.Description("This command downloads the HTML source of a web page for parsing")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements System.Web to achieve automation")]
    public class HTTPRequestCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL")]
        public string v_WebRequestURL { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        public string v_userVariableName { get; set; }

        public HTTPRequestCommand()
        {
            this.CommandName = "HTTPRequestCommand";
            this.SelectionName = "HTTP Request";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(v_WebRequestURL);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResponse = reader.ReadToEnd();

            strResponse.StoreInUserVariable(sender, v_userVariableName);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target URL: '" + v_WebRequestURL + "' and apply result to '" + v_userVariableName + "']";
        }

    }

        [Serializable]
        [Attributes.ClassAttributes.Group("WebAPI Commands")]
        [Attributes.ClassAttributes.Description("This command processes an HTML source object")]
        [Attributes.ClassAttributes.ImplementationDescription("TBD")]
        public class HTTPQueryResultCommand : ScriptCommand
        {
            [XmlAttribute]
            [Attributes.PropertyAttributes.PropertyDescription("Select variable containing HTML")]
            public string v_userVariableName { get; set; }

            [XmlAttribute]
            [Attributes.PropertyAttributes.PropertyDescription("XPath Query")]
            public string v_xPathQuery { get; set; }

            [XmlAttribute]
            [Attributes.PropertyAttributes.PropertyDescription("Apply Query Result To Variable")]
            public string v_applyToVariableName { get; set; }

            public HTTPQueryResultCommand()
            {
                this.CommandName = "HTTPRequestQueryCommand";
                this.SelectionName = "HTTP Result Query";
                this.CommandEnabled = true;
            }

            public override void RunCommand(object sender)
            {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(v_userVariableName.ConvertToUserVariable(sender));

            var div = doc.DocumentNode.SelectSingleNode(v_xPathQuery);
            string divString = div.InnerText;

            divString.StoreInUserVariable(sender, v_applyToVariableName);


        }

            public override string GetDisplayValue()
            {
                return base.GetDisplayValue() + " [Query Variable '" + v_userVariableName + "' and apply result to '" + v_applyToVariableName + "']";
            }
        }

        #endregion
    }


public class ImageRecognitionFingerPrint
{
    public int pixelID { get; set; }
    public Color PixelColor { get; set; }
    public int xLocation { get; set; }
    public int yLocation { get; set; }
    bool matchFound { get; set; }

}




