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
    public partial class WebFormPizza : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvPizza_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void imgInsert_Click(object sender, ImageClickEventArgs e)
        {
            SqlParameter pCod = new SqlParameter("@CodPizza", System.Data.SqlDbType.NVarChar);
            pCod.Value = tbCod.Text;

            SqlParameter pDenumire = new SqlParameter("@Denumire", System.Data.SqlDbType.NVarChar);
            pDenumire.Value = tbDenumire.Text;

            SqlParameter pMarime = new SqlParameter("@Marime", System.Data.SqlDbType.NVarChar);
            pMarime.Value = tbMarime.Text;

            SqlParameter pDescriere= new SqlParameter("@ScurtaDescriere", System.Data.SqlDbType.NVarChar);
            pDescriere.Value = tbDescriere.Text;

            SqlParameter pPret = new SqlParameter("@Pret", System.Data.SqlDbType.Int);
            pPret.Value = int.Parse(tbPret.Text);

            string strInsert = "INSERT INTO [Pizza] ([CodPizza], [Denumire], [Marime], [ScurtaDescriere], [Pret]) VALUES (@CodPizza, @Denumire, @Marime, @ScurtaDescriere, @Pret)";

            SqlConnection conInsert =
                new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog = PizzaDB; Integrated Security = True; Pooling = False");

            SqlCommand myComm = new SqlCommand(strInsert, conInsert);
            myComm.Parameters.Add(pCod);
            myComm.Parameters.Add(pDenumire);
            myComm.Parameters.Add(pMarime);
            myComm.Parameters.Add(pDescriere);
            myComm.Parameters.Add(pPret);


            try
            {
                conInsert.Open();
                int n = myComm.ExecuteNonQuery();

                tbCod.Text = "";
                tbDenumire.Text = "";
                tbMarime.Text = "";
                tbDescriere.Text = "";
                tbPret.Text = "";

                gvPizza.DataBind();

                //tbMesaj.Text += "OK";
            }
            catch(Exception ex)
            {
                //tbMesaj.Text = ex.Message;
            }
            finally
            {
                conInsert.Close();
            }
        }

        protected void imgView_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("WebFormComenzi.aspx?tip=");
        }

        protected void btnVizualizeazaGraficPizza_Click(object sender, EventArgs e)
        {
            DataSourceSelectArguments args = new DataSourceSelectArguments();
            DataView dataView1 = (DataView)SqlDSPizza.Select(args);
            DataTable dataTabl1 = dataView1.ToTable();
            DataSet ds = new DataSet(); ds.Tables.Add(dataTabl1);
            Cache["PizzaCache"] = ds;

            this.ZedGraphWebPizza.RenderGraph += new ZedGraph.Web.ZedGraphWebControlEventHandler(this.OnRenderGraph);
        }

        private void OnRenderGraph(ZedGraph.Web.ZedGraphWeb z, System.Drawing.Graphics g, ZedGraph.MasterPane masterPane)
        {

            DataSet ds = (DataSet)Cache["PizzaCache"];
            GraphPane myPane = masterPane[0];
            myPane.Title.Text = "";
            myPane.XAxis.Title.Text = "Pizza (cod)"; myPane.YAxis.Title.Text = "Pret (in lei)";
            Color[] colors = {
                             Color.Red, Color.Yellow, Color.Green, Color.Blue,
                             Color.Purple,Color.Pink,Color.Plum,Color.Silver, Color.Salmon
                         };

            List<string> listaX = new List<string>();
            PointPairList list = new PointPairList();
            int i = 0;
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                listaX.Add(r[0].ToString()); // pizza
                list.Add(0, (double)r[4], i++); // pret
            }
            LineItem curve = myPane.AddCurve("Raport preturi", list, Color.Green, SymbolType.Diamond);
            curve.Line.IsSmooth = true;
            curve.Line.SmoothTension = 0.5F;
            curve.Line.Width = 2;

            curve.Symbol.Fill = new Fill(Color.White);
            curve.Symbol.Size = 10;

            myPane.XAxis.Scale.TextLabels = listaX.ToArray();
            myPane.XAxis.Type = AxisType.Text;

        }

        protected void imgProcedura_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("WebFormProcedura.aspx?tip=");
        }
    }
}