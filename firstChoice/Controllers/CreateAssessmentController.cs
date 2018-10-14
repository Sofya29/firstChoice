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
            DocxParser docx = new DocxParser();
            string path1 = Server.MapPath(Request.Files["btnUpload"].FileName).Replace("createAssessment", "DocxFiles");
            string path2 = Server.MapPath(Request.Files["btnAnswerSheet"].FileName).Replace("createAssessment", "DocxFiles");
            Request.Files["btnUpload"].SaveAs(AppDomain.CurrentDomain.BaseDirectory + "DocxFiles\\" + Request.Files["btnAnswerSheet"].FileName);
            Request.Files["btnAnswerSheet"].SaveAs(AppDomain.CurrentDomain.BaseDirectory + "DocxFiles\\" + Request.Files["btnAnswerSheet"].FileName);
            List<Question> questions = docx.getQuestionswithCorrectAnswers(path1, path2);
            if (questions.Count != 0)
            {
                int newAssessmentID = 0;
                Common.dbAccess.storeAssessment(Request.Form["txtAssessmentTitle"], "CI6110", Convert.ToInt32(Request.Form["txtPercentageWorth"]), Convert.ToInt32(Request.Form["txtDuration"]), "KU14009",ref newAssessmentID);
                foreach (Question question in questions)
                {
                    string questionID = newAssessmentID + "." + question.questionNumber;
                    Common.dbAccess.storeQuestion(questionID, question.question, newAssessmentID, question.questionNumber);
                    foreach (Answer answer in question.possibleAnswers)
                    {
                        string answerID = newAssessmentID + "." + question.questionNumber + "." + answer.answerLetter;
                        Common.dbAccess.storeAnswer(answerID, answer.answer, newAssessmentID, answer.answerLetter, answer.correct.ToString(), questionID);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}