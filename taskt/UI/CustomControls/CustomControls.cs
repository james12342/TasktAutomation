//Copyright (c) 2019 Jason Bayldon
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

namespace taskt.UI.CustomControls
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

    public partial class UIPanel : Panel
    {
        private taskt.Core.Theme _Theme = new taskt.Core.Theme();
        public taskt.Core.Theme Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {


            var brush = this.Theme.CreateGradient(this.ClientRectangle);
            e.Graphics.FillRectangle(brush, this.ClientRectangle);


            base.OnPaint(e);
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
        public override string Text
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
                var stringSize = e.Graphics.MeasureString(DisplayText, new Font("Segoe UI Bold", 8, FontStyle.Bold), 200);
                e.Graphics.DrawString(DisplayText, new Font("Segoe UI", 8, FontStyle.Bold), new SolidBrush(DisplayTextBrush), ((this.Width / 2) - (stringSize.Width / 2)), this.Height - 14);
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
            this.Image = JAuto.Properties.Resources.logo;
            this.DisplayTextBrush = Color.White;
            this.Size = new Size(48, 48);
            this.DisplayText = "Text";
            this.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            this.MouseEnter += UIPictureButton_MouseEnter;
            this.MouseLeave += UIPictureButton1_MouseLeave;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
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
        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = this.Scroll;
            if (handler != null) handler(this, e);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x115)
            { // Trap WM_VSCROLL
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), 0));
            }
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

    public class UIPictureBox : PictureBox
    {
        private string encodedimage;

        public string EncodedImage
        {
            get
            {
                return encodedimage;
            }
            set
            {
                encodedimage = value;
            }
        }
    }

    public class UIMenuStrip : MenuStrip
    {
        public UIMenuStrip()
        {
            //this.Renderer = new UIMenuStripRenderer();

            var renderer = new ToolStripProfessionalRenderer(new UIMenuStripColorTable());
            renderer.RenderMenuItemBackground += Renderer_RenderMenuItemBackground;
            this.Renderer = renderer;

        }

        private void Renderer_RenderMenuItemBackground(object sender, ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color c = e.Item.Selected ? Color.FromArgb(59, 59, 59) : Color.FromArgb(30, 30, 30);
            using (SolidBrush brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, rc);
        }
    }

    public class UIMenuStripColorTable : ProfessionalColorTable
    {
      
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(59, 59, 59); }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(59, 59, 59); }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(59, 59, 59); }
        }
        
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(59,59,59); }
        }
        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(30,30,30); }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(30, 30, 30); }
        }
        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(30, 30, 30); }
        }
        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color ButtonSelectedHighlight
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

    }

    #endregion Custom UI Components
}
namespace taskt.UI
{
    public static class Images
    {
        //private static Dictionary<string, Image> imageList = new Dictionary<string, Image>();
        private static ImageList uiImages = new ImageList();
        private static Dictionary<string, string> imageCommandTable = new Dictionary<string, string>()
        {
            // API
            {"ExecuteDLLCommand", "JAuto.Properties.Resources.command_run_code"},
            {"RESTCommand", "JAuto.Properties.Resources.command_run_code"},
            {"HTTPRequestCommand", "JAuto.Properties.Resources.command_web"},
            {"HTTPQueryResultCommand", "JAuto.Properties.Resources.command_search"},

            // Data
            {"ConvertListToJSONCommand", "JAuto.Properties.Resources.command_function"},
            {"DateCalculationCommand", "JAuto.Properties.Resources.command_function"},
            {"FormatDataCommand", "JAuto.Properties.Resources.command_function"},
            {"GetListCountCommand", "JAuto.Properties.Resources.command_function"},
            {"GetListItemCommand", "JAuto.Properties.Resources.command_function"},
            {"GetWordLengthCommand", "JAuto.Properties.Resources.command_function"},
            {"GetWordCountCommand", "JAuto.Properties.Resources.command_function"},
            {"LogDataCommand", "JAuto.Properties.Resources.command_files"},
            {"MathCalculationCommand", "JAuto.Properties.Resources.command_function"},
            {"ModifyVariableCommand", "JAuto.Properties.Resources.command_function"},
            {"ParseDatasetRowCommand", "JAuto.Properties.Resources.command_function"},
            {"ParseJSONArrayCommand", "JAuto.Properties.Resources.command_parse"},
            {"ParseJsonCommand", "JAuto.Properties.Resources.command_parse"},
            {"ParseJsonModelCommand", "JAuto.Properties.Resources.command_parse"},
            {"PDFTextExtractionCommand", "JAuto.Properties.Resources.command_function"},
            {"RegExExtractorCommand", "JAuto.Properties.Resources.command_function"},
            {"StringCheckStringCommand", "JAuto.Properties.Resources.command_string"},
            {"StringReplaceCommand", "JAuto.Properties.Resources.command_string"},
            {"StringSplitCommand", "JAuto.Properties.Resources.command_string"},
            {"StringSubstringCommand", "JAuto.Properties.Resources.command_string"},
            {"TextExtractorCommand", "JAuto.Properties.Resources.command_function"},

            // Database
            {"DatabaseDefineConnectionCommand", "JAuto.Properties.Resources.command_database"},
            {"DatabaseExecuteQueryCommand", "JAuto.Properties.Resources.command_database"},

            // DataTable
            {"LoadTaskCommand", "JAuto.Properties.Resources.command_start_process"},
            {"UnloadTaskCommand", "JAuto.Properties.Resources.command_stop_process"},
            {"ExcelAddWorksheetCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"AddDataRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"CreateDataTableCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"FilterDataTableCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"GetDataRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"GetDataRowCountCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"GetDataRowValueCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"RemoveDataRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"WriteDataRowValueCommand", "JAuto.Properties.Resources.command_spreadsheet"},

            // Dictionary
            {"AddDictionaryCommand", "JAuto.Properties.Resources.command_dictionary"},
            {"CreateDictionaryCommand", "JAuto.Properties.Resources.command_dictionary"},
            {"GetDictionaryValueCommand", "JAuto.Properties.Resources.command_dictionary"},
            {"LoadDictionaryCommand", "JAuto.Properties.Resources.command_dictionary"},

            // Engine
            {"ErrorHandlingCommand", "JAuto.Properties.Resources.command_error"},
            {"GetDataCommand", "JAuto.Properties.Resources.command_server"},  // get bot data
            {"PauseCommand", "JAuto.Properties.Resources.command_pause"},
            {"SetEngineDelayCommand", "JAuto.Properties.Resources.command_pause"},
            {"ShowEngineContextCommand", "JAuto.Properties.Resources.command_window"},
            {"SetEnginePreferenceCommand", "JAuto.Properties.Resources.command_window"},
            {"StopwatchCommand", "JAuto.Properties.Resources.command_stopwatch"},
            {"UploadDataCommand", "JAuto.Properties.Resources.command_server"},   // upload bot store

            // Error
            {"CatchExceptionCommand", "JAuto.Properties.Resources.command_try"},
            {"EndTryCommand", "JAuto.Properties.Resources.command_try"},
            {"FinallyCommand", "JAuto.Properties.Resources.command_try"},
            {"ThrowExceptionCommand", "JAuto.Properties.Resources.command_try"},
            {"TryCommand", "JAuto.Properties.Resources.command_try"},

            // Excel
            {"ExcelActivateSheetCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelAddWorkbookCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelAppendCellCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelAppendRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelCheckExcelInstanceExistsCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelCloseApplicationCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelCreateDatasetCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelCreateApplicationCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteCellCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteWorksheetCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelGetCellCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelGetCellRCCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelGetLastRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelGetRangeCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelGetRangeCommandAsDT", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelGoToCellCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelOpenWorkbookCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelRenameWorksheetCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelRunMacroCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelSaveCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelSaveAsCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelSetCellCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelSetCellRCCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelSplitRangeByColumnCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelWriteRangeCommand", "JAuto.Properties.Resources.command_spreadsheet"},
            {"ExcelWriteRowCommand", "JAuto.Properties.Resources.command_spreadsheet"},

            // File
            {"CheckFileExistsCommand", "JAuto.Properties.Resources.command_files"},
            {"DeleteFileCommand", "JAuto.Properties.Resources.command_files"},
            {"ExtractFileCommand", "JAuto.Properties.Resources.command_files"},
            {"GetFilesCommand", "JAuto.Properties.Resources.command_files"},
            {"MoveFileCommand", "JAuto.Properties.Resources.command_files"},
            {"RenameFileCommand", "JAuto.Properties.Resources.command_files"},
            {"WaitForFileToExistCommand", "JAuto.Properties.Resources.command_files"},

            // Folder
            {"CheckFolderExistsCommand", "JAuto.Properties.Resources.command_files"},
            {"CreateFolderCommand", "JAuto.Properties.Resources.command_files"},
            {"DeleteFolderCommand", "JAuto.Properties.Resources.command_files"},
            {"GetFoldersCommand", "JAuto.Properties.Resources.command_files"},
            {"MoveFolderCommand", "JAuto.Properties.Resources.command_files"},
            {"RenameFolderCommand", "JAuto.Properties.Resources.command_files"},

            // IE
            {"IEBrowserCloseCommand", "JAuto.Properties.Resources.command_window_close"},
            {"IEBrowserCreateCommand", "JAuto.Properties.Resources.command_web"},
            {"IEBrowserElementCommand", "JAuto.Properties.Resources.command_web"},
            {"WebBrowserFindBrowserCommand", "JAuto.Properties.Resources.command_web"},
            {"IEBrowserNavigateCommand", "JAuto.Properties.Resources.command_web"},

            // If
            {"BeginIfCommand", "JAuto.Properties.Resources.command_begin_if"},
            {"BeginMultiIfCommand", "JAuto.Properties.Resources.command_begin_multi_if"},
            {"ElseCommand", "JAuto.Properties.Resources.command_else"},
            {"EndIfCommand", "JAuto.Properties.Resources.command_end_if"},

            // Image
            {"ImageRecognitionCommand", "JAuto.Properties.Resources.command_camera"},
            {"OCRCommand", "JAuto.Properties.Resources.command_camera"},
            {"ScreenshotCommand", "JAuto.Properties.Resources.command_camera"},

            // Input
            {"FileDialogCommand", "JAuto.Properties.Resources.command_input"},
            {"FolderDialogCommand", "JAuto.Properties.Resources.command_input"},
            {"HTMLInputCommand", "JAuto.Properties.Resources.command_input"},
            {"UserInputCommand", "JAuto.Properties.Resources.command_input"}, // prompt
            {"SendAdvancedKeyStrokesCommand", "JAuto.Properties.Resources.command_input"},
            {"SendHotkeyCommand", "JAuto.Properties.Resources.command_input"},
            {"SendKeysCommand", "JAuto.Properties.Resources.command_input"},
            {"SendMouseMoveCommand", "JAuto.Properties.Resources.command_input"},
            {"SendMouseClickCommand", "JAuto.Properties.Resources.command_input"},
            {"UIAutomationCommand", "JAuto.Properties.Resources.command_input"},

            // Loop
            {"BeginLoopCommand", "JAuto.Properties.Resources.command_startloop"},
            {"BeginMultiLoopCommand", "JAuto.Properties.Resources.command_startloop"},
            {"EndLoopCommand", "JAuto.Properties.Resources.command_endloop"},
            {"ExitLoopCommand", "JAuto.Properties.Resources.command_exitloop"},
            {"BeginContinousLoopCommand", "JAuto.Properties.Resources.command_startloop"},
            {"BeginListLoopCommand", "JAuto.Properties.Resources.command_startloop"},
            {"BeginNumberOfTimesLoopCommand", "JAuto.Properties.Resources.command_startloop"},
            {"NextLoopCommand", "JAuto.Properties.Resources.command_nextloop"},

            // Misc
            {"CommentCommand", "JAuto.Properties.Resources.command_comment"},
            {"EncryptionCommand", "JAuto.Properties.Resources.command_input"},
            {"ClipboardGetTextCommand", "JAuto.Properties.Resources.command_files"},
            {"PingCommand", "JAuto.Properties.Resources.command_web"},
            {"SMTPSendEmailCommand", "JAuto.Properties.Resources.command_smtp"},
            {"SequenceCommand", "JAuto.Properties.Resources.command_sequence"},
            {"ClipboardSetTextCommand", "JAuto.Properties.Resources.command_files"},
            {"MessageBoxCommand", "JAuto.Properties.Resources.command_comment"},

            // NLG
            {"NLGCreateInstanceCommand", "JAuto.Properties.Resources.command_nlg"},
            {"NLGGeneratePhraseCommand", "JAuto.Properties.Resources.command_nlg"},
            {"NLGSetParameterCommand", "JAuto.Properties.Resources.command_nlg"},

            // Outlook
            {"OutlookDeleteEmailsCommand", "JAuto.Properties.Resources.command_smtp"},
            {"OutlookForwardEmailsCommand", "JAuto.Properties.Resources.command_smtp"},
            {"OutlookGetEmailsCommand", "JAuto.Properties.Resources.command_smtp"},
            {"OutlookMoveEmailsCommand", "JAuto.Properties.Resources.command_smtp"},
            {"OutlookReplyToEmailsCommand", "JAuto.Properties.Resources.command_smtp"},
            {"OutlookEmailCommand", "JAuto.Properties.Resources.command_smtp"},

            // Program
            {"RunCustomCodeCommand", "JAuto.Properties.Resources.command_script"},
            {"RunScriptCommand", "JAuto.Properties.Resources.command_script"},
            {"StartProcessCommand", "JAuto.Properties.Resources.command_start_process"},
            {"StopProcessCommand", "JAuto.Properties.Resources.command_stop_process"},

            // Regex
            {"GetRegexMatchesCommand", "JAuto.Properties.Resources.command_function"},

            // Remote
            {"RemoteAPICommand", "JAuto.Properties.Resources.command_remote"},
            {"RemoteTaskCommand", "JAuto.Properties.Resources.command_remote"},

            // System
            {"EnvironmentVariableCommand", "JAuto.Properties.Resources.command_system"},
            {"RemoteDesktopCommand", "JAuto.Properties.Resources.command_system"},
            {"OSVariableCommand", "JAuto.Properties.Resources.command_system"},
            {"SystemActionCommand", "JAuto.Properties.Resources.command_script"},

            // Task
            {"RunTaskCommand", "JAuto.Properties.Resources.command_start_process"},
            {"StopTaskCommand", "JAuto.Properties.Resources.command_stop_process"},

            // Text
            {"ReadTextFileCommand", "JAuto.Properties.Resources.command_files"},
            {"WriteTextFileCommand", "JAuto.Properties.Resources.command_files"},

            // Variable
            {"AddToVariableCommand", "JAuto.Properties.Resources.command_function"},
            {"AddVariableCommand", "JAuto.Properties.Resources.command_function"},
            {"VariableCommand", "JAuto.Properties.Resources.command_function"},
            {"SetVariableIndexCommand", "JAuto.Properties.Resources.command_function"},

            // Web
            {"SeleniumBrowserCheckBrowserInstanceExistsCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserCloseCommand", "JAuto.Properties.Resources.command_window_close"},
            {"SeleniumBrowserCreateCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserElementActionCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserExecuteScriptCommand", "JAuto.Properties.Resources.command_script"},
            {"SeleniumBrowserInfoCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserNavigateBackCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserNavigateForwardCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserNavigateURLCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserRefreshCommand", "JAuto.Properties.Resources.command_web"},
            {"SeleniumBrowserSwitchFrameCommand", "JAuto.Properties.Resources.command_window"},
            {"SeleniumBrowserSwitchWindowCommand", "JAuto.Properties.Resources.command_window"},
            {"SeleniumBrowserTakeScreenshotCommand", "JAuto.Properties.Resources.command_web"},

            // Window
            {"ActivateWindowCommand", "JAuto.Properties.Resources.command_window"},
            {"CheckWindowNameExistsCommand", "JAuto.Properties.Resources.command_window"},
            {"CloseWindowCommand", "JAuto.Properties.Resources.command_window_close"},
            {"GetWindowNamesCommand", "JAuto.Properties.Resources.command_window"},
            {"MoveWindowCommand", "JAuto.Properties.Resources.command_window"},
            {"ResizeWindowCommand", "JAuto.Properties.Resources.command_window"},
            {"SetWindowStateCommand", "JAuto.Properties.Resources.command_window"},
            {"WaitForWindowCommand", "JAuto.Properties.Resources.command_window"},

            // Word
            {"WordAddDocumentCommand", "JAuto.Properties.Resources.command_files"},
            {"WordAppendDataTableCommand", "JAuto.Properties.Resources.command_files"},
            {"WordAppendImageCommand", "JAuto.Properties.Resources.command_files"},
            {"WordAppendTextCommand", "JAuto.Properties.Resources.command_files"},
            {"WordCheckWordInstanceExistsCommand", "JAuto.Properties.Resources.command_files"},
            {"WordCloseApplicationCommand", "JAuto.Properties.Resources.command_files"},
            {"WordCreateApplicationCommand", "JAuto.Properties.Resources.command_files"},
            {"WordExportToPDFCommand", "JAuto.Properties.Resources.command_files"},
            {"WordOpenDocumentCommand", "JAuto.Properties.Resources.command_files"},
            {"WordReadDocumentCommand", "JAuto.Properties.Resources.command_files"},
            {"WordReplaceTextCommand", "JAuto.Properties.Resources.command_files"},
            {"WordSaveCommand", "JAuto.Properties.Resources.command_files"},
            {"WordSaveAsCommand", "JAuto.Properties.Resources.command_files"},

            //// NOTHING ///
            {"BeginExcelDatasetLoopCommand", "JAuto.Properties.Resources.command_startloop"},
            {"ThickAppClickItemCommand", "JAuto.Properties.Resources.command_input"},
            {"ThickAppGetTextCommand", "JAuto.Properties.Resources.command_search"},
            {"Setcommand_windowtateCommand", "JAuto.Properties.Resources.command_window"},
            {"_NotFoundCommand", "JAuto.Properties.Resources.command_files"},
        };

        public static Dictionary<string, Image> UIImageDictionary()
        {
            //var uiImages = new Dictionary<string, Image>();
            //Dictionary<string, Image> uiImages = imageList;
            Dictionary<string, Image> uiImages = new Dictionary<string, Image>();

            // API
            uiImages.Add("ExecuteDLLCommand", JAuto.Properties.Resources.command_run_code);
            uiImages.Add("RESTCommand", JAuto.Properties.Resources.command_run_code);
            uiImages.Add("HTTPRequestCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("HTTPQueryResultCommand", JAuto.Properties.Resources.command_search);

            // Data
            uiImages.Add("DateCalculationCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("FormatDataCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("GetListCountCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("GetListItemCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("GetWordLengthCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("GetWordCountCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("LogDataCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("MathCalculationCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("ModifyVariableCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("ParseDatasetRowCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("ParseJsonArrayCommand", JAuto.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonCommand", JAuto.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonModelCommand", JAuto.Properties.Resources.command_parse);
            uiImages.Add("PDFTextExtractionCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("RegExExtractorCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("StringReplaceCommand", JAuto.Properties.Resources.command_string);
            uiImages.Add("StringSplitCommand", JAuto.Properties.Resources.command_string);
            uiImages.Add("StringSubstringCommand", JAuto.Properties.Resources.command_string);
            uiImages.Add("TextExtractorCommand", JAuto.Properties.Resources.command_function);

            // Database
            uiImages.Add("DatabaseDefineConnectionCommand", JAuto.Properties.Resources.command_database);
            uiImages.Add("DatabaseExecuteQueryCommand", JAuto.Properties.Resources.command_database);

            // DataTable
            uiImages.Add("LoadTaskCommand", JAuto.Properties.Resources.command_start_process);
            uiImages.Add("UnloadTaskCommand", JAuto.Properties.Resources.command_stop_process);
            uiImages.Add("ExcelAddWorksheetCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("AddDataRowCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("CreateDataTableCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("FilterDataTableCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCountCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowValueCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("RemoveDataRowCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("WriteDataRowValueCommand", JAuto.Properties.Resources.command_spreadsheet);

            // Dictionary
            uiImages.Add("AddDictionaryCommand", JAuto.Properties.Resources.command_dictionary);
            uiImages.Add("CreateDictionaryCommand", JAuto.Properties.Resources.command_dictionary);
            uiImages.Add("GetDictionaryValueCommand", JAuto.Properties.Resources.command_dictionary);
            uiImages.Add("LoadDictionaryCommand", JAuto.Properties.Resources.command_dictionary);

            // Engine
            uiImages.Add("ErrorHandlingCommand", JAuto.Properties.Resources.command_error);
            uiImages.Add("GetDataCommand", JAuto.Properties.Resources.command_server);  // get bot data
            uiImages.Add("PauseCommand", JAuto.Properties.Resources.command_pause);
            uiImages.Add("SetEngineDelayCommand", JAuto.Properties.Resources.command_pause);
            uiImages.Add("ShowEngineContextCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("SetEnginePreferenceCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("StopwatchCommand", JAuto.Properties.Resources.command_stopwatch);
            uiImages.Add("UploadDataCommand", JAuto.Properties.Resources.command_server);   // upload bot store

            // Error
            uiImages.Add("CatchExceptionCommand", JAuto.Properties.Resources.command_try);
            uiImages.Add("EndTryCommand", JAuto.Properties.Resources.command_try);
            uiImages.Add("FinallyCommand", JAuto.Properties.Resources.command_try);
            uiImages.Add("ThrowExceptionCommand", JAuto.Properties.Resources.command_try);
            uiImages.Add("TryCommand", JAuto.Properties.Resources.command_try);

            // Excel
            uiImages.Add("ExcelActivateSheetCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAddWorkbookCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendCellCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendRowCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCloseApplicationCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCreateDatasetCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCreateApplicationCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteCellCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteRowCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetCellCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetLastRowCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommandAsDT", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGoToCellCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelOpenWorkbookCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelRunMacroCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveAsCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSetCellCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSplitRangeByColumnCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRangeCommand", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRowCommand", JAuto.Properties.Resources.command_spreadsheet);

            // File
            uiImages.Add("DeleteFileCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("ExtractFileCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("GetFilesCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("MoveFileCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("RenameFileCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WaitForFileToExistCommand", JAuto.Properties.Resources.command_files);

            // Folder
            uiImages.Add("CreateFolderCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("DeleteFolderCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("GetFoldersCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("MoveFolderCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("RenameFolderCommand", JAuto.Properties.Resources.command_files);

            // IE
            uiImages.Add("IEBrowserCloseCommand", JAuto.Properties.Resources.command_window_close);
            uiImages.Add("IEBrowserCreateCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("IEBrowserElementCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("WebBrowserFindBrowserCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("IEBrowserNavigateCommand", JAuto.Properties.Resources.command_web);

            // If
            uiImages.Add("BeginIfCommand", JAuto.Properties.Resources.command_begin_if);
            uiImages.Add("BeginMultiIfCommand", JAuto.Properties.Resources.command_begin_multi_if);
            uiImages.Add("ElseCommand", JAuto.Properties.Resources.command_else);
            uiImages.Add("EndIfCommand", JAuto.Properties.Resources.command_end_if);

            // Image
            uiImages.Add("ImageRecognitionCommand", JAuto.Properties.Resources.command_camera);
            uiImages.Add("OCRCommand", JAuto.Properties.Resources.command_camera);
            uiImages.Add("ScreenshotCommand", JAuto.Properties.Resources.command_camera);

            // Input
            uiImages.Add("HTMLInputCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("UserInputCommand", JAuto.Properties.Resources.command_input); // prompt
            uiImages.Add("SendAdvancedKeyStrokesCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("SendHotkeyCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("SendKeysCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("SendMouseMoveCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("SendMouseClickCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("UIAutomationCommand", JAuto.Properties.Resources.command_input);

            // Loop
            uiImages.Add("BeginLoopCommand", JAuto.Properties.Resources.command_startloop);
            uiImages.Add("BeginMultiLoopCommand", JAuto.Properties.Resources.command_startloop);
            uiImages.Add("EndLoopCommand", JAuto.Properties.Resources.command_endloop);
            uiImages.Add("ExitLoopCommand", JAuto.Properties.Resources.command_exitloop);
            uiImages.Add("BeginContinousLoopCommand", JAuto.Properties.Resources.command_startloop);
            uiImages.Add("BeginListLoopCommand", JAuto.Properties.Resources.command_startloop);
            uiImages.Add("BeginNumberOfTimesLoopCommand", JAuto.Properties.Resources.command_startloop);
            uiImages.Add("NextLoopCommand", JAuto.Properties.Resources.command_nextloop);

            // Misc
            uiImages.Add("CommentCommand", JAuto.Properties.Resources.command_comment);
            uiImages.Add("EncryptionCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("ClipboardGetTextCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("PingCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SMTPSendEmailCommand", JAuto.Properties.Resources.command_smtp);
            uiImages.Add("SequenceCommand", JAuto.Properties.Resources.command_sequence);
            uiImages.Add("ClipboardSetTextCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("MessageBoxCommand", JAuto.Properties.Resources.command_comment);

            // NLG
            uiImages.Add("NLGCreateInstanceCommand", JAuto.Properties.Resources.command_nlg);
            uiImages.Add("NLGGeneratePhraseCommand", JAuto.Properties.Resources.command_nlg);
            uiImages.Add("NLGSetParameterCommand", JAuto.Properties.Resources.command_nlg);

            // Outlook
            uiImages.Add("OutlookDeleteEmailsCommand", JAuto.Properties.Resources.command_smtp);
            uiImages.Add("OutlookForwardEmailsCommand", JAuto.Properties.Resources.command_smtp);
            uiImages.Add("OutlookGetEmailsCommand", JAuto.Properties.Resources.command_smtp);
            uiImages.Add("OutlookMoveEmailsCommand", JAuto.Properties.Resources.command_smtp);
            uiImages.Add("OutlookReplyToEmailsCommand", JAuto.Properties.Resources.command_smtp);
            uiImages.Add("OutlookEmailCommand", JAuto.Properties.Resources.command_smtp);

            // Program
            uiImages.Add("RunCustomCodeCommand", JAuto.Properties.Resources.command_script);
            uiImages.Add("RunScriptCommand", JAuto.Properties.Resources.command_script);
            uiImages.Add("StartProcessCommand", JAuto.Properties.Resources.command_start_process);
            uiImages.Add("StopProcessCommand", JAuto.Properties.Resources.command_stop_process);

            // Regex
            uiImages.Add("GetRegexMatchesCommand", JAuto.Properties.Resources.command_function);

            // Remote
            uiImages.Add("RemoteAPICommand", JAuto.Properties.Resources.command_remote);
            uiImages.Add("RemoteTaskCommand", JAuto.Properties.Resources.command_remote);

            // System
            uiImages.Add("EnvironmentVariableCommand", JAuto.Properties.Resources.command_system);
            uiImages.Add("RemoteDesktopCommand", JAuto.Properties.Resources.command_system);
            uiImages.Add("OSVariableCommand", JAuto.Properties.Resources.command_system);
            uiImages.Add("SystemActionCommand", JAuto.Properties.Resources.command_script);

            // Task
            uiImages.Add("RunTaskCommand", JAuto.Properties.Resources.command_start_process);
            uiImages.Add("StopTaskCommand", JAuto.Properties.Resources.command_stop_process);

            // Text
            uiImages.Add("ReadTextFileCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WriteTextFileCommand", JAuto.Properties.Resources.command_files);

            // Variable
            uiImages.Add("AddToVariableCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("AddVariableCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("VariableCommand", JAuto.Properties.Resources.command_function);
            uiImages.Add("SetVariableIndexCommand", JAuto.Properties.Resources.command_function);

            // Web
            uiImages.Add("SeleniumBrowserCloseCommand", JAuto.Properties.Resources.command_window_close);
            uiImages.Add("SeleniumBrowserCreateCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserElementActionCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserExecuteScriptCommand", JAuto.Properties.Resources.command_script);
            uiImages.Add("SeleniumBrowserInfoCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateBackCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateForwardCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateURLCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserRefreshCommand", JAuto.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserSwitchFrameCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserSwitchWindowCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserTakeScreenshotCommand", JAuto.Properties.Resources.command_web);

            // Window
            uiImages.Add("ActivateWindowCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("CloseWindowCommand", JAuto.Properties.Resources.command_window_close);
            uiImages.Add("MoveWindowCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("ResizeWindowCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("SetWindowStateCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("WaitForWindowCommand", JAuto.Properties.Resources.command_window);

            // Word
            uiImages.Add("WordAddDocumentCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordAppendDataTableCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordAppendImageCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordAppendTextCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordCloseApplicationCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordCreateApplicationCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordExportToPDFCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordOpenDocumentCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordReadDocumentCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordReplaceTextCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordSaveCommand", JAuto.Properties.Resources.command_files);
            uiImages.Add("WordSaveAsCommand", JAuto.Properties.Resources.command_files);

            //// NOTHING ///
            uiImages.Add("BeginExcelDatasetLoopCommand", JAuto.Properties.Resources.command_startloop);
            uiImages.Add("ThickAppClickItemCommand", JAuto.Properties.Resources.command_input);
            uiImages.Add("ThickAppGetTextCommand", JAuto.Properties.Resources.command_search);
            uiImages.Add("Setcommand_windowtateCommand", JAuto.Properties.Resources.command_window);
            uiImages.Add("_NotFoundCommand", JAuto.Properties.Resources.command_files);

            // release
            //GC.Collect();

            return uiImages;
        }
        public static ImageList UIImageList()
        {
            //Dictionary<string, Image> imageIcons = UIImageDictionary();
            //if (imageList.Count == 0)
            //{
            //    UIImageDictionary();
            //}

            //ImageList uiImages = new ImageList();

            if (uiImages.Images.Count > 0)
            {
                return uiImages;
            }

            uiImages.ImageSize = new Size(16, 16);
            //foreach (var icon in imageIcons)
            //foreach (var icon in imageIcons)
            //{

            //    //var someImage = icon.Value;

            //    //using (Image src = icon.Value)
            //    //using (Bitmap dst = new Bitmap(16, 16))
            //    //using (Graphics g = Graphics.FromImage(dst))
            //    //{
            //    //    g.SmoothingMode = SmoothingMode.AntiAlias;
            //    //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    //    g.DrawImage(src, 0, 0, dst.Width, dst.Height);
            //    //    uiImages.Images.Add(icon.Key, dst);
            //    //}

            //    uiImages.Images.Add(icon.Key, icon.Value);
            //}
            uiImages.Images.Add("JAuto.Properties.Resources.command_begin_if", JAuto.Properties.Resources.command_begin_if);
            uiImages.Images.Add("JAuto.Properties.Resources.command_begin_multi_if", JAuto.Properties.Resources.command_begin_multi_if);
            uiImages.Images.Add("JAuto.Properties.Resources.command_camera", JAuto.Properties.Resources.command_camera);
            uiImages.Images.Add("JAuto.Properties.Resources.command_comment", JAuto.Properties.Resources.command_comment);
            uiImages.Images.Add("JAuto.Properties.Resources.command_database", JAuto.Properties.Resources.command_database);
            uiImages.Images.Add("JAuto.Properties.Resources.command_dictionary", JAuto.Properties.Resources.command_dictionary);
            uiImages.Images.Add("JAuto.Properties.Resources.command_else", JAuto.Properties.Resources.command_else);
            uiImages.Images.Add("JAuto.Properties.Resources.command_endloop", JAuto.Properties.Resources.command_endloop);
            uiImages.Images.Add("JAuto.Properties.Resources.command_end_if", JAuto.Properties.Resources.command_end_if);
            uiImages.Images.Add("JAuto.Properties.Resources.command_error", JAuto.Properties.Resources.command_error);
            uiImages.Images.Add("JAuto.Properties.Resources.command_exitloop", JAuto.Properties.Resources.command_exitloop);
            uiImages.Images.Add("JAuto.Properties.Resources.command_files", JAuto.Properties.Resources.command_files);
            uiImages.Images.Add("JAuto.Properties.Resources.command_function", JAuto.Properties.Resources.command_function);
            uiImages.Images.Add("JAuto.Properties.Resources.command_input", JAuto.Properties.Resources.command_input);
            uiImages.Images.Add("JAuto.Properties.Resources.command_nextloop", JAuto.Properties.Resources.command_nextloop);
            uiImages.Images.Add("JAuto.Properties.Resources.command_nlg", JAuto.Properties.Resources.command_nlg);
            uiImages.Images.Add("JAuto.Properties.Resources.command_parse", JAuto.Properties.Resources.command_parse);
            uiImages.Images.Add("JAuto.Properties.Resources.command_pause", JAuto.Properties.Resources.command_pause);
            uiImages.Images.Add("JAuto.Properties.Resources.command_remote", JAuto.Properties.Resources.command_remote);
            uiImages.Images.Add("JAuto.Properties.Resources.command_run_code", JAuto.Properties.Resources.command_run_code);
            uiImages.Images.Add("JAuto.Properties.Resources.command_script", JAuto.Properties.Resources.command_script);
            uiImages.Images.Add("JAuto.Properties.Resources.command_search", JAuto.Properties.Resources.command_search);
            uiImages.Images.Add("JAuto.Properties.Resources.command_sequence", JAuto.Properties.Resources.command_sequence);
            uiImages.Images.Add("JAuto.Properties.Resources.command_server", JAuto.Properties.Resources.command_server);
            uiImages.Images.Add("JAuto.Properties.Resources.command_smtp", JAuto.Properties.Resources.command_smtp);
            uiImages.Images.Add("JAuto.Properties.Resources.command_spreadsheet", JAuto.Properties.Resources.command_spreadsheet);
            uiImages.Images.Add("JAuto.Properties.Resources.command_startloop", JAuto.Properties.Resources.command_startloop);
            uiImages.Images.Add("JAuto.Properties.Resources.command_start_process", JAuto.Properties.Resources.command_start_process);
            uiImages.Images.Add("JAuto.Properties.Resources.command_stopwatch", JAuto.Properties.Resources.command_stopwatch);
            uiImages.Images.Add("JAuto.Properties.Resources.command_stop_process", JAuto.Properties.Resources.command_stop_process);
            uiImages.Images.Add("JAuto.Properties.Resources.command_string", JAuto.Properties.Resources.command_string);
            uiImages.Images.Add("JAuto.Properties.Resources.command_system", JAuto.Properties.Resources.command_system);
            uiImages.Images.Add("JAuto.Properties.Resources.command_try", JAuto.Properties.Resources.command_try);
            uiImages.Images.Add("JAuto.Properties.Resources.command_web", JAuto.Properties.Resources.command_web);
            uiImages.Images.Add("JAuto.Properties.Resources.command_window", JAuto.Properties.Resources.command_window);
            uiImages.Images.Add("JAuto.Properties.Resources.command_window_close", JAuto.Properties.Resources.command_window_close);

            // release
            //imageIcons.Clear();

            return uiImages;
        }
        public static Image ResizeImageFile(Image image)
        {
            using (System.Drawing.Image oldImage = image)
            {
                using (Bitmap newImage = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), new Size(16,16)));
                        return newImage;
                    }
                }
            }
        }

        public static int GetUIImageList(string commandName)
        {
            //return uiImages.Images.IndexOfKey(imageCommandTable[commandName]);
            try
            {
                return uiImages.Images.IndexOfKey(imageCommandTable[commandName]);
            }
            catch (Exception)
            {
                return uiImages.Images.IndexOfKey("JAuto.Properties.Resources.command_files");
            }
        }

        public static Image GetUIImage(string commandName)
        {
            ////var uiImageDictionary = UIImageDictionary();
            //if (imageList.Count == 0)
            //{
            //    UIImageDictionary();
            //}

            //Image uiImage;
            //try
            //{
            //    //uiImage = uiImageDictionary[commandName];
            //    uiImage = imageList[commandName];
            //}
            //catch (Exception)
            //{
            //    uiImage = Properties.Resources.command_files;
            //}

            if (uiImages.Images.Count == 0)
            {
                UIImageList();
            }

            Image retImage;
            try
            {
                retImage = uiImages.Images[uiImages.Images.IndexOfKey(imageCommandTable[commandName])];
            }
            catch (Exception)
            {
                retImage = uiImages.Images[uiImages.Images.IndexOfKey("JAuto.Properties.Resources.command_files")];
            }

            return retImage;
        }
    }
}