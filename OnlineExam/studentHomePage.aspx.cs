using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace OnlineExam
{
    public partial class studentHomePage1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            fetchExams();

        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString());

        protected void btn_entereid_Click(object sender, EventArgs e)
        {
            insertIntoStudentExam();

            
        }

        void insertIntoStudentExam()
        {
            con.Open();


            string query = "INSERT INTO StudentExam VALUES(@p1,@p2,@p3)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@p1", Session["mail"].ToString());
            cmd.Parameters.AddWithValue("@p2", tb_enter_rid.Text);
            cmd.Parameters.AddWithValue("@p3", "0");

            cmd.ExecuteNonQuery();
            con.Close();

            fetchExams();

        }

        void fetchExams()
        {
            con.Open();
            string query = "SELECT * FROM ExamDetails WHERE eid in (Select eid FROM StudentExam Where email=@p1)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@p1", Session["mail"].ToString());

            SqlDataAdapter da = new SqlDataAdapter(cmd);


            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            Repeater1.DataSource = ds;
            Repeater1.DataBind();


            con.Close();
        }

        protected void Click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Click")
            {
               Session["click_eid"]  = e.CommandArgument;
                Response.Redirect("Give_Exam.aspx");
            }
        }
    }
}