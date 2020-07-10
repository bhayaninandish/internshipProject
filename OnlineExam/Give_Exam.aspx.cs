using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace OnlineExam
{


    public partial class Give_Exam : System.Web.UI.Page
    {
        List<string> fetchAns = new List<string>();
        List<string> ans = new List<string>();
        int tot = 0;
        int cnt = 0; // shows marks
        string fetcheddate;
        string fetchedtime;
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            con.Open();
            string query = "select date_time,time_limit from ExamDetails where eid=@p1";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@p1", Session["click_eid"].ToString());


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            if(dt.Rows.Count==1)
            {
                fetcheddate = dt.Rows[0][0].ToString();
                fetchedtime = dt.Rows[0][1].ToString();
            }


            con.Close();


            string checkdate = fetcheddate.Replace("T", " ");
            DateTime storedDate = DateTime.ParseExact(checkdate, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            fetchedtime = fetchedtime.Replace(":",".");
            
            DateTime addedtime = storedDate.AddHours(double.Parse(fetchedtime));
            
            DateTime current = DateTime.Now;
            if(DateTime.Compare(current, storedDate) >0 && DateTime.Compare(current,addedtime)<0)
            {
                Response.Write("<script language=javascript>alert('Exam started');</script>");
                getQues();
            }
            else
            {
                Response.Write("<script language=javascript>alert('NO Exam Found At This Time');</script>");
            }
            
        }
        

        void getQues()
        {
            con.Open();

            string query = "Select * from QuestionBank Where eid = @p1" ;

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@p1", Session["click_eid"].ToString());

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            tot = dt.Rows.Count;

            int i = 0;
            int j = dt.Columns.Count;

            while (i < dt.Rows.Count)
            {
                fetchAns.Add(dt.Rows[i]["answer"].ToString()); // Fetch all the correct answers from given table and stores in fetchAns List.

                Label newLineque = new Label();
                newLineque.Text = "<br/>";
                Panel1.Controls.Add(newLineque);


                Label no = new Label();
                no.Text = (i + 1).ToString() + " ";
                Panel1.Controls.Add(no);

                Label ques = new Label();
                ques.Text = dt.Rows[i]["questions"].ToString();
                Panel1.Controls.Add(ques);

                Label newLineque1 = new Label();
                newLineque1.Text = "<br/>";
                Panel1.Controls.Add(newLineque1);

                RadioButtonList rbl = new RadioButtonList();
                rbl.ID = "rbl_options" + i.ToString();
                for (int k = 3; k <= j - 2; k++)
                {
                    rbl.Items.Add(dt.Rows[i][k].ToString());
                    
                }
                Panel1.Controls.Add(rbl);


                i++;


            }

        }  //Loads all the question from given table.

        void check()
        {

            getSelectedAns();



            for (int i = 0; i < tot; i++)
            {
                if (ans[i].Equals(fetchAns[i]))
                {
                    cnt++;
                }
            }

            Label1.Text = "Marks are: " + cnt.ToString();
        } // checks the questions and set marks.

        void getSelectedAns()
        {
            string temp;
            int x;
            foreach (Control rbl in Panel1.Controls)
            {
                if (rbl is RadioButtonList)
                {
                    temp = ((RadioButtonList)rbl).SelectedIndex.ToString();
                    x = Int32.Parse(temp) + 1;
                    ans.Add(x.ToString());
                }
            }
        }   // stores all the selected ans in List.


        protected void Button1_Click(object sender, EventArgs e)
        {
            check();

           // fetchDataFromExams();

           insertData();
        }

       void insertData()
        {


            try
            {
                con.Open();

                string query = "UPDATE StudentExam SET marks=@p1 WHERE email=@p2 AND eid=@p3";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@p1", cnt.ToString());
                cmd.Parameters.AddWithValue("@p2", Session["mail"].ToString());
                cmd.Parameters.AddWithValue("@p3", Session["click_eid"].ToString());
                

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }

        } //insert data into database.

      /*  void fetchDataFromExams()
        {
            try
            {
                con.Open();

                string query = "SELECT * FROM Exams WHERE examid = @p1";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@p1", Session["examid"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    subect = dt.Rows[0]["subject"].ToString();
                    date_time = dt.Rows[0]["date_time"].ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }

        }*/ //to get subject and date_time information from Exams table.




    }
}