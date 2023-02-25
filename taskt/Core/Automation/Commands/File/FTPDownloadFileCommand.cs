using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.IO;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command download files from a FTP site")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FTPDownloadFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the FTP/SFTP URL/destination (ex. sftp.wayfair.com/orders, {{{vFTPUrl}}}, ftp://wayfair.com/orders)")]
        //[Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage(@"**C:\temp\myfile.zip** , **{{{vFTPUrl}}}** or **ftp://wayfair.com/orders**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FTPUrl { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the download folder (ex. C:\\temp\\myfile\\, {{{vFolderPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage(@"**C:\temp\** or **{{{vFilePath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PathDestination { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Create folder if destination does not exist (default is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the FTP/SFTP Account (ex. mypass, {{{vFTPAccount}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter FTP/SFTP Account.")]
        [Attributes.PropertyAttributes.SampleUsage(@"**mypass** or {{{vFTPAccount}}}")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FTPAccount { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the FTP/SFTP password (ex. mypass, {{{vFTPPassword}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter FTP/SFTP password.")]
        [Attributes.PropertyAttributes.SampleUsage(@"**mypass** or {{{vFTPPassword}}}")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FTPPassword { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the file extension to receive a list of download file names (ex. .csv) {{{vFileExtension}}}")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vFileExtension**")]
        [Attributes.PropertyAttributes.Remarks("Enter the file type extension want to download, like .csv .mv4 .txt")]
        public string v_FileExtension { get; set; }

        public FTPDownloadFileCommand()
        {
            this.CommandName = "FTPDownloadFileCommand";
            this.SelectionName = "FTP Download File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get absolute variable path or URL to source file
            var vFTPUrl = v_FTPUrl.ConvertToUserVariable(sender);
            //track local file location
            string vLocalSourceFile = v_FTPUrl;
            //get file path to destination files
            var vPathDestination = v_PathDestination.ConvertToUserVariable(sender);
            var vCreateDirectory = v_CreateDirectory.ConvertToUserVariable(sender);
            var vFTPAccount = v_FTPAccount.ConvertToUserVariable(sender);
            var vFTPPassword = v_FTPPassword.ConvertToUserVariable(sender);
            var vFileExtension = v_FileExtension.ConvertToUserVariable(sender);

            //if (String.IsNullOrEmpty(vCreateDirectory))
            //{
            //    vCreateDirectory = "No";
            //}
            //get optional password
            // var vPassword = v_Password.ConvertToUserVariable(sender);
            //auto-detect extension
            //var vFileType = Path.GetExtension(v_FTPUrl);
            //create tracking list
            var fileList = new List<string>();

            if (v_FTPUrl.StartsWith("ftp://") || v_FTPUrl.StartsWith("sftp://") || v_FTPUrl.StartsWith("www."))
            {
                

                //check if directory does not exist then create directory
                if (!System.IO.Directory.Exists(vPathDestination))
                {
                    System.IO.Directory.CreateDirectory(vPathDestination);
                }

                List<string> List_files = new List<string>();
                
                NetworkCredential credentials = new NetworkCredential(vFTPAccount, vFTPPassword);

                DownloadFtpDirectory(vFTPUrl, credentials, vPathDestination, vFileExtension);

              
            }


          
            //// If the directory doesn't exist, create it.
            //if (!Directory.Exists(vPathDestination))
            //{
            //    if (vCreateDirectory.ToLower() == "yes")
            //    {
            //        Directory.CreateDirectory(vPathDestination);
            //    }
            //    else
            //    {
            //        throw new Exception("No download folder: " + vPathDestination);
            //    }
            //}

          

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FTPUrl", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CreateDirectory", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FTPAccount", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FTPPassword", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileExtension", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [FTP Download: '{v_FTPUrl}', Destination Folder: '{v_PathDestination}'";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_FTPUrl))
            {
                this.validationResult += "File path is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_PathDestination))
            {
                this.validationResult += "Download folder is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }

        public static void DownloadFtpDirectory(string url, NetworkCredential credentials, string localPath,string vFileExtension)
        {
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();

            using (var listResponse = (FtpWebResponse)listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (var listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            int i_filewithExtension = 99;
            foreach (string line in lines)
            {
                if (line.Contains(vFileExtension))

                {
                    string[] tokens =line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                    //get the exactly i for the extension
                    if (i_filewithExtension == 99)
                    {
                        for (int i = 0; i <= i_filewithExtension - 1; i++)
                        {
                            if (tokens[i].Contains(vFileExtension))
                            {
                                i_filewithExtension = i;
                                break;
                            }
                        }
                    }
                   
                    string name = tokens[i_filewithExtension];
                    string permissions = tokens[0];

                    string localFilePath = Path.Combine(localPath, name);
                    string fileUrl = url + "/"+name;
                    

                    FtpWebRequest downloadRequest =(FtpWebRequest)WebRequest.Create(fileUrl);
                    downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    downloadRequest.Credentials = credentials;

                    using (FtpWebResponse downloadResponse =(FtpWebResponse)downloadRequest.GetResponse())
                    using (Stream sourceStream = downloadResponse.GetResponseStream())
                    using (Stream targetStream = File.Create(localFilePath))
                    {
                        byte[] buffer = new byte[10240];
                        int read;
                        while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            targetStream.Write(buffer, 0, read);
                        }
                    }

                }
                
        }
        }
        
    }
}