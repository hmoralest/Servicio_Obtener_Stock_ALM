using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Leer_Stock
{
    public static class Program
    {

        static string path = ConfigurationManager.ConnectionStrings["dbf"].ConnectionString;
        static string stop = ConfigurationManager.ConnectionStrings["stop"].ConnectionString;
        //static string entidad = ConfigurationManager.ConnectionStrings["ent"].ConnectionString;

        public static void Procesa(TextWriter tw)
        {

            /*NetworkShare.ConnectToShare(destinFolder, usuario, contrasena);

            DirectoryInfo source = new DirectoryInfo(sourceFolder);
            // el método GetFiles obtiene todos los archivos de un folder especifico, 
            // incluso puedes poner un filtro a que archivos te traiga, como por ejemplo
            // todos los archivos *.png o *.txt
            FileInfo[] filesToCopy = source.GetFiles();
            foreach (FileInfo filea in filesToCopy)
            {
                try
                {
                    tw.WriteLine(filea.Name);

                    // Boora de destino si existe antes del copiado
                    if (File.Exists(destinFolder + "\\" + filea.Name))
                    {
                        //Delete_RunAsDifferentUser(usuario, contrasena, ".", filea);
                        File.Delete(destinFolder + "\\" + filea.Name);
                        tw.WriteLine("delete existe dest " + filea.Name);
                    }
                    
                    // Realiza el copiado
                    //filea.CopyTo(destinFolder + "\\" + filea.Name);
                    filea.CopyTo(destinFolder+ "\\" + filea.Name);
                    tw.WriteLine("copia " + filea.Name);

                    // Borra de origen si ya realizó el copiado
                    if (File.Exists(destinFolder + "\\" + filea.Name))
                    {
                        File.Delete(sourceFolder + "\\" + filea.Name);
                        tw.WriteLine("delete final orig " + filea.Name);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }*/

        }

        public static void Procesa()
        {
            if (!System.IO.File.Exists(stop))
            {
                string entidad = "";

                if (Environment.GetEnvironmentVariable("codtda") != null)
                { entidad = Environment.GetEnvironmentVariable("codtda").ToString(); }
                if(entidad.Length == 3)
                { entidad = "50" + entidad; }

                string sConn = "Provider = vfpoledb.1;Data Source=" + System.IO.Path.GetDirectoryName(path) + ";Collating Sequence=general";

                //List<BataTransaccion.Ent_Stock> resul2 = new List<BataTransaccion.Ent_Stock>();

                DataTable result = new DataTable();
                result.TableName = "Stock_Almacen";
                result.Columns.Add("EMPRE", typeof(string));
                result.Columns.Add("CADEN", typeof(string));
                result.Columns.Add("CODAL", typeof(string));
                result.Columns.Add("ENTID", typeof(string));
                result.Columns.Add("ARTIC", typeof(string));
                result.Columns.Add("CALID", typeof(string));
                result.Columns.Add("TALLA", typeof(string));
                result.Columns.Add("PARES", typeof(string));

                using (System.Data.OleDb.OleDbConnection dbConn = new System.Data.OleDb.OleDbConnection(sConn))
                {
                    try
                    {
                        // Abro conexión
                        dbConn.Open();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    try
                    {
                        string sql_ano = "SELECT MAX(csal_ano) as CSAL_ANO FROM SCACSAL";
                        System.Data.OleDb.OleDbCommand com_ano = new System.Data.OleDb.OleDbCommand(sql_ano, dbConn);
                        System.Data.OleDb.OleDbDataAdapter ada_ano = new System.Data.OleDb.OleDbDataAdapter(com_ano);
                        DataTable tab_ano = new DataTable();
                        ada_ano.Fill(tab_ano);
                
                        string sql_sem = "SELECT MAX(csal_seman) as CSAL_SEMAN FROM SCACSAL WHERE CSAL_ANO= '"+ tab_ano.Rows[0]["CSAL_ANO"].ToString() +"'";
                        System.Data.OleDb.OleDbCommand com_sem = new System.Data.OleDb.OleDbCommand(sql_sem, dbConn);
                        System.Data.OleDb.OleDbDataAdapter ada_sem = new System.Data.OleDb.OleDbDataAdapter(com_sem);
                        DataTable tab_sem = new DataTable();
                        ada_sem.Fill(tab_sem);

                        string sql_stk = "SELECT CSAL_EMPRE, CSAL_CADEN, CSAL_CODAL, CSAL_ARTIC, CSAL_CALID, CSAL_RMED, CSAL_MED00, CSAL_MED01, CSAL_MED02, CSAL_MED03, CSAL_MED04, CSAL_MED05, CSAL_MED06, CSAL_MED07, CSAL_MED08, CSAL_MED09, CSAL_MED10, CSAL_MED11, ";
                        sql_stk = sql_stk + " RMED_MED00, RMED_MED01, RMED_MED02, RMED_MED03, RMED_MED04, RMED_MED05, RMED_MED06, RMED_MED07, RMED_MED08, RMED_MED09, RMED_MED10, RMED_MED11 ";
                        sql_stk = sql_stk + " FROM SCACSAL INNER JOIN SCARMED ON SCACSAL.CSAL_RMED = SCARMED.RMED_CODIG WHERE CSAL_ANO = '" + tab_ano.Rows[0]["CSAL_ANO"].ToString() + "' AND CSAL_SEMAN = '" + tab_sem.Rows[0]["CSAL_SEMAN"].ToString() + "'";
                        //string sql_stk = "SELECT CSAL_EMPRE, CSAL_CADEN, CSAL_CANAL, CSAL_SECCI, CSAL_ALMAC, CSAL_CODAL, CSAL_ARTIC, CSAL_CALID, CSAL_RMED, CSAL_STOCK, CSAL_MED00, CSAL_MED01, CSAL_MED02, CSAL_MED03, CSAL_MED04, CSAL_MED05, CSAL_MED06, CSAL_MED07, CSAL_MED08, CSAL_MED09, CSAL_MED10, CSAL_MED11 ";
                        //sql_stk = sql_stk + " FROM SCACSAL WHERE CSAL_ANO = '" + tab_ano.Rows[0]["CSAL_ANO"].ToString() + "' AND CSAL_SEMAN = '" + tab_sem.Rows[0]["CSAL_SEMAN"].ToString() + "'";
                        System.Data.OleDb.OleDbCommand com_stk = new System.Data.OleDb.OleDbCommand(sql_stk, dbConn);
                        System.Data.OleDb.OleDbDataAdapter ada_stk = new System.Data.OleDb.OleDbDataAdapter(com_stk);
                        DataTable tab_stk = new DataTable();
                        ada_stk.Fill(tab_stk);

                        //string sql_med = "SELECT CSAL_EMPRE, CSAL_CADEN, CSAL_CANAL, CSAL_SECCI, CSAL_ALMAC, CSAL_CODAL, CSAL_ARTIC, CSAL_CALID, CSAL_RMED, CSAL_STOCK, CSAL_MED00, CSAL_MED01, CSAL_MED02, CSAL_MED03, CSAL_MED04, CSAL_MED05, CSAL_MED06, CSAL_MED07, CSAL_MED08, CSAL_MED09, CSAL_MED10, CSAL_MED11  FROM SCARMED";
                        //System.Data.OleDb.OleDbCommand com_med = new System.Data.OleDb.OleDbCommand(sql_med, dbConn);
                        //System.Data.OleDb.OleDbDataAdapter ada_med = new System.Data.OleDb.OleDbDataAdapter(com_med);
                        //DataTable tab_med = new DataTable();
                        //ada_med.Fill(tab_med);

                        if (tab_stk.Rows.Count > 0)
                        {
                            foreach (DataRow row in tab_stk.Rows)
                            {
                                if (Convert.ToInt32(row["CSAL_MED00"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED00"].ToString(), row["CSAL_MED00"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED01"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED01"].ToString(), row["CSAL_MED01"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED02"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED02"].ToString(), row["CSAL_MED02"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED03"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED03"].ToString(), row["CSAL_MED03"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED04"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED04"].ToString(), row["CSAL_MED04"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED05"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED05"].ToString(), row["CSAL_MED05"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED06"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED06"].ToString(), row["CSAL_MED06"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED07"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED07"].ToString(), row["CSAL_MED07"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED08"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED08"].ToString(), row["CSAL_MED08"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED09"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED09"].ToString(), row["CSAL_MED09"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED10"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED10"].ToString(), row["CSAL_MED10"].ToString());
                                }
                                if (Convert.ToInt32(row["CSAL_MED11"].ToString()) > 0)
                                {
                                    result.Rows.Add(row["CSAL_EMPRE"].ToString(), row["CSAL_CADEN"].ToString(), row["CSAL_CODAL"].ToString(), entidad, row["CSAL_ARTIC"].ToString(), row["CSAL_CALID"].ToString(), row["RMED_MED11"].ToString(), row["CSAL_MED11"].ToString());
                                }
                            }
                        }

                        /*resul2 = (from DataRow row in result.Rows
                                  select new BataTransaccion.Ent_Stock()
                                  {
                                    = row["EMPRE"].ToString(),
                                    = row["CADEN"].ToString(),
                                    = row["CODAL"].ToString(),
                                      cod_tda = row["ENTID"].ToString(),
                                      art_cod = row["ARTIC"].ToString(),
                                      art_cal = row["CALID"].ToString(),
                                      art_talla = row["TALLA"].ToString(),
                                      art_pares = Convert.ToInt32(row["PARES"]),
                                  }).ToList();*/

                        //if (resul2.Count > 0)
                        //{
                        //   var array = new BataTransaccion.Ent_Lista_Stock();
                        //    array.lista_stock = result.ToArray();

                        //    BataTransaccion.Ent_MsgTransac msg = batatran.ws_envia_stock_tda(header_user, array);

                        /*Nota*/
                        //msg.codigo = "0";
                        //msg.descripcion = "Se actualizo correctamente";

                        //msg.codigo = "1";
                        //msg.descripcion = "descripcion de error";
                        //}

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }

            }   // Si NO Existe Archivo
        }

        public static string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }
        static void Main(string[] args)
        {
            string path = @"C:\log.txt";
            TextWriter tw = new StreamWriter(path, true);
            tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ", Intervalo: " );
            //tw.Close();
            try
            {
                Procesa();
                tw.WriteLine("ok " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                tw.WriteLine("error " + ex.Message);
                tw.Close();
            }
            tw.Close();
        }
    }
}
