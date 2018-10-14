using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace firstChoice.Models
{
    public class DatabaseAccess
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection();
        private SqlCommand sqlCommand = new SqlCommand();
        private SqlDataReader reader;

        public void setupStoredProcedure(string storedProcedure)
        {
            try{
                sqlConnection.ConnectionString = connectionString;
                sqlConnection.Open();
            }catch(Exception ex){
                Console.WriteLine("");
            }
            finally{
                sqlConnection.CreateCommand();
            }

            sqlCommand = new SqlCommand(storedProcedure, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
        }
        public void storeAssessment(string title, string moduleID, int percentageWorth, int duration, string lecturerID, ref int newAssessmentID)
        {
            setupStoredProcedure("storeAssessment");
            sqlCommand.Parameters.AddWithValue("@title", title);
            sqlCommand.Parameters.AddWithValue("@moduleID", moduleID);
            sqlCommand.Parameters.AddWithValue("@percentageWorth", percentageWorth);
            sqlCommand.Parameters.AddWithValue("@duration", duration);
            sqlCommand.Parameters.AddWithValue("@lecturerID", lecturerID);
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                newAssessmentID = Convert.ToInt32(reader.GetValue(0));
            }
            reader.Close();
            sqlConnection.Close();
        }
        public void storeQuestion(string questionID, string question, int assessmentID, int questionNumber)
        {
            setupStoredProcedure("storeQuestion");
            sqlCommand.Parameters.AddWithValue("@questionID", questionID);
            sqlCommand.Parameters.AddWithValue("@question", question);
            sqlCommand.Parameters.AddWithValue("@assessmentID", assessmentID);
            sqlCommand.Parameters.AddWithValue("@questionNumber", questionNumber);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void storeAnswer(string answerID, string answer, int assessmentID, string answerLetter, string correct, string questionID)
        {
            setupStoredProcedure("storeAnswer");
            sqlCommand.Parameters.AddWithValue("@answerID", answerID);
            sqlCommand.Parameters.AddWithValue("@answer", answer);
            sqlCommand.Parameters.AddWithValue("@assessmentID", assessmentID);
            sqlCommand.Parameters.AddWithValue("@answerLetter", answerLetter);
            sqlCommand.Parameters.AddWithValue("@correct", correct);
            sqlCommand.Parameters.AddWithValue("@questionID", questionID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<Assessment> getAssessmentByLecturerID(string lecturerID)
        {
            List<Assessment> assessments = new List<Assessment>();
            setupStoredProcedure("getAssessmentBySingleColumnValue");
            sqlCommand.Parameters.AddWithValue("@lecturerID",lecturerID);
            reader = sqlCommand.ExecuteReader();
            while(reader.Read())
            {
                Assessment assessment = new Assessment(){ assessmentID = reader.GetInt32(0), title = reader.GetString(1), moduleID = reader.GetString(2), percentageWorth = reader.GetInt32(3), duration = reader.GetInt32(4), lecturerID = reader.GetString(5)  };
                assessments.Add(assessment);
            }
            reader.Close();
            return assessments;
        }
    }
}