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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpRPA.UI.CustomControls
{
    #region Custom UI Components

    public class UITabControl : TabControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;

            Brush br = new SolidBrush(Color.Black);
            Brush brTab = new SolidBrush(Color.Orange);

            Rectangle TabControlArea = ClientRectangle;
            Rectangle TabArea = DisplayRectangle;

            g.FillRectangle(br, TabControlArea);
            g.FillRectangle(brTab, TabArea);

            br.Dispose();
            brTab.Dispose();

            //for (int i = 0; i < TabCount; i++)
            //    DrawTab(g, TabPages[i], i, false);

            //if (_mouseTabIndex != null && _mouseTabIndex != _mouseTabIndexSave && _mouseTabIndex != SelectedIndex)
            //    DrawTab(g, TabPages[(int)_mouseTabIndex], (int)_mouseTabIndex, true);

            //_mouseTabIndexSave = _mouseTabIndex;
        }

        internal void DrawTab(Graphics g, TabPage tabPage, int nIndex, bool mouseOverTab)
        {
            //var recBounds = GetTabRect(nIndex);

            //SetBounds(ref recBounds);
            //var pt = SetPointsForTabFill(recBounds);

            //DrawTabBounds(g, recBounds);

            //FillTabl(g, recBounds, pt, false);

            //DrawTabSeparators(g, recBounds, nIndex, 0 /*y-bottom*/);

            //if (SelectedIndex == nIndex)
            //{
            //    DrawTabGradient(g, recBounds, pt, nIndex, 0/*width*/, 1/*height*/);
            //    DrawTabSeparators(g, recBounds, nIndex, 1 /*y-bottom*/);
            //}

            //if (mouseOverTab)
            //    DrawTabGradient(g, recBounds, pt, nIndex, -2/*width*/, 0/*height*/);

            //DrawText(g, recBounds, tabPage.Text);
        }

        private void DrawText(Graphics g, Rectangle recBounds, string text)
        {
            var strFormat = new StringFormat();
            strFormat.Alignment = strFormat.LineAlignment = StringAlignment.Center;

            g.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //var fnt = new Font(MsFonts.familyPTSans, 8F, FontStyle.Regular,  GraphicsUnit.Point, (byte)204);
            var fnt = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));

            RectangleF tabTextArea = recBounds;
            var br = new SolidBrush(Color.Black);
            g.DrawString(text, fnt, br, tabTextArea);

            br.Dispose();
            strFormat.Dispose();
            fnt.Dispose();
        }
    }

    public partial class UIPictureButton : PictureBox
    {
        private bool isMouseOver;
        public bool IsMouseOver
        {
            get
            {
                return isMouseOver;
            }
            set
            {
                this.isMouseOver = value;
                this.Invalidate();
            }
        }
        private string displayText;
        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                this.displayText = value;
                this.Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (IsMouseOver)
            {
                this.Cursor = Cursors.Hand;
                this.BackColor = Color.Transparent;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
                this.BackColor = Color.Transparent;
            }

            if (this.Image != null)
                e.Graphics.DrawImage(this.Image, (this.Width / 2) - 16, 3, 32, 32);

            if (DisplayText != null)
            {
                var stringSize = e.Graphics.MeasureString(DisplayText, new Font("Segoe UI", 8, FontStyle.Regular), 200);
                e.Graphics.DrawString(DisplayText, new Font("Segoe UI", 8, FontStyle.Regular), new SolidBrush(DisplayTextBrush), ((this.Width / 2) - (stringSize.Width / 2)), this.Height - 14);
            }
        }
        private Color displayTextBrush;
        public Color DisplayTextBrush
        {
            get
            {
                return displayTextBrush;
            }
            set
            {
                displayTextBrush = value;
                this.Invalidate();
            }
        }
        public UIPictureButton()
        {
            this.Image = Properties.Resources.added;
            this.DisplayTextBrush = Color.White;
            this.Size = new Size(48, 48);
            this.DisplayText = "Text";
            this.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            this.MouseEnter += UIPictureButton_MouseEnter;
            this.MouseLeave += UIPictureButton1_MouseLeave;
        }

        private void UIPictureButton_MouseEnter(object sender, System.EventArgs e)
        {
            this.IsMouseOver = true;
        }
        private void UIPictureButton1_MouseLeave(object sender, System.EventArgs e)
        {
            this.IsMouseOver = false;
        }
    }

    public class UIElement
    {
        public string AutomationID { get; set; }
        public string ControlName { get; set; }
        public string ControlType { get; set; }
    }

    public class UIListView : ListView
    {
        public UIListView()
        {
            this.DoubleBuffered = true;
        }
    }

    public class UISplitContainer : SplitContainer
    {
        public UISplitContainer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            MethodInfo mi = typeof(Control).GetMethod("SetStyle",
              BindingFlags.NonPublic | BindingFlags.Instance);
            object[] args = new object[] { ControlStyles.OptimizedDoubleBuffer, true };
            mi.Invoke(this.Panel1, args);
            mi.Invoke(this.Panel2, args);
        }
    }

    public class UITreeView : TreeView
    {
        [System.Runtime.InteropServices.DllImport("uxtheme.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        public UITreeView()
        {
            this.DoubleBuffered = true;
            SetWindowTheme(this.Handle, "explorer", null);
        }
    }

    public class UIGroupBox : GroupBox
    {
        public UIGroupBox()
        {
            this.DoubleBuffered = true;
            this.TitleBackColor = Color.SteelBlue;
            this.TitleForeColor = Color.White;
            this.TitleFont = new Font(this.Font.FontFamily, Font.Size, FontStyle.Bold);
            this.BackColor = Color.Transparent;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GroupBoxRenderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);
            var rect = ClientRectangle;

            SolidBrush backColorBrush = new SolidBrush(TitleBackColor);
            e.Graphics.FillRectangle(backColorBrush, 0, 0, this.Width, 18);
            backColorBrush.Dispose();

            TextRenderer.DrawText(e.Graphics, Text, TitleFont, new Point(2, 2), TitleForeColor);
            //ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.SteelBlue, ButtonBorderStyle.None);
        }
        public Color TitleBackColor { get; set; }
        public HatchStyle TitleHatchStyle { get; set; }
        public Font TitleFont { get; set; }
        public Color TitleForeColor { get; set; }
    }

    public class UIFlowLayoutPanel : FlowLayoutPanel
    {
        public UIFlowLayoutPanel()
        {
            this.DoubleBuffered = true;
        }
    }


    #endregion Custom UI Components
}
namespace sharpRPA.UI
{
    public static class Images
    {
        public static Dictionary<string, Image> UIImageDictionary()
        {
            var uiImages = new Dictionary<string, Image>();
            uiImages.Add("PauseCommand", sharpRPA.Properties.Resources.pause);
            uiImages.Add("CommentCommand", sharpRPA.Properties.Resources.comment);
            uiImages.Add("ActivateWindowCommand", sharpRPA.Properties.Resources.windows);
            uiImages.Add("MoveWindowCommand", sharpRPA.Properties.Resources.windows);
            uiImages.Add("ThickAppClickItemCommand", sharpRPA.Properties.Resources.mouse);
            uiImages.Add("ThickAppGetTextCommand", sharpRPA.Properties.Resources.search);
            uiImages.Add("ResizeWindowCommand", sharpRPA.Properties.Resources.windows);
            uiImages.Add("MessageBoxCommand", sharpRPA.Properties.Resources.message);
            uiImages.Add("StopProcessCommand", sharpRPA.Properties.Resources.stop);
            uiImages.Add("StartProcessCommand", sharpRPA.Properties.Resources.start);
            uiImages.Add("VariableCommand", sharpRPA.Properties.Resources.function);
            uiImages.Add("RunScriptCommand", sharpRPA.Properties.Resources.script);
            uiImages.Add("CloseWindowCommand", sharpRPA.Properties.Resources.close);
            uiImages.Add("WebBrowserCreateCommand", sharpRPA.Properties.Resources.createbrowser16);
            uiImages.Add("WebBrowserNavigateCommand", sharpRPA.Properties.Resources.navigate);
            uiImages.Add("WebBrowserCloseCommand", sharpRPA.Properties.Resources.closebrowser16);
            uiImages.Add("WebBrowserElementCommand", sharpRPA.Properties.Resources.element);
            uiImages.Add("SendKeysCommand", sharpRPA.Properties.Resources.computer);
            uiImages.Add("SendMouseMoveCommand", sharpRPA.Properties.Resources.computer);
            uiImages.Add("SetWindowStateCommand", sharpRPA.Properties.Resources.windows);
            uiImages.Add("WebBrowserFindBrowserCommand", sharpRPA.Properties.Resources.createbrowser16);
            uiImages.Add("BeginLoopCommand", sharpRPA.Properties.Resources.loop);
            uiImages.Add("EndLoopCommand", sharpRPA.Properties.Resources.endloop);
            uiImages.Add("ClipboardGetTextCommand", sharpRPA.Properties.Resources.clipboard);
            uiImages.Add("ExcelCreateApplicationCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelOpenWorkbookCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelAddWorkbookCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelGoToCellCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelCloseApplicationCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelSetCellCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelGetCellCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("ExcelRunMacroCommand", sharpRPA.Properties.Resources.excelicon);
            uiImages.Add("SeleniumBrowserCreateCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SeleniumBrowserNavigateURLCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SeleniumBrowserNavigateForwardCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SeleniumBrowserNavigateBackCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SeleniumBrowserRefreshCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SeleniumBrowserCloseCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SeleniumBrowserElementActionCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("SMTPSendEmailCommand", sharpRPA.Properties.Resources.navigate);
            uiImages.Add("ErrorHandlingCommand", sharpRPA.Properties.Resources.error);
            uiImages.Add("StringSubstringCommand", sharpRPA.Properties.Resources._string);
            uiImages.Add("StringSplitCommand", sharpRPA.Properties.Resources._string);
            uiImages.Add("BeginIfCommand", sharpRPA.Properties.Resources.flag2);
            uiImages.Add("EndIfCommand", sharpRPA.Properties.Resources.flag2);
            uiImages.Add("ElseCommand", sharpRPA.Properties.Resources.flag3);
            uiImages.Add("ScreenshotCommand", sharpRPA.Properties.Resources.photo_camera);
            uiImages.Add("OCRCommand", sharpRPA.Properties.Resources.photo_camera);
            uiImages.Add("ImageRecognitionCommand", sharpRPA.Properties.Resources.photo_camera);
            uiImages.Add("HTTPRequestCommand", sharpRPA.Properties.Resources.web);
            uiImages.Add("HTTPQueryResultCommand", sharpRPA.Properties.Resources.search);
            return uiImages;
        }
        public static ImageList UIImageList()
        {
            Dictionary<string, Image> imageIcons = UIImageDictionary();
            ImageList uiImages = new ImageList();
            foreach (var icon in imageIcons)
            {
                uiImages.Images.Add(icon.Key, icon.Value);
            }

            return uiImages;
        }
        public static Image GetUIImage(string commandName)
        {
            var uiImageDictionary = UIImageDictionary();
            return uiImageDictionary[commandName];
        }
    }
}