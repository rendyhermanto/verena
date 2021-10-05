using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using skillTest.Models;
namespace skillTest.Service
{
    public class clsAPIService
    {
        private readonly IConfiguration _configuration;

        public clsAPIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region About

        public APIMessages<List<InquiryEmployeeResponse>> InquiryAbout(APIMessages<InquiryEmployeeRequest> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            //return result
            APIMessages<List<InquiryEmployeeResponse>> messageResponse = new APIMessages<List<InquiryEmployeeResponse>>();
            List<InquiryEmployeeResponse> listResponse = new List<InquiryEmployeeResponse>();
            //result db
            DataSet dsOut = new DataSet();
            //query
            string strQuery = "";

            try
            {
                #region query
                strQuery = @"
                        SELECT Employee, Department, Jabatan, FasilitasId
                        FROM Employee
                    ";

                //jika parameter tidak sama dengan 0
                if (paramIn.Data.Employee != 0)
                    strQuery += "WHERE Employee = @pnEmployee";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);
                    command.Parameters.Add("@pnAboutId", SqlDbType.BigInt);
                    command.Parameters["@pnAboutId"].Value = paramIn.Data.Employee;

                    using SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(command);
                    _sqlDataAdapter.Fill(dsOut);
                }
                #endregion

                #region Result
                if (dsOut == null || dsOut.Tables.Count == 0 || dsOut.Tables[0].Rows.Count == 0)
                    throw new Exception("Data About tidak ditemukan");

                else
                {
                    listResponse = JsonConvert.DeserializeObject<List<InquiryEmployeeResponse>>(
                        JsonConvert.SerializeObject(dsOut.Tables[0], Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }));

                    messageResponse.Data = listResponse;
                    messageResponse.IsSuccess = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                messageResponse.IsSuccess = false;
                messageResponse.ErrorDescription = ex.Message;
            }

            return messageResponse;
        }

        /*
        public APIMessage InsertAboutToDB(APIMessages<InsertAboutRequest> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("PortfolioConnection");
            //return result
            APIMessage messageResponse = new APIMessage();
            //result db
            DataSet dsOut = new DataSet();
            //query
            int executeQuery = -1;
            string strQuery = "";
            string strAction = "I";

            try
            {
                #region query
                strQuery = @"

                    DECLARE @AboutId bigint

                    INSERT About_TM
                    VALUES (@pcAboutName, @pcAboutDescription, @pdCreateDate)

                    SET @AboutId = SCOPE_IDENTITY()

                    INSERT About_TH
                    VALUES (@AboutId, @pcAboutName, @pcAboutDescription, @pdCreateDate, 0, @pcAction, @pdInsertDate)

                ";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);

                    //INSERT TM
                    command.Parameters.AddWithValue("@pcAboutName", paramIn.Data.AboutName);
                    command.Parameters.AddWithValue("@pcAboutDescription", paramIn.Data.AboutDescription);
                    command.Parameters.AddWithValue("@pdCreateDate", System.DateTime.Now);

                    //INSERT TH
                    command.Parameters.AddWithValue("@pcAction", strAction);
                    command.Parameters.AddWithValue("@pdInsertDate", System.DateTime.Now);

                    conn.Open();
                    executeQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion

                #region Result
                if (executeQuery < 0)
                    throw new Exception("Gagal Insert About ke database");

                else
                {
                    messageResponse.IsSuccess = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                messageResponse.IsSuccess = false;
                messageResponse.ErrorDescription = ex.Message;
            }

            return messageResponse;
        }

        public APIMessage UpdateAboutToDB(APIMessages<UpdateAboutRequest> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("PortfolioConnection");
            //return result
            APIMessage responseMessage = new APIMessage();
            //result db
            DataSet dsOut = new DataSet();
            //query
            int executeQuery = -1;
            string strQuery = "";
            //call other method 
            APIMessages<InquiryAboutRequest> _inqAboutRq;
            APIMessages<List<InquiryAboutResponse>> _inqAboutRs;

            try
            {
                //get old data
                _inqAboutRq = new APIMessages<InquiryAboutRequest>();
                _inqAboutRq.Data = new InquiryAboutRequest();
                _inqAboutRq.Data.AboutId = paramIn.Data.AboutId;
                _inqAboutRs = this.InquiryAbout(_inqAboutRq);

                if (!_inqAboutRs.IsSuccess)
                    throw new Exception(_inqAboutRs.ErrorDescription);

                //Cek jika ada data duplicat
                if (_inqAboutRs.Data.Count != 1)
                    throw new Exception("Data ada yang duplikat");

                if (_inqAboutRs.Data[0].AboutName == paramIn.Data.AboutName)
                    paramIn.Data.AboutName = _inqAboutRs.Data[0].AboutName;

                if (_inqAboutRs.Data[0].AboutDescription == paramIn.Data.AboutDescription)
                    paramIn.Data.AboutDescription = _inqAboutRs.Data[0].AboutDescription;


                #region query
                strQuery = @"

                --INSERT old data to TH
                INSERT About_TH
                VALUES (@pnOldAboutId, @pcOldAboutName, @pcOldAboutDescription, @pdOldCreateDate, 0, 'O', @pdInsertDate)

                --Update data About TM
                UPDATE About_TM
                SET AboutName = @pcAboutName,
                    AboutDescription = @pcAboutDescription
                WHERE AboutId = @pnAboutId

                --INSERT new data to TH
                INSERT About_TH
                VALUES (@pnAboutId, @pcAboutName, @pcAboutDescription, @pdOldCreateDate, 0, 'U', @pdInsertDate)

            ";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);

                    //INSERT TH
                    command.Parameters.AddWithValue("@pnOldAboutId", _inqAboutRs.Data[0].AboutId);
                    command.Parameters.AddWithValue("@pcOldAboutName", _inqAboutRs.Data[0].AboutName);
                    command.Parameters.AddWithValue("@pcOldAboutDescription", _inqAboutRs.Data[0].AboutDescription);
                    command.Parameters.AddWithValue("@pdOldCreateDate", _inqAboutRs.Data[0].CreateDate);
                    command.Parameters.AddWithValue("@pdInsertDate", System.DateTime.Now);

                    //UPDATE TM
                    command.Parameters.AddWithValue("@pcAboutName", paramIn.Data.AboutName);
                    command.Parameters.AddWithValue("@pcAboutDescription", paramIn.Data.AboutDescription);
                    command.Parameters.AddWithValue("@pnAboutId", paramIn.Data.AboutId);

                    conn.Open();
                    executeQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion

                #region Result
                if (executeQuery < 0)
                    throw new Exception("Gagal Insert About ke database");

                else
                {
                    responseMessage.IsSuccess = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccess = false;
                responseMessage.ErrorDescription = "Failed to update About Error : " + ex.Message;
            }

            return responseMessage;
        }

        public APIMessage DeleteAbout(APIMessages<DeleteAboutRequest> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("PortfolioConnection");
            //return result
            APIMessage responseMessage = new APIMessage();
            //result db
            DataSet dsOut = new DataSet();
            //query
            int executeQuery = -1;
            string strQuery = "";
            //get data 
            APIMessages<InquiryAboutRequest> _inqAboutRq;
            APIMessages<List<InquiryAboutResponse>> _inqAboutRs;

            try
            {
                //get old data
                _inqAboutRq = new APIMessages<InquiryAboutRequest>();
                _inqAboutRq.Data = new InquiryAboutRequest();
                _inqAboutRq.Data.AboutId = paramIn.Data.AboutId;
                _inqAboutRs = this.InquiryAbout(_inqAboutRq);

                if (!_inqAboutRs.IsSuccess)
                    throw new Exception(_inqAboutRs.ErrorDescription);

                if (_inqAboutRs.Data.Count != 1)
                    throw new Exception("Data duplikati di temukan");

                #region query
                strQuery = @"

                    --Insert data to TH 
                    INSERT About_TH
                    VALUES (@pnOldAboutId, @pcOldAboutName, @pcOldAboutDescription, @pdOldCreateDate, 0, 'D', @pdInsertDate)

                    --Delete data TM
                    DELETE About_TM
                    WHERE AboutId = @pnAboutId

                ";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);

                    //INSERT TH
                    command.Parameters.AddWithValue("@pnOldAboutId", _inqAboutRs.Data[0].AboutId);
                    command.Parameters.AddWithValue("@pcOldAboutName", _inqAboutRs.Data[0].AboutName);
                    command.Parameters.AddWithValue("@pcOldAboutDescription", _inqAboutRs.Data[0].AboutDescription);
                    command.Parameters.AddWithValue("@pdOldCreateDate", _inqAboutRs.Data[0].CreateDate);
                    command.Parameters.AddWithValue("@pdInsertDate", System.DateTime.Now);

                    //Delete TM
                    command.Parameters.AddWithValue("@pnAboutId", paramIn.Data.AboutId);

                    conn.Open();
                    executeQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion

                #region Result
                if (executeQuery < 0)
                    throw new Exception("Gagal delete About");

                else
                {
                    responseMessage.IsSuccess = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccess = false;
                responseMessage.ErrorDescription = "Failed to update Delete About, Error : " + ex.Message;
            }

            return responseMessage;
        }
        */
        #endregion

    }
}
