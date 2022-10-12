using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZedGraph;

namespace Proiect_BDI___Pizzerie
{
    public partial class WebFormComenzi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvComenzi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnInsertComanda_Click(object sender, EventArgs e)
        {
            SqlParameter pID = new SqlParameter("@IdComanda", System.Data.SqlDbType.Int);
            pID.Value = int.Parse(tbId.Text);

            SqlParameter pNume = new SqlParameter("@Nume", System.Data.SqlDbType.NVarChar);
            pNume.Value = tbNume.Text;

            SqlParameter pPrenume = new SqlParameter("@Prenume", System.Data.SqlDbType.NVarChar);
            pPrenume.Value = tbPrenume.Text;

            SqlParameter pAdresa = new SqlParameter("@Adresa", System.Data.SqlDbType.NVarChar);
            pAdresa.Value = tbAdresa.Text;

            SqlParameter pCodPizza = new SqlParameter("@CodPizza", System.Data.SqlDbType.NVarChar);
            pCodPizza.Value = tbCodPizza.Text;

            SqlParameter pCantitate = new SqlParameter("@Cantitate", System.Data.SqlDbType.Int);
            pCantitate.Value = int.Parse(tbCantitate.Text);

            string strInsert = "INSERT INTO [Comenzi] ([IdComanda], [Nume], [Prenume], [Adresa], [CodPizza], [Cantitate]) VALUES (@IdComanda, @Nume, @Prenume, @Adresa, @CodPizza, @Cantitate)";
            SqlConnection conInsert = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=PizzaDB;Integrated Security=True;Pooling=False");

            SqlCommand myComm = new SqlCommand(strInsert, conInsert);
            myComm.Parameters.Add(pID);
            myComm.Parameters.Add(pNume);
            myComm.Parameters.Add(pPrenume);
            myComm.Parameters.Add(pAdresa);
            myComm.Parameters.Add(pCodPizza);
            myComm.Parameters.Add(pCantitate);

            try
            {
                conInsert.Open();
                int n = myComm.ExecuteNonQuery();
                gvComenzi.DataBind();

                tbId.Text = "";
                tbNume.Text = "";
                tbPrenume.Text = "";
                tbAdresa.Text = "";
                tbCodPizza.Text = "";
                tbCantitate.Text = "";

                //tbMesaj.Text += "\r\nInserare OK";

            }
            catch (Exception ex)
            {
               //tbMesaj.Text += "\r\nInserare esuata " + ex.Message;
            }
            finally
            {
                conInsert.Close();
            }
        }

        protected void btnVizualizeazaGraficComenzi_Click(object sender, EventArgs e)
        {
            DataSourceSelectArguments args = new DataSourceSelectArguments();
            DataView dataView1 = (DataView)SqlDataSourceComenzi.Select(args);
            DataTable dataTabl1 = dataView1.ToTable();
            DataSet ds = new DataSet(); ds.Tables.Add(dataTabl1);
            Cache["ComenziCache"] = ds;

            this.ZedGraphWebComenzi.RenderGraph += new ZedGraph.Web.ZedGraphWebControlEventHandler(this.OnRenderGraph);
        }

        private void OnRenderGraph(ZedGraph.Web.ZedGraphWeb z, System.Drawing.Graphics g, ZedGraph.MasterPane masterPane)
        {

            DataSet ds = (DataSet)Cache["ComenziCache"];
            GraphPane myPane = masterPane[0];
            myPane.Title.Text = "";
            myPane.XAxis.Title.Text = "Pizza (cod)"; myPane.YAxis.Title.Text = "Cantitate (in bucati)";
            Color[] colors = {
                             Color.Red, Color.Yellow, Color.Green, Color.Blue,
                             Color.Purple,Color.Pink,Color.Plum,Color.Silver, Color.Salmon
                         };

            List<string> listaX = new List<string>();
            PointPairList list = new PointPairList();
            int i = 0;
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                listaX.Add(r[4].ToString()); // cod pizza
                list.Add(0, (int)r[5], i++); // cantitate
            }

            BarItem myCurve = myPane.AddBar("Pizza - cantitate", list, Color.Blue);
            myCurve.Bar.Fill = new Fill(colors);
            myCurve.Bar.Fill.Type = FillType.GradientByZ;
            myCurve.Bar.Fill.RangeMin = 0;
            myCurve.Bar.Fill.RangeMax = list.Count;
            myPane.XAxis.Type = AxisType.Text;
            myPane.XAxis.Scale.TextLabels = listaX.ToArray();

        }

        protected void btnInapoi_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebFormPizza.aspx?tip=");
        }
        
    }
}