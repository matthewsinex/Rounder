using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rounder
{
    public class Round
    {
        private List<string> roles = new List<string>();
        private List<string> questions = new List<string>();
        private TimeSpan time = new TimeSpan();

        public List<string> Roles
        {
            get
            {
                return roles;
            }
            set
            {
                foreach(string _role in value)
                {
                    roles.Add(_role);
                }
            }
        }

        public List<string> Questions
        {
            get
            {
                return questions;
            }
            set
            {
                foreach(string _question in value)
                {
                    questions.Add(_question);
                }
            }
        }

        public void setDuration(int minutes, int seconds)
        {
            time = new TimeSpan(0, minutes, seconds);
        }
        public TimeSpan getDuration()
        {
            return time;
        }
    }
}
