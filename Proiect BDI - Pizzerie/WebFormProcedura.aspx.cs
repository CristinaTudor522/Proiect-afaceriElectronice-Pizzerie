using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Proiect_BDI___Pizzerie
{
    public partial class WebFormProcedura : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void imgBack_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("WebFormPizza.aspx?tip=");
        }

        protected void imgViewProcPizza_Click(object sender, ImageClickEventArgs e)
        {
            string strApelProc = "PizzaIntervalPreturi";
            SqlConnection myConn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = PizzaDB; Integrated Security = True; Pooling = False");

            SqlCommand myComm = new SqlCommand(strApelProc, myConn);
            myComm.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter pLimitaMin = new SqlParameter("@limitaMinPret", System.Data.SqlDbType.Float);
            pLimitaMin.Value = int.Parse(tbLimitaMin.Text);
            SqlParameter pLimitaMax = new SqlParameter("@limitaMaxPret", System.Data.SqlDbType.Float);
            pLimitaMax.Value = double.Parse(tbLimitaMax.Text);
            SqlParameter pNrPizza = new SqlParameter("@nrPizza", System.Data.SqlDbType.Int);
            pNrPizza.Direction = System.Data.ParameterDirection.Output;
            myComm.Parameters.Add(pLimitaMin);
            myComm.Parameters.Add(pLimitaMax);
            myComm.Parameters.Add(pNrPizza);
            SqlDataReader dr = null;
            try
            {
                myConn.Open();
                dr = myComm.ExecuteReader();
                while (dr.Read())
                {
                    tbListaPizza.Text += "\r\n" +" *** "+ dr[0] + " - " + dr[1] + " - " + dr[4] + " lei";
                }
            }
            catch (Exception ex)
            {
                tbListaPizza.Text += "\r\n" + ex.Message;
            }
            finally
            {
                dr.Close();
                myConn.Close();

            }

            tbNrPizza.Text = pNrPizza.Value.ToString();
        
        }

    }
}