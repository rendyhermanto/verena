using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using APISkillTest.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace APISkillTest.Service
{
    public class clsAPIService : IServices
    {
        private readonly IConfiguration _configuration;

        public clsAPIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Employee

        public APIMessages<List<InquiryEmployeeRs>> InquiryEmp(APIMessages<InquiryEmployeeRq> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            //return result
            APIMessages<List<InquiryEmployeeRs>> messageResponse = new APIMessages<List<InquiryEmployeeRs>>();
            List<InquiryEmployeeRs> listResponse = new List<InquiryEmployeeRs>();
            //result db
            DataSet dsOut = new DataSet();
            //query
            string strQuery = "";

            try
            {
                #region query
                strQuery = @"
                        SELECT *
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
                    command.Parameters.Add("@pnEmployee", SqlDbType.BigInt);
                    command.Parameters["@pnEmployee"].Value = paramIn.Data.Employee;

                    using SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(command);
                    _sqlDataAdapter.Fill(dsOut);
                }
                #endregion

                #region Result
                if (dsOut == null || dsOut.Tables.Count == 0 || dsOut.Tables[0].Rows.Count == 0)
                    throw new Exception("Data Employee tidak ditemukan");

                else
                {
                    listResponse = JsonConvert.DeserializeObject<List<InquiryEmployeeRs>>(
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

        public APIMessage InsertEmpToDB(APIMessages<CRUDEmployee> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            //return result
            APIMessage messageResponse = new APIMessage();
            //result db
            DataSet dsOut = new DataSet();
            //query
            int executeQuery = -1;
            string strQuery = "";

            try
            {
                #region query
                strQuery = @"

                    declare @lastId bigint
                            , @newId bigint

                    select @lastId = max(Employee) from Employee
                    select @newId = @lastId + 1

                    INSERT Employee
                    VALUES (@newId, @pcDepartement, @pcJabatan, @pnFasilitasId)
                ";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);
                    command.Parameters.AddWithValue("@pcDepartement", paramIn.Data.Department);
                    command.Parameters.AddWithValue("@pcJabatan", paramIn.Data.Jabatan);
                    command.Parameters.AddWithValue("@pnFasilitasId", paramIn.Data.FasilitasId);

                    conn.Open();
                    executeQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion

                #region Result
                if (executeQuery < 0)
                    throw new Exception("Gagal Insert Employee ke database");

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

        public APIMessage UpdateEmpToDB(APIMessages<CRUDEmployee> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            //return result
            APIMessage responseMessage = new APIMessage();
            //result db
            DataSet dsOut = new DataSet();
            //query
            int executeQuery = -1;
            string strQuery = "";
            //call other method 
            APIMessages<InquiryEmployeeRq> _inqEmpRq;
            APIMessages<List<InquiryEmployeeRs>> _inqEmpRs;

            try
            {
                //get old data
                _inqEmpRq = new APIMessages<InquiryEmployeeRq>();
                _inqEmpRq.Data = new InquiryEmployeeRq();
                _inqEmpRq.Data.Employee = paramIn.Data.Employee;
                _inqEmpRs = this.InquiryEmp(_inqEmpRq);

                if (!_inqEmpRs.IsSuccess)
                    throw new Exception(_inqEmpRs.ErrorDescription);

                //Cek jika ada data duplicat
                if (_inqEmpRs.Data.Count != 1)
                    throw new Exception("Data ada yang duplikat");

                if (_inqEmpRs.Data[0].Department == paramIn.Data.Department)
                    paramIn.Data.Department = _inqEmpRs.Data[0].Department;

                if (_inqEmpRs.Data[0].Jabatan == paramIn.Data.Jabatan)
                    paramIn.Data.Jabatan = _inqEmpRs.Data[0].Jabatan;

                if (_inqEmpRs.Data[0].FasilitasId == paramIn.Data.FasilitasId)
                    paramIn.Data.FasilitasId = _inqEmpRs.Data[0].FasilitasId;

                #region query
                strQuery = @"

                --Update data Employee
                UPDATE Employee
                SET Department = @pcDepartment,
                    Jabatan = @pcJabatan,
                    FasilitasId = @pnFasilitasId
                WHERE Employee = @pnEmployee
                ";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);

                    //UPDATE TM
                    command.Parameters.AddWithValue("@pcDepartment", paramIn.Data.Department);
                    command.Parameters.AddWithValue("@pcJabatan", paramIn.Data.Jabatan);
                    command.Parameters.AddWithValue("@pnFasilitasId", paramIn.Data.FasilitasId);
                    command.Parameters.AddWithValue("@pnEmployee", paramIn.Data.Employee);

                    conn.Open();
                    executeQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion

                #region Result
                if (executeQuery < 0)
                    throw new Exception("Gagal Insert Employee ke database");

                else
                {
                    responseMessage.IsSuccess = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccess = false;
                responseMessage.ErrorDescription = "Failed to update Employee Error : " + ex.Message;
            }

            return responseMessage;
        }

        public APIMessage DeleteEmployee(APIMessages<CRUDEmployee> paramIn)
        {
            //koneksi db
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            //return result
            APIMessage responseMessage = new APIMessage();
            //result db
            DataSet dsOut = new DataSet();
            //query
            int executeQuery = -1;
            string strQuery = "";
            //get data 
            APIMessages<InquiryEmployeeRq> _inqEmpRq;
            APIMessages<List<InquiryEmployeeRs>> _inqEmpRs;

            try
            {
                //get old data
                _inqEmpRq = new APIMessages<InquiryEmployeeRq>();
                _inqEmpRq.Data = new InquiryEmployeeRq();
                _inqEmpRq.Data.Employee = paramIn.Data.Employee;
                _inqEmpRs = this.InquiryEmp(_inqEmpRq);

                if (!_inqEmpRs.IsSuccess)
                    throw new Exception(_inqEmpRs.ErrorDescription);

                if (_inqEmpRs.Data.Count != 1)
                    throw new Exception("Data duplikati di temukan");

                #region query
                strQuery = @"

                    --Delete data TM
                    DELETE Employee
                    WHERE Employee = @pnEmployee

                ";
                #endregion

                #region Parameter Query
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    SqlCommand command = new SqlCommand(strQuery, conn);

                    //Delete TM
                    command.Parameters.AddWithValue("@pnEmployee", paramIn.Data.Employee);

                    conn.Open();
                    executeQuery = command.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion

                #region Result
                if (executeQuery < 0)
                    throw new Exception("Gagal delete Employee");

                else
                {
                    responseMessage.IsSuccess = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                responseMessage.IsSuccess = false;
                responseMessage.ErrorDescription = "Failed to update Delete Employee, Error : " + ex.Message;
            }

            return responseMessage;
        }

        #endregion

    }
}
