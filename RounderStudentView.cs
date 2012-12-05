using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rounder
{
    public partial class RounderStudentView : Form
    {
        private TimeSpan originalDuration, currentDuration, remaining;
        private DateTime startTime;
        List<string> roles;
        List<string> questions;
        private int currentRound = -1;

        public RounderStudentView()
        {
            InitializeComponent();
        }

        private void RounderStudentView_Load(object sender, EventArgs e)
        {

        }

        public void populate(Round oldRound)
        {
            Round myRound = oldRound;

            //set the duration
            int minutes = (int) myRound.getDuration().Minutes;
            int seconds = (int) myRound.getDuration().Seconds;
            originalDuration = new TimeSpan(0, minutes, seconds);
            currentDuration = originalDuration;

            //set the roles
            roles = myRound.Roles;

            //set the questions
            questions = myRound.Questions;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (currentRound == -1)
              firstRound();
            
            //changes button text, counts down timer
            if (btnStart.Text == "Start")
            {
                timer1.Enabled = true;
                btnStart.Text = "Pause";
                startTime = DateTime.Now;
            }
            else
            {
                currentDuration = remaining;
                timer1.Enabled = false;
                btnStart.Text = "Start";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //calcuates and displays the remaining time
            remaining = currentDuration - (DateTime.Now - startTime);
            if (remaining.TotalSeconds > 0)
                lblTime.Text = remaining.ToString("mm':'ss");
            else if (currentRound < questions.Count - 1)
                nextRound();
            else
            {
                btnStart.PerformClick();
                this.Close();
            }
            
        }

        private void firstRound()
        {
            //get the roles
            string _roles = "";
            for (int i = 0; i < roles.Count; i++)
                _roles += (i+1).ToString() + ": " + roles[i] + "\n";
            lblRoles.Text = _roles;

            //get the first question
            currentRound = 0;
            rtbQuestion.Text = questions[currentRound];

            //make the labels visible
            lblRoles.Visible = true;
            rtbQuestion.Visible = true;
            lblTime.Visible = true;
        }

        private void nextRound()
        {
            //cycle the roles
            string roleText = "";
            string tempRole;
            tempRole = roles[roles.Count-1];
            for (int i = roles.Count-2; i >= 0; i--)
            {
                roles[i + 1] = roles[i];
            }
            roles[0] = tempRole;

            //assign the roles text
            for (int i = 0; i < roles.Count; i++)
                roleText += (i+1).ToString() + ": " + roles[i] + "\n";

            //print the roles
            lblRoles.Text = roleText;

            //change the question
            currentRound++;
            rtbQuestion.Text = questions[currentRound];

            //reset the duration
            currentDuration = originalDuration;
            startTime = DateTime.Now;

        }
    }
}
