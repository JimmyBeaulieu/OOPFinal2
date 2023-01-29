//Jimmy Beaulieu
//03/08/2022
//Final Exam

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace FinalExam
{
    public partial class Form1 : Form
    {
        GradeJimmy user = new GradeJimmy();

        Regex numberChecker = new Regex("^[0|1]?[0-9]?[0-9]$");
        Regex seasonChecker = new Regex("^(Summer$)|(Fall$)|(Winter)$");
        Regex courseChecker = new Regex("^(420-CT2-AS)$|(420-AP1-AS)$|(420-DW2-AS)$");
        Regex yearChecker = new Regex("^(2019)$|(2020)$|(2021)$|(2022)$|(2023)$|(2024)$|(2025)$"); //You probably wanted me to do something like [2019-2025] but it kept telling me it was in reverse order even if I changed it to [2025-2019] so I went with the longer way to write it.

        string directoryPath = "./2212007/";
        static string textPath = "./2212007/Final.txt";
        static string xmlPath = "./2212007/Final.xml";

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!File.Exists(textPath))
            {
                File.Create(textPath).Close();
            }
            if (!File.Exists(xmlPath))
            {
                File.Create(xmlPath).Close();
            }
        }

        private void btn_validate_Click(object sender, EventArgs e)
        {
            if(IsValid())
            {
                tx_READONLY_midtermPercent.Text = (Convert.ToDouble(tx_midterm.Text) * 0.3).ToString() + "%";
                tx_READONLY_projectPercent.Text = (Convert.ToDouble(tx_project.Text) * 0.3).ToString() + "%";
                tx_READONLY_finalPercent.Text = (Convert.ToDouble(tx_final.Text) * 0.4).ToString() + "%";

                tx_READONLY_numberGrade.Text = (Convert.ToDouble(tx_midterm.Text) * 0.3
                                                +
                                                Convert.ToDouble(tx_project.Text) * 0.3
                                                +
                                                Convert.ToDouble(tx_final.Text)*0.3).ToString() + "%";
                tx_READONLY_letterGrade.Text = user.GradeLetter(Convert.ToDouble(
                                               Convert.ToDouble(tx_midterm.Text) * 0.3
                                               +
                                               Convert.ToDouble(tx_project.Text) * 0.3
                                               +
                                               Convert.ToDouble(tx_final.Text) * 0.3));
            }
            
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            MessageBoxButtons button = MessageBoxButtons.OKCancel;
            MessageBox.Show("Are you sure you want to exit?", "Jimmy Beaulieu", button);
            Application.Exit();
        }

        private bool IsValid()
        {
            int counter = 0;
            if (//check the grade number
                numberChecker.IsMatch(tx_midterm.Text)
                &&
                numberChecker.IsMatch(tx_project.Text)
                &&
                numberChecker.IsMatch(tx_final.Text)
                )
            {
                counter++;
            }
            else
            {
                tx_midterm.Text = "0";
                tx_project.Text = "0";
                tx_final.Text = "0";
                MessageBox.Show("Please enter only a number", "Jimmy Beaulieu");
            }

            if (seasonChecker.IsMatch(tx_session.Text))//check the season entered
            {
                counter++;
            }
            else
            {
                MessageBox.Show("Please enter either:\nSummer\nFall\nWinter", "Jimmy Beaulieu");
            }

            if (courseChecker.IsMatch(tx_courseNumber.Text))//check the course number entered
            {
                counter++;
            }
            else
            {
                MessageBox.Show("Please enter either:\n420-CT2-AS\n420-AP1-AS\n420-DW2-AS", "Jimmy Beaulieu");
            }

            if (yearChecker.IsMatch(tx_year.Text))
            {
                counter++;
            }
            else
            {
                MessageBox.Show("Please enter a year between 2019 and 2026", "Jimmy Beaulieu");
            }
            if (counter == 4) { return true; }
            else return false;
        }





        private void btn_write_Click(object sender, EventArgs e)
        {
            StreamWriter textWriter = File.AppendText(textPath);

            try { 

                if (IsValid())
                {                                                                   //Index number for info[]
                    textWriter.Write(tx_READONLY_studentName.Text + "|" +           //0
                                         tx_courseNumber.Text + "|" +               //1
                                         tx_session.Text + "|" +                    //2
                                         tx_year.Text + "|" +                       //3
                                         tx_midterm.Text + "|" +                    //4
                                         tx_project.Text + "|" +                    //5
                                         tx_final.Text + "|" +                      //6
                                         tx_READONLY_midtermPercent.Text + "|" +    //7
                                         tx_READONLY_projectPercent.Text + "|" +    //8
                                         tx_READONLY_finalPercent.Text + "|" +      //9
                                         tx_READONLY_numberGrade.Text + "|" +       //10
                                         tx_READONLY_letterGrade.Text               //11
                                          );
                    textWriter.Close();
                }

            }
            catch
            {
                MessageBox.Show("Error while writing to Final.txt","Jimmy Beaulieu");
            }


        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            try
            {

                StreamReader reader = File.OpenText(textPath);
                string[] info = reader.ReadToEnd().Split('|');

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = (" ");
                XmlWriter xmlOut = XmlWriter.Create(xmlPath, settings);
                xmlOut.WriteStartDocument();
                xmlOut.WriteStartElement("GradeRoot");
                xmlOut.WriteStartElement("Grade");
                xmlOut.WriteElementString("Name", info[0]);
                xmlOut.WriteElementString("Course", info[1]);
                xmlOut.WriteElementString("Session", info[2]);
                xmlOut.WriteElementString("Year", info[3]);
                xmlOut.WriteElementString("Midterm", info[4]);
                xmlOut.WriteElementString("Project", info[5]);
                xmlOut.WriteElementString("Final", info[6]);
                xmlOut.WriteElementString("Total", info[10]);

                xmlOut.Close();
            }
            catch
            {
                MessageBox.Show("Error while writing to XML file", "Jimmy Beaulieu");
            }

        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                // create the XmlReader object using settings object
                XmlReader xmlIn = XmlReader.Create(xmlPath, settings);
                // read past all nodes to the first UserName node
                string[] info = { "", "", "", "", "", "", "", "", "" };
                
                if (xmlIn.ReadToDescendant("Grade")) {
                    // create Element1 and Element2 string for each Child node
                    do{ xmlIn.ReadStartElement("Grade");
                        info[0] = xmlIn.ReadElementContentAsString();
                        info[1] = xmlIn.ReadElementContentAsString();
                        info[2] = xmlIn.ReadElementContentAsString();
                        info[3] = xmlIn.ReadElementContentAsString();
                        info[4] = xmlIn.ReadElementContentAsString();
                        info[5] = xmlIn.ReadElementContentAsString();
                        info[6] = xmlIn.ReadElementContentAsString();
                        info[7] = xmlIn.ReadElementContentAsString();
                        info[8] =
                            info[0] + ", " +
                            info[1] + ", " +
                            info[2] + ", " +
                            info[3] + ", " +
                            info[4] + ", " +
                            info[5] + ", " +
                            info[6] + ", " +
                            info[7];
                    } while (xmlIn.ReadToNextSibling("Grade"));
                }
                MessageBox.Show(info[8]); 
                //Show the elements of all the Childs in one MessageBox 
                // close the XmlReader object
                xmlIn.Close();
            }
            catch
            {
                MessageBox.Show("Error while trying to read from XML file", "Jimmy Beaulieu");
            }
        }
    }
}
