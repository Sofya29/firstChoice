using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace firstChoice.Models
{
    public class Question
    {
        public int id { get; set; }
        public string question { get; set; }
        public int assessmentId { get; set; }
        public int questionNumber { get; set; }
        public List<Answer> possibleAnswers { get; set; }

        public Question()
        {

        }
        public Question(int id, string question, int assessmentId, int questionNumber, List<Answer> possibleAnswers)
        {
            this.id = id;
            this.question = question;
            this.assessmentId = assessmentId;
            this.questionNumber = questionNumber;
            this.possibleAnswers = possibleAnswers;
        }
        public Question(string question, int questionNumber, List<Answer> possibleAnswers)
        {
            this.question = question;
            this.questionNumber = questionNumber;
            this.possibleAnswers = possibleAnswers;
        }
        public Question(string question, int questionNumber)
        {
            this.question = question;
            this.questionNumber = questionNumber;
        }
    }
}