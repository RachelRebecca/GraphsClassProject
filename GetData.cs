using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GraphsClassProject
{
    public class GetData
    {
        public Dictionary<String, String> GraphTypes { get; set; } 

        public GetData(String server, String database)
        {
            LoadVerticesFromSQL(server, database);
        }

        private void LoadVerticesFromSQL(String server, String database)
        {
            // table contains graphName, graphType
            GraphTypes = new Dictionary<string, string>();

            SqlConnection sqlCon = null;
            try
            {
                String strConnect = $"Server={server};Database={database};Trusted_Connection=True;";
                sqlCon = new SqlConnection(strConnect);
                sqlCon.Open();

                // get graph names
                SqlCommand getAllGraphs = new SqlCommand("spGetGraphNames", sqlCon);
                getAllGraphs.CommandType = CommandType.StoredProcedure;
                getAllGraphs.ExecuteNonQuery();
                SqlDataAdapter da1 = new SqlDataAdapter(getAllGraphs);
                DataSet dataset1 = new DataSet();
                da1.Fill(dataset1, "Graphs");

                var nrGraphs = dataset1.Tables["Graphs"].Rows.Count;
                for (int row = 0; row < nrGraphs; ++row)
                {
                    String name = (String)dataset1.Tables["Graphs"].Rows[row].ItemArray[0];
                    String type = (String)dataset1.Tables["Graphs"].Rows[row].ItemArray[1];
                    GraphTypes.Add(name, type);
                }    
            }
            catch (Exception ex)

            {
                 MessageBox.Show(" " + DateTime.Now.ToLongTimeString() + ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            finally

            {

                if (sqlCon != null && sqlCon.State == ConnectionState.Open)

                    sqlCon.Close();

            }

        }
    }
}
