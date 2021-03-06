﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// Represents a dialog, that allows to create the StartProcessInstruction.
    /// </summary>
    public partial class StartProcessInstructionDialog : InstructionBaseDialog
    {
        #region VariableInfo

        private class VariableInfo
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        #endregion

        private StartProcessInstruction instruction;
        private readonly List<VariableInfo> variables = new List<VariableInfo>();

        public override SetupInstruction SetupInstruction
        {
            set
            {
                StartProcessInstruction instruction = value as StartProcessInstruction;
                if (instruction != null)
                {
                    this.instruction = instruction;
                    txtProgram.Text = instruction.FileName;
                    txtParameters.Text = instruction.Parameters;
                    chkWaitUntilExit.Checked = instruction.WaitForExit;
                }
            }
            get
            {
                return this.instruction;
            }
        }

        public StartProcessInstructionDialog() : base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtProgram.SetVariableNames(this.VariableNames);
            txtParameters.SetVariableNames(this.VariableNames);

            foreach (MenuItem item in this.argumentsMenu.MenuItems)
            {
                item.Click += this.OnArgumentMenuItemClick;
            }

            // Fill list with overridable environment variables
            foreach (DictionaryEntry var in Environment.GetEnvironmentVariables())
            {
                string key = (string)var.Key;
                if (this.instruction.EnvironmentVariables.ContainsKey(key))
                {
                    variables.Add(new VariableInfo { Name = key, Value = this.instruction.EnvironmentVariables[key] });
                }
                else
                {
                    variables.Add(new VariableInfo() { Name = key, Value = string.Empty });
                }
            }
            olvEnvironmentVariables.SetObjects(variables);
        }

        private void OnArgumentMenuItemClick(object sender, EventArgs e)
        {
            txtParameters.AppendText(((MenuItem)sender).Tag as string);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // At least, a file name needs to be given
            if (string.IsNullOrEmpty(txtProgram.Text))
            {
                MessageBox.Show(this, "You did not specify an application to start.\r\n\r\nSpecify an application in order to add the setup instruction.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None);
                DialogResult = DialogResult.None;
                return;
            }

            if (instruction == null)
            {
                this.instruction = new StartProcessInstruction();
            }

            this.instruction.FileName = txtProgram.Text;
            this.instruction.Parameters = txtParameters.Text;
            this.instruction.WaitForExit = chkWaitUntilExit.Checked;
            foreach (VariableInfo var in this.variables)
            {
                if (!string.IsNullOrEmpty(var.Value))
                {
                    this.instruction.EnvironmentVariables[var.Name] = var.Value;
                }
            }
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                try
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(txtProgram.Text);
                    dialog.FileName = Path.GetFileName(txtProgram.Text);
                }
                catch (Exception)
                {
                    // user input, may not work
                }

                dialog.Filter = "Executable file (.exe)|*.exe";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtProgram.Text = dialog.FileName;
                }
            }
        }
    }
}
