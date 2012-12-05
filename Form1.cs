using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rounder
{
    public partial class Rounder : Form
    {
        public Rounder()
        {
            InitializeComponent();
        }

        //adds a role to the role listbox
        private void btnAddRole_Click(object sender, EventArgs e)
        {
            if (txtAddRole.Text != string.Empty)
            {
              lstRoles.Items.Add(txtAddRole.Text);
              txtAddRole.Clear();
            }
        }

        private void txtAddRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddRole.PerformClick();
            }
        }

        //removes selected role from listbox
        private void lstRoles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lstRoles.SelectedIndex >= 0)
                {
                    lstRoles.Items.RemoveAt(lstRoles.SelectedIndex);
                }
            }
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            //call error checking method
            validation();
            
            if (errProvider.GetError(lstRoles).Length != 0)
                return;
            if (errProvider.GetError(txtQuestions).Length != 0)
                return;
            if (errProvider.GetError(grpDuration).Length != 0)
                return;
            
            Round myRound = new Round(); //round object
            
            //roles
            List<string> roles = new List<string>();
            for (int i = 0; i < lstRoles.Items.Count; i++)
            {
                roles.Add(lstRoles.Items[i].ToString());
            }

            //questions
            List<string> questions = new List<string>();
            string _questions = txtQuestions.Text;
            char[] delimiters = new char[] { '\r', '\n' };
            string[] parts = _questions.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                myRound.Questions.Add(parts[i]);
            }
            
            //duration
            int minutes;
            int.TryParse(txtMinutes.Text, out minutes);
            int seconds;
            int.TryParse(txtSeconds.Text, out seconds);

            //set properties for Round object
            myRound.setDuration(minutes, seconds);
            myRound.Questions = questions;
            myRound.Roles = roles;

            //open the student view form
            RounderStudentView rounderSV = new RounderStudentView();
            rounderSV.populate(myRound);
            rounderSV.Show();
        }

        private void validation()
        {
            //roles validation
            if (lstRoles.Items.Count == 0)
                errProvider.SetError(lstRoles, "You must enter at least one role.");
            else
                errProvider.SetError(lstRoles, string.Empty);

            //questions validation
            if (txtQuestions.Text == string.Empty)
                errProvider.SetError(txtQuestions, "You must enter at least one question.");
            else
                errProvider.SetError(txtQuestions, string.Empty);

            //time validation
            if (txtMinutes.Text != string.Empty && txtSeconds.Text == string.Empty)
                txtSeconds.Text = "0";

            try
            {
                if (int.Parse(txtMinutes.Text) + int.Parse(txtSeconds.Text) < 1)
                    errProvider.SetError(grpDuration, "Total time must be at least 1 second.");
                else
                    errProvider.SetError(grpDuration, string.Empty);
            }
            catch
            {
                errProvider.SetError(grpDuration, "Time must be in whole numbers.");
                txtMinutes.Clear();
                txtSeconds.Clear();
            }

        }

        private void loadProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            DialogResult result = openFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                string file = openFile.FileName;
                try
                {
                    string fileText = File.ReadAllText(file);
                    string[] lines = fileText.Split('\n');
                    
                    //strips each string of the carriage return
                    for(int i = 0; i < lines.Length; i++)
                    {
                        lines[i] = lines[i].Trim();
                    }

                    //load roles
                    int j = 0;
                    while (lines[j] != "#")
                    {
                        lstRoles.Items.Add(lines[j]);
                        j++;
                    }

                    //load questions
                    j++;
                    while (lines[j] != "#")
                    {
                        txtQuestions.Text += lines[j] + "\n";
                        j++;
                    }

                    //load time
                    j++;
                    txtMinutes.Text = lines[j];
                    j++;
                    txtSeconds.Text = lines[j];

                }
                catch (IOException)
                {
                    MessageBox.Show("Error opening file!");
                }
            }
        }

    }
}
