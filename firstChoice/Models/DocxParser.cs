using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace firstChoice.Models
{
    public class DocxParser
    {
        public XmlDocument xmlDoc { get; set; }
        public XmlReader xmlReader { get; set; }
        public Boolean topLevel { get; set; } = false;
        public Question question { get; set; } = new Question();
        public Answer answer { get; set; }
        public List<Question> questionList { get; set; }

        public List<Question> getQuestionswithCorrectAnswers(string filename1, string filename2)
        {
            return convertAnswerSheet(filename2,convertQuestionPaper(filename1));
        }
        public List<Question> convertQuestionPaper(string filename1)
        {
            try{
                unzipFile(filename1);
                return validateQuestionSheet();
            }catch(Exception ex){
                questionList = new List<Question>();
                return questionList;
            }
            
        }
        public List<Question> convertAnswerSheet(string filename2,List<Question> questions)
        {
            try{
                unzipFile(filename2);
                return updateCorrectAnswers(questions);
            } catch (Exception ex)
            {
                questionList = new List<Question>();
                return questionList;
            }
        }
        public void unzipFile(string sourcePath)
        {
            string extractToPath = "C:\\Users\\Sofya.SHAJI-DOMAIN\\Documents\\Visual Studio 2017\\Projects\\firstChoice\\firstChoice\\ExtractedFiles\\" + "Folder" + Common.random.Next(1, 1000000000);
            ZipFile.ExtractToDirectory(sourcePath, extractToPath);
            xmlDoc = new XmlDocument();
            xmlDoc.Load(extractToPath + "\\word\\document.xml");
        }
        public List<Question> getQuestionSheet()
        {
            string[] answers = { "a", "b", "c", "d" };
            List<Answer> possibleAnswers = new List<Answer>();
            List<Question> listOfQuestions = new List<Question>();
            xmlReader = XmlReader.Create(new System.IO.StringReader(xmlDoc.InnerXml));
            readNextItem();
            int answerArrayPointer = 0;
            int i = 0;
            while (xmlReader.Read() || question.possibleAnswers != null)
            {
                if(topLevel || (xmlReader.Value == "" && question.possibleAnswers != null))
                {
                    if(i != 0) { listOfQuestions.Add(question); }
                    i++;
                    question = new Question(xmlReader.Value,i);
                    answerArrayPointer = 0;
                    topLevel = false;
                    possibleAnswers = new List<Answer>();
                }
                else
                {
                    try
                    {
                        answer = new Answer(xmlReader.Value, i, answers[answerArrayPointer]);
                        possibleAnswers.Add(answer);
                        question.possibleAnswers = possibleAnswers;
                        answerArrayPointer++;
                    }catch(Exception e)
                    {
                        listOfQuestions.Clear();
                        break;
                    }
                }
                readNextItem(); 
            }
            return listOfQuestions;
        }
        public List<Question> updateCorrectAnswers(List<Question> listOfQuestions)
        {
            xmlReader = XmlReader.Create(new System.IO.StringReader(xmlDoc.InnerXml));
            readNextItem();
            topLevel = true;
            int i = 0;
            int correctAnswers = 0;

            while (xmlReader.Read())
            {
                Boolean broken = false; 
                i++;
                foreach (Question question in listOfQuestions)
                {
                    if (broken) { break; }
                    foreach (Answer answer in question.possibleAnswers)
                    {
                        if(answer.questionNumber == i && answer.answerLetter.ToLower() == xmlReader.Value.ToLower())
                        {
                            answer.correct = true;
                            correctAnswers++;
                            broken = true;
                            break;
                        }
                    }
                }
                readNextItem();
                if (topLevel == false) { break; } else if (topLevel == true && i == listOfQuestions.Count) { listOfQuestions.Clear(); break;  }
            }
            if (correctAnswers != listOfQuestions.Count) { listOfQuestions.Clear(); }
            return listOfQuestions;
        }
        public void isNextATopLevel()
        {
            if (xmlReader.GetAttribute("w:val") == "0")
            {
                topLevel = true;
            }
            else
            {
                topLevel = false;
            }
        }
        public void readNextItem()
        {
            xmlReader.ReadToFollowing("w:ilvl");
            isNextATopLevel();
            xmlReader.ReadToFollowing("w:r");
            xmlReader.ReadToDescendant("w:t");
        }
        public List<Question> validateQuestionSheet()
        {
            List<Question>  listOfQuestions = getQuestionSheet();
            Boolean validDocument = true;
            foreach (Question item in listOfQuestions)
            {
                try{
                    if (item.possibleAnswers.Count < 2 || item.possibleAnswers.Count > 5 || item.question.Length == 0 || item.question == null)
                    {
                        validDocument = false;
                        break;
                    }
                } catch(Exception ex){
                    break;
                }

            }
            if(validDocument == false) { listOfQuestions.Clear(); }
            return listOfQuestions;
        }
    }
}