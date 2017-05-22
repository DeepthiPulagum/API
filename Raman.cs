using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml;

/// <summary>
/// Summary description for Raman
/// </summary>
public class Raman
{
    public Raman()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string VerifyUser(string emailId, string userPassword)
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
    public XmlDocument get_CLient(string userEmailId, string userPassword, string client_id)
    {
        //aakashService.Service logService = new aakashService.Service();
     //   logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        //query database using sql client - google

        SqlConnection conn;
        int intCount = 1;
        string strSub = "";
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                            "</REQUEST>";


        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (client_id != "" & client_id != "0")
            {
                strSub = " and c.client_id=" + client_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select c.client_id, c.client_name, c.pmo_id, c.business_type_id, cd.client_address1, cd.client_address2," +
                        "cd.client_city,cd.client_postal_code, cd.client_country, cd.client_phoneNumber, cd.client_faxNumber" +
                        " from ovms_clients as c join ovms_client_details as cd on c.client_id = cd.client_id  where c.active = 1 and cd.active = 1 " + strSub;
                    //  string strSql = "SELECT vm.vendor_name, vd.vendor_address1, vd.vendor_address2,vd.vendor_city,vd.vendor_postal_code,vd.vendor_country,vd.vendor_PhoneNumber,vd.vendor_FaxNumber FROM ovms_vendor_details as vd join ovms_vendors as vm on vd.vendor_id = vm.vendor_id where vm.vendor_id='" + client_id + "' and  vm.active=1 and vd.active=1 ";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    //xml_string += "<RESPONSE>";
                    int count = reader.FieldCount;
                    xml_string = xml_string + "<CLIENTS>";
                    //for (int j = 0; j < reader.; j++)
                    while (reader.Read())
                    {
                        //reader.Read();


                        xml_string = xml_string + "<CLIENT ID=\"" + intCount + "\">" +
                                   "<CLIENT_ID>" + reader.GetValue(0).ToString() + "</CLIENT_ID>" +
                        "<CLIENT_NAME><![CDATA[" + reader.GetValue(1).ToString() + "]]></CLIENT_NAME>" +
                                   "<cLIENT_ADDRESS1><![CDATA[" + reader.GetValue(2).ToString() + "]]></cLIENT_ADDRESS1>" +
                                   "<cLIENT_ADDRESS2><![CDATA[" + reader.GetValue(3).ToString() + "]]></cLIENT_ADDRESS2>" +
                                   "<CLIENT_CITY>" + reader.GetValue(4).ToString() + "</CLIENT_CITY>" +
                                   "<CLIENT_POSTAL_CODE>" + reader.GetValue(5).ToString() + "</CLIENT_POSTAL_CODE>" +
                                   "<CLIENT_COUNTRY>" + reader.GetValue(6).ToString() + "</CLIENT_COUNTRY>" +
                                   "<CLIENT_PHONENUMBER>" + reader.GetValue(7).ToString() + "</CLIENT_PHONENUMBER>" +
                                   "<CLIENT_FAXNUMBER>" + reader.GetValue(8).ToString() + "</CLIENT_FAXNUMBER>" + "</CLIENT>";
                        intCount++;
                    }
                    xml_string = xml_string + "</CLIENTS>" + "\n\n";
                    cmd.Dispose();
                    reader.Dispose();
                }

            }

            catch (Exception ex)
            {
                //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<RESPONSE_MESSAGE> Unable to select client</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        //    }

        //else
        //    {
        //    xml_string += "<CLIENT_ID>client id should not be null</CLINT_ID>";
        //    }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument insert_client(string userEmailId, string userPassword, string clientname, string pm_id, string business_type_id, string caddress1, string caddress2, string ccity, string cpostal_code, string ccountry, string cphonenumber, string cfaxnumber)
    {
        //aakashService.Service logService = new aakashService.Service();
        //logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        string xml_string = "";
        int newclient_id = 0;
        string errString = "";
        //query database using sql client - google

        xml_string = "<XML>" +
                "<REQUEST>" +
                "<CLIENT_NAME><![CDATA[" + clientname + " ]]></CLIENT_NAME>" +
                "<CLIENT_PM_ID>" + pm_id + "</CLIENT_PM_ID>" +
                "<CLIENT_BUSINESS_TYPE>" + business_type_id + "</CLIENT_BUSINESS_TYPE>" +
                "<CLIENT_ADDRESS1><![CDATA[" + caddress1 + " ]]></CLIENT_ADDRESS1>" +
                "<CLIENT_ADDRESS2><![CDATA[" + caddress2 + "]]></CLIENT_ADDRESS2>" +
                "<CLIENT_CITY>" + ccity + "</CLIENT_CITY>" +
                "<CLIENT_POSTAL_CODE>" + cpostal_code + "</CLIENT_POSTAL_CODE>" +
                "<CLIENT_COUNTRY>" + ccountry + "</CLIENT_COUNTRY>" +
                "<CLIENT_PHONENUMBER>" + cphonenumber + "</CLIENT_PHONENUMBER>" +
                "<CLIENT_FAXNUMBER>" + cfaxnumber + "</CLIENT_FAXNUMBER>" +
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


                        string sql = "insert into ovms_clients (client_name, pm_id, business_type_id)values('" + clientname + "','" + pm_id + "','" + business_type_id + "') select cast(scope_identity() as int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        //using (sqlcommand cmd = new sqlcommand(sql, conn))
                        //{

                        newclient_id = (int)cmd.ExecuteScalar();

                        if (newclient_id > 0)
                        {
                            xml_string += "<CLIENT_NAME>" + clientname + "</CLIENT_NAME>" +
                            "<CLIENT_PM_ID>" + pm_id + "</CLIENT_PM_ID>" +
                             "<CLIENT_BUSINESS_TYPE>" + business_type_id + "</CLIENT_BUSINESS_TYPE>";

                        }
                        else
                        {
                            xml_string += "<CLIENT_NAME>client not inserted</CLIENT_NAME>";
                        }
                        sql = "insert into ovms_client_details(client_id, client_address1, client_address2, client_city, client_postal_code, client_country, client_phonenumber, client_faxnumber)" +
                            "values('" + newclient_id + "', '" + caddress1 + "', '" + caddress2 + "', '" + ccity + "', '" + cpostal_code + "', '" + ccountry + "', '" + cphonenumber + "', '" + cfaxnumber + "')";

                        cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<CLIENT_ADDRESS1><![CDATA[" + caddress1 + "]]></CLIENT_ADDRESS1>" +
                                    "<CLIENT_ADDRESS2><![CDATA[" + caddress2 + "]]></CLIENT_ADDRESS2>" +
                                    "<CLIENT_CITY>" + ccity + "</CLIENT_CITY>" +
                                    "<CLIENT_POSTAL_CODE>" + cpostal_code + "</CLIENT_POSTAL_CODE>" +
                                    "<CLIENT_COUNTRY>" + ccountry + "</CLIENT_COUNTRY>" +
                                    "<CLIENT_PHONENUMBER>" + cphonenumber + "</CLIENT_PHONENUMBER>" +
                                    "<CLIENT_FAXNUMBER>" + cfaxnumber + "</CLIENT_FAXNUMBER>";
                        }
                        else
                        {
                            xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                            xml_string += "<INSERT_STRING>client not inserted</INSERT_STRING>";
                            //Error//logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to create new vendor");
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>client not inserted</INSERT_STRING>";
                    //Error//logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument update_Client(string userEmailId, string userPassword, int ClientID, string ClientName, string pm_id, string business_type_id, string Caddress1, string Caddress2, string Ccity, string Cpostal_code, string Ccountry, string CPhoneNumber, string CFaxNumber)
    {
        //aakashService.Service logService = new aakashService.Service();
      ///  logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        string xml_string = "";
        string strSql1 = "";
        string errString = "";

        xml_string = "<XML>" +
            "<REQUEST>" +
                "<Client_ID>" + ClientID + "</Client_ID>" +
            "</REQUEST><RESPONSE>";

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
                    int FStatus = 0;
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();


                        if (ClientName != "")
                        {
                            string strSql = "update ovms_clients set client_name ='" + ClientName + "',pm_id ='" + pm_id + "',business_type_id ='" + business_type_id + "' where client_id =  '" + ClientID + "' and active=1";
                            SqlCommand cmd = new SqlCommand(strSql, conn);


                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                xml_string += "<RESPONSE_MESSAGE>saved successfully</RESPONSE_MESSAGE>";
                            }
                        }
                        strSql1 = "update ovms_client_details set ";
                        if (Caddress1 != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_address1='" + Caddress1 + "'";
                            FStatus = 1;
                        }

                        if (Caddress2 != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_address2 ='" + Caddress2 + "'";
                            FStatus = 1;
                        }
                        if (Ccity != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_city='" + Ccity + "'";
                            FStatus = 1;
                        }
                        if (Cpostal_code != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_postal_code ='" + Cpostal_code + "'";
                            FStatus = 1;
                        }
                        if (Ccountry != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_country ='" + Ccountry + "'";
                            FStatus = 1;
                        }

                        if (CPhoneNumber != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_phoneNumber = '" + CPhoneNumber + "'";
                        }


                        if (CFaxNumber != "")
                        {
                            strSql1 += (FStatus == 1 ? "," : "") + " client_faxNumber = '" + CFaxNumber + "'";
                        }

                        strSql1 += " where client_id = " + ClientID;


                        SqlCommand cmd1 = new SqlCommand(strSql1, conn);


                        if (cmd1.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<RESPONSE_MESSAGE>saved successfully</RESPONSE_MESSAGE>";
                        }
                        // SqlCommand cmd = new SqlCommand(strSql1, conn);
                        else
                        {
                            xml_string += "<UPDATE_STRING>client not updated</UPDATE_STRING>" +
                                        "<UPDATE_STRING>0</UPDATE_STRING>";
                            //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update client");
                        }

                    }

                }

                catch (Exception ex)
                {
                    //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        //output final
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument delete_Client(string userEmailId, string userPassword, string ClientID)
    {
        //aakashService.Service logService = new aakashService.Service();
       //logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        string xml_string = "";
        string errString = "";
        //query database using sql client - google

        xml_string = "<XML>" +
                        "<REQUEST>" +
                            "<Client_ID>" + ClientID + "</Client_ID>" +
                        "</REQUEST>";

        xml_string += "<RESPONSE>";

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

                    }
                    else
                    {
                        xml_string += "<DATA>no records found</DATA>";
                    }

                    string strSql = "update ovms_clients set  active=0 where client_id ='" + ClientID + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>deleted from Client master successfully</RESPONSE_MESSAGE>";
                    }

                    string strSql1 = "update ovms_client_details set  active=0 where client_id = '" + ClientID + "'";

                    SqlCommand cmd1 = new SqlCommand(strSql1, conn);
                    if (cmd1.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>deleted from client details successfully</RESPONSE_MESSAGE>";
                    }
                    // SqlCommand cmd = new SqlCommand(strSql1, conn);
                    else
                    {
                        xml_string += "<DELETE_STRING>client not deleted</DELETE_STRING>" +
                                    "<DELETE_VALUE>0</DELETE_VALUE>";
                        //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete client");
                    }
                }

                catch (Exception ex)
                {
                    //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        //output final
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }
    [WebMethod]
    public XmlDocument get_Recent_jobs(string userEmailId, string userPassword, string utype_id, string User_id, string vendorID)
    {
       // aakashService.Service logService = new aakashService.Service();
       // logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        string errString = "";
        //int intCount = 0;
        SqlConnection conn;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<UTYPE_ID>" + utype_id + "</UTYPE_ID>" +
                                 "<USER_ID>" + User_id + "</USER_ID>" +
                                 "<VENDORID>" + vendorID + "</VENDORID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE><ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (utype_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        //string strSQL = "Select j.job_id,concat(c.client_alias,'000',right('0000'+convert(varchar(4),j.job_id),4)) as client_alias,j.job_title,j.no_of_openings, jd.job_location, DATEDIFF(day,posting_start_date, posting_end_date) as Length_Of_Posting, dbo.GetDateDifference(j.create_date) as createDate  from ovms_clients as c join ovms_jobs as j on c.client_id = j.client_id join ovms_users as u on c.client_id = u.client_id join ovms_vendors as v on v.vendor_id = u.vendor_id join ovms_job_details as jd on j.job_id = jd.job_id where u.utype_id = 2 and u.User_id=v.vendor_id and j.vendor_id=1 and v.active=1 and c.active=1 and j.active=1 and u.active=1 and v.active=1 order by j.create_date desc";
                        //string strSQL = "Select top 5 j.job_id,dbo.GetClientNo(j.job_id) as client_alias,j.job_title," +
                        //    "j.no_of_openings,concat(jl.address1, ', ', jl.post_code, ', ', jl.city, ', ', jl.province, ', ', jl.country) job_location, " +
                        //    "DATEDIFF(day, posting_start_date, posting_end_date) as Length_Of_Posting, " +
                        //    "dbo.GetDateDifference(j.create_date) as createDate  from ovms_clients as c" +
                        //    " join ovms_jobs as j on c.client_id = j.client_id" +
                        //    " join ovms_users as u on c.client_id = u.client_id" +
                        //    " join ovms_vendors as v on v.vendor_id = u.vendor_id" +
                        //    " join ovms_job_details as jd on j.job_id = jd.job_id" +
                        //    " join ovms_job_locations as jl on jd.job_location_id = jl.job_location_id" +
                        //    " where u.utype_id = "+ utype_id +" and u.user_id="+ User_id +" and u.User_id = v.vendor_id and j.vendor_id = 1 and v.active = 1 and c.active = 1 and j.active = 1" +
                        //    " and u.active = 1 and v.active = 1 order by j.create_date desc";

                        string strSQL = "Select top 5 j.job_id,dbo.GetClientNo(j.job_id) as client_alias,j.job_title," +
                            "j.no_of_openings,concat(jl.city, ', ', jl.province) job_location, " +
                            "DATEDIFF(day, posting_start_date, posting_end_date) as Length_Of_Posting, " +
                            "dbo.GetDateDifference(j.create_date) as createDate  from ovms_clients as c" +
                            " join ovms_jobs as j on c.client_id = j.client_id" +
                            " join ovms_users as u on c.client_id = u.client_id" +
                            " join ovms_vendors as v on v.vendor_id = u.vendor_id" +
                            " join ovms_job_details as jd on j.job_id = jd.job_id" +
                            " join ovms_job_locations as jl on jd.job_location_id = jl.job_location_id";
                        if (vendorID != "")
                        {
                            strSQL = strSQL + " where u.utype_id = " + utype_id + " and u.user_id=" + User_id + " and j.vendor_id = " + vendorID + " and v.active = 1 and c.active = 1 and j.active = 1";
                        }
                        else
                        {
                            strSQL = strSQL + " where u.utype_id = " + utype_id + " and u.user_id=" + User_id + " and v.active = 1 and c.active = 1 and j.active = 1";
                        }
                        //" where u.utype_id = " + utype_id + " and u.user_id=" + User_id + " and j.vendor_id = "+ vendorID + " and v.active = 1 and c.active = 1 and j.active = 1" +
                        strSQL = strSQL + " and u.active = 1 and v.active = 1 order by j.create_date desc";
                        //" where u.utype_id = " + utype_id + " and u.user_id=" + User_id + " and u.User_id = v.vendor_id and j.vendor_id = "+ vendorID + " and v.active = 1 and c.active = 1 and j.active = 1" +
                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        //xml_string = xml_string + "<JOBS>";
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            //for (int j = 0; j < 4; j++)
                            {
                                //reader.Read();
                                // xml_string = xml_string + "<JOB_ALIAS>" + "<JOB_DESCRIPTION>" + "<CREATE_DATE>";
                                //for (int i = 0; i < count; i++)

                                xml_string = xml_string + "<JOB_ID ID=\"" + reader.GetValue(0).ToString() + "\">" +
                                    "<RESPONSE_MESSAGE>YES</RESPONSE_MESSAGE>" +
                                    "<JOB_ALIAS>" + reader.GetValue(1).ToString() + "</JOB_ALIAS>" +
                                    "<JOB_TITLE><![CDATA[" + reader.GetValue(2).ToString() + "]]></JOB_TITLE>" +
                                    "<NO_OF_OPENINGS>" + reader.GetValue(3).ToString() + "</NO_OF_OPENINGS>" +
                                    "<LOCATION>" + reader.GetValue(4).ToString() + "</LOCATION>" +
                                    "<LENGTH_OF_DAYS>" + reader.GetValue(5).ToString() + "</LENGTH_OF_DAYS>" +
                                    "<DATE_CREATED>" + reader.GetValue(6).ToString() + "</DATE_CREATED>" + "</JOB_ID>";
                                //if (intCount >= 4)
                                //    break;
                                //++intCount;
                            }
                        }
                        else
                        {
                            xml_string = xml_string + "<JOB_ID ID=\"0\"><RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE></JOB_ID>";
                        }
                        cmd.Dispose();
                        reader.Dispose();
                        //xml_string = xml_string + "</JOBS>"  + "\n\n";
                    }

                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<JOB_ID ID=\"0\"><RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE></JOB_ID>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
            else
            {
                xml_string += "<RESPONSE_MESSAGE>NO</RESPONSE_MESSAGE><UTYPE_ID>Vendor id should not be null</UTYPE_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument update_TimeSheet_Status(string userEmailId, string userPassword, string job_id, string employee_id, string client_id, string timesheetStausID, string timesheetID)

    {
        //aakashService.Service logService = new aakashService.Service();
    //    logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();
        string xml_string = "";
        string strSql1 = "";
        string errString = "";

        xml_string = "<XML>" +
            "<REQUEST>" +
                "<Client_ID>" + client_id + "</Client_ID>" +
                "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                "<JOB_ID>" + job_id + "</JOB_ID>" +
                "<TIMESHEET_ID>" + timesheetID + "</TIMESHEET_ID>" +
                "<TIMESHEET_STATUS_ID>" + timesheetStausID + "</TIMESHEET_STATUS_ID>" +
            "</REQUEST><RESPONSE>";

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
                    int FStatus = 0;
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();


                        string strSql = "select e.employee_id,e.client_id,e.job_id, td.timesheet_status_id from ovms_employees as e" +
                                        " join ovms_timesheet as t" +
                                        " on t.employee_id = e.employee_id" +
                                        " join ovms_timesheet_details as td" +
                                        " on td.timesheet_id = t.timesheet_id" +
                                        " where e.employee_id = '" + employee_id + "' and e.job_id = '" + job_id + "' and e.active = 1 and t.active = 1 and td.active = 1";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<RESPONSE_MESSAGE>saved successfully</RESPONSE_MESSAGE>";
                        }
                    }
                    strSql1 = "update ovms_timesheet_details set ";
                    if (timesheetStausID != "")
                    {
                        strSql1 += (FStatus == 1 ? "," : "") + " timesheet_status_id='" + timesheetStausID + "'";
                        FStatus = 1;
                    }
                    strSql1 += " where timesheet_id = " + timesheetID;


                    SqlCommand cmd1 = new SqlCommand(strSql1, conn);


                    if (cmd1.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>Timesheetstatus changed successfully</RESPONSE_MESSAGE>";
                    }
                    // SqlCommand cmd = new SqlCommand(strSql1, conn);
                    else
                    {
                        xml_string += "<UPDATE_STRING>status not updated</UPDATE_STRING>" +
                                    "<UPDATE_STRING>0</UPDATE_STRING>";
                        //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update timesheet Status");
                    }

                }


                catch (Exception ex)
                {
                    //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        //output final
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument update_TimeSheet(string userEmailId, string userPassword, string employeeID, string day, string month, string year, string hours)
    {
        //aakashService.Service logService = new aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        string xml_string = "";
        string errString = "";

        xml_string = "<XML>" +
            "<REQUEST>" +
                "<EMPLOYEE_ID>" + employeeID + "</EMPLOYEE_ID>" +
            "</REQUEST><RESPONSE>";

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
                        // update table ovms_timesheet
                        if (hours != "")
                        {
                            string strSql = "update ovms_timesheet set hours='" + hours + "' where day ='" + day + "' and month = '" + month + "' and year = '" + year + "' and active=1";
                            SqlCommand cmd = new SqlCommand(strSql, conn);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                xml_string += "<TIMESHEET_MESSAGE>TimeSheet saved successfully</TIMESHEET_MESSAGE>";
                            }
                        }
                        else
                        {
                            xml_string += "<UPDATE_STRING>Timesheet not updated</UPDATE_STRING>" +
                                        "<UPDATE_STRING>0</UPDATE_STRING>";
                            //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update TimeSheet");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        // output final
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument Show_Employee_Reports(string userEmailId, string userPassword, string Vendor_id)
    {
        //aakashService.Service logService = new aakashService.Service();
     //   logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>"
                            + "<USER_ID>" + Vendor_id + "</USER_ID></REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (Vendor_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string strSQL = "select concat('WOPUS','000',right('0000'+convert(varchar(4),e.employee_id),4))as Employee_ID, ed.first_name +' '+ ed.last_name as Employee_Name, j.job_id, ed.start_date, ed.end_date, j.job_title, e.active" +
                                         "   from ovms_employees as e" +
                                          "  join ovms_employee_details as ed" +
                                          "  on e.employee_id = ed.employee_id" +
                                          "  join ovms_jobs as j" +
                                          "  on e.job_id = j.job_id" +
                                         "   where e.vendor_id = '" + Vendor_id + "' and e.active = 1";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;


                        while (reader.Read())
                        {
                            xml_string = xml_string + "<REPORTS ID=\"" + intCount + "\"> <EMPLOYEE_ID>" + reader["Employee_ID"] + "</EMPLOYEE_ID>" +
                                "<EMPLOYEE_NAME>" + reader["Employee_Name"] + "</EMPLOYEE_NAME >" +
                                "<JOB_ID>" + reader["job_id"] + "</JOB_ID >" +
                                "<JOB_TITLE>" + reader["job_title"] + "</JOB_TITLE >" +
                                "<START_DATE>" + reader["start_date"] + "</START_DATE >" +
                                "<END_DATE>" + reader["end_date"] + "</END_DATE >" +
                                " <ACTIONS> " + reader["active"] + " </ACTIONS></REPORTS>";
                            intCount++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select employee reports for vednor</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>Unable to select employee reports for vendor</VENDOR_ID>";
            }
        }
        //output final
        xml_string += " </RESPONSE> </XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }



    [WebMethod]
    public XmlDocument get_Message(string userEmailId, string userPassword, string Vendor_id)
    {
        //aakashService.Service logService = new aakashService.Service();
        //logAPI.Service ErrorlogService = new logAPI.Service();
        //Service service = new Service();

        SqlConnection conn;
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (Vendor_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select m.Msg_Subject, m.message from ovms_messages as m join ovms_pmo  as pmo on m.vendor_id = pmo.vendor_id";
                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<EMAIL_MESSAGE>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<VENDOR_SUBJECT>" + reader["Msg_Subject"] + "</VENDOR_SUBJECT >" +
                               "<VENDOR_MESSAGE>" + reader["message"] + "</VENDOR_MESSAGE>";
                        }
                        xml_string = xml_string + "</EMAIL_MESSAGE>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select vendor id</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>Vendor id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Select_Jobs_For_BarChart(string userEmailId, string userPassword)
    {
        //aakashService.Service logService = new aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" +

                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            //if (job_id != "0")
            //    {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    string strSQL = "Select * from (select count(job_id) as total_Jobs from ovms_jobs where active = 1 or active = 0)  x, " +
                                    "(select count(job_id) as jobs_Worked_On from ovms_jobs where active = 1)  y, " +
                                    " (select count(job_id) as jobs_NOT_Worked_On from ovms_jobs where active = 0)  z";

                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = reader.FieldCount;


                    while (reader.Read())
                    {
                        xml_string = xml_string + "<JOBS><ALL_JOBS> " + reader[0] + "</ALL_JOBS>" +
                              "<JOBS_WORKED_ON>" + reader[1] + "</JOBS_WORKED_ON>" +
                              "<JOBS_NOT_WORKED_ON>" + reader[2] + "</JOBS_NOT_WORKED_ON></JOBS>";
                        intCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<RESPONSE_MESSAGE> Unable to select job id</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            //}

            //else
            //    {
            //    xml_string += "<JOB_ID>Unable to select job id</JOB_ID>";
            //    }
        }
        //output final
        xml_string += " </RESPONSE> </XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }



    [WebMethod]
    public XmlDocument Get_Job_Questions(string userEmailId, string userPassword, string job_id, string Vendor_id)
    {
       // aakashService.Service logService = new aakashService.Service();
       // logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>"
                            + "<JOB_ID>" + job_id + "</JOB_ID>" +
                            " <VENDOR_ID> " + Vendor_id + "</VENDOR_ID></REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (job_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string strSQL = "select Question_ID, Question, Rating, Worker_ID, Vendor_ID, Client_ID, User_ID,job_id, Create_DateTime from ovms_job_question_rating where job_id = '" + job_id + "' and Vendor_ID='" + Vendor_id + "'";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;


                        while (reader.Read())
                        {
                            xml_string = xml_string + "<QUESTIONS ID=\"" + reader["Question_ID"] + "\">" +
                                " <QUESTION> " + reader["Question"] + " </QUESTION> " +
                                "<RATING>" + reader["Rating"] + "</RATING></QUESTIONS> ";
                            intCount++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select questions</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<QUESTION_ID>Unable to select questions</QUESTION_ID>";
            }
        }
        //output final
        xml_string += " </RESPONSE> </XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    // change condition client and vendor
    public XmlDocument Send_Message_PMO(string userEmailId, string userPassword, string Msg_Subject, string user_id, string message, string pmo_id, string Vendor_id, string client_id, string Actions)
    {
       ////
        string xml_string = "";
        string strVendorID = "";
        string strPMOID = "";
        string strUserID = "";
        string sql = "";
        string errString = "";
        //  int newclient_id = 0;
        //query database using sql client - google
        strVendorID = Vendor_id.ToString();
        strPMOID = pmo_id.ToString();
        strUserID = client_id.ToString();

        xml_string = "<XML>" +
                     "<REQUEST>" +
                     "<PMO_ID>" + pmo_id + "</PMO_ID></REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (Actions == "Send_P_C")
            {
                strVendorID = "null";
                sql = "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id,client_id, Actions,IsRead,IsSend)" +
                                      "values('" + pmo_id + "', '" + message + "'," + strVendorID + ", '" + Msg_Subject + "'," + user_id + "," + strUserID + ", '" + Actions + "','" + 0 + "', '" + 1 + "');";

                // strUserID = "null";
                //vendor_id = 0;
            }
            if (Actions == "Send_P_V")
            {
                //pmo_id = 0;

                strUserID = "null";
                sql = "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id,client_id, Actions,IsRead,IsSend)" +
                                     "values('" + pmo_id + "', '" + message + "'," + strVendorID + ", '" + Msg_Subject + "'," + user_id + "," + strUserID + ", '" + Actions + "','" + 0 + "', '" + 1 + "');";


                // strPMOID = "null";
            }
            if (Actions == "Send_P_C , Send_P_V")
            {
                sql = "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id,client_id, Actions,IsRead,IsSend)" +
                                     "values('" + pmo_id + "', '" + message + "'," + strVendorID + ", '" + Msg_Subject + "','" + user_id + "'," + strUserID + ", 'Send_P_V','" + 0 + "', '" + 1 + "');" +
                                 "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id,client_id, Actions,IsRead,IsSend)" +
                                     "values('" + pmo_id + "', '" + message + "'," + strVendorID + ", '" + Msg_Subject + "','" + user_id + "'," + strUserID + ", 'Send_P_C','" + 0 + "', '" + 1 + "');";

            }

            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();


                        //string sql = "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id,client_id, Actions,IsRead,IsSend)" +
                        //             "values('" + pmo_id + "', '" + message + "'," + strVendorID + ", '" + Msg_Subject + "','" + User_id + "'," + strUserID + ", '" + Actions + "','" + 0 + "', '" + 1 + "')";
                        //insert into ovms_clients (client_name, pm_id, business_type_id)values('" + Msg_Subject + "','" + message + "') select cast(scope_identity() as int)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            xml_string += "<SUBJECT>" + Msg_Subject + "</SUBJECT>" +
                                          "<MESSAGE>" + message + "</MESSAGE>" +
                                          "<VENDOR_ID>" + strVendorID + "</VENDOR_ID>" +
                                          "<USER_ID>" + user_id + "</USER_ID>" +
                                          "<CLIENT_ID>" + strUserID + "</CLIENT_ID>" +
                                          "<IS_SEND>" + 1 + "</IS_SEND>" +
                                          "<IS_READ>" + 0 + "</IS_READ>" +
                                          "<ACTIONS>" + Actions + "</ACTIONS>";

                        }
                        else
                        {
                            xml_string += "<PMO_ID>Pmo cannot inserted message</PMO_ID>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>PMO not inserted</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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

    [WebMethod]//change condition if pmo want to reply particular id
    public XmlDocument Reply_From_PMO(string userEmailId, string userPassword, string user_id, string message_id, string message, string pmo_id, string Vendor_id, string client_id, string Actions, string Msg_Subject)
    {
//logAPI.Service logService = new logAPI.Service();
        string xml_string = "";
        string errString = "";
        // int new_Reply = 0;
        //query database using sql client - google

        xml_string = "<XML>" +

                    "<REQUEST>" +
                    "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                    "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                    //"<SUBJECT>" + Msg_Subject + "</SUBJECT>" +
                    "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
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

                        //{ }
                        if (pmo_id != "0")
                        {
                            string sql = "insert into ovms_messages(pmo_id, message, vendor_id, Msg_Subject, SourceID, User_id, client_id,IsSend,IsRead)" +
                                " values('" + pmo_id + "','" + message + "','" + Vendor_id + "', (select Msg_Subject from ovms_messages where message_id = '" + message_id + "'), '" + message_id + "', '" + user_id + "','" + client_id + "', 1,0) EXEC dbo.ReplaceAction '" + message_id + "'";

                            SqlCommand cmd = new SqlCommand(sql, conn);
                            sql = "select Actions from ovms_messages where message_id='" + message_id + "'";


                            if (cmd.ExecuteNonQuery() >= 0)
                            {
                                xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                                                "<SOURCE_ID>" + message_id + "</SOURCE_ID>" +
                                                "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
                                                 "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                                                   "<USER_ID>" + user_id + "</USER_ID>" +
                                                "<MESSAGE>" + message + "</MESSAGE>" +
                                                 "<MESSAGE_SUBJECT>" + Msg_Subject + "</MESSAGE_SUBJECT>" +
                                                "<ACTIONS>" + Actions + "</ACTIONS>";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>PMO Could not able to reply for message</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument get_All_Messages_For_PMO(string userEmailId, string userPassword, string pmo_id)
    {
      // //

        SqlConnection conn;
        string strSub = "";
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (pmo_id != "" & pmo_id != "0")
            {
                strSub = " and msg.pmo_id=" + pmo_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSQL = "select msg.message_id, msg.pmo_id,msg.client_id,pmo.First_name+pmo.Last_Name as PMO_Name,  msg.User_id,msg.vendor_id, msg.Msg_Subject, msg.message, dbo.GetDateDifference(msg.create_date) as createDate , msg.IsRead" +
                                    " from ovms_messages as msg" +
                                    " join ovms_pmo as pmo" +
                                    " on msg.pmo_id = '" + pmo_id + "' where pmo.pmo_id = '" + pmo_id + "' and (Actions='Send_C_P' or Actions='Send_V_P')" +
                                    " order by createDate asc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;


                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(0).ToString() + "</MESSAGE_ID>" +
                                                   "<PMO_ID>" + reader.GetValue(1).ToString() + "</PMO_ID>" +
                                                    "<PMO_NAME>" + reader.GetValue(3).ToString() + "</PMO_NAME>" +
                                                    "<CLIENT_ID>" + reader.GetValue(2).ToString() + "</CLIENT_ID>" +
                                                     "<VENDOR_ID>" + reader.GetValue(5).ToString() + "</VENDOR_ID>" +
                                                    "<MESSAGE_SUBJECT>" + reader.GetValue(6).ToString() + "</MESSAGE_SUBJECT>" +
                                                    "<MESSAGE>" + reader.GetValue(7).ToString() + "</MESSAGE>" +
                                                    "<DATE>" + reader.GetValue(8).ToString() + "</DATE>" +
                                                  "<USER_ID>" + reader.GetValue(4).ToString() + "</USER_ID>" +
                                                      "<IS_READ>" + reader.GetValue(9).ToString() + "</IS_READ>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<RESPONSE_MESSAGE> Unable to select all messages for PMO id</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        //}

        //else
        //    {
        //    xml_string += "<UTYPE_ID>Vendor id should not be null</UTYPE_ID>";
        //    }
        //output final
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }


    [WebMethod]// Done
    public XmlDocument get_All_Messages_for_Vendor(string userEmailId, string userPassword, string Vendor_id)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        //string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            //if (Vendor_id != "" & Vendor_id != "0")
            //    {
            //        strSub = " and msg.Vendor_id=" + Vendor_id;
            //    }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSQL = "select msg.message_id, v.vendor_name,msg.employee_id,msg.pmo_id,msg.User_id, msg.Msg_Subject, msg.message, dbo.GetDateDifference(msg.create_date) as createDate, msg.IsRead ,p.First_name + ' '+p.Last_Name as PMOName, msg.Actions" +
                                  "  from ovms_messages as msg" +
                                  "  join ovms_pmo as p" +
                                  "  on msg.pmo_id = p.pmo_id" +
                                  "  join ovms_vendors as v" +
                                  "  on msg.vendor_id = v.vendor_id and msg.Vendor_id = '" + Vendor_id + "' and (Actions = 'Send_E_V' or Actions = 'Send_V_E' or Actions = 'Send_V_P' or Actions = 'Send_P_V' or Actions = 'Send_C_V') and SourceID=0" +
                                  "  order by createDate desc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;


                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(0).ToString() + "</MESSAGE_ID>" +
                                                  "<VENDOR_NAME>" + reader.GetValue(1).ToString() + "</VENDOR_NAME>" +
                                                  "<MESSAGE_SUBJECT>" + reader.GetValue(5).ToString() + "</MESSAGE_SUBJECT>" +
                                                  "<MESSAGE>" + reader.GetValue(6).ToString() + "</MESSAGE>" +
                                                  "<DATE>" + reader.GetValue(7).ToString() + "</DATE>" +
                                                    "<IS_READ>" + reader.GetValue(8).ToString() + "</IS_READ>" +
                                                      "<PMO_NAME>" + reader.GetValue(9).ToString() + "</PMO_NAME>" +
                                                       "<ACTIONS>" + reader.GetValue(10).ToString() + "</ACTIONS>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<RESPONSE_MESSAGE> Unable to select all messages for vendor id</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //}

        //else
        //    {
        //    xml_string += "<UTYPE_ID>Vendor id should not be null</UTYPE_ID>";
        //    }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Add_TimeSheet(string userEmailId, string userPassword, string employee_id)
    {
       // aakashService.Service logService = new aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
       Service service = new Service();
        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>"
                            + "<JOB_ID>" + employee_id + "</JOB_ID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (employee_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        //get employeeID from userID from users table as employee is already inserted in user table
                        //with flag = 4 meaning employee

                        string strSQLGetEmployeeID = "select employee_id,job_id, user_id from ovms_employees where user_id =" + employee_id;

                        SqlCommand cmdGetEmployeeID = new SqlCommand(strSQLGetEmployeeID, conn);
                        SqlDataReader readerGetEmployeeID = cmdGetEmployeeID.ExecuteReader();
                        int countEmpID = readerGetEmployeeID.FieldCount;

                        string employeeIDFromUserID = "";
                        string employeeJOBID = "";
                        while (readerGetEmployeeID.Read())
                        {
                            employeeIDFromUserID = readerGetEmployeeID["employee_id"].ToString();
                            employeeJOBID = readerGetEmployeeID["job_id"].ToString();
                        }
                        readerGetEmployeeID.Close();
                        cmdGetEmployeeID.Dispose();

                        //string strSQL = "select start_date,DATEDIFF(WEEK,start_date, end_date) AS DiffDate, end_date from ovms_employee_details where employee_id=" + employeeIDFromUserID;
                        string strSQL = "select contract_start_date, DATEDIFF(WEEK, contract_start_date, contract_end_date) AS DiffDate, contract_end_date from ovms_jobs  where job_id = " + employeeJOBID;

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        while (reader.Read())
                        {
                            xml_string = xml_string + "<TIMESHEET>" +
                                " <START_DATE> " + reader["contract_start_date"] + " </START_DATE> " +
                                " <WEEKS> " + reader["DiffDate"] + " </WEEKS> " +
                                "<END_DATE>" + reader["contract_end_date"] + "</END_DATE>" +
                                "<JOB_ID>" + employeeJOBID + " </JOB_ID>" +
                                "</TIMESHEET> ";
                            intCount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select questions</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<QUESTION_ID>Unable to select questions</QUESTION_ID>";
            }
        }
        //output final
        xml_string += " </RESPONSE> </XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }


    [WebMethod]
    public XmlDocument Send_Message_Vendor(string userEmailId, string userPassword, string user_id, string Msg_Subject, string message, string pmo_id, string Vendor_id, string client_id, string Employee_id, string Actions)
    {
        //logAPI.Service logService = new logAPI.Service();
        string xml_string = "";
        // string strVendorID = "";
        string strPMOID = "";
        string strUserID = "";
        string strempID = "";
        string errString = "";
        //  int newclient_id = 0;
        //query database using sql client - google
        xml_string = "<XML>" +
                    "<REQUEST>" +
                    "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID></REQUEST>";
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

                        strUserID = client_id.ToString();
                        strPMOID = pmo_id.ToString();
                        strempID = Employee_id.ToString();
                        if (Actions == "Send_V_C")
                        {
                            strPMOID = "null";
                            strempID = "null";

                        }
                        if (Actions == "Send_V_P")
                        {
                            strempID = "null";
                            strUserID = "null";

                        }
                        if (Actions == "Send_V_E")
                        {
                            strUserID = "null";
                            strPMOID = "null";
                        }
                        string sql = "insert into ovms_messages(vendor_id,message,pmo_id,employee_id,User_id,Msg_Subject,client_id, Actions,IsRead,IsSend)" +
                                      "values('" + Vendor_id + "','" + message + "'," + strPMOID + "," + strempID + "," + user_id + ",'" + Msg_Subject + "'," + strUserID + ",'" + Actions + "',0,1); ";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            xml_string += "<DATA>MESSAGE_INSERTED</DATA><VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
                                "<MESSAGE_SUBJECT>" + Msg_Subject + "</MESSAGE_SUBJECT>" +
                                          "<MESSAGE>" + message + "</MESSAGE>" +
                                          "<PMO_ID>" + strPMOID + "</PMO_ID>" +
                                          "<EMPLOYEE_ID>" + strempID + "</EMPLOYEE_ID>" +
                                           "<USER_ID>" + user_id + "</USER_ID>" +
                                           "<CLIENT_ID>" + strUserID + "</CLIENT_ID>" +
                                          "<IS_SEND>" + 1 + "</IS_SEND>" +
                                          "<IS_READ>" + 0 + "</IS_READ>" +
                                          "<ACTIONS>" + Actions + "</ACTIONS>";
                        }
                        else
                        {
                            xml_string += "<VENDOR_ID>Vendor cannot able to insert message</VENDOR_ID>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Vendor cannot able to insert message</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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

    //public XmlDocument Send_Message_Vendor(string userEmailId, string userPassword, string Msg_Subject, string message, string pmo_id, string Vendor_id, string client_id, string Employee_id, string Actions)
    //{
    //   //
    //    string xml_string = "";
    //    // string strVendorID = "";
    //    string strPMOID = "";
    //    string strUserID = "";
    //    string strempID = "";
    //    string errString = "";
    //    //  int newclient_id = 0;
    //    //query database using sql client - google
    //    xml_string = "<XML>" +
    //                "<REQUEST>" +
    //                "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID></REQUEST>";
    //    xml_string = xml_string + "<RESPONSE>";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {
    //        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //            try
    //            {
    //                if (conn.State == System.Data.ConnectionState.Closed)
    //                {
    //                    conn.Open();

    //                    strUserID = client_id.ToString();
    //                    strPMOID = pmo_id.ToString();
    //                    strempID = Employee_id.ToString();
    //                    if (Actions == "Send_V_C")
    //                    {
    //                        strPMOID = "null";
    //                        strempID = "null";

    //                    }
    //                    if (Actions == "Send_V_P")
    //                    {
    //                        strempID = "null";
    //                        strUserID = "null";

    //                    }
    //                    if (Actions == "Send_V_E")
    //                    {
    //                        strUserID = "null";
    //                        strPMOID = "null";
    //                    }
    //                    string sql = "insert into ovms_messages(vendor_id,message,pmo_id,employee_id,User_id,Msg_Subject,client_id, Actions,IsRead,IsSend)" +
    //                                  "values('" + Vendor_id + "','" + message + "'," + strPMOID + "," + strempID + "," + Vendor_id + ",'" + Msg_Subject + "'," + strUserID + ",'" + Actions + "',0,1); ";
    //                    SqlCommand cmd = new SqlCommand(sql, conn);

    //                    if (cmd.ExecuteNonQuery() != 0)
    //                    {
    //                        xml_string += "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
    //                            "<MESSAGE_SUBJECT>" + Msg_Subject + "</MESSAGE_SUBJECT>" +
    //                                      "<MESSAGE>" + message + "</MESSAGE>" +
    //                                      "<PMO_ID>" + strPMOID + "</PMO_ID>" +
    //                                      "<EMPLOYEE_ID>" + strempID + "</EMPLOYEE_ID>" +
    //                                       //"<USER_ID>" + Vendor_id + "</USER_ID>" +
    //                                       "<CLIENT_ID>" + strUserID + "</CLIENT_ID>" +
    //                                      "<IS_SEND>" + 1 + "</IS_SEND>" +
    //                                      "<IS_READ>" + 0 + "</IS_READ>" +
    //                                      "<ACTIONS>" + Actions + "</ACTIONS>";
    //                    }
    //                    else
    //                    {
    //                        xml_string += "<VENDOR_ID>Vendor cannot able to insert message</VENDOR_ID>";
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
    //                xml_string += "<INSERT_STRING>Vendor cannot able to insert message</INSERT_STRING>";
    //                //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //            }
    //            finally
    //            {
    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }
    //        }
    //    }
    //    xml_string = xml_string + "</RESPONSE></XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}

    [WebMethod]
    public XmlDocument Send_Message_Employee(string userEmailId, string userPassword, string vendor_id, string employee_id, string Msg_Subject, string Actions, string message)
    {
    //    logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        // string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "insert into ovms_messages(vendor_id, message, employee_id, Msg_Subject, SourceID, User_id, IsSend,IsRead, Actions)" +
                            " values('" + vendor_id + "','" + message + "','" + employee_id + "','" + Msg_Subject + "',0,'" + employee_id + "',1,0,'" + Actions + "')";

                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            xml_string += "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                            "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                            "<SUBJECT>" + Msg_Subject + "</SUBJECT>" +
                                             "<SOURCE_ID>" + 0 + "</SOURCE_ID>" +
                                               //"<User_ID>" + employee_id + "</User_ID>" +
                                               "<IS_SEND>" + 1 + "</IS_SEND>" +
                                               "<IS_READ>" + 0 + "</IS_READ>" +
                                               "<ACTIONS>" + Actions + "</ACTIONS>" +
                                            "<MESSAGE>" + message + "</MESSAGE>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Message not sent from employee</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument Reply_from_Vendor(string userEmailId, string userPassword, string user_id, string message_id, string message, string vendor_id, string pmo_id, string client_id, string employee_id, string Actions, string MsgSubject)
    {
      //  logAPI.Service logService = new logAPI.Service();
        string xml_string = "";
        string errString = "";
        // int new_Reply = 0;
        //query database using sql client - google

        xml_string = "<XML>" +
                    "<REQUEST>" +
                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                    "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                    "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                    "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                    "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                    "<USER_ID>" + user_id + "</USER_ID></REQUEST>";
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
                        { }
                        string sql = "insert into ovms_messages(pmo_id, message, employee_id, Msg_Subject, SourceID, vendor_id, User_id,client_id, IsSend,IsRead)" +
                            " values('" + pmo_id + "','" + message + "','" + employee_id + "', (select Msg_Subject from ovms_messages where message_id = '" + message_id + "'), '" +
                            message_id + "', '" + vendor_id + "', '" + user_id + "','" + client_id + "', 1,0)  EXEC dbo.ReplaceAction '" + message_id + "';" +
                           //   "select (select newMsg.message_id from ovms_messages as newMsg where newMsg.source_id=oldMsg.message_id) as new_MessageID, oldMsg.create_date, oldMsg.Actions from ovms_messages as oldMsg where oldMsg.message_id='" + message_id + "'";
                           " select(select max(newMsg.message_id) from ovms_messages as newMsg where newMsg.SourceID = oldMsg.message_id) as new_MessageID," +
                         "  oldMsg.create_date, oldMsg.Actions from ovms_messages as oldMsg where oldMsg.message_id = '" + message_id + "'";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        //sql = "select create_date, Actions from ovms_messages where message_id='" + message_id + "'";

                        while (reader.Read())
                        {
                            xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                                            "<SOURCE_ID>" + message_id + "</SOURCE_ID>" +
                                            "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                             "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                                               "<NEW_MESSAGE_ID>" + reader["new_MessageID"] + "</NEW_MESSAGE_ID>" +
                                            "<MESSAGE>" + message + "</MESSAGE>" +
                                            "<MESSAGE_SUBJECT>" + MsgSubject + "</MESSAGE_SUBJECT>" +
                                            "<ACTIONS>" + Actions + "</ACTIONS>" +
                                            "<USER_ID>" + user_id + "</USER_ID>" +
                                            "<DATE>" + reader["create_date"] + "</DATE>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Vendor could not replied</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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

    //public XmlDocument Reply_from_Vendor(string userEmailId, string userPassword, string message_id, string message, string vendor_id, string pmo_id, string client_id, string employee_id, string Actions, string MsgSubject)
    //{
    //   //
    //    string xml_string = "";
    //    string errString = "";
    //    // int new_Reply = 0;
    //    //query database using sql client - google

    //    xml_string = "<XML>" +
    //                "<REQUEST>" +
    //                "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
    //                "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
    //                "<PMO_ID>" + pmo_id + "</PMO_ID>" +
    //                "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
    //                "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
    //                "<USER_ID>" + vendor_id + "</USER_ID></REQUEST>";
    //    xml_string = xml_string + "<RESPONSE>";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {
    //        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //            try
    //            {
    //                if (conn.State == System.Data.ConnectionState.Closed)
    //                {
    //                    conn.Open();
    //                    { }
    //                    string sql = "insert into ovms_messages(pmo_id, message, employee_id, Msg_Subject, SourceID, vendor_id, User_id,client_id, IsSend,IsRead)" +
    //                        " values('" + pmo_id + "','" + message + "','" + employee_id + "', (select Msg_Subject from ovms_messages where message_id = '" + message_id + "'), '" + message_id + "', '" + vendor_id + "', '" + vendor_id + "','" + client_id + "', 1,0)  EXEC dbo.ReplaceAction '" + message_id + "'";

    //                    SqlCommand cmd = new SqlCommand(sql, conn);
    //                    sql = "select Actions from ovms_messages where message_id='" + message_id + "'";

    //                    if (cmd.ExecuteNonQuery() >= 0)
    //                    {
    //                        xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
    //                                        "<SOURCE_ID>" + message_id + "</SOURCE_ID>" +
    //                                        "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
    //                                        "<MESSAGE>" + message + "</MESSAGE>" +
    //                                        "<MESSAGE_SUBJECT>" + MsgSubject + "</MESSAGE_SUBJECT>" +
    //                                        "<ACTIONS>" + Actions + "</ACTIONS>";
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
    //                xml_string += "<INSERT_STRING>Vendor could not replied</INSERT_STRING>";
    //                //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //            }
    //            finally
    //            {
    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }

    //        }
    //    }

    //    xml_string = xml_string + "</RESPONSE></XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}

    //[WebMethod]
    //public XmlDocument Send_Message_from_client(string Msg_Subject, string message,int vendor_id, int pmo_id, int user_id, int client_id, string Actions)
    //    {
    //    Log_Api.Service logService = new Log_Api.Service();
    //    string xml_string = "";
    //    //  int newclient_id = 0;
    //    //query database using sql client - google
    //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //        try
    //            {
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //                {
    //                conn.Open();
    //                xml_string = "<xml>" +

    //                                     "<request>" +
    //                                "<CLIENT_ID>" + client_id + "</CLIENT_ID>" + "</request>";

    //                string sql = "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id,client_id, Actions,IsRead,IsSend)" +
    //                             "values('" + pmo_id + "', '" + message + "','" + vendor_id + "', '" + Msg_Subject + "','" + user_id + "','"+client_id+"', '" + Actions + "','" + 0 + "', '" + 1 + "')";
    //                //insert into ovms_clients (client_name, pm_id, business_type_id)values('" + Msg_Subject + "','" + message + "') select cast(scope_identity() as int)";
    //                SqlCommand cmd = new SqlCommand(sql, conn);
    //                xml_string = xml_string + "<RESPONSE>";
    //                if (cmd.ExecuteNonQuery() != 0)
    //                    {
    //                    xml_string += "<CLIENT_SUBJECT>" + Msg_Subject + "</CLIENT_SUBJECT>" +
    //                 "<CLIENT_MESSAGE>" + message + "</CLIENT_MESSAGE>" +
    //                 "<IS_SEND>" + 1 + "</IS_SEND>" +
    //                 "<IS_READ>" + 0 + "</IS_READ>" +
    //                  "<ACTIONS>" + Actions + "</ACTIONS>";

    //                    }
    //                else
    //                    {
    //                    xml_string += "<client_name>Vendor not inserted</client_name>";
    //                    }
    //                }
    //            }
    //        catch (Exception ex)
    //            {
    //            xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
    //            xml_string += "<INSERT_STRING>vendor not inserted</INSERT_STRING>";
    //            //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //            }
    //        finally
    //            {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //            }


    //        }
    //    xml_string = xml_string + "</RESPONSE></xml>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //    }

    [WebMethod]//change condition if pmo want to reply particular id
    public XmlDocument Reply_from_Client(string userEmailId, string userPassword, string message_id, string message, string pmo_id, string Vendor_id, string client_id, string Actions, string MsgSubject)
    {
      // //
        string xml_string = "";
        string errString = "";
        // int new_Reply = 0;
        //query database using sql client - google
        xml_string = "<XML>" +
                    "<REQUEST>" +
                        "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                        "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                        "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                        "<SUBJECT>" + MsgSubject + "</SUBJECT>" +
                        "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
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

                        string sql = "insert into ovms_messages(pmo_id, message, vendor_id, Msg_Subject, SourceID, User_id, client_id,IsSend,IsRead)" +
                            " values('" + pmo_id + "','" + message + "','" + Vendor_id + "', (select Msg_Subject from ovms_messages where message_id = '" + message_id + "'), '" + message_id + "', '" + client_id + "','" + client_id + "', 1,1) EXEC dbo.ReplaceAction '" + message_id + "'";

                        SqlCommand cmd = new SqlCommand(sql, conn);

                        sql = "select Actions from ovms_messages where message_id='" + message_id + "'";

                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                                            "<SOURCE_ID>" + message_id + "</SOURCE_ID>" +
                                            "<VENDOR_ID>" + Vendor_id + "</VENDOR_ID>" +
                                            "<MESSAGE>" + message + "</MESSAGE>" +
                                            "<ACTIONS>" + Actions + "</ACTIONS>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Client can not able to reply message</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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

    [WebMethod]// Done
    public XmlDocument get_All_Messages_for_Client(string userEmailId, string userPassword, string client_id)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string strSub = "";
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (client_id != "" & client_id != "0")
            {
                strSub = " and msg.pmo_id=" + client_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSQL = "select msg.User_id, msg.vendor_id, v.vendor_name, msg.pmo_id,pmo.First_name+pmo.Last_Name as PMO_Name, msg.IsSend,msg.message, msg.message_id, msg.Msg_Subject, msg.IsRead, msg.Actions, msg.create_date,msg.client_id,c.client_name" +
                                        " from ovms_messages as msg" +
                                      "  join ovms_vendors as v" +
                                      "  on msg.vendor_id = v.vendor_id" +
                                      "  join ovms_pmo as pmo" +
                                       " on msg.pmo_id = pmo.pmo_id join ovms_clients as c on c.client_id = msg.client_id " +
                                       " where msg.client_id = '" + client_id + "' and (Actions='Send_V_C' or Actions='Send_P_C')" +
                                       " order by create_date asc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;

                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(7).ToString() + "</MESSAGE_ID>" +
                                                  "<VENDOR_NAME>" + reader.GetValue(2).ToString() + "</VENDOR_NAME>" +
                                                  "<VENDOR_ID>" + reader.GetValue(1).ToString() + "</VENDOR_ID>" +
                                                  "<MESSAGE_SUBJECT>" + reader.GetValue(8).ToString() + "</MESSAGE_SUBJECT>" +
                                                  "<MESSAGE>" + reader.GetValue(6).ToString() + "</MESSAGE>" +
                                                   "<PMO_ID>" + reader.GetValue(3).ToString() + "</PMO_ID>" +
                                                  "<PMO_NAME>" + reader.GetValue(4).ToString() + "</PMO_NAME>" +
                                                     "<IS_SEND>" + reader.GetValue(5).ToString() + "</IS_SEND>" +
                                                     "<IS_READ>" + reader.GetValue(9).ToString() + "</IS_READ>" +
                                                     "<ACTIONS>" + reader.GetValue(10).ToString() + "</ACTIONS>" +
                                                  "<DATE>" + reader.GetValue(11).ToString() + "</DATE>" +
                                                  "<CLIENT_NAME>" + reader.GetValue(13).ToString() + "</CLIENT_NAME>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<RESPONSE_MESSAGE> Unable to select all messages to client id</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //}

        //else
        //    {
        //    xml_string += "<UTYPE_ID>Vendor id should not be null</UTYPE_ID>";
        //    }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]// Done
    public XmlDocument get_All_Messages_for_Employee(string userEmailId, string userPassword, string employee_id)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string strSub = "";
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (employee_id != "" & employee_id != "0")
            {
                strSub = " and msg.pmo_id=" + employee_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSQL = "select msg.vendor_id, msg.employee_id,v.vendor_name, emp.first_name+emp.last_name as Emp_Name,msg.IsSend,msg.message, msg.message_id, msg.Msg_Subject, msg.IsRead, msg.Actions, msg.create_date" +
                                    " from ovms_messages as msg" +
                                    " join ovms_vendors as v on msg.vendor_id = v.vendor_id" +
                                    " join ovms_employee_details as emp" +
                                    " on msg.employee_id = emp.employee_id" +
                                    "  where msg.employee_id = 1 and(Actions = 'Send_V_E')" +
                                    "  order by create_date asc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;


                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGES ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(6).ToString() + "</MESSAGE_ID>" +
                                                 "<VENDOR_NAME>" + reader.GetValue(2).ToString() + "</VENDOR_NAME>" +
                                                  "<EMPLOYEE_NAME>" + reader.GetValue(3).ToString() + "</EMPLOYEE_NAME>" +
                                                  "<VENDOR_ID>" + reader.GetValue(0).ToString() + "</VENDOR_ID>" +
                                                  "<ENPLOYEE_ID>" + reader.GetValue(1).ToString() + "</ENPLOYEE_ID>" +
                                                  "<MESSAGE_SUBJECT>" + reader.GetValue(7).ToString() + "</MESSAGE_SUBJECT>" +
                                                  "<MESSAGE>" + reader.GetValue(5).ToString() + "</MESSAGE>" +
                                                  "<IS_SEND>" + reader.GetValue(4).ToString() + "</IS_SEND>" +
                                                  "<IS_READ>" + reader.GetValue(8).ToString() + "</IS_READ>" +
                                                  "<ACTIONS>" + reader.GetValue(9).ToString() + "</ACTIONS>" +
                                                  "<DATE>" + reader.GetValue(10).ToString() + "</DATE>" +
                                                  "</MESSAGES>";
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = "<RESPONSE_MESSAGE> Unable to select all messages to employee id</RESPONSE_MESSAGE>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //}

        //else
        //    {
        //    xml_string += "<UTYPE_ID>Vendor id should not be null</UTYPE_ID>";
        //    }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Reply_from_Employee(string userEmailId, string userPassword, string message_id, string message, string vendor_id, string employee_id, string Actions, string MsgSubject)
    {
        //logAPI.Service logService = new logAPI.Service();
        string xml_string = "";
        string errString = "";
        // int new_Reply = 0;
        //query database using sql client - google
        xml_string = "<XML>" +

                        "<REQUEST>" +
                        "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                        "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                    "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
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

                        string sql = "insert into ovms_messages(message, employee_id, Msg_Subject, SourceID, vendor_id, IsSend, IsRead,User_id)" +

                        "values('" + message + "','" + employee_id + "', (select Msg_Subject from ovms_messages where message_id = '" + message_id + "'), '" + message_id + "', '" + vendor_id + "', 1, 0,'" + employee_id + "')  EXEC dbo.ReplaceAction '" + message_id + "'";

                        SqlCommand cmd = new SqlCommand(sql, conn);
                        sql = "select Actions from ovms_messages where message_id='" + message_id + "'";

                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            xml_string += "<SOURCE_ID>" + message_id + "</SOURCE_ID>" +
                                            "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                            "<MESSAGE>" + message + "</MESSAGE>" +
                                            "<ACTIONS>" + Actions + "</ACTIONS>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Employee could not able to reply</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument Send_Message_client(string userEmailId, string userPassword, string Msg_Subject, string message, string vendor_id, string pmo_id, string client_id, string Actions)
    {
        //logAPI.Service logService = new logAPI.Service();
        string xml_string = "";
        string strVendorID = "";
        string strPMOID = "";
        string errString = "";
        //  int newclient_id = 0;
        //query database using sql client - google

        xml_string = "<XML>" +
                            "<REQUEST>" +
                     "<CLIENT_ID>" + client_id + "</CLIENT_ID></REQUEST>";
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

                        strVendorID = vendor_id.ToString();
                        strPMOID = pmo_id.ToString();

                        if (Actions == "Send_C_P")
                        {
                            strVendorID = "null";
                            //vendor_id = 0;
                        }
                        if (Actions == "Send_C_V")
                        {
                            //pmo_id = 0;
                            strPMOID = "null";
                        }
                        string sql = "insert into ovms_messages(pmo_id,message,vendor_id,Msg_Subject, User_id, client_id,Actions,IsRead,IsSend)" +
                                     "values(" + strPMOID + ", '" + message + "','" + strVendorID + "', '" + Msg_Subject + "','" + client_id + "', '" + client_id + "','" + Actions + "','" + 0 + "', '" + 1 + "')";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        if (cmd.ExecuteNonQuery() != 0)
                        {
                            xml_string += "<CLIENT_SUBJECT>" + Msg_Subject + "</CLIENT_SUBJECT>" +
                         "<CLIENT_MESSAGE>" + message + "</CLIENT_MESSAGE>" +
                         "<IS_SEND>" + 1 + "</IS_SEND>" +
                         "<IS_READ>" + 0 + "</IS_READ>" +
                          "<ACTIONS>" + Actions + "</ACTIONS>";

                        }
                        else
                        {
                            xml_string += "<CLIENT_NAME>Client not inserted</CLIENT_NAME>";
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>vendor not inserted</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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

    public XmlDocument DropDown_PMO_To_Vendor(string userEmailId, string userPassword, string pmo_id)
    {
       // aakashService.Service logService = new aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        string errString = "";

        SqlConnection conn;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<PMO_ID>" + pmo_id + "</PMO_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (pmo_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select distinct msg.vendor_id, v.vendor_name, msg.pmo_id" +
                                        " from ovms_messages as msg" +
                                        " join ovms_vendors as v" +
                                        " on msg.vendor_id = v.vendor_id" +
                                        " where msg.pmo_id = '" + pmo_id + "'" +
                                        "order by vendor_name";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<VENDORS>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<VENDOR_NAME>" + reader["vendor_name"] + "</VENDOR_NAME >" +
                               "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID >";
                        }
                        xml_string = xml_string + "</VENDORS>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select Vendor id</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>Vendor id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]

    public XmlDocument DropDown_PMO_To_Client(string userEmailId, string userPassword, string pmo_id)
    {
       // aakashService.Service logService = new aakashService.Service();
        //logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        string errString = "";

        SqlConnection conn;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<PMO_ID>" + pmo_id + "</PMO_ID></REQUEST>";

        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (pmo_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select distinct msg.client_id,c.client_name, msg.pmo_id" +
                                        " from ovms_messages as msg" +
                                        " join ovms_clients as c" +
                                        " on msg.client_id = c.client_id" +
                                        " where msg.pmo_id = '" + pmo_id + "'" +
                                        "order by client_name";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<CLIENTS>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<CLIENT_ID>" + reader["client_id"] + "</CLIENT_ID>" +
                               "<CLIENT_NAME>" + reader["client_name"] + "</CLIENT_NAME>";
                        }
                        xml_string = xml_string + "</CLIENTS>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select client id</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<PMO_ID>Client id should not be null</PMO_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]

    public XmlDocument DropDown_VENDOR_To_PMO(string userEmailId, string userPassword, string vendor_id)
    {
       // aakashService.Service logService = new aakashService.Service();
        //logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        string errString = "";

        SqlConnection conn;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<VENDOR_ID>" + vendor_id + "</VENDOR_ID></REQUEST>";

        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select distinct pmo.pmo_id, pmo.first_name + pmo.last_name as PMO_Name " +
                                "from ovms_pmo as pmo where pmo.vendor_id = '" + vendor_id + "' order by PMO_Name asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<VENDORS>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<PMO_ID>" + reader["pmo_id"] + "</PMO_ID >" +
                               "<PMO_NAME>" + reader["pmo_name"] + "</PMO_NAME >";
                        }
                        xml_string = xml_string + "</VENDORS>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select vendor to pmo id </RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>vendor id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument DropDown_Vendor_To_Employee(string userEmailId, string userPassword, string vendor_id)
    {
     //   aakashService.Service logService = new //aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        string errString = "";

        SqlConnection conn;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select distinct msg.employee_id, msg.vendor_id, e.first_name + e.last_name as Employee_Name" +
                                        " from ovms_messages as msg" +
                                        " join ovms_employee_details as e" +
                                        " on msg.employee_id = e.employee_id" +
                                        " where msg.vendor_id = '" + vendor_id + "'" +
                                        " order by Employee_Name asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<EMPLOYEES>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID >" +
                               "<EMPLOYEE_NAME>" + reader["employee_name"] + "</EMPLOYEE_NAME >";
                        }
                        xml_string = xml_string + "</EMPLOYEES>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<XML>" + "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                    "<RESPONSE_MESSAGE> Unable to select vendor id to Employee</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>vendor id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]

    public XmlDocument DropDown_VENDOR_To_Client(string userEmailId, string userPassword, string vendor_id)
    {
     //   aakashService.Service logService = new aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        string errString = "";

        SqlConnection conn;

        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<VENDOR_ID>" + vendor_id + "</VENDOR_ID></REQUEST>";

        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select distinct msg.client_id, msg.vendor_id, c.client_name" +
                                        " from ovms_messages as msg" +
                                        " join ovms_clients as c" +
                                        " on msg.client_id = c.client_id" +
                                        " where msg.vendor_id = '" + vendor_id + "'" +
                                        " order by client_name asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<CLIENTS>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<CLIENT_ID>" + reader["client_id"] + "</CLIENT_ID >" +
                               "<CLIENT_NAME>" + reader["client_name"] + "</CLIENT_NAME >";
                        }
                        xml_string = xml_string + "</CLIENTS>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select vendor id to client</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>vendor id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]

    public XmlDocument DropDown_CLient_To_PMO(string userEmailId, string userPassword, string client_id)
    {
//aakashService.Service logService = new /aakashService.Service();
    //    logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();

        SqlConnection conn;
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<CLIENT_ID>" + client_id + "</CLIENT_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (client_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select distinct msg.client_id, msg.pmo_id, p.First_Name+p.Last_Name as PMO_Name" +
                                        "  from ovms_messages as msg" +
                                        " join ovms_clients as c" +
                                        "  on msg.pmo_id=p.pmo_id" +
                                        "  where msg.client_id='" + client_id + "'" +
                                        " order by PMO_Name asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<VENDORS>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<PMO_ID>" + reader["pmo_id"] + "</PMO_ID>" +
                               "<PMO_NAME>" + reader["pmo_name"] + "</PMO_NAME>";
                        }
                        xml_string = xml_string + "</VENDORS>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select client id</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>client id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]

    public XmlDocument DropDown_CLient_To_Vendor(string userEmailId, string userPassword, string client_id)
    {
       // aakashService.Service logService = new aakashService.Service();
      //  logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();

        SqlConnection conn;
        string errString = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                             "<CLIENT_ID>" + client_id + "</CLIENT_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (client_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = " select distinct msg.client_id, msg.vendor_id,v.vendor_name" +
                                       "  from ovms_messages as msg" +
                                       " join ovms_vendors as v" +
                                       " on msg.pmo_id = v.vendor_id" +
                                      " where msg.client_id = 1" +
                                    " order by vendor_name asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;

                        xml_string = xml_string + "<VENDORS>";
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID >" +
                               "<VENDOR_NAME>" + reader["vendor_name"] + "</VENDOR_NAME >";
                        }
                        xml_string = xml_string + "</VENDORS>";

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select client id</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>client id should not be null</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Show_notification_for_Vendor(string userEmailId, string userPassword, string user_id)
    {
      //  aakashService.Service logService = new aakashService.Service();
       // logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();

        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
       "<USER_ID>" + user_id + "</USER_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (user_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();


                        //string strSQL = "select (select count(*) from ovms_messages where User_id=1 and IsRead=0  and (Actions = 'Send_P_V' or Actions= 'Send_C_V')) as TotalUnRead, " +
                        //               " p.First_name + p.Last_Name as Name,msg.message, msg.message_id,msg.Msg_Subject, msg.IsRead,msg.vendor_id,v.vendor_name, Actions,dbo.GetDateDifference(msg.create_date) as createDate" +
                        //              "  from ovms_messages as msg" +
                        //               " join ovms_pmo as p" +
                        //                " on msg.pmo_id = p.pmo_id" +
                        //               " join ovms_vendors as v" +
                        //               " on msg.vendor_id = v.vendor_id " +
                        //              "  where msg.IsRead = 0 and(msg.Actions = 'Send_P_V' or msg.Actions = 'Send_C_V') and msg.user_id = '"+user_id+"'" +
                        //              "  order by createDate asc";

                        string strSQL = "select (select count(*) from ovms_messages as m where m.User_id=msg.user_id  and m.IsRead=0" +
                                " and(m.Actions = 'Send_P_V' or m.Actions = 'Send_C_V')) as TotalUnRead," +
                                " p.First_name + p.Last_Name as Name,msg.message, msg.message_id,msg.Msg_Subject, msg.IsRead,msg.vendor_id,v.vendor_name," +
                                 " Actions,dbo.GetDateDifference(msg.create_date) as createDate" +
                                  " from ovms_messages as msg" +
                                  " join ovms_pmo as p on msg.pmo_id = p.pmo_id" +
                                   " join ovms_vendors as v on msg.vendor_id = v.vendor_id   where msg.IsRead = 0 and(msg.Actions = 'Send_P_V'" +
                                   " or msg.Actions = 'Send_C_V') and msg.user_id = '" + user_id + "'  order by createDate asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;


                        while (reader.Read())
                        {
                            xml_string = xml_string + "<NOTIFICATION ID=\"" + intCount + "\"> <MESSAGE_ID>" + reader["message_id"] + "</MESSAGE_ID>" +
                                "<MESSAGE_SUBJECT>" + reader["Msg_Subject"] + "</MESSAGE_SUBJECT >" +
                                "<MESSAGE>" + reader["message"] + "</MESSAGE >" +
                                "<ISREAD>" + reader["TotalUnRead"] + "</ISREAD >" +
                                "<PMO_NAME>" + reader["Name"] + "</PMO_NAME >" +

                                "<TIME>" + reader["createDate"] + "</TIME ></NOTIFICATION>";
                            intCount++;
                        }


                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<XML>" + "<USER_ID>" + user_id + "</USER_ID>" +
                                    "<RESPONSE> Unable to select notifications for vednor</RESPONSE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>unable to select notification for vendor</VENDOR_ID>";
            }
        }
        //output final
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]

    public XmlDocument Show_notification_for_PMO(string userEmailId, string userPassword, string user_id)
    {
       Service service = new Service();
        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" + "<USER_ID>" + user_id + "</USER_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (user_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select (select count(*) from ovms_messages where User_id=1 and IsRead=0  and (Actions = 'Send_V_P' or Actions = 'Send_C_P')) as TotalUnRead," +
                                        " p.First_name + p.Last_Name as Name,msg.message, msg.message_id,msg.Msg_Subject, msg.IsRead,msg.vendor_id,v.vendor_name," +
                                        "  Actions,dbo.GetDateDifference(msg.create_date) as createDate" +
                                        "  from ovms_messages as msg" +
                                        "  join ovms_pmo as p" +
                                        "  on msg.pmo_id = p.pmo_id" +
                                        "  join ovms_vendors as v" +
                                        "  on msg.vendor_id = v.vendor_id " +
                                        "  where msg.IsRead = 0 and(msg.Actions = 'Send_V_P' or msg.Actions = 'Send_C_P') and msg.user_id = 1" +
                                        "  order by createDate asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;


                        while (reader.Read())
                        {
                            xml_string = xml_string + "<NOTIFICATION ID=\"" + intCount + "\"> <MESSAGE_ID>" + reader["message_id"] + "</MESSAGE_ID>" +
                            "<MESSAGE_SUBJECT>" + reader["Msg_Subject"] + "</MESSAGE_SUBJECT>" +
                            "<MESSAGE>" + reader["message"] + "</MESSAGE>" +
                            "<UNREAD>" + reader["TotalUnRead"] + "</UNREAD>" +
                            "<VENDOR_NAME>" + reader["vendor_name"] + "</VENDOR_NAME>" +
                            "<TIME>" + reader["createDate"] + "</TIME></NOTIFICATION>";
                            intCount++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select notifications for pmo</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<PMO_ID>unable to select notification for PMO</PMO_ID>";
            }
            //output final
        }
        xml_string += "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    public XmlDocument Show_notification_for_Employee(string userEmailId, string userPassword, string user_id)
    {
       // aakashService.Service logService = new aakashService.Service();
       // logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();

        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<USER_ID>" + user_id + "</USER_ID>" + "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (user_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        string strSQL = "select (select count(*) from ovms_messages where User_id=1 and IsRead=0  and Actions = 'Send_V_E') as TotalUnRead," +
                                         " e.employee_id,e.first_name + e.last_name as Employee_Name ,msg.message, msg.message_id,msg.Msg_Subject, msg.IsRead,msg.vendor_id,v.vendor_name," +
                                         "  Actions,dbo.GetDateDifference(msg.create_date) as createDate" +
                                         " from ovms_messages as msg" +
                                         "  join ovms_employee_details as e" +
                                         "  on msg.employee_id = e.employee_id" +
                                         "  join ovms_vendors as v" +
                                         "  on msg.vendor_id = v.vendor_id" +
                                         "  where msg.IsRead = 0 and msg.Actions = 'Send_V_E' and msg.user_id = 1" +
                                         "  order by createDate asc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;


                        while (reader.Read())
                        {
                            xml_string = xml_string + "<NOTIFICATION ID=\"" + intCount + "\"> <MESSAGE_ID>" + reader["message_id"] + "</MESSAGE_ID>" +
                                "<MESSAGE_SUBJECT>" + reader["Msg_Subject"] + "</MESSAGE_SUBJECT >" +
                                "<MESSAGE>" + reader["message"] + "</MESSAGE >" +
                                "<ISREAD>" + reader["TotalUnRead"] + "</ISREAD >" +
                                "<EMPLOYEE_NAME>" + reader["Employee_Name"] + "</EMPLOYEE_NAME >" +
                                "<TIME>" + reader["createDate"] + "</TIME ></NOTIFICATION>";
                            intCount++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select notifications for vednor</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>unable to select notification for vendor</VENDOR_ID>";
            }
        }
        //output final
        xml_string += " </RESPONSE> </XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Show_notification_for_Client(string userEmailId, string userPassword, string user_id)
    {
        //aakashService.Service logService = new aakashService.Service();
//logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();
        SqlConnection conn;
        int intCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>"
                            + "<USER_ID>" + user_id + "</USER_ID></REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (user_id != "0")
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string strSQL = "select (select count(*) from ovms_messages where User_id=1 and IsRead=0  and (Actions = 'Send_V_c' OR Actions='Send_P_C')) as TotalUnRead," +
                                            "  c.client_id,c.client_name ,msg.message, msg.message_id,msg.Msg_Subject, msg.IsRead,msg.vendor_id,v.vendor_name," +
                                            "  Actions,dbo.GetDateDifference(msg.create_date) as createDate" +
                                            "  from ovms_messages as msg" +
                                            "  join ovms_clients as c" +
                                            "  on msg.client_id = c.client_id" +
                                            "  join ovms_vendors as v" +
                                            "  on msg.vendor_id = v.vendor_id" +
                                            "  where msg.IsRead = 0 and(msg.Actions = 'Send_V_C' OR msg.Actions = 'Send_P_C') and msg.user_id = 1" +
                                            "  order by createDate desc";

                        SqlCommand cmd = new SqlCommand(strSQL, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = reader.FieldCount;


                        while (reader.Read())
                        {
                            xml_string = xml_string + "<NOTIFICATION ID=\"" + intCount + "\"> <MESSAGE_ID>" + reader["message_id"] + "</MESSAGE_ID>" +
                                "<MESSAGE_SUBJECT>" + reader["Msg_Subject"] + "</MESSAGE_SUBJECT >" +
                                "<MESSAGE>" + reader["message"] + "</MESSAGE >" +
                                "<ISREAD>" + reader["TotalUnRead"] + "</ISREAD >" +
                                "<EMPLOYEE_NAME>" + reader["client_name"] + "</EMPLOYEE_NAME >" +
                                "<TIME>" + reader["createDate"] + "</TIME ></NOTIFICATION>";
                            intCount++;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                    xml_string = "<RESPONSE_MESSAGE> Unable to select notifications for vednor</RESPONSE_MESSAGE>";
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }

            else
            {
                xml_string += "<VENDOR_ID>unable to select notification for vendor</VENDOR_ID>";
            }
        }
        //output final
        xml_string += " </RESPONSE> </XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_ISRead(string userEmailId, string userPassword, string message_id)
    {
       // aakashService.Service logService = new aakashService.Service();
       // logAPI.Service ErrorlogService = new logAPI.Service();
        Service service = new Service();

        SqlConnection conn;
        //string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" + "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                    "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";
        string errString = service.VerifyUser(userEmailId, userPassword);

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

                    string strSQL = "update ovms_messages set IsRead=1 where message_id = '" + message_id + "'" +
                                      " select IsRead from ovms_messages where message_id = '" + message_id + "'";

                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Read();
                    xml_string = xml_string + "<IS_READ>" + reader["IsRead"] + "</IS_READ>";
                }
            }
            catch (Exception ex)
            {
                //Error//logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string = xml_string +
                                "<STATUS> Unable to select message id</STATUS>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //  xml_string = xml_string + "</RESPONSE>";
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);
        return xmldoc;
    }
}