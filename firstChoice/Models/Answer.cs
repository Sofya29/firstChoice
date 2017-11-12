using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace firstChoice.Models
{
    public class Answer
    {
        public int id { get; set; }
        public int answerId { get; set; }
        public string answer { get; set; }
        public int assessmentId { get; set; }
        public Boolean correct { get; set; }
        public int questionNumber { get; set; }
        public string answerLetter { get; set; }

        public Answer(int id, int answerId, string answer, int assessmentId, Boolean correct, int questionNumber, string answerLetter)
        {
            this.id = id;
            this.answerId = answerId;
            this.answer = answer;
            this.assessmentId = assessmentId;
            this.correct = correct;
            this.questionNumber = questionNumber;
            this.answerLetter = answerLetter;
        }
        public Answer(string answer, int assessmentId, Boolean correct, int questionNumber, string answerLetter)
        {
            this.answer = answer;
            this.assessmentId = assessmentId;
            this.correct = correct;
            this.questionNumber = questionNumber;
            this.answerLetter = answerLetter;
        }
        public Answer(string answer, int questionNumber, string answerLetter)
        {
            this.answer = answer;
            this.questionNumber = questionNumber;
            this.answerLetter = answerLetter;
        }
    }
}