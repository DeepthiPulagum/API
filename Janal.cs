using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml;


public class Janal
{
    public Janal()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string VerifyUser(string emailId, string userPassword)
    {
        //aakashService.Service aService = new aakashService.Service();
        Aakash aService = new Aakash();

        //UserInfo.Service userinf = new UserInfo.Service();
        XmlDocument _xUserInfo = new XmlDocument();
        _xUserInfo.LoadXml("<XML>" + aService.get_Login(emailId, userPassword).InnerXml + "</XML>");

        string _Error = "";
        try
        {
            _Error = _xUserInfo.SelectSingleNode("XML/RESPONSE/ERROR").InnerText;
        }
        catch (Exception ex)
        {
            _Error = "";
        }


        return _Error;

    }
    [WebMethod]
    public XmlDocument get_vendor(string clientid, string userEmailId, string userPassword)
    {

        //       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                       + "<REQUEST>"
                       + "<CLIENT_ID>" + clientid + "</CLIENT_ID>"
                       + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";

        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (clientid != "" & clientid != "0")
            {
                strSub = " and client_id=" + clientid;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    //xml_string +=

                    //   "<REQUEST>" + "<VENDOR_ID>" + vendorid + "</VENDOR_ID>" +
                    //                                   "</REQUEST>";
                    string strSql = " SELECT vm.vendor_name,vm.vendor_id, vd.vendor_address1,vd.vendor_city,vd.vendor_postal_code,vd.vendor_country,vd.vendor_PhoneNumber,vd.vendor_FaxNumber FROM ovms_vendor_details as vd join  " +
                                    " ovms_vendors as vm on vd.vendor_id = vm.vendor_id  " +
                                    "     where vd.active = 1  " +
                                    "     and vendor_name = 'ALL'  " +
                                    "     UNION  " +
                                    "     SELECT vm.vendor_name,vm.vendor_id, vd.vendor_address1, vd.vendor_city,vd.vendor_postal_code,vd.vendor_country,vd.vendor_PhoneNumber,vd.vendor_FaxNumber FROM ovms_vendor_details as vd join " +
                                    "     ovms_vendors as vm on vd.vendor_id = vm.vendor_id " +
                                    "     where vd.active = 1 " +
                                    " " + strSub + "" +
                                    "     and vendor_name <> 'ALL' ";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<VENDOR_ID ID ='" + RowID + "'>" +
                            "  <VENDOR_ID> " + reader["vendor_id"] + " </VENDOR_ID> " +
                       "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +

                       "<VENDOR_ADDRESS1><![CDATA[" + reader["vendor_address1"] + "]]></VENDOR_ADDRESS1>" +

                       "<VENDOR_CITY>" + reader["vendor_city"] + "</VENDOR_CITY>" +
                       "<VENDOR_POSTAL_CODE>" + reader["vendor_postal_code"] + "</VENDOR_POSTAL_CODE>" +
                       "<VENDOR_COUNTRY>" + reader["vendor_country"] + "</VENDOR_COUNTRY>" +
                       "<VENDOR_PHONENUMBER>" + reader["vendor_PhoneNumber"] + "</VENDOR_PHONENUMBER>" +
                       "<VENDOR_FAXNUMBER>" + reader["vendor_FaxNumber"] + "</VENDOR_FAXNUMBER>" +
                       "</VENDOR_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<DATA> Unable to select vendor" + "</DATA>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        xml_string += "</RESPONSE>" +
                      "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    //    [WebMethod]
    //    public XmlDocument get_vendor(string vendorid, string userEmailId, string userPassword)
    //    {

    ////       //
    //        SqlConnection conn;
    //        string xml_string = "";
    //        string strSub = "";
    //        string errString = "";
    //        errString = VerifyUser(userEmailId, userPassword);
    //        xml_string += "<XML>"
    //                       + "<REQUEST>"
    //                       + "<VENDOR_ID>" + vendorid + "</VENDOR_ID>"
    //                       + "</REQUEST>";
    //        xml_string += "<RESPONSE>";
    //        if (errString != "")
    //        {
    //            xml_string += "<ERROR>" + errString + "</ERROR>";

    //        }
    //        else
    //        {
    //            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
    //            if (vendorid != "" & vendorid != "0")
    //            {
    //                strSub = " and CLIENT_id=" + vendorid;
    //            }
    //            try
    //            {
    //                if (conn.State == System.Data.ConnectionState.Closed)
    //                {
    //                    conn.Open();
    //                    //xml_string +=

    //                    //   "<REQUEST>" + "<VENDOR_ID>" + vendorid + "</VENDOR_ID>" +
    //                    //                                   "</REQUEST>";
    //                    string strSql = " SELECT vm.vendor_name,vm.vendor_id, vd.vendor_address1, vd.vendor_address2,vd.vendor_city,vd.vendor_postal_code,vd.vendor_country,vd.vendor_PhoneNumber,vd.vendor_FaxNumber FROM ovms_vendor_details as vd join  " +
    //                                    " ovms_vendors as vm on vd.vendor_id = vm.vendor_id  " +
    //                                    "     where vd.active = 1  " +
    //                                    "     and vendor_name = 'ALL'  " +
    //                                    "     UNION  " +
    //                                    "     SELECT vm.vendor_name,vm.vendor_id, vd.vendor_address1, vd.vendor_address2,vd.vendor_city,vd.vendor_postal_code,vd.vendor_country,vd.vendor_PhoneNumber,vd.vendor_FaxNumber FROM ovms_vendor_details as vd join " +
    //                                    "     ovms_vendors as vm on vd.vendor_id = vm.vendor_id " +
    //                                    "     where vd.active = 1 " +
    //                                    " " + strSub + "" +    
    //                                    "     and vendor_name <> 'ALL' " ;
    //                    SqlCommand cmd = new SqlCommand(strSql, conn);
    //                    SqlDataReader reader = cmd.ExecuteReader();

    //                    int RowID = 1;
    //                    if (reader.HasRows == true)
    //                    {
    //                        while (reader.Read())
    //                        {
    //                            xml_string += "<VENDOR_ID ID ='" + RowID + "'>" +
    //                            "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
    //                       "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
    //                       "<VENDOR_ADDRESS1><![CDATA[" + reader["vendor_address1"] + "]]></VENDOR_ADDRESS1>" +
    //                       "<VENDOR_ADDRESS2><![CDATA[" + reader["vendor_address2"] + "]]></VENDOR_ADDRESS2>" +
    //                       "<VENDOR_CITY>" + reader["vendor_city"] + "</VENDOR_CITY>" +
    //                       "<VENDOR_POSTAL_CODE>" + reader["vendor_postal_code"] + "</VENDOR_POSTAL_CODE>" +
    //                       "<VENDOR_COUNTRY>" + reader["vendor_country"] + "</VENDOR_COUNTRY>" +
    //                       "<VENDOR_PHONENUMBER>" + reader["vendor_PhoneNumber"] + "</VENDOR_PHONENUMBER>" +
    //                       "<VENDOR_FAXNUMBER>" + reader["vendor_FaxNumber"] + "</VENDOR_FAXNUMBER>" +
    //                       "</VENDOR_ID>";
    //                            RowID++;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        xml_string = xml_string + "<DATA>no records found</DATA>";
    //                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
    //                    }
    //                    cmd.Dispose();
    //                    reader.Dispose();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //                xml_string = "<DATA> Unable to select vendor" + "</DATA>";
    //            }
    //            finally
    //            {
    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }
    //        }

    //        xml_string += "</RESPONSE>" +
    //                      "</XML>";
    //        XmlDocument xmldoc;
    //        xmldoc = new XmlDocument();
    //        xmldoc.LoadXml(xml_string);

    //        return xmldoc;
    //    }

    [WebMethod]
    public XmlDocument insert_vendor(string userEmailId, string userPassword, string vendorName, string vaddress1, string vaddress2, string vcity, string vpostal_code, string vcountry, string vPhoneNumber, string vFaxNumber)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";
        int newVid = 0;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                             + "<REQUEST>" +
                               "<VENDOR_NAME><![CDATA[" + vendorName + "]]></VENDOR_NAME>" +
                               "<VENDOR_ADDRESS1><![CDATA[" + vaddress1 + "]]></VENDOR_ADDRESS1>" +
                               "<VENDOR_ADDRESS2><![CDATA[" + vaddress2 + "]]></VENDOR_ADDRESS2>" +
                               "<VENDOR_CITY>" + vcity + "</VENDOR_CITY>" +
                               "<VENDOR_POSTAL_CODE>" + vpostal_code + "</VENDOR_POSTAL_CODE>" +
                               "<VENDOR_COUNTRY>" + vcountry + "</VENDOR_COUNTRY>" +
                               "<VENDOR_PHONENUMBER>" + vPhoneNumber + "</VENDOR_PHONENUMBER>" +
                               "<VENDOR_FAXNUMBER>" + vFaxNumber + "</VENDOR_FAXNUMBER>" +
                               "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";

        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();


                    strSql = "INSERT INTO ovms_vendors (vendor_name)VALUES('" + vendorName + "') SELECT CAST(scope_identity() AS int)";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    newVid = (int)cmd.ExecuteScalar();

                    if (newVid > 0)
                    {
                        xml_string += "<VENDOR_NAME><![CDATA[" + vendorName + "]]></VENDOR_NAME>";
                    }
                    else
                    {
                        xml_string += "<VENDOR_NAME>Vendor not inserted</VENDOR_NAME>";
                    }
                    strSql = "insert into ovms_vendor_details(vendor_id,vendor_address1, vendor_address2, vendor_city, vendor_postal_code, vendor_country, vendor_PhoneNumber, vendor_FaxNumber)" +
                        "values(" + newVid + ", '" + vaddress1 + "', '" + vaddress2 + "', '" + vcity + "', '" + vpostal_code + "', '" + vcountry + "', '" + vPhoneNumber + "', '" + vFaxNumber + "'); ";

                    cmd = new SqlCommand(strSql, conn);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STATUS>1</STATUS>";
                        xml_string += "<STRING>New vendor inserted successfully</STRING>";
                    }
                    else
                    {
                        xml_string += "<STATUS>0</STATUS>";
                        xml_string += "<STRING>vendor not inserted</STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to create new vendor");
                    }
                }
            }
            catch (Exception ex)
            {
                xml_string += "<STATUS>0</STATUS>";
                xml_string += "<STRING>vendor not inserted</STRING>";
                //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument update_vendor(string userEmailId, string userPassword, int vid, string vendorName, string vaddress1, string vaddress2, string vcity, string vpostal_code, string vcountry, string vPhoneNumber, string vFaxNumber)
    {
//       //
        SqlConnection conn;
        string xml_string = "";
        string strSql1 = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                     + "<REQUEST>"
                     + "<VENDER_ID>" + vid + "</VENDER_ID>" +
                      "<VENDOR_NAME><![CDATA[" + vendorName + "]]></VENDOR_NAME>" +
                        "<VENDOR_ADDRESS1><![CDATA[" + vaddress1 + "]]></VENDOR_ADDRESS1>" +
                        "<VENDOR_ADDRESS2><![CDATA[" + vaddress2 + "]]></VENDOR_ADDRESS2>" +
                        "<VENDOR_CITY>" + vcity + "</VENDOR_CITY>" +
                        "<VENDOR_POSTAL_CODE>" + vpostal_code + "</VENDOR_POSTAL_CODE>" +
                        "<VENDOR_COUNTRY>" + vcountry + "</VENDOR_COUNTRY>" +
                        "<VENDOR_PHONENUMBER>" + vPhoneNumber + "</VENDOR_PHONENUMBER>" +
                        "<VENDOR_FAXNUMBER>" + vFaxNumber + "</VENDOR_FAXNUMBER>"
                     + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";

        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                int FStatus = 0;
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    if (vendorName != "")
                    {
                        string strSql = "update ovms_vendors set vendor_name ='" + vendorName + "' where vendor_id =  '" + vid + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STATUS>saved successfully</STATUS>";
                        }
                    }
                    strSql1 = "update ovms_vendor_details set ";
                    if (vaddress1 != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_address1='" + vaddress1 + "'";
                        FStatus = 1;
                    }

                    if (vaddress2 != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_address2 ='" + vaddress2 + "'";
                        FStatus = 1;
                    }
                    if (vcity != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_city='" + vcity + "'";
                        FStatus = 1;
                    }
                    if (vpostal_code != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_postal_code ='" + vpostal_code + "'";
                        FStatus = 1;
                    }
                    if (vcountry != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_country ='" + vcountry + "'";
                        FStatus = 1;
                    }

                    if (vPhoneNumber != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_PhoneNumber = '" + vPhoneNumber + "'";
                    }


                    if (vFaxNumber != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " vendor_FaxNumber = '" + vFaxNumber + "'";
                    }

                    strSql1 += " where vendor_id = " + vid;
                    SqlCommand cmd1 = new SqlCommand(strSql1, conn);


                    if (cmd1.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<UPDATE_STRING>vendor updated successfully</UPDATE_STRING>" +
                                   "<UPDATE_VALUE>1</UPDATE_VALUE>";
                    }
                    else
                    {
                        xml_string += "<UPDATE_STRING>vendor not updated</UPDATE_STRING>" +
                                    "<UPDATE_STRING>0</UPDATE_STRING>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update vendor");
                    }
                    cmd1.Dispose();

                }

            }

            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument delete_vendor(string userEmailId, string userPassword, string vendorid)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                            + "<REQUEST>"
                            + "<VENDOR_ID>" + vendorid + "</VENDOR_ID>"
                            + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";

        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_vendors set  active=0 where vendor_id ='" + vendorid + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STATUS>deleted from vendor master successfully</STATUS>";
                    }
                    else
                    {
                        xml_string += "<DATA>no records found</DATA>";
                    }
                    cmd.Dispose();
                }
                string strSql1 = "update ovms_vendor_details set  active=0 where vendor_id = '" + vendorid + "'";

                SqlCommand cmd1 = new SqlCommand(strSql1, conn);

                if (cmd1.ExecuteNonQuery() > 0)
                {
                    xml_string += "<DELETE_STRING>vendor deleted successfully</DELETE_STRING>" +
                               "<DELETE_VALUE>1</DELETE_VALUE>";
                }
                else
                {
                    xml_string += "<DELETE_STRING>vendor not deleted</DELETE_STRING>" +
                                "<DELETE_VALUE>0</DELETE_VALUE>";
                    //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete vendor");
                }

                cmd1.Dispose();

            }

            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_department(string userEmailId, string userPassword, string department_id, string client_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<DEPARTMENT_ID>" + department_id + "</DEPARTMENT_ID>"
                    + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (department_id != "" & department_id != "0")
            {
                strSub = " and department_id=" + department_id;
            }

            if (client_id != "" & client_id != "0")
            {
                strSub = " and client_ID =" + client_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select department_name,department_id,client_id from ovms_departments where active=1" + strSub + " order by department_name asc";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {
                        while (reader.Read())

                        {
                            xml_string += "<DEPARTMENT_ID ID ='" + RowID + "'>" +
                                                  "<DEPARTMENT_NAME><![CDATA[" + reader["department_name"] + "]]></DEPARTMENT_NAME>" +
                                                  "<CLIENT_ID>" + reader["client_id"] + "</CLIENT_ID>" +
                                                   "<DEPARTMENT_ID>" + reader["department_id"] + "</DEPARTMENT_ID>" +
                                          "</DEPARTMENT_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<status> Unable to select department </ status > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //else
        //{
        //    xml_string += "<DEPARTMENT_ID>department_id should not be null</DEPARTMENT_ID>";
        //}

        xml_string += "</RESPONSE>" +
                      "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);
        return xmldoc;
    }

    //[WebMethod]
    //public XmlDocument set_job_status(string jobStatus, string userEmailId, string userPassword)
    //{
    //   //
    //    string errString = "";
    //    string xml_string = "<XML>" +
    //                    "<REQUEST>" +
    //                    "<JOB_STATUS>" + jobStatus + "</JOB_STATUS>" +
    //                    "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    errString = VerifyUser(userEmailId, userPassword);
    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {
    //        SqlConnection conn;
    //        string strSql = "";


    //        if (jobStatus != "")
    //        {
    //            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
    //            try
    //            {
    //                if (conn.State == System.Data.ConnectionState.Closed)
    //                {
    //                    conn.Open();
    //                    strSql = "Insert into ovms_job_status(job_status) values('" + jobStatus + "')";
    //                    SqlCommand cmd = new SqlCommand(strSql, conn);


    //                    if (cmd.ExecuteNonQuery() > 0)
    //                    {
    //                        xml_string += "<INSERT_STRING>Job Status inserted successfully</INSERT_STRING>" +
    //                                   "<INSERT_VALUE>1</INSERT_VALUE>";
    //                    }
    //                    else
    //                    {
    //                        xml_string += "<INSERT_STRING>Job Status not inserted</INSERT_STRING>" +
    //                                    "<INSERT_VALUE>0</INSERT_VALUE>";
    //                        //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert new job status");
    //                    }

    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //            }
    //            finally
    //            {
    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }
    //        }
    //    }

    //    xml_string += "</RESPONSE>";
    //    xml_string += "</XML>";

    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}

    [WebMethod]
    public XmlDocument insert_department(string userEmailId, string userPassword, string department_name, string client_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                         "<DEPARTMENT_NAME><![CDATA[" + department_name + "]]></DEPARTMENT_NAME>" +
                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                       "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (department_name != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "INSERT INTO ovms_departments (department_name,client_id)VALUES('" + department_name + "'," + client_id + ") SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<INSERT_STRING>department name inserted successfully</INSERT_STRING>" +
                                       "<INSERT_VALUE>1</INSERT_VALUE>";
                        }
                        else
                        {
                            xml_string += "<STRING>department name not inserted</STRING>" +
                                        "<VALUE>0</INSERT_VALUE>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert department name");
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument update_department(string userEmailId, string userPassword, int department_id, string department_name, int client_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";


        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                     + "<REQUEST>"
                     + "<DEPARTMENT_ID>" + department_id + "</DEPARTMENT_ID>"
                     + "<DEPARTMENT_NAME><![CDATA[" + department_name + "]]></DEPARTMENT_NAME>"
                     + "<CLIENT_ID>" + client_id + "</CLIENT_ID>"
                     + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    if (department_name != "")
                    {
                        string strSql = "update ovms_departments set department_name ='" + department_name + "' ,client_id='" + client_id + "' where department_id =  '" + department_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>department name updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>department name not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update department name");
                        }
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_department(string userEmailId, string userPassword, string department_id)
    {
        //logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                "<REQUEST>" +
                                    "<DEPARTMENT_ID>" + department_id + "</DEPARTMENT_ID>" +
                               "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";

        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_departments set  active=0 where department_id ='" + department_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>departments deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>departments not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete departments");
                    }
                    cmd.Dispose();

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_job_position_type(string userEmailId, string userPassword, string position_type_id)
    {
//        logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                  + "<REQUEST>"
                  + "<JOB_POSITION_TYPE_ID>" + position_type_id + "</JOB_POSITION_TYPE_ID>"
                  + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (position_type_id != "" & position_type_id != "0")
            {
                strSub = " and position_type_id=" + position_type_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select job_position_type,position_type_id from ovms_job_position_type where active=1" + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {


                        while (reader.Read())


                        {
                            xml_string += "<JOB_POSITION_TYPE_ID ID ='" + RowID + "'>" +
                           "<JOB_POSITION_TYPE><![CDATA[" + reader["job_position_type"] + "]]></JOB_POSITION_TYPE>" +
                             "<JOB_POSITION_TYPE_ID>" + reader["position_type_id"] + "</JOB_POSITION_TYPE_ID>" +
                                                    "</JOB_POSITION_TYPE_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>"; //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string =

                            "<STATUS> Error 120. Unable to select job_position_type </ STATUS>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //else
        //{
        //    xml_string += "<JOB_POSITION_TYPE_ID>job_position_type should not be null</JOB_POSITION_TYPE_ID>";
        //}

        xml_string += "</RESPONSE>" +
                              "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_job_position_type(string userEmailId, string userPassword, string job_position_type)
    {
        //logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                               "<POSITION_TYPE><![CDATA[" + job_position_type + "]]></POSITION_TYPE>" +
                                "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (job_position_type != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "INSERT INTO ovms_job_position_type (job_position_type)VALUES('" + job_position_type + "') SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>job position type inserted successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>job position type not inserted</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert job position type");
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_job_position_type(string userEmailId, string userPassword, int position_type_id, string job_position_type)
    {
//        logAPI.Service logService = new ///logAPI.Service();
        SqlConnection conn;
        string xml_string = "";


        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                "<REQUEST>" +
                                    "<JOB_POSITION_TYPE_ID><![CDATA[" + position_type_id + "]]></JOB_POSITION_TYPE_ID>" +
                               "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();


                    if (job_position_type != "")
                    {
                        string strSql = "update ovms_job_position_type set job_position_type ='" + job_position_type + "' where position_type_id =  '" + position_type_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>job position type updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>job position type not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update job position type");
                        }
                        cmd.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument delete_job_position_type(string userEmailId, string userPassword, int position_type_id)
    {
//       //
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                "<REQUEST>" +
                                    "<JOB_POSITION_TYPE_ID>" + position_type_id + "</JOB_POSITION_TYPE_ID>" +
                               "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    strSql = "update ovms_job_position_type set  active=0 where position_type_id ='" + position_type_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>job position type deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>job position type not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete job position type");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_timesheet_comment(string userEmailId, string userPassword, string timesheet_comment_id)
    {
       ////
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>"
            + "<REQUEST>"
                      + "<TIMESHEET_COMMENT_ID>" + timesheet_comment_id + "</TIMESHEET_COMMENT_ID>"
                 + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (timesheet_comment_id != "" & timesheet_comment_id != "0")
            {
                strSub = " and timesheet_comment_id=" + timesheet_comment_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    string strSql = "select timesheet_comments,timesheet_comment_id from ovms_timesheet_comments where  active=1" + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<TIMESHEET_COMMENT_ID ID ='" + RowID + "'>" +
                                          "<TIMESHEET_COMMENT><![CDATA[" + reader["timesheet_comments"] + "]]></TIMESHEET_COMMENT>" +
                                          "<TIMESHEET_COMMENT_ID>" + reader["timesheet_comment_id"] + "</TIMESHEET_COMMENT_ID>" +
                                          "</TIMESHEET_COMMENT_ID>";
                            RowID++;
                        }
                        reader.Close();
                        cmd.Dispose();
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<XML>" +

                            "<STATUS>Error 120. Unable to select comment_id </ STATUS > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_timesheet_comments(string userEmailId, string userPassword, string timesheet_comments)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<TIMESHEET_COMMENTS><![CDATA[" + timesheet_comments + "]]></TIMESHEET_COMMENTS>" +
                                   "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (timesheet_comments != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();


                        string sql = "INSERT INTO ovms_timesheet_comments (timesheet_comments)VALUES('" + timesheet_comments + "') SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>timesheet comments inserted successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>timesheet comments not inserted</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert timesheet comments");
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_timesheet_comments(string userEmailId, string userPassword, int timesheet_comment_id, string timesheet_comments)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                   "<REQUEST>" +
                                     "<TIMESHEET_COMMENT_ID>" + timesheet_comment_id + "</TIMESHEET_COMMENT_ID>" +
                                "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();


                    if (timesheet_comments != "")
                    {
                        string strSql = "update ovms_timesheet_comments set timesheet_comments ='" + timesheet_comments + "' where timesheet_comment_id =  '" + timesheet_comment_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>timesheet comments updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>timesheet comments not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update timesheet comments");
                        }
                        cmd.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_timesheet_comments(string userEmailId, string userPassword, string timesheet_comment_id)
    {
        //logAPI.Service logService = new /logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
           "<REQUEST>" +
            "<TIMESHEET_COMMENT_ID>" + timesheet_comment_id + "</TIMESHEET_COMMENT_ID>" +
       "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_timesheet_comments set  active=0 where timesheet_comment_id ='" + timesheet_comment_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>timesheet comments deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>timesheet comments not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete timesheet comments");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_timesheet_status(string userEmailId, string userPassword, string timesheet_status_id)
    {
       ////
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                   "<REQUEST>" +
                                "<TIMESHEET_STATUS_ID>" + timesheet_status_id + "</TIMESHEET_STATUS_ID>" +
                           "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (timesheet_status_id != "" & timesheet_status_id != "0")
            {
                strSub = " and timesheet_status_id=" + timesheet_status_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select timesheet_status,timesheet_status_id from ovms_timesheet_status where  active=1" + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    int RowID = 1;

                    if (reader.HasRows == true)
                    {

                        while (reader.Read())
                        {
                            xml_string += "<TIMESHEET_STATUS_ID ID ='" + RowID + "'>" +
                                                  "<TIMESHEET_STATUS><![CDATA[" + reader["timesheet_status"] + "]]></TIMESHEET_STATUS>" +
                                                   "<TIMESHEET_STATUS_ID>" + reader["timesheet_status_id"] + "</TIMESHEET_STATUS_ID>" +
                                                 "</TIMESHEET_STATUS_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<STATUS>Error 120. Unable to select timesheet status</STATUS> ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_timesheet_status(string userEmailId, string userPassword, string timesheet_status)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<TIMESHEET_COMMENTS><![CDATA[" + timesheet_status + "]]></TIMESHEET_COMMENTS>" +
                                   "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {

            if (timesheet_status != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "INSERT INTO ovms_timesheet_status (timesheet_status)VALUES('" + timesheet_status + "') SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<INSERT_STRING>timesheet status inserted successfully</INSERT_STRING>" +
                                       "<INSERT_VALUE>1</INSERT_VALUE>";
                        }
                        else
                        {
                            xml_string += "<INSERT_STRING>timesheet status not inserted</INSERT_STRING>" +
                                        "<INSERT_VALUE>0</INSERT_VALUE>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert timesheet status");
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_timesheet_status(string userEmailId, string userPassword, int timesheet_status_id, string timesheet_status)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                   "<REQUEST>" +
                                     "<TIMESHEET_STATUS_ID>" + timesheet_status_id + "</TIMESHEET_STATUS_ID>" +
                                "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    if (timesheet_status != "")
                    {
                        string strSql = "update ovms_timesheet_status set timesheet_status ='" + timesheet_status + "' where timesheet_status_id =  '" + timesheet_status_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>timesheet status updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>timesheet status not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update timesheet status");
                        }
                        cmd.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_timesheet_status(string userEmailId, string userPassword, string timesheet_status_id)
    {
       ////
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                       "<REQUEST>" +
                                           "<TIMESHEET_STATUS_ID>" + timesheet_status_id + "</TIMESHEET_STATUS_ID>" +
                                      "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_timesheet_status set  active=0 where timesheet_status_id ='" + timesheet_status_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>timesheet status deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>timesheet status not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete timesheet status");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_timesheet(string userEmailId, string userPassword, string timesheet_id, string employee_id, int vendor_id, string start_date, string end_date)
    {
       ////
        SqlConnection conn;
        //   string startDateconert = Convert.ToDateTime(start_date);
        string strsub = "";

        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                            "<TIMESHEET_ID>" + timesheet_id + "</TIMESHEET_ID>" +
                             "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                            "<START_DATE>" + start_date + "</START_DATE>" +
                            "<END_DATE>" + end_date + "</END_DATE>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (timesheet_id != "")
        {
            strsub = " and ts.timesheet_id = " + timesheet_id;
        }
        if (employee_id != "")
        {
            strsub += "and ed.employee_id = '" + employee_id + "'";
        }
        if (start_date != "")
        {
            strsub += " and ed.start_date ='" + start_date + "'";
        }
        if (end_date != "")
        {
            strsub += "and ed.end_date = '" + end_date + "'";
        }

        if (vendor_id != 0)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);


            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();

                string strSql = "select ts.employee_id, ed.first_name+' '+ed.last_name as Employee_Name,ed.address1, ts.day,ts.month,ts.year,ts.hours,ts.overtime,ts.create_date, j.job_id, j.contract_start_date,j.contract_end_date, tds.timesheet_comment_id,tds.timesheet_detail_id," +
                              "  tds.timesheet_status_id,concat('T', c.client_alias, '000', right('0000' + convert(varchar(4), tds.timesheet_detail_id), 4)) timesheet_id , tss.timesheet_status" +
                               "  from ovms_timesheet as ts" +
                               " join ovms_employees as e" +
                              "  on ts.employee_id = e.employee_id" +
                              "  join ovms_employee_details as ed" +
                              "  on ed.employee_id = e.employee_id" +
                              "  join ovms_jobs as j" +
                              "  on j.job_id = e.job_id" +
                              "  join ovms_timesheet_details as tds " +
                              "  on tds.timesheet_id = ts.timesheet_id " +
                              "  join ovms_timesheet_status as tst " +
                               " on tst.timesheet_status_id = tds.timesheet_status_id " +
                              "  join ovms_clients as c " +
                              "  on c.client_id = e.client_id " +
                              "  join ovms_timesheet_status as tss " +
                             "   on tss.timesheet_status_id = tds.timesheet_status_id " +
                              "  where j.job_id = 2 and e.vendor_id=1 and e.employee_id = 1 and ts.active = 1 and e.active = 1 and j.active = 1 and tds.active = 1 and tst.active = 1 " +
                              "  order by j.contract_start_date desc";


                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int RowID = 1;
                //  xml_string += ;
                //if (reader.Read())
                //{

                //  xml_string += "<RESPONSE>";
                if (reader.HasRows == true)
                {


                    while (reader.Read())


                    {
                        xml_string += "<TIMESHEET_ID ID ='" + RowID + "'>" +
                                  "<TIMESHEET_ID>" + reader["timesheet_id"] + "</TIMESHEET_ID>" +
                                  "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                  "<EMPLOYEE_NAME><![CDATA[" + reader["Employee_Name"] + "]]></EMPLOYEE_NAME>" +
                                  "<ADDRESS><![CDATA[" + reader["address1"] + "]]></ADDRESS>" +
                                  "<DAY>" + reader["day"] + "</DAY>" +
                                  "<MONTH>" + reader["month"] + "</MONTH>" +
                                  "<YEAR>" + reader["year"] + "</YEAR>" +
                                  "<HOURS>" + reader["hours"] + "</HOURS>" +
                                  "<OVERTIME>" + reader["overtime"] + "</OVERTIME>" +
                                  "<END_DATE>" + reader["contract_end_date"] + "</END_DATE>" +
                                  "<START_DATE>" + reader["contract_start_date"] + "</START_DATE>" +
                                  "<TIMESHEET_STATUS><![CDATA[" + reader["timesheet_status"] + "]]></TIMESHEET_STATUS>" +
                                  //"<TIMESHEET_COMMENTS>" + reader["timesheet_comments"] + "</TIMESHEET_COMMENTS>" +
                                  "</TIMESHEET_ID>";
                        RowID++;
                    }
                    //dispose
                    reader.Close();
                    cmd.Dispose();

                }
                else
                {
                    xml_string += "<DATA>No records found</DATA>";

                }
                //}
                //else
                //{
                //}


                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            else
            {
                xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
            }
        }
        xml_string += "</RESPONSE>" +
                       "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;


    }
    [WebMethod]
    public XmlDocument search_timesheet(string userEmailId, string userPassword, string first_name, string last_name, string timesheet_id, string end_date, string start_date, string timesheet_status, string vendor_id)
    {
        string xml_string = "";
        //query database using sql client - google
        //logAPI.Service logService = new logAPI.Service();
        int RowID = 1;
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();

                xml_string = "<XML>" +
                            "<REQUEST>" +
                               "<TIMESHEET_ID>" + timesheet_id + "</TIMESHEET_ID>" +
                                "<FIRST_NAME>" + first_name + "</FIRST_NAME>" +
                                  "<LAST_NAME>" + last_name + "</LAST_NAME>" +
                                    "<END_DATE>" + end_date + "</END_DATE>" +
                                  "<START_DATE>" + start_date + "</START_DATE>" +
                                  "<TIMESHEET_STATUS>" + timesheet_status + "</TIMESHEET_STATUS>" +
                                "</REQUEST>";

                string strSql = "select ts.employee_id,ed.first_name,ed.last_name,ed.address1,ts.day,ts.month,ts.year, " +
                   " ts.hours,ed.start_date,ed.end_date,ts.overtime,tsd.timesheet_detail_id,concat('T',clt.client_alias, '000', right('0000' + convert(varchar(4), tsd.timesheet_detail_id), 4)) timesheetname_id," +
                                //  ",tsd.timesheet_detail_id" +
                                "tss.timesheet_status,tsc.timesheet_comments " +
                                "from ovms_employee_details as ed " +
                                "join ovms_employees as em " +
                                "on ed.employee_id = em.employee_id " +
                                "join ovms_timesheet as ts " +
                                "on em.employee_id = ts.employee_id " +
                                "join ovms_timesheet_details as tsd " +
                                "on ts.timesheet_id = tsd.timesheet_id " +
                                " join ovms_timesheet_status as tss " +
                                " on tsd.timesheet_status_id = tss.timesheet_status_id " +
                                "join ovms_timesheet_comments as tsc " +
                                "on tsd.timesheet_comment_id = tsc.timesheet_comment_id " +
                                "join ovms_clients as clt on em.client_id=clt.client_id " +
                                "where tsd.active = 1 and ts.active = 1 and em.active = 1 and ed.active = 1";
                if (first_name != "")
                {
                    strSql = strSql + " and ed.first_name='" + first_name + "'";
                }
                if (last_name != "")
                {
                    strSql = strSql + " and ed.last_name = '" + last_name + "'";
                }
                if (timesheet_id != "")
                {
                    strSql = strSql + " and concat('T',clt.client_alias, '000', right('0000' + convert(varchar(4), tsd.timesheet_detail_id), 4)) = '" + timesheet_id + "'";
                }
                if (end_date != "")
                {
                    strSql = strSql + " and ed.end_date= '" + end_date + "'";
                }
                if (start_date != "")
                {
                    strSql = strSql + " and ed.start_date= '" + start_date + "'";
                }
                if (timesheet_status != "")
                {
                    strSql = strSql + " and  tss.timesheet_status= '" + timesheet_status + "'";
                }

                if (vendor_id != "" & vendor_id != "0")
                {
                    strSql = strSql + " and em.vendor_id= " + vendor_id;
                }

                try
                {
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    //using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    //{
                    xml_string = xml_string + "<RESPONSE>";
                    SqlDataReader reader = cmd.ExecuteReader();
                    //using (SqlDataReader reader = cmd.ExecuteReader())
                    //{
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {

                            xml_string += "<TIMESHEET_ID ID ='" + RowID + "'>" +
                               "<TIMESHEET_ID>" + reader["timesheetname_id"] + "</TIMESHEET_ID>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                  "<FIRST_NAME>" + reader["first_name"] + "</FIRST_NAME>" +
                                  "<LAST_NAME>" + reader["last_name"] + "</LAST_NAME>" +
                                  "<ADDRESS>" + reader["address1"] + "</ADDRESS>" +
                                  "<OVERTIME>" + reader["overtime"] + "</OVERTIME>" +
                                  "<END_DATE>" + reader["end_date"] + "</END_DATE>" +
                                  "<START_DATE>" + reader["start_date"] + "</START_DATE>" +
                                  "<TIMESHEET_STATUS>" + reader["timesheet_status"] + "</TIMESHEET_STATUS>" +
                                  "<TIMESHEET_COMMENTS>" + reader["timesheet_comments"] + "</TIMESHEET_COMMENTS>" +
                                         "</TIMESHEET_ID>";


                        }

                    }
                    else
                    {
                        xml_string = xml_string + "<ERROR>no records found</ERROR>";
                        //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No employee Found");

                    }
                    xml_string = xml_string + "</RESPONSE> ";

                }
                catch (Exception ex)
                {
                    xml_string = xml_string + "<RESPONSE>error:xxx.systemerror</RESPONSE>";
                    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

                }

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        //output final
        xml_string = xml_string + "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }

    [WebMethod]
    public XmlDocument get_job_detail_id(string userEmailId, string userPassword, string JOB_ID)
    {
       ////
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (JOB_ID != "" & JOB_ID != "0")
        {
            strSub = " and job_id =" + JOB_ID;
        }
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                         "<JOB_ID>" + JOB_ID + "</JOB_ID>" +
                            "</REQUEST>";
                string strSql = "select job_detail_id from ovms_job_details where active=1" + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<JOB_DETAIL_ID>" + reader["job_detail_id"] + "</JOB_DETAIL_ID>";
                    }
                }
                else
                {
                    xml_string = xml_string + "<DATA>no records found</DATA>";
                    //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                }
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {

            //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

            xml_string = "<XML>" +
                         "<STRING> Unable to select job comment</STRING> ";
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }

        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument insert_timesheet(string userEmailId, string userPassword, string employee_id, string day, string month, string year,
           string hours, string overtime, string timesheet_status_id, string timesheet_comment_id)
    {
       ////
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                                    "<REQUEST>" +
                                    "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                                  "<DAY>" + day + "</DAY>" +
                                                  "<MONTH>" + month + "</MONTH>" +
                                                  "<YEAR>" + year + "</YEAR>" +
                                                  "<HOURS>" + hours + "</HOURS>" +
                                                  "<OVERTIME>" + overtime + "</OVERTIME>" +
                                                  "<TIMESHEET_STATUS_ID>" + timesheet_status_id + "</TIMESHEET_STATUS_ID>" +
                                                  "<TIMESHEET_COMMENT_ID>" + timesheet_comment_id + "</TIMESHEET_COMMENT_ID>" +
                                    "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);


            int newtsid = 0;

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();



                    string strSql = "INSERT INTO ovms_timesheet(employee_id, day, month, year,hours,overtime)" +
                              " VALUES(" + employee_id + "," + day + "," + month + "," + year + "," + hours + "," + overtime + ")" +
                              " SELECT CAST(scope_identity() AS int)";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    newtsid = (int)cmd.ExecuteScalar();

                    if (newtsid > 0)
                    {
                        xml_string += "<TIMESHEET_ID>" + newtsid + "</TIMESHEET_ID>";
                    }
                    else
                    {
                        xml_string += "<TIMESHEET_ID>TIMESHEET not inserted</TIMESHEET_ID>";
                    }

                    strSql = "insert into ovms_timesheet_details(timesheet_id,timesheet_status_id, timesheet_comment_id)" +
                               "values(" + newtsid + ", '" + timesheet_status_id + "', '" + timesheet_comment_id + "'); ";

                    cmd = new SqlCommand(strSql, conn);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STATUS>1</STATUS>";
                        xml_string += "<STRING>New timesheet inserted successfully</STRING>";
                    }
                    else
                    {
                        xml_string += "<STATUS>0</STATUS>";
                        xml_string += "<STRING>timesheet not inserted</STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to create new timesheet");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<STATUS>0</STATUS>";
                xml_string += "<STRING>vendor not inserted</STRING>";
                //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_timesheet(string userEmailId, string userPassword, int employee_id, int day, int month, int year,
        float hours, int overtime, int timesheet_id, int timesheet_status_id, int timesheet_comment_id)
    {
//       //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                "<REQUEST>" +
                                    "<TIMESHEET_ID>" + timesheet_id + "</TIMESHEET_ID>" +
                                "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                int FStatus = 0;
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    if (employee_id != 0)
                    {
                        string strSql = "update ovms_timesheet set employee_id ='" + employee_id + "',day='" + day + "',month='" + month + "',year='" + year + "',hours='" + hours + "',overtime='" + overtime + "' where timesheet_id =  '" + timesheet_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STATUS>saved successfully</STATUS>";
                        }
                    }
                    string strSql1 = "update ovms_timesheet_details set ";
                    if (timesheet_status_id != 0)
                    {
                        strSql1 += " timesheet_status_id=" + timesheet_status_id;
                        FStatus = 1;
                    }

                    if (timesheet_comment_id != 0)
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " timesheet_comment_id =" + timesheet_comment_id;
                        FStatus = 1;
                    }
                    strSql1 += " where timesheet_id = " + timesheet_id;
                    SqlCommand cmd1 = new SqlCommand(strSql1, conn);
                    if (cmd1.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>timesheet detail updated successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>timesheet detail not updated</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update timesheet detail");
                    }
                    cmd1.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_timesheet(string userEmailId, string userPassword, int timesheet_id)
    {
        //logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                               "<REQUEST>" +
                                    "<TIMESHEET_ID>" + timesheet_id + "</TIMESHEET_ID>" +
                                "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_timesheet set  active=0 where timesheet_id =" + timesheet_id + ";" +
                       " update ovms_timesheet_details set  active=0 where timesheet_id = " + timesheet_id + ";";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>timesheet detail deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>timesheet detail not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete timesheet detail");
                    }
                    cmd.Dispose();
                }
                //string strSql1 = "update ovms_timesheet_details set  active=0 where timesheet_id = " + timesheet_id + ";";

                //SqlCommand cmd1 = new SqlCommand(strSql1, conn);




            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_business_type(string userEmailId, string userPassword, string business_type_id)
    {
      // //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>" +
                          "<REQUEST>" +
                                       "<BUSINESS_TYPE_ID>" + business_type_id + "</BUSINESS_TYPE_ID>" +
                                  "</REQUEST>";
        xml_string += "<RESPONSE>";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (business_type_id != "" & business_type_id != "0")
            {
                strSub = " and business_type_id=" + business_type_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select business_type_name,business_type_id from ovms_business_type where active=1" + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<BUSINESS_TYPE_ID ID ='" + RowID + "'>" +
                                                  "<BUSINESS_TYPE_NAME><![CDATA[" + reader["business_type_name"] + "]]></BUSINESS_TYPE_NAME>" +
                                                      "<BUSINESS_TYPE_ID>" + reader["business_type_id"] + "</BUSINESS_TYPE_ID>" +
                                               "</BUSINESS_TYPE_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }

            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<XML>" +

                            "<STATUS>Error 120. Unable to select businesstype</STATUS>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument insert_business_type(string userEmailId, string userPassword, string business_type_name)
    {
      // //
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<BUSINESS_TYPE_NAME><![CDATA[" + business_type_name + "]]></BUSINESS_TYPE_NAME>" +
                                   "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {


            if (business_type_name != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "INSERT INTO ovms_business_type (business_type_name)VALUES('" + business_type_name + "') SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>business_type_name  inserted successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>business_type_name not inserted</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert job comment");

                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_business_type(string userEmailId, string userPassword, int business_type_id, string business_type_name)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                "<REQUEST>" +
                                     "<BUSINESS_TYPE_ID>" + business_type_id + "</BUSINESS_TYPE_ID>" +
                                "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    if (business_type_name != "")
                    {
                        string strSql = "update ovms_business_type set business_type_name ='" + business_type_name + "' where business_type_id =  '" + business_type_id + "' ";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>business type updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>business type not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update business type");
                        }
                        cmd.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_business_type(string userEmailId, string userPassword, int business_type_id)
    {
       // logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                 "<REQUEST>" +
                                      "<BUSINESS_TYPE_ID>" + business_type_id + "</BUSINESS_TYPE_ID>" +
                                 "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_business_type set  active=0 where business_type_id ='" + business_type_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);


                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>business type deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>business type not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete business type");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_employee_comments(string userEmailId, string userPassword, string comment_id)
    {
        //logAPI.Service logService = new l7///7gAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>" +
                          "<REQUEST>" +
                                "<COMMENT_ID>" + comment_id + "</COMMENT_ID>" +
                           "</REQUEST>";
        xml_string += "<RESPONSE>";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {

            if (comment_id != "" & comment_id != "0")
            {
                strSub = " and comment_id=" + comment_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select comments,comment_id from ovms_employee_comments where  active=1" + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;

                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<COMMENT_ID ID ='" + RowID + "'>" +
                                                  "<COMMENT><![CDATA[" + reader["comments"] + "]]></COMMENT>" +
                                                   "<COMMENT_ID>" + reader["comment_id"] + "</COMMENT_ID>" +
                                               "</COMMENT_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<XML>" +
                                                "<STATUS>Error 120. Unable to select employee comment </ STATUS > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                             "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_employee_comments(string userEmailId, string userPassword, string comments, int employee_id, DateTime commentdate, int user_id)
    {
        ///logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                      "<REQUEST>" + "<COMMENTS>" + comments + "</COMMENTS>" +
                                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                         "<COMMENTDATE>" + commentdate + "</COMMENTDATE>" +
                                             "<USER_ID>" + user_id + "</USER_ID>" +

                                             "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (comments != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string sql = "INSERT INTO ovms_employee_comments (comments,employee_id,comment_date,user_id)VALUES('" + comments + "','" + employee_id + "','" + commentdate + "','" + user_id + "') SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>employee comment inserted successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>employee comment not inserted</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert employee comment");
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_employee_comments(string userEmailId, string userPassword, int comment_id, string comments, int employee_id, DateTime commentdate, int user_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                   "<REQUEST>" +
                                     "<COMMENT_ID>" + comment_id + "</COMMENT_ID>" +
                                "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    if (comments != "")
                    {
                        string strSql = "update ovms_employee_comments set comments ='" + comments + "',employee_id='" + employee_id + "',comment_date='" + commentdate + "',user_id='" + user_id + "' where comment_id =  '" + comment_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>employee comments updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>employee comment not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update employee comment");
                        }
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_employee_comments(string userEmailId, string userPassword, string comment_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                   "<REQUEST>" +
                                    "<COMMENT_ID>" + comment_id + "</COMMENT_ID>" +
                               "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update ovms_employee_comments set  active=0 where comment_id ='" + comment_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>employee comments deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>employee comments not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete employee comments");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_job_comments(string userEmailId, string userPassword, string comment_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (comment_id != "" & comment_id != "0")
        {
            strSub = " and comment_id=" + comment_id;
        }
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                   "<REQUEST>" +
                                "<COMMENT_ID>" + comment_id + "</COMMENT_ID>" +
                           "</REQUEST>";
                string strSql = "select comments,comment_id from ovms_job_comments where active=1" + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                xml_string += "<RESPONSE>";

                int RowID = 1;

                if (reader.HasRows == true)
                {

                    while (reader.Read())


                    {
                        xml_string += "<COMMENT_ID ID ='" + RowID + "'>" +
                                              "<COMMENT><![CDATA[" + reader["comments"] + "]]></COMMENT>" +
                                                "<COMMENT_ID>" + reader["comment_id"] + "</COMMENT_ID>" +
                                              "</COMMENT_ID>";
                        RowID++;
                    }
                }
                else
                {
                    xml_string = xml_string + "<DATA>no records found</DATA>";
                    //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                }
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {

            //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

            xml_string = "<XML>" +
                        "<REQUEST>" + comment_id + "</REQUEST>" +
                        "<RESPONSE> Unable to select job comment</ RESPONSE > ";
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        //}
        //else
        //{
        //    xml_string += "<COMMENT_ID>comment_id should not be null</COMMENT_ID>";
        //}
        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_rating_with_employeeid(string userEmailId, string userPassword, string emp_rating_1, string emp_rating_2,
       string emp_rating_3, string emp_rating_4, string emp_rating_5, string job_Id, string employee_id)
    {
        //logAPI.Service logService = new logAPI.Service();

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                        "<JOB_ID>" + job_Id + "</JOB_ID>" +
                        "<EMPLOYEE_RATING1>" + emp_rating_1 + "</EMPLOYEE_RATING1>" +
                         "<EMPLOYEE_RATING2>" + emp_rating_2 + "</EMPLOYEE_RATING2>" +
                          "<EMPLOYEE_RATING3>" + emp_rating_3 + "</EMPLOYEE_RATING3>" +
                           "<EMPLOYEE_RATING4>" + emp_rating_4 + "</EMPLOYEE_RATING4>" +
                            "<EMPLOYEE_RATING5>" + emp_rating_5 + "</EMPLOYEE_RATING5>" +
                        "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "update ovms_job_question_rating set employee_id='" + employee_id + "',emp_rating_1='" + emp_rating_1 + "',emp_rating_2='" + emp_rating_2 + "',emp_rating_3='" + emp_rating_3 + "',emp_rating_4='" + emp_rating_4 + "',emp_rating_5='" + emp_rating_5 + "'where job_ID='" + job_Id + "'";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>employee rating updated successfully</STRING>" +
                                          "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>employee rating not updated</STRING>" +
                                          "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to updated employee rating");
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Insert_Job_Questions(string userEmailId, string userPassword, string employee_id, string vendorID, string clientID,
        string question_1, string rating1, string question_2, string rating2, string question_3, string rating3, string question_4,
        string rating4, string question_5, string rating5, string userID, string jobID)
    {
      //  aakashService.Service logService = new aakashService.Service();
       // logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        string xml_string = "";
        int newclient_id = 0;
        string errString = "";
        //query database using sql client - google

        xml_string = "<XML>" +
                "<REQUEST>" +
                            "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                            "<VENDOR_ID>" + vendorID + "</VENDOR_ID>" +
                            "<CLIENT_ID>" + clientID + "</CLIENT_ID>" +
                            "<QUESTIONS1><![CDATA[" + question_1 + "]]></QUESTIONS1>" +
                            "<RATING1>" + rating1 + "</RATING1>" +
                            "<QUESTIONS2><![CDATA[" + question_2 + "]]></QUESTIONS2>" +
                            "<RATING2>" + rating2 + "</RATING2>" +
                            "<QUESTIONS3><![CDATA[" + question_3 + "]]></QUESTIONS3>" +
                            "<RATING3>" + rating3 + "</RATING3>" +
                            "<QUESTIONS4><![CDATA[" + question_4 + "]]></QUESTIONS4>" +
                            "<RATING4>" + rating4 + "</RATING4>" +
                            "<QUESTIONS5><![CDATA[" + question_5 + "]]></QUESTIONS5>" +
                            "<RATING5>" + rating5 + "</RATING5>" +
                            "<USER_ID>" + userID + "</USER_ID>" +
                            "<JOB_ID>" + jobID + "</JOB_ID>" +
                "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "insert into dbo.ovms_job_question_rating(employee_id,question_1,rating_1,question_2,rating_2,question_3,rating_3,question_4,rating_4,question_5,rating_5,Vendor_ID,Client_ID,User_ID,job_ID) values('" + employee_id + "', '" + question_1 + "', '" + rating1 + "', '" + question_2 + "','" + rating2 + "', '" + question_3 + "','" + rating3 + "' ,'" + question_4 + "','" + rating4 + "', '" + question_5 + "','" + rating5 + "','" + vendorID + "','" + clientID + "','" + userID + "','" + jobID + "') ";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<DATA>inserted successfully</DATA>";
                        }
                        else
                        {
                            xml_string += "<QUESTIONS>Questions not inserted</QUESTIONS>";
                        }
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Questions not inserted</INSERT_STRING>";
                    //ErrorlogService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_job_comments(string userEmailId, string userPassword, string comments, int job_id, DateTime commentdate, int user_id)
    {
        //logAPI.Service logService = new logAPI.Service();

        string xml_string = "<XML>" +
                        "<REQUEST>" + "<COMMENTS><![CDATA[" + comments + "]]></COMMENTS>" +
                                           "<JOB_ID>" + job_id + "</JOB_ID>" +
                                           "<COMMENTDATE>" + commentdate + "</COMMENTDATE>" +
                                               "<USER_ID>" + user_id + "</USER_ID>" +
                                   "</REQUEST>";
        xml_string += "<RESPONSE>";
        SqlConnection conn;

        if (comments != "")
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    string sql = "INSERT INTO ovms_job_comments (comments,job_id,comment_date,user_id)VALUES('" + comments.Replace("'", "''") + "','" + job_id + "','" + commentdate + "','" + user_id + "') SELECT CAST(scope_identity() AS int)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Job comment inserted successfully</INSERT_STRING>" +
                                   "<INSERT_VALUE>1</INSERT_VALUE>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING>Job comment not inserted</INSERT_STRING>" +
                                    "<INSERT_VALUE>0</INSERT_VALUE>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert job comment");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_job_comments(string userEmailId, string userPassword, int comment_id, string comments, int job_id, DateTime commentdate, int user_id)
    {
       //
        string xml_string = "";
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    xml_string = "<XML>" +
                                  "<REQUEST>" +
                                    "<COMMENT_ID>" + comment_id + "</COMMENT_ID>" +
                               "</REQUEST>";

                    if (comments != "")
                    {
                        string strSql = "update ovms_job_comments set comments ='" + comments.Replace("'", "''") + "',job_id='" + job_id + "',comment_date='" + commentdate + "',user_id='" + user_id + "' where comment_id =  '" + comment_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        xml_string += "<RESPONSE>";
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<UPDATE_STRING>Job comments updated successfully</UPDATE_STRING>" +
                                       "<UPDATE_VALUE>1</UPDATE_VALUE>";
                        }
                        else
                        {
                            xml_string += "<UPDATE_STRING>Job comment not updated</UPDATE_STRING>" +
                                        "<UPDATE_STRING>0</UPDATE_STRING>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update job comment");
                        }
                        xml_string += "</RESPONSE>";
                        cmd.Dispose();

                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_job_comments(string userEmailId, string userPassword, int comment_id)
    {
       //
        string xml_string = "";
        //query database using sql client - google
        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    xml_string = "<XML>" +
                                   "<REQUEST>" +
                                    "<COMMENT_ID>" + comment_id + "</COMMENT_ID>" +
                               "</REQUEST>";
                    string strSql = "update ovms_job_comments set  active=0 where comment_id ='" + comment_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    xml_string += "<RESPONSE>";
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<DELETE_STRING>Job comment deleted successfully</DELETE_STRING>" +
                                   "<DELETE_VALUE>1</DELETE_VALUE>";
                    }
                    else
                    {
                        xml_string += "<DELETE_STRING>Job comment not deleted</DELETE_STRING>" +
                                    "<DELETE_VALUE>0</DELETE_VALUE>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete job comment");
                    }
                    xml_string += "</RESPONSE>";
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]

    public XmlDocument get_dates(string userEmailId, string userPassword, string JOB_ID)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (JOB_ID != "" & JOB_ID != "0")
        {
            strSub = " and job_id =" + JOB_ID;
        }
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                         "<JOB_ID>" + JOB_ID + "</JOB_ID>" +
                            "</REQUEST>";
                string strSql = "select contract_start_date,contract_end_date from ovms_jobs where active=1" + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                           "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>";
                    }
                }
                else
                {
                    xml_string = xml_string + "<DATA>no records found</DATA>";
                    //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                }
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {

            //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

            xml_string = "<XML>" +
                         "<STRING> Unable to select job comment</STRING> ";
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        //}
        //else
        //{
        //    xml_string += "<COMMENT_ID>comment_id should not be null</COMMENT_ID>";
        //}
        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_employees(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
    {
        ///
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        //int RowID = 1;
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                             "<REQUEST>" +
                                    "<EMPLOYEEID>" + employee_id + "</EMPLOYEEID>" +
                                    "<VENDORID>" + vendor_id + "</VENDORID>" +
                                    "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                                    "<FROMDATE>" + fromdate + "</FROMDATE>" +
                                    "<ENDDATE>" + enddate + "</ENDDATE>" +
                                    "<USER_ID>" + user_id + "</USER_ID>" +
                                    "</REQUEST>";
        xml_string += "<RESPONSE>";
        string strSql = "";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "select concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_id, " +
                    " em.user_id,ed.first_name,em.submit_candidate_check, em.employee_id, ed.middle_name,ed.last_name, ed.comments, ed.email, ed.date_of_birth, ed.phone," +
                    "ed.suite_no, ed.address1, ed.address2,j.job_title,ven.vendor_name,ed.ext_requested,  ed.licence_no, ed.city,ed.province," +
                    " concat(ed.city, ', ', ed.province)location,ed.profile_picture_path, ed.postal,ed.province,ed.country,ed.active,ed.skype_id, " +
                    "  ed.availability_for_interview, ed.start_date,ed.Last_4_Digits_of_SSN_SIN,ed.pay_rate, ed.end_date,ed.create_date, " +
                    " ed.active,ven.vendor_name,em.job_id, clt.client_name,eact.interview_requested,eact.candidate_rejected, eact.more_info,eact.vendor_moreInfo_reply, " +
                   "  eact.interview_date, eact.interview_time, eact.reason_of_rejection,eact.reason_of_rejection,eact.vendor_reject_candidate_comment, eact.candidate_approve, eact.interview_resheduled, eact.interview_confirm,eact.vendor_interview_comment ," +
                   "eact.vendor_interview_comment2,eact.vendor_reject_candidate,eact.vendor_reject_candidate_comment,eact.create_date as action_create_date,eact.vendor_interview_comment3,eact.vendor_interview_comment4,eact.vendor_interview_comment5, " +
                    "eact.interview_request_comment,eact.interview_request_comment2,eact.interview_request_comment3,eact.interview_request_comment4,eact.interview_request_comment5, " +
                    " eact.interview_cancel_by_client,eact.interview_cancel_by_client_comment," +
                     " eact.action_id,j.contract_start_date,j.contract_end_date from ovms_employees as em " +
                    " join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
                    " join ovms_vendors as ven on em.vendor_id = ven.vendor_id " +
                    " join ovms_clients as clt on em.client_id = clt.client_id " +
                    "  join ovms_jobs as j on j.job_id = em.job_id " +
                   " left join ovms_employee_actions as eact on em.employee_id = eact.employee_id " +
                    " where 1 = 1 and em.submit_candidate_check=1";


                    if (employee_id != "" & employee_id != "0")
                    {
                        strSql = strSql + " and  em.employee_id='" + employee_id + "'";
                    }
                    if (vendor_id != "" & vendor_id != "0")
                    {
                        strSql = strSql + " and em.vendor_id = " + vendor_id + "";
                    }
                    if (client_id != "" & client_id != "0")
                    {
                        strSql = strSql + " and em.client_id = " + client_id + "";
                    }
                    if (fromdate != "")
                    {
                        strSql = strSql + " and (ed.start_date >= '" + fromdate + "' )";
                    }
                    if (enddate != "")
                    {
                        strSql = strSql + " and (ed.end_date <= '" + enddate + "')";
                    }
                    if (active != "")
                    {
                        strSql = strSql + " and em.active = " + active;
                    }
                    if (user_id != "" & user_id != "0")
                    {
                        strSql = strSql + " and em.user_id = " + user_id + "";
                    }
                    strSql = strSql + " order by em.employee_id desc,create_date desc";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<EMPLOYEE_NAME_ID ID='" + RowID + "'>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                        "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
                        "<MIDDLE_NAME><![CDATA[" + reader["middle_name"] + "]]></MIDDLE_NAME>" +
                        "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
                        "<EMAIL>" + reader["email"] + "</EMAIL>" +
                        "<PHONE>" + reader["phone"] + "</PHONE>" +
                        "<DATE_OF_BIRTH>" + reader["date_of_birth"] + "</DATE_OF_BIRTH>" +
                        "<SUITE_NO>" + reader["suite_no"] + "</SUITE_NO>" +
                        "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
                        "<ADDRESS2><![CDATA[" + reader["address2"] + "]]></ADDRESS2>" +
                        "<LOCATION>" + reader["location"] + "</LOCATION>" +
                        "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
                        "<JOB_TITLE><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["JOB_TITLE"].ToString()) + "]]></JOB_TITLE>" +
                        "<CITY>" + reader["city"] + "</CITY>" +
                        "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
                        "<INTERVIEW_RESHEDULED>" + reader["interview_resheduled"] + "</INTERVIEW_RESHEDULED>" +
                        "<POSTAL>" + reader["postal"] + "</POSTAL>" +
                        "<COUNTRY>" + reader["country"] + "</COUNTRY>" +
                        "<LICENCE_NO>" + reader["licence_no"] + "</LICENCE_NO>" +
                        "<SKYPE>" + reader["skype_id"] + "</SKYPE>" +
                        "<AVAILABILITY_FOR_INTERVIEW>" + reader["availability_for_interview"] + "</AVAILABILITY_FOR_INTERVIEW>" +
                        "<ACTIVE>" + ((reader["active"].ToString() == "1") ? "Working" : "Not Working") + "</ACTIVE>" +
                        "<STARTDATE>" + reader["start_date"] + "</STARTDATE>" +
                        "<ENDDATE>" + reader["end_date"] + "</ENDDATE>" +
                        "<COMMENTS><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["comments"].ToString()) + "]]></COMMENTS>" +
                        "<EXT_REQUESTED>" + reader["ext_requested"] + "</EXT_REQUESTED>" +
                        "<PROFILE_PICTURE_PATH>" + reader["profile_picture_path"] + "</PROFILE_PICTURE_PATH>" +
                        "<LAST_4_DIGITS_OF_SSN_SIN>" + reader["Last_4_Digits_of_SSN_SIN"] + "</LAST_4_DIGITS_OF_SSN_SIN>" +
                        "<PAY_RATE>" + reader["pay_rate"] + "</PAY_RATE>" +
                        "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                        "<USER_ID>" + reader["user_id"] + "</USER_ID>" +
                         "<SUBMIT_CANDIDATE_CHECK>" + reader["submit_candidate_check"] + "</SUBMIT_CANDIDATE_CHECK>" +
                        "<INTERVIEW_REQUESTED>" + reader["interview_requested"] + "</INTERVIEW_REQUESTED>" +
                        "<CANDIDATE_REJECTED>" + reader["candidate_rejected"] + "</CANDIDATE_REJECTED>" +
                        "<MORE_INFO>" + reader["more_info"] + "</MORE_INFO>" +
                        "<INTERVIEW_DATE>" + reader["interview_date"] + "</INTERVIEW_DATE>" +
                        "<INTERVIEW_TIME>" + reader["interview_time"] + "</INTERVIEW_TIME>" +
                        "<REASON_OF_REJECTION><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["reason_of_rejection"].ToString()) + "]]></REASON_OF_REJECTION>" +
                        "<CANDIDATE_APPROVE>" + reader["candidate_approve"] + "</CANDIDATE_APPROVE>" +
                        "<INTERVIEW_CONFIRM>" + reader["interview_confirm"] + "</INTERVIEW_CONFIRM>" +
                        "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                        "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +
                        "<REASON_OF_REJECTION>" + reader["reason_of_rejection"] + "</REASON_OF_REJECTION>" +
                        "<VENDOR_REJECT_CANDIDATE_COMMENT>" + reader["vendor_reject_candidate_comment"] + "</VENDOR_REJECT_CANDIDATE_COMMENT>" +

                        "<ACTIVE>" + reader["active"] + "</ACTIVE>" +
                        "<ACTION_ID>" + reader["action_id"] + "</ACTION_ID>" +
                        "<VENDOR_INTERVIEW_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT>" +
                        "<VENDOR_INTERVIEW_COMMENT2><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment2"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT2>" +
                        "<VENDOR_INTERVIEW_COMMENT3><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment3"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT3>" +
                        "<VENDOR_INTERVIEW_COMMENT4><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment4"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT4>" +
                        "<VENDOR_INTERVIEW_COMMENT5><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment5"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT5>" +
                        "<INTERVIEW_REQUEST_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT>" +
                        "<INTERVIEW_REQUEST_COMMENT2><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment2"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT2>" +
                        "<INTERVIEW_REQUEST_COMMENT3><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment3"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT3>" +
                        "<INTERVIEW_REQUEST_COMMENT4><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment4"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT4>" +
                        "<INTERVIEW_REQUEST_COMMENT5><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment5"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT5>" +
                        "<VENDOR_MOREINFO_REPLY><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_moreInfo_reply"].ToString()) + "]]></VENDOR_MOREINFO_REPLY>" +
                        "<VENDOR_REJECT_CANDIDATE><![CDATA[" + reader["vendor_reject_candidate"] + "]]></VENDOR_REJECT_CANDIDATE>" +
                        "<VENDOR_REJECT_CANDIDATE_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_reject_candidate_comment"].ToString()) + "]]></VENDOR_REJECT_CANDIDATE_COMMENT>" +
                        "<ACTION_CREATE_DATE><![CDATA[" + reader["action_create_date"] + "]]></ACTION_CREATE_DATE>" +
                        "<INTERVIEW_CANCEL_BY_CLIENT>" + reader["interview_cancel_by_client"] + "</INTERVIEW_CANCEL_BY_CLIENT>" +
                        "<INTERVIEW_CANCEL_BY_CLIENT_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_cancel_by_client_comment"].ToString()) + "]]></INTERVIEW_CANCEL_BY_CLIENT_COMMENT>" +
                        "</EMPLOYEE_NAME_ID>";
                        RowID = RowID + 1;
                    }
                    reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
                //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

            }

            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string = xml_string + "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    //[WebMethod]
    //public XmlDocument get_employees(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
    //{
    //    
    //    SqlConnection conn;
    //    string xml_string = "";

    //    string errString = "";
    //    int RowID = 1;
    //    errString = VerifyUser(userEmailId, userPassword);
    //    xml_string = "<XML>" +
    //                         "<REQUEST>" +
    //                                "<EMPLOYEEID>" + employee_id + "</EMPLOYEEID>" +
    //                                "<VENDORID>" + vendor_id + "</VENDORID>" +
    //                                "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
    //                                "<FROMDATE>" + fromdate + "</FROMDATE>" +
    //                                "<ENDDATE>" + enddate + "</ENDDATE>" +
    //                                "<USER_ID>" + user_id + "</USER_ID>" +
    //                                "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    string strSql = "";
    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {
    //        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

    //        try
    //        {
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();

    //                strSql = "select concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_id, " +
    //                " em.user_id,ed.first_name, em.employee_id, ed.middle_name,ed.last_name, ed.comments, ed.email, ed.date_of_birth, ed.phone," +
    //                "ed.suite_no, ed.address1, ed.address2,j.job_title,ven.vendor_name,ed.ext_requested,  ed.licence_no, ed.city,ed.province," +
    //                " concat(ed.city, ', ', ed.province)location,ed.profile_picture_path, ed.postal,ed.province,ed.country,ed.active,ed.skype_id, " +
    //                "  ed.availability_for_interview, ed.start_date,ed.Last_4_Digits_of_SSN_SIN,ed.pay_rate, ed.end_date,ed.create_date, " +
    //                " ed.active,ven.vendor_name,em.job_id, clt.client_name,eact.interview_requested,eact.candidate_rejected, eact.more_info,eact.vendor_moreInfo_reply, " +
    //               "  eact.interview_date, eact.interview_time, eact.reason_of_rejection, eact.candidate_approve, eact.interview_resheduled, eact.interview_confirm,eact.vendor_interview_comment ," +
    //               "eact.vendor_interview_comment2,eact.vendor_reject_candidate,eact.vendor_reject_candidate_comment,eact.create_date as action_create_date,eact.vendor_interview_comment3,eact.vendor_interview_comment4,eact.vendor_interview_comment5, " +
    //                "eact.interview_request_comment,eact.interview_request_comment2,eact.interview_request_comment3,eact.interview_request_comment4,eact.interview_request_comment5, " +
    //                " eact.interview_cancel_by_client,eact.interview_cancel_by_client_comment," +
    //                 " eact.action_id,j.contract_start_date,j.contract_end_date from ovms_employees as em " +
    //                " join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
    //                " join ovms_vendors as ven on em.vendor_id = ven.vendor_id " +
    //                " join ovms_clients as clt on em.client_id = clt.client_id " +
    //                "  join ovms_jobs as j on j.job_id = em.job_id " +
    //               " left join ovms_employee_actions as eact on em.employee_id = eact.employee_id " +
    //                " where 1 = 1";


    //                if (employee_id != "" & employee_id != "0")
    //                {
    //                    strSql = strSql + " and  em.employee_id='" + employee_id + "'";
    //                }
    //                if (vendor_id != "" & vendor_id != "0")
    //                {
    //                    strSql = strSql + " and em.vendor_id = " + vendor_id + "";
    //                }
    //                if (client_id != "" & client_id != "0")
    //                {
    //                    strSql = strSql + " and em.client_id = " + client_id + "";
    //                }
    //                if (fromdate != "")
    //                {
    //                    strSql = strSql + " and (ed.start_date >= '" + fromdate + "' )";
    //                }
    //                if (enddate != "")
    //                {
    //                    strSql = strSql + " and (ed.end_date <= '" + enddate + "')";
    //                }
    //                if (active != "")
    //                {
    //                    strSql = strSql + " and em.active = " + active;
    //                }
    //                if (user_id != "" & user_id != "0")
    //                {
    //                    strSql = strSql + " and em.user_id = " + user_id + "";
    //                }
    //                strSql = strSql + " order by em.employee_id desc,create_date desc";

    //                SqlCommand cmd = new SqlCommand(strSql, conn);
    //                SqlDataReader reader = cmd.ExecuteReader();
    //                int RowID = 1;
    //                while (reader.Read())
    //                {
    //                    xml_string = xml_string + "<EMPLOYEE_NAME_ID ID='" + RowID + "'>" +
    //                       "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
    //                       "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
    //                       "<MIDDLE_NAME><![CDATA[" + reader["middle_name"] + "]]></MIDDLE_NAME>" +
    //                       "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
    //                       "<EMAIL>" + reader["email"] + "</EMAIL>" +
    //                       "<PHONE>" + reader["phone"] + "</PHONE>" +
    //                       "<DATE_OF_BIRTH>" + reader["date_of_birth"] + "</DATE_OF_BIRTH>" +
    //                       "<SUITE_NO>" + reader["suite_no"] + "</SUITE_NO>" +
    //                       "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
    //                       "<ADDRESS2><![CDATA[" + reader["address2"] + "]]></ADDRESS2>" +
    //                       "<LOCATION>" + reader["location"] + "</LOCATION>" +
    //                       "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
    //                    "<JOB_TITLE><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["JOB_TITLE"].ToString()) + "]]></JOB_TITLE>" +
    //                     "<CITY>" + reader["city"] + "</CITY>" +
    //                       "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
    //                       "<INTERVIEW_RESHEDULED>" + reader["interview_resheduled"] + "</INTERVIEW_RESHEDULED>" +
    //                       "<POSTAL>" + reader["postal"] + "</POSTAL>" +
    //                       "<COUNTRY>" + reader["country"] + "</COUNTRY>" +
    //                       "<LICENCE_NO>" + reader["licence_no"] + "</LICENCE_NO>" +
    //                       "<SKYPE>" + reader["skype_id"] + "</SKYPE>" +
    //                       "<AVAILABILITY_FOR_INTERVIEW>" + reader["availability_for_interview"] + "</AVAILABILITY_FOR_INTERVIEW>" +
    //                       "<ACTIVE>" + ((reader["active"].ToString() == "1") ? "Working" : "Not Working") + "</ACTIVE>" +
    //                       "<STARTDATE>" + reader["start_date"] + "</STARTDATE>" +
    //                       "<ENDDATE>" + reader["end_date"] + "</ENDDATE>" +
    //                       "<COMMENTS><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["comments"].ToString()) + "]]></COMMENTS>" +
    //                       "<EXT_REQUESTED>" + reader["ext_requested"] + "</EXT_REQUESTED>" +
    //                       "<PROFILE_PICTURE_PATH>" + reader["profile_picture_path"] + "</PROFILE_PICTURE_PATH>" +
    //                       "<LAST_4_DIGITS_OF_SSN_SIN>" + reader["Last_4_Digits_of_SSN_SIN"] + "</LAST_4_DIGITS_OF_SSN_SIN>" +
    //                       "<PAY_RATE>" + reader["pay_rate"] + "</PAY_RATE>" +
    //                       "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
    //                       "<USER_ID>" + reader["user_id"] + "</USER_ID>" +
    //                       "<INTERVIEW_REQUESTED>" + reader["interview_requested"] + "</INTERVIEW_REQUESTED>" +
    //                       "<CANDIDATE_REJECTED>" + reader["candidate_rejected"] + "</CANDIDATE_REJECTED>" +
    //                        "<MORE_INFO>" + reader["more_info"] + "</MORE_INFO>" +
    //                        "<INTERVIEW_DATE>" + reader["interview_date"] + "</INTERVIEW_DATE>" +
    //                        "<INTERVIEW_TIME>" + reader["interview_time"] + "</INTERVIEW_TIME>" +
    //                        "<REASON_OF_REJECTION><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["reason_of_rejection"].ToString()) + "]]></REASON_OF_REJECTION>" +
    //                        "<CANDIDATE_APPROVE>" + reader["candidate_approve"] + "</CANDIDATE_APPROVE>" +
    //                        "<INTERVIEW_CONFIRM>" + reader["interview_confirm"] + "</INTERVIEW_CONFIRM>" +
    //                          "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
    //                            "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +

    //                            "<ACTIVE>" + reader["active"] + "</ACTIVE>" +
    //                         "<ACTION_ID>" + reader["action_id"] + "</ACTION_ID>" +
    //                         "<VENDOR_INTERVIEW_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT>" +
    //                          "<VENDOR_INTERVIEW_COMMENT2><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment2"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT2>" +
    //                           "<VENDOR_INTERVIEW_COMMENT3><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment3"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT3>" +
    //                            "<VENDOR_INTERVIEW_COMMENT4><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment4"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT4>" +
    //                             "<VENDOR_INTERVIEW_COMMENT5><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_interview_comment5"].ToString()) + "]]></VENDOR_INTERVIEW_COMMENT5>" +
    //                         "<INTERVIEW_REQUEST_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT>" +
    //                         "<INTERVIEW_REQUEST_COMMENT2><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment2"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT2>" +
    //                         "<INTERVIEW_REQUEST_COMMENT3><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment3"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT3>" +
    //                         "<INTERVIEW_REQUEST_COMMENT4><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment4"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT4>" +
    //                           "<INTERVIEW_REQUEST_COMMENT5><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_request_comment5"].ToString()) + "]]></INTERVIEW_REQUEST_COMMENT5>" +
    //                            "<VENDOR_MOREINFO_REPLY><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_moreInfo_reply"].ToString()) + "]]></VENDOR_MOREINFO_REPLY>" +
    //                             "<VENDOR_REJECT_CANDIDATE><![CDATA[" + reader["vendor_reject_candidate"] + "]]></VENDOR_REJECT_CANDIDATE>" +
    //                              "<VENDOR_REJECT_CANDIDATE_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["vendor_reject_candidate_comment"].ToString()) + "]]></VENDOR_REJECT_CANDIDATE_COMMENT>" +
    //                            "<ACTION_CREATE_DATE><![CDATA[" + reader["action_create_date"] + "]]></ACTION_CREATE_DATE>" +
    //                             "<INTERVIEW_CANCEL_BY_CLIENT>" + reader["interview_cancel_by_client"] + "</INTERVIEW_CANCEL_BY_CLIENT>" +
    //                             "<INTERVIEW_CANCEL_BY_CLIENT_COMMENT><![CDATA[" + HttpContext.Current.Server.HtmlDecode(reader["interview_cancel_by_client_comment"].ToString()) + "]]></INTERVIEW_CANCEL_BY_CLIENT_COMMENT>" +
    //                    "</EMPLOYEE_NAME_ID>";
    //                    RowID = RowID + 1;
    //                }
    //                reader.Close();
    //                cmd.Dispose();
    //            }
    //        }

    //        catch (Exception ex)
    //        {
    //            xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
    //            logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

    //        }

    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //    }
    //    xml_string = xml_string + "</RESPONSE>" +
    //                      "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //} 

    //[WebMethod]
    public XmlDocument get_rating_with_jobid(string userEmailId, string userPassword, string JOB_ID)
    {
       ///
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (JOB_ID != "" & JOB_ID != "0")
        {
            strSub = " job_id =" + JOB_ID;
        }

        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                         "<JOB_ID>" + JOB_ID + "</JOB_ID>" +

                            "</REQUEST>";
                string strSql = "select Question_ID,question_1,rating_1,question_2,rating_2,question_3,rating_3,question_4,rating_4,question_5,rating_5,emp_rating_1,emp_rating_2,emp_rating_3,emp_rating_4,emp_rating_5,employee_id,job_ID from ovms_job_question_rating where " + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int RowID = 1;
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<QUESTIONS_NO ID='" + RowID + "'>" +
                                        "<QUESTION_ID>" + reader["QUESTION_ID"] + "</QUESTION_ID>" +
                                        "<QUESTION1><![CDATA[" + reader["question_1"] + "]]></QUESTION1>" +
                                        "<RATING1>" + reader["rating_1"] + "</RATING1>" +
                                        "<QUESTION2><![CDATA[" + reader["question_2"] + "]]></QUESTION2>" +
                                        "<RATING2>" + reader["rating_2"] + "</RATING2>" +
                                        "<QUESTION3><![CDATA[" + reader["question_3"] + "]]></QUESTION3>" +
                                        "<RATING3>" + reader["rating_3"] + "</RATING3>" +
                                        "<QUESTION4><![CDATA[" + reader["question_4"] + "]]></QUESTION4>" +
                                        "<RATING4>" + reader["rating_4"] + "</RATING4>" +
                                        "<QUESTION5><![CDATA[" + reader["question_5"] + "]]></QUESTION5>" +
                                        "<RATING5>" + reader["rating_5"] + "</RATING5>" +
                                        "<EMP_RATING_1>" + reader["emp_rating_1"] + "</EMP_RATING_1>" +
                                        "<EMP_RATING_2>" + reader["emp_rating_2"] + "</EMP_RATING_2>" +
                                        "<EMP_RATING_3>" + reader["emp_rating_3"] + "</EMP_RATING_3>" +
                                        "<EMP_RATING_4>" + reader["emp_rating_4"] + "</EMP_RATING_4>" +
                                        "<EMP_RATING_5>" + reader["emp_rating_5"] + "</EMP_RATING_5>" +
                                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                        "<JOB_ID>" + reader["job_ID"] + "</JOB_ID>" +
                        "</QUESTIONS_NO>";
                        RowID = RowID + 1;
                    }
                }
                else
                {
                    xml_string = xml_string + "<DATA>no records found</DATA>";
                    // logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                }
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {

            // logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

            xml_string = "<XML>" +
                         "<STRING> Unable to select job ID</STRING> ";
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }

        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument search_employees(string userEmailId, string userPassword, string first_name, string Last_name, string city, string country, string province, string postal, string skype_id, string vendor_id, string active)
    {
       //
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                    "<REQUEST>" +
                        "<VENDORID>" + vendor_id + "</VENDORID>" +
                        "<FIRSTNAME><![CDATA[" + first_name + "]]></FIRSTNAME>" +
                         "<LASTNAME><![CDATA[" + Last_name + "]]></LASTNAME>" +
                         "<CITY>" + city + "</CITY>" +
                         "<COUNTRY>" + country + "</COUNTRY>" +
                         "<PROVINCE>" + province + "</PROVINCE>" +
                         "<POSTAL>" + postal + "</POSTAL>" +
                         "<SKYPEID>" + skype_id + "</SKYPEID>" +
                        "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();

                string strSql = "select concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_id," +
    "ed.first_name, ed.middle_name,ed.last_name, ed.email, ed.profile_picture_path, ed.date_of_birth, ed.phone, ed.suite_no, ed.address1, ed.address2,ed.city, ed.availability_for_interview, ed.postal," +
    "ed.province,ed.country,ed.skype_id,ed.start_date, ed.end_date, ed.comments,ed.active,ed.create_date,ed.active,ed.securityID,ven.vendor_name," +
    "em.job_id ,clt.client_name, ja.std_pay_rate_from, ja.job_id,ja.st_bill_rate_to, ja.st_pay_rate_to, ja.markup, ja.ot_factor_of_st," +
    "ja.ot_pay_rate_from, ja.ot_pay_rate_to, ja.ot_bill_rate_to,ja.job_accounting_id, ja.dbl_pay_rate_from, ja.dbl_pay_rate_to, ja.dbl_factor_of_st, ja.st_bill_rate_from," +
    "ja.st_bill_rate_from, ja.ot_bill_rate_from, ja.ot_bill_rate_to,ja.dt_bill_rate_from, ja.dt_bill_rate_to, ja.cost_allocation" +
    " from ovms_employees as em join ovms_employee_details as ed on" +
    " em.employee_id = ed.employee_id join ovms_vendors as ven on" +
    " em.vendor_id = ven.vendor_id join ovms_clients as clt on em.client_id = clt.client_id join ovms_job_accounting as ja on" +
    " ja.job_id = em.job_id ";

                if (first_name != "")
                {
                    strSql = strSql + " and ed.first_name='" + first_name + "'";
                }
                if (Last_name != "")
                {
                    strSql = strSql + " and ed.last_name = '" + Last_name + "'";
                }
                if (city != "")
                {
                    strSql = strSql + " and ed.city= '" + city + "'";
                }
                if (country != "")
                {
                    strSql = strSql + " and ed.country= '" + country + "'";
                }
                if (province != "")
                {
                    strSql = strSql + " and ed.province= '" + province + "'";
                }
                if (postal != "")
                {
                    strSql = strSql + " and ed.postal= '" + postal + "'";
                }
                if (skype_id != "")
                {
                    strSql = strSql + " and ed.skype= '" + skype_id + "'";
                }
                if (vendor_id != "" & vendor_id != "0")
                {
                    strSql = strSql + " and em.vendor_id= " + vendor_id;
                }

                if (active != "")
                {
                    strSql += " and em.active = " + active;
                }
                try
                {
                    SqlCommand cmd = new SqlCommand(strSql, conn);


                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<EMPLOYEE_NAME_ID ID=\"" + reader["employee_id"] + "\">" +
                                                        "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
                                                        "<MIDDLE_NAME><![CDATA[" + reader["middle_name"] + "]]></MIDDLE_NAME>" +
                                                        "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
                                                        "<EMAIL>" + reader["email"] + "</EMAIL>" +
                                                        "<PHONE>" + reader["phone"] + "</PHONE>" +
                                                        "<DATE_OF_BIRTH>" + reader["date_of_birth"] + "</DATE_OF_BIRTH>" +
                                                        "<SUITE_NO>" + reader["suite_no"] + "</SUITE_NO>" +
                                                        "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
                                                        "<ADDRESS2><![CDATA[" + reader["address2"] + "]]></ADDRESS2>" +
                                                        "<CITY>" + reader["city"] + "</CITY>" +
                                                        "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
                                                        "<POSTAL>" + reader["postal"] + "</POSTAL>" +
                                                        "<COUNTRY>" + reader["country"] + "</COUNTRY>" +
                                                        "<SECURITY_ID>" + reader["securityID"] + "</SECURITY_ID>" +
                                                        "<SKYPE>" + reader["skype_id"] + "</SKYPE>" +
                                                        "<AVAILABILITY_FOR_INTERVIEW>" + reader["availability_for_interview"] + "</AVAILABILITY_FOR_INTERVIEW>" +
                                                        "<ACTIVE>" + ((reader["active"].ToString() == "1") ? "Working" : "Not Working") + "</ACTIVE>" +
                                                        "<STARTDATE>" + reader["start_date"] + "</STARTDATE>" +
                                                        "<ENDDATE>" + reader["end_date"] + "</ENDDATE>" +
                                                        "<COMMENTS><![CDATA[" + reader["comments"] + "]]></COMMENTS>" +
                                                        "<PROFILE_PICTURE_PATH>" + reader["profile_picture_path"] + "</PROFILE_PICTURE_PATH>" +
                                                        "<STANDARD_PAY_RATE_FROM>" + reader["std_pay_rate_from"] + "</STANDARD_PAY_RATE_FROM>" +
                                                        "<STANDARD_PAY_RATE_TO>" + reader["st_pay_rate_to"] + "</STANDARD_PAY_RATE_TO>" +
                                                        "<STANDARD_BILL_RATE_FROM>" + reader["st_bill_rate_from"] + "</STANDARD_BILL_RATE_FROM>" +
                                                        "<STANDARD_BILL_RATE_TO>" + reader["st_bill_rate_to"] + "</STANDARD_BILL_RATE_TO>" +
                                                        "<OVERTIME_PAY_RATE_FROM>" + reader["ot_pay_rate_from"] + "</OVERTIME_PAY_RATE_FROM>" +
                                                        "<OVERTIME_PAY_RATE_TO>" + reader["ot_pay_rate_to"] + "</OVERTIME_PAY_RATE_TO>" +
                                                        "<OVERTIME_FACTOR_OF_STANDARD_TIME>" + reader["ot_factor_of_st"] + "</OVERTIME_FACTOR_OF_STANDARD_TIME>" +
                                                        "<OVERTIME_BILL_RATE_FROM>" + reader["ot_bill_rate_from"] + "</OVERTIME_BILL_RATE_FROM>" +
                                                        "<OVERTIME_BILL_RATE_TO>" + reader["ot_bill_rate_to"] + "</OVERTIME_BILL_RATE_TO>" +
                                                        "<MARKUP>" + reader["markup"] + "</MARKUP>" +
                                                        "<JOB_ACCOUNTING_ID>" + reader["job_accounting_id"] + "</JOB_ACCOUNTING_ID>" +
                                                        "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                                        "<DOUBLE_BILL_RATE_FROM>" + reader["dt_bill_rate_from"] + "</DOUBLE_BILL_RATE_FROM>" +
                                                        "<DOUBLE_TIME_BILL_RATE_TO>" + reader["dt_bill_rate_to"] + "</DOUBLE_TIME_BILL_RATE_TO>" +
                                                        "<DOUBLE_PAY_RATE_FROM>" + reader["dbl_pay_rate_from"] + "</DOUBLE_PAY_RATE_FROM>" +
                                                        "<DOUBLE_PAY_RATE_TO>" + reader["dbl_pay_rate_to"] + "</DOUBLE_PAY_RATE_TO>" +
                                                        "<DOUBLE_FACTOR_OF_STANDARD_TIME>" + reader["dbl_factor_of_st"] + "</DOUBLE_FACTOR_OF_STANDARD_TIME>" +
                                                        "<COST_ALLOCATION>" + reader["cost_allocation"] + "</COST_ALLOCATION>" +
                                                        "</EMPLOYEE_NAME_ID>";


                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<ERROR>no records found</ERROR>";
                        //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No employee Found");
                    }

                }
                catch (Exception ex)
                {
                    xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
                    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

                }

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }
    [WebMethod]
    public XmlDocument get_all_available_job_for_particuler_vendor(string userEmailId, string userPassword, string vendor_id)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                  "<REQUEST>" +
                               "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                          "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (errString != "")
        {
            xml_string = xml_string + "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (vendor_id != "" & vendor_id != "0")
            {
                //strSub = " and vendor_id=" + vendor_id;
                strSub = " and vendor_id = patindex('%" + vendor_id + "%', vendors)";
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    //string strSql = "select  job_title ,concat('J',clt.client_alias, '00', right('000' + convert(varchar(4),job_id),4))job_id  from ovms_jobs as j" +
                    //                " join ovms_clients as clt on j.client_id = clt.client_id where job_status_id = 1 and clt.active =1 and j.active =1 " + strSub;
                    string strSql = "select  dbo.CamelCase(job_title) as job_title ,concat('J',clt.client_alias, '00', right('000' + convert(varchar(4),job_id),4))job_id  from ovms_jobs as j" +
                                    " join ovms_clients as clt on j.client_id = clt.client_id where j.submit=1 and job_status_id = 1 and clt.active =1 and j.active =1 " +
                                    " and patindex('%" + vendor_id + "%', vendors)>0 ";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;

                    if (reader.HasRows == true)
                    {

                        while (reader.Read())


                        {
                            xml_string = xml_string + "<JOB_NO ID ='" + RowID + "'>" +
                                                        "<JOB_TITLE-JOB_ID><![CDATA[" + reader["job_title"] + "-" + reader["job_id"] + "]]></JOB_TITLE-JOB_ID>" +
                                                        "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                           "</JOB_NO>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = xml_string + "<XML>" +

                            "<STATUS> Unable to select vendor id</ STATUS > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string = xml_string + "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument set_employee(string userEmailId, string userPassword, string first_name, string middle_name, string last_name, string email, string phone, string date_of_birth, string suite_no, string address1, string address2, string city, string province, string postal, string country, string comments, string profile_picture_path, string availability_for_interview, string skype_id, string startDate, string endDate, int job_id, int vendor_id, int client_id, string liecence_no, string Last_4_Digits_of_SSN_SIN, string pay_rate)
    {
       //
        SqlConnection conn;
        string errString = "";
        int newuserid = 0;
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                                    "<REQUEST>" +
                                    "<FIRSTNAME><![CDATA[" + first_name + "]]></FIRSTNAME>" +
                                    "<MIDDLE_NAME><![CDATA[" + middle_name + "]]></MIDDLE_NAME>" +
                                    "<LASTNAME><![CDATA[" + last_name + "]]></LASTNAME>" +
                                    "<EMAIL>" + email + "</EMAIL>" +
                                    "<PHONE>" + phone + "</PHONE>" +
                                    "<DATE_OF_BIRTH>" + date_of_birth + "</DATE_OF_BIRTH>" +
                                    "<SUITE_NO>" + suite_no + "</SUITE_NO>" +
                                    "<ADDRESS1><![CDATA[" + address1 + "]]></ADDRESS1>" +
                                    "<ADDRESS2><![CDATA[" + address2 + "]]></ADDRESS2>" +
                                    "<CITY>" + city + "</CITY>" +
                                    "<PROVINCE>" + province + "</PROVINCE>" +
                                    "<POSTAL>" + postal + "</POSTAL>" +
                                    "<COUNTRY>" + country + "</COUNTRY>" +
                                    "<SKYPE_ID>" + skype_id + "</SKYPE_ID>" +
                                    "<START_DATE>" + startDate + "</START_DATE>" +
                                    "<END_DATE>" + endDate + "</END_DATE>" +
                                    "<AVAILABILITY_FOR_INTERVIEW>" + availability_for_interview + "</AVAILABILITY_FOR_INTERVIEW>" +
                                    "<PROFILE_PICTURE_PATH>" + profile_picture_path + "</PROFILE_PICTURE_PATH>" +
                                    "<COMMENTS><![CDATA[" + comments + "]]></COMMENTS>" +
                                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                    "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                                    "<JOB_ID>" + job_id + "</JOB_ID>" +
                                    "<PAY_RATE>" + pay_rate + "</PAY_RATE>" +
                                    "<LIECENCE_NO>" + liecence_no + "</LIECENCE_NO>" +
                                    "<LAST_4_DIGITS_OF_SSN_SIN>" + Last_4_Digits_of_SSN_SIN + "</LAST_4_DIGITS_OF_SSN_SIN>" +
                                   "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";

        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    // SqlCommand cmd;

                    string strSql = "INSERT INTO ovms_users(first_name, last_name, email_id, user_password,utype_id,client_id,vendor_id)" +
                      " VALUES('" + first_name + "','" + last_name + "','" + email + "','1234','4','" + client_id + "','" + vendor_id + "')" +
                      " SELECT CAST(scope_identity() AS int)";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    newuserid = (int)cmd.ExecuteScalar();

                    int employee_id = 1;
                    strSql = " INSERT INTO ovms_employees(vendor_id,client_id,job_id,user_id) values (" + vendor_id + "," + client_id + "," + job_id + "," + newuserid + ")";
                    //    "select cast(scope_identity() as int)";
                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;
                    int ireturn = cmd.ExecuteNonQuery();
                    //employee_id = (int)cmd.ExecuteScalar();

                    string sqlGetLastInsertedEmployee = "select max(employee_id) as employee_id from ovms_employees where vendor_id = "+ vendor_id + " and client_id = "+ client_id  + " and job_id = "+ job_id  + " and user_id = "+ newuserid  + " and active = 1";
                    SqlCommand cmdLastInsertedEmployee = new SqlCommand(sqlGetLastInsertedEmployee, conn);
                    SqlDataReader readerInsertedEmployee = cmdLastInsertedEmployee.ExecuteReader();

                    if (readerInsertedEmployee.HasRows == true)
                    {
                        while (readerInsertedEmployee.Read())
                        {
                            employee_id = Convert.ToInt32(readerInsertedEmployee["employee_id"].ToString());
                        }
                    }
                    //close
                    readerInsertedEmployee.Close();
                    cmdLastInsertedEmployee.Dispose();


                    string strSql1 = "INSERT INTO ovms_employee_details (employee_id,first_name,last_name,email,phone,address1,profile_picture_path,address2,city,province,postal,country,skype_id,start_date,end_date,middle_name, date_of_birth, comments, availability_for_interview, suite_no,licence_no,Last_4_Digits_of_SSN_SIN,pay_rate)" +
                    " VALUES(" + employee_id + ", '" + first_name + "', '" + last_name + "', '" + email + "', '" + phone + "', '" + address1 + "','" + profile_picture_path + "', '" + address2 + "', '" + city + "','" + province + "','" + postal + "','" + country + "','" + skype_id + "','" + startDate + "','" + endDate + "', '" + middle_name + "', '" + date_of_birth + "', '" + comments + "','" + availability_for_interview + "','" + suite_no + "','" + liecence_no + "','" + Last_4_Digits_of_SSN_SIN + "','" + pay_rate + "')" + "select cast(scope_identity() as int)";
                    SqlCommand cmd1 = new SqlCommand(strSql1, conn);
                    cmd1 = conn.CreateCommand();
                    // cmd.CommandText = strSql;


                    cmd1 = new SqlCommand(strSql1, conn);
                    if (cmd1.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>user inserted successfully</STRING><EMPLOYEE_ID> " + employee_id + " </EMPLOYEE_ID> ";

                    }
                    else
                    {
                        xml_string += "<STRING><ERROR>user  not inserted</ERROR> </STRING>";
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        xml_string += "</RESPONSE>";
        xml_string += "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;



    }
    [WebMethod]
    public XmlDocument delete_employee(string userEmailId, string userPassword, int employeeId)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSql = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                             "<REQUEST>" +
                              "<USERID>" + employeeId + "</USERID>" +
                         "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    //logAPI.Service logService = new logAPI.Service();
                    //SqlConnection conn;
                    //string xml_string = "";
                    //string errString = "";
                    //errString = VerifyUser(userEmailId, userPassword);
                    //xml_string = "<XML>" +
                    //                    "<REQUEST>" +
                    //                        "<USERID>" + employeeId + "</USERID>" +
                    //                    "</REQUEST>";

                    //xml_string += "<RESPONSE>";
                    //if (errString != "")
                    //{
                    //    xml_string += "<ERROR>" + errString + "</ERROR>";
                    //}
                    //else
                    //{
                    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

                    //    if (conn.State == System.Data.ConnectionState.Closed)
                    //    {
                    //        conn.Open();

                    strSql = " update ovms_employees set active=0 where employee_id= " + employeeId + ";" +
                    " update ovms_employee_details set active=0 where employee_id= " + employeeId + ";";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>employee deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>employee  not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete employee ");
                    }
                    cmd.Dispose();
                }
                //string strSql1 = "update ovms_timesheet_details set  active=0 where timesheet_id = " + timesheet_id + ";";

                //SqlCommand cmd1 = new SqlCommand(strSql1, conn);




            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;



    }

    [WebMethod]
    public XmlDocument get_job_alias(string userEmailId, string userPassword, string JOB_ID)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (JOB_ID != "" & JOB_ID != "0")
        {
            strSub = " and job_id =" + JOB_ID;
        }
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                         "<JOB_ID>" + JOB_ID + "</JOB_ID>" +
                            "</REQUEST>";
                string strSql = "select dbo.GetJobNo(job_id) job_alias,job_id from ovms_jobs where active=1" + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<JOB_ALIAS>" + reader["job_alias"] + "</JOB_ALIAS>" +
                           "<JOB_ID>" + reader["job_id"] + "</JOB_ID>";
                    }
                    //Dispose

                }
                else
                {
                    xml_string = xml_string + "<DATA>no records found</DATA>";
                    //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                }
                reader.Close();
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {

            //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

            xml_string = "<XML>" +
                         "<STRING> Unable to select job comment</STRING> ";
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        //}
        //else
        //{
        //    xml_string += "<COMMENT_ID>comment_id should not be null</COMMENT_ID>";
        //}
        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_resume(string userEmailId, string userPassword, string emp_id, string resume_id)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                   "<REQUEST>" +
                     "<EMPLOYEEID>" + emp_id + "</EMPLOYEEID>" +
                                "<RESUME_ID>" + resume_id + "</RESUME_ID>" +
                           "</REQUEST>";
        xml_string += "<RESPONSE>";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (emp_id != "" & emp_id != "0")
            {
                strSub = " and employee_id=" + emp_id;
            }
            if (resume_id != "" & resume_id != "0")
            {
                strSub += " and resume_id=" + resume_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select employee_id,job_id,resume_path,resume_id,User_id,vendor_id from ovms_resume where active=1" + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<RESUME_ID ID ='" + RowID + "'>" +
                                                  "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                                    "<RESUME_ID>" + reader["resume_id"] + "</RESUME_ID>" +
                                                      "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                                    "<RESUME_PATH>" + reader["resume_path"] + "</RESUME_PATH>" +
                                                    "<USER_ID>" + reader["user_id"] + " </USER_ID>" +
                                                    "<VENDOR_ID>" + reader["vendor_id"] + " </VENDOR_ID>" +
                                                  "</RESUME_ID>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    reader.Close();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<XML>" +
                            "<REQUEST>" + resume_id + "</REQUEST>" +
                            "<RESPONSE> Unable to select resume</ RESPONSE > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }


    [WebMethod]
    public XmlDocument insert_resume(string userEmailId, string userPassword, string resume_path, string job_id, string employee_id, string user_id, string vendor_id)
    {
       //
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                                "<JOB_ID>" + job_id + "</JOB_ID>" +
                                                "<RESUME_PATH>" + resume_path + "</RESUME_PATH>" +
                                                "<USER_ID>" + user_id + " </USER_ID>" +
                                                "<VENDOR_ID>" + vendor_id + " </VENDOR_ID>" +
                                   "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (resume_path != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "INSERT INTO ovms_resume (employee_id,job_id,resume_path,User_id,vendor_id)VALUES('" + employee_id + "','" + job_id + "','" + resume_path + "','" + user_id + "','" + vendor_id + "') SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>resume_path inserted successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>resume_path not inserted</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert resume_path");
                        }
                        //reader.Close();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_resume(string userEmailId, string userPassword, string resume_path, string job_id, string employee_id, string user_id, string vendor_id)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                  "<REQUEST>" +
                                  "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                               "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    if (resume_path != "")
                    {
                        string strSql = "update ovms_resume set resume_path ='" + resume_path + "',job_id='" + job_id + "',User_id='" + user_id + "',vendor_id='" + vendor_id + "' where employee_id =  '" + employee_id + "' and active=1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>resume_path updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>resume_path not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update resume_path");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_resume(string userEmailId, string userPassword, int resume_id)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                  "<REQUEST>" +
                                   "<RESUME_ID>" + resume_id + "</RESUME_ID>" +
                              "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "update ovms_resume set  active=0 where resume_id ='" + resume_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);


                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STRING>resume path deleted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                    }
                    else
                    {
                        xml_string += "<STRING>resume path not deleted</STRING>" +
                                    "<STATUS>0</STATUS>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete resume path");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_candiate_for_that_particuler_job(string userEmailId, string userPassword, string job_id, string vendorID)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                  "<REQUEST>" +
                               "<JOB_ID>" + job_id + "</JOB_ID>" +
                          "</REQUEST>";
        xml_string += "<RESPONSE>";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (vendorID != "" & vendorID != "0")
            {
                strSub = " and em.vendor_id=" + vendorID;
            }
            if (job_id != "" & job_id != "0")
            {
                strSub += " and em.job_id=" + job_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select distinct em.employee_id,ed.first_name,ed.middle_name, " +
                         "  ed.last_name,em.job_id,ed.pay_rate,ed.email, " +
                           " concat(ed.city, ', ', ed.province)location,re.resume_path,re.createdate " +
                         "  from ovms_employees as em " +
                         " inner join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
                       "   inner join ovms_resume as re on em.job_id = re.job_id " +
                       "   where em.active = 1 " + strSub + " order by re.createdate desc";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;

                    if (reader.HasRows == true)
                    {

                        while (reader.Read())


                        {
                            xml_string += "<EMPLOYEE_NO ID ='" + RowID + "'>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<MIDDEL_NAME><![CDATA[" + reader["middle_name"] + "]]></MIDDEL_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                          "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                          "<RESUME_PATH>" + reader["resume_path"] + "</RESUME_PATH>" +
                                          "<EMAIL>" + reader["email"] + "</EMAIL>" +
                                          "<LOCATION>" + reader["location"] + "</LOCATION>" +
                                          "<SUBMIT_DATE>" + reader["createdate"] + "</SUBMIT_DATE>" +
                                          "<PAY_RATE>" + reader["pay_rate"] + "</PAY_RATE>" +
                                          "<STATUS>" + "Submited-review in progress" + "</STATUS>" +
                                          "</EMPLOYEE_NO>";
                            RowID++;
                        }
                        reader.Close();
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<XML>" +

                            "<STATUS> Unable to select job id</ STATUS > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument search_anything(string userEmailId, string userPassword, string saerch_string)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                  "<REQUEST>" +
                               "<SEARCH>" + saerch_string + "</SEARCH>" +

                          "</REQUEST>";
        xml_string += "<RESPONSE>";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = " select concat('W', clt.client_alias, '00', right('0000' + convert(varchar(4), em.employee_id), 4)) employee_id, " +
                                        " em.employee_id,ed.first_name,ed.last_name, ed.email,j.job_title, " +
                                        " ed.city,ed.province, " +
                                        " concat('J', clt.client_alias, '00', right('0000' + convert(varchar(4), em.job_id), 4)) job_id " +
                                        " from ovms_employees as em " +
                                        " join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
                                        " join ovms_vendors as ven on em.vendor_id = ven.vendor_id " +
                                        " join ovms_clients as clt on em.client_id = clt.client_id " +
                                        " join ovms_job_accounting as ja on ja.job_id = em.job_id " +
                                        " join ovms_jobs as j on ja.job_id = j.job_id where 1 = 1";
                    if (saerch_string != "" & saerch_string != "0")
                    {
                        strSql = strSql + " and concat('J',clt.client_alias, '00', right('0000' + convert(varchar(4),em.job_id),4)) like '%" + saerch_string + "%' " +
                        " and concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) like'%" + saerch_string + "%' " +

                        "  or  ed.first_name like '%" + saerch_string + "%' " +
                     "  or  ed.last_name like '%" + saerch_string + "%' " +
                     "  or  ed.city like '%" + saerch_string + "%' " +
                     "  or ed.province like '%" + saerch_string + "%' " +
                      "  or j.job_title like '%" + saerch_string + "%' " +
                     "  or  ed.email like '%" + saerch_string + "%' ";
                    }
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;

                    if (reader.HasRows == true)
                    {

                        while (reader.Read())
                        {
                            xml_string += "<EMPLOYEE_NO ID ='" + RowID + "'>" +
                                                  "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                                  "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                                  "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                                  "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                                  "<JOB_TITLE><![CDATA[" + reader["job_title"] + "]]></JOB_TITLE>" +
                                                  "<CITY>" + reader["city"] + "</CITY>" +
                                                  "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
                                                  "<EMAIL>" + reader["email"] + "</EMAIL>" +
                                                  "</EMPLOYEE_NO>";
                            RowID++;
                        }
                        reader.Close();
                        cmd.Dispose();
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                }
            }
            catch (Exception ex)
            {

                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<XML>" +
                            "<STATUS> Unable to select job id</ STATUS > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                          "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_country(string userEmailId, string userPassword, string country_id)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<COUNTRY_ID>" + country_id + "</COUNTRY_ID>"
                    + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (country_id != "" & country_id != "0")
            {
                strSub = " and country_id=" + country_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select country_name,country_code,country_id from ovms_country where 1=1 " + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {
                        while (reader.Read())

                        {
                            xml_string += "<COUNTRY_NO ID ='" + RowID + "'>" +
                                                  "<COUNTRY_NAME>" + reader["country_name"] + "</COUNTRY_NAME>" +
                                                  "<COUNTRY_CODE>" + reader["country_code"] + "</COUNTRY_CODE>" +
                                                   "<COUNTRY_ID>" + reader["country_id"] + "</COUNTRY_ID>" +
                                                                                             "</COUNTRY_NO>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<status> Unable to select country_id </ status > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>" +
                      "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);
        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_state(string userEmailId, string userPassword, string country_id)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<COUNTRY_ID>" + country_id + "</COUNTRY_ID>"
                    + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (country_id != "" & country_id != "0")
            {
                strSub = " and country_id=" + country_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select state_name,state_code,country_id,state_id from ovms_state where 1=1 " + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {
                        while (reader.Read())

                        {
                            xml_string += "<STATE_NO ID ='" + RowID + "'>" +
                                                  "<STATE_NAME>" + reader["state_name"] + "</STATE_NAME>" +
                                                  "<STATE_CODE>" + reader["state_code"] + "</STATE_CODE>" +
                                                   "<COUNTRY_ID>" + reader["country_id"] + "</COUNTRY_ID>" +
                                                   "<STATE_ID>" + reader["state_id"] + "</STATE_ID>" +
                                          "</STATE_NO>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<status> Unable to select country_id </ status > ";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //else
        //{
        //    xml_string += "<DEPARTMENT_ID>department_id should not be null</DEPARTMENT_ID>";
        //}

        xml_string += "</RESPONSE>" +
                      "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);
        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_employee(string userEmailId, string userPassword, string employye_id, string job_id, string vendor_id, string client_id, string first_name, string middle_name, string last_name, string email, string phone, string date_of_birth, string suite_no, string address1, string address2, string city, string province, string postal, string country, string comments, string profile_picture_path, string availability_for_interview, string skype_id, string startDate, string endDate, string licence_id, string Last_4_Digits_of_SSN_SIN, string pay_rate)
    {
       //
        SqlConnection conn;
        string xml_string = "";
        string strSql1 = "";
        string strSql = "";


        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                     + "<REQUEST>"
                     + "<EMPLOYEE_ID>" + employye_id + "</EMPLOYEE_ID>"
                     + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                int FStatus = 0;
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    if (employye_id != "")
                    {
                        if (vendor_id + client_id + job_id != "")
                            strSql = "update ovms_employees set ";
                        if (vendor_id != "")
                        {
                            strSql += (FStatus == 1 ? "," : "") + " vendor_id=" + vendor_id;
                            FStatus = 1;
                        }

                        if (client_id != "")
                        {
                            strSql += (FStatus == 1 ? "," : "") + " client_id =" + client_id;
                            FStatus = 1;
                        }
                        if (job_id != "")
                        {
                            strSql += (FStatus == 1 ? "," : "") + " job_id=" + job_id;
                            FStatus = 1;
                        }
                        if (vendor_id + client_id + job_id != "")
                        {
                            strSql += " where employee_id = " + employye_id + ";";
                            //SqlCommand cmd = new SqlCommand(strSql, conn);
                            //if (cmd.ExecuteNonQuery() > 0)
                            //{
                            //    xml_string += "<STATUS>saved successfully</STATUS>";
                            //}
                        }

                    }

                    FStatus = 0;

                    if (first_name != "")
                    {
                        strSql1 = (FStatus == 1 ? "," : "") + " first_name='" + first_name + "'";
                        FStatus = 1;
                    }

                    if (middle_name != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " middle_name ='" + middle_name + "'";
                        FStatus = 1;
                    }
                    if (last_name != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " last_name ='" + last_name + "'";
                        FStatus = 1;
                    }
                    if (email != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " email ='" + email + "'";
                        FStatus = 1;
                    }
                    if (phone != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " phone ='" + phone + "'";
                        FStatus = 1;
                    }
                    if (date_of_birth != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " date_of_birth='" + date_of_birth + "'";
                        FStatus = 1;
                    }

                    if (suite_no != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " suite_no ='" + suite_no + "'";
                        FStatus = 1;
                    }
                    if (address1 != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " address1 = '" + address1 + "'";
                        FStatus = 1;
                    }
                    if (address2 != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " address2 = '" + address2 + "'";
                        FStatus = 1;
                    }
                    if (city != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " city = '" + city + "'";
                        FStatus = 1;
                    }
                    if (province != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " province = '" + province + "'";
                        FStatus = 1;
                    }
                    if (postal != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " postal='" + postal + "'";
                        FStatus = 1;
                    }

                    if (country != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " country ='" + country + "'";
                        FStatus = 1;
                    }
                    if (comments != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " comments ='" + comments + "'";
                        FStatus = 1;
                    }
                    if (profile_picture_path != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " profile_picture_path ='" + profile_picture_path + "'";
                        FStatus = 1;
                    }
                    if (availability_for_interview != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " availability_for_interview ='" + availability_for_interview + "'";
                        FStatus = 1;
                    }

                    if (skype_id != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " skype_id = '" + skype_id + "'";
                        FStatus = 1;
                    }
                    if (startDate != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " start_date = '" + startDate + "'";
                        FStatus = 1;
                    }
                    if (endDate != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " end_date='" + endDate + "'";
                        FStatus = 1;
                    }

                    if (licence_id != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " licence_no ='" + licence_id + "'";
                        FStatus = 1;
                    }
                    if (Last_4_Digits_of_SSN_SIN != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " Last_4_Digits_of_SSN_SIN ='" + Last_4_Digits_of_SSN_SIN + "'";
                        FStatus = 1;
                    }
                    if (pay_rate != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " pay_rate ='" + pay_rate + "'";
                        FStatus = 1;
                    }
                    if (strSql1 != "")
                    {
                        strSql += " update ovms_employee_details set " + strSql1 + " where employee_id = " + employye_id + ";";
                    }

                    //strSql1 += " where employee_id = " + employye_id;
                    SqlCommand cmd = new SqlCommand(strSql, conn);


                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>YES</RESPONSE_MESSAGE><UPDATE_STRING>employee updated successfully</UPDATE_STRING>" +
                                   "<UPDATE_VALUE>1</UPDATE_VALUE>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE><UPDATE_STRING>employee not updated</UPDATE_STRING>" +
                                    "<UPDATE_STRING>0</UPDATE_STRING>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update employee");
                    }
                    cmd.Dispose();
                }
                else
                {
                    xml_string += "<RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE>";
                }

            }

            catch (Exception ex)
            {
                //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string += "<RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
}