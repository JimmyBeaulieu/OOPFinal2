//Jimmy Beaulieu
//03/08/2022
//Final Exam
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam
{
    internal class GradeJimmy
    {
        private string name;
        private double grade;
        //I put private attributes because it was asked but didn't find any good use for them

        public GradeJimmy()
        {
            Name = "?";
            Grade = 0;
        }
        public string Name { get; set; }
        public double Grade { get; set; }

        public string GradeLetter(double grade)
        {
        /*
         *  90 - 100 = A
            80 – 89.9 = B
            70 – 79.9 = C
            60 – 69.9 = D
            0 – 59.9 = F
         */
            if (grade <= 100 && grade >= 90) { return "A"; }
            if (grade < 90 && grade >= 80) { return "B"; }
            if (grade < 80 && grade >= 70) { return "C"; }
            if (grade < 70 && grade >= 60) { return "D"; }
            if (grade < 60 && grade >= 0) { return "F";}
            else return "Error";
        }
    }
}
