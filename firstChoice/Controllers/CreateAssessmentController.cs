using firstChoice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace firstChoice.Controllers
{
    public class CreateAssessmentController : Controller
    {

        // GET: CreateAssessment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult createAssessment()
        {
            DatabaseAccess dbAccess = new DatabaseAccess();
            DocxParser docx = new DocxParser();
            string path1 = Server.MapPath(Request.Files["btnUpload"].FileName).Replace("createAssessment","DocxFile");
            string path2 = Server.MapPath(Request.Files["btnAnswerSheet"].FileName).Replace("createAssessment", "DocxFile");
            List<Question> questions = docx.getQuestionswithCorrectAnswers(path1,path2);
            String x = Server.MapPath(Request.Files["btnUpload"].FileName);
            if (questions.Count != 0)
            { 
                dbAccess.storeAssessment(Request.Form["txtAssessmentTitle"], "CI6110", Convert.ToInt32(Request.Form["txtPercentageWorth"]), Convert.ToInt32(Request.Form["txtDuration"]), "KU14009");
                foreach (Question question in questions)
                {
                    List<Assessment> assessments = dbAccess.getAssessmentByLecturerID("KU14009");
                    string questionID = assessments.ElementAt(0).assessmentID + "." + question.questionNumber;
                    dbAccess.storeQuestion(questionID, question.question, assessments.ElementAt(0).assessmentID, question.questionNumber);
                    foreach (Answer answer in question.possibleAnswers)
                    {
                        string answerID = assessments.ElementAt(0).assessmentID + "." + question.questionNumber + "." + answer.answerLetter;
                        dbAccess.storeAnswer(answerID,answer.answer, assessments.ElementAt(0).assessmentID, answer.answerLetter, answer.correct.ToString(), questionID);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}