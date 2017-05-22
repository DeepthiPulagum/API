using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml;

/// <summary>
/// Summary description for Aakash
/// </summary>
public class Aakash
{
    SqlConnection conn;
    public Aakash()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    [WebMethod]

    private string VerifyUser(string emailId, string userPassword)
    {
        //aakashService.Service aService = new aakashService.Service();

        //UserInfo.Service userinf = new UserInfo.Service();
        XmlDocument _xUserInfo = new XmlDocument();
        _xUserInfo.LoadXml("<XML>" + get_Login(emailId, userPassword).InnerXml + "</XML>");

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
    public XmlDocument get_Login(string email, string Password)
    {

        string xml_string = "";
        // SqlCommand cmd;

        // int userid = 0;

        //query database using sql client - google
        //logAPI.Service logService = new logAPI.Service();

        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        try
        {

            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                                                    "<REQUEST>" +
                                                        "<USERNAME>" + email + "</USERNAME>" +
                                                        "<PASSWORD><![CDATA[" + Password + "]]></PASSWORD>" +
                                                    "</REQUEST>";

                string strSql = "Select a.first_name,a.last_name,a.user_id,a.user_password,a.active,a.create_date,a.email_id, a.utype_id,a.client_id,a.vendor_id, " +
                   " (select primary_email from ovms_vendor_details where vendor_id = a.vendor_id) vendor_email, " +
                               " (select pmo_id from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmoid, " +
                               " (select first_name + ' ' + last_name from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmo_name " +
                               " from ovms_users a " +
                               " where a.email_id = '" + email + "' " +
                               " and a.user_password = '" + Password + "' " +
                               " and a.active = 1";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {


                        xml_string = xml_string + "<RESPONSE>" +
                                                        "<USERID>" + reader["user_id"] + "</USERID>" +
                                                        "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
                                                        "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
                                                        "<EMAIL>" + reader["email_id"] + "</EMAIL>" +
                                                        "<USERTYPE>" + reader["utype_id"] + "</USERTYPE>" +
                                                        "<PMOID>" + reader["pmoid"] + "</PMOID>" +
                                                        "<PMO_NAME>" + reader["pmo_name"] + "</PMO_NAME>" +
                                                        "<CLIENTID>" + reader["client_id"] + "</CLIENTID>" +
                                                        "<VENDORID>" + reader["vendor_id"] + "</VENDORID>" +
                                                        "<VENDORID_EMAIL>" + reader["vendor_email"] + "</VENDORID_EMAIL>" +
                                       "</RESPONSE>";
                    }
                    //close
                    reader.Close();
                }
                else
                {
                    xml_string = xml_string + "<RESPONSE><ERROR>Invalid email or password</ERROR>" +
                        "<EMAIL>email</EMAIL></RESPONSE>";

                    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No User Found");
                }
                //Dispose
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        { 
            xml_string = xml_string + "<RESPONSE>error:100.systemerror</RESPONSE>";
            //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        //output final
        xml_string = xml_string + "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
  



    ////output final
    //xml_string = xml_string + "</XML>";
    //XmlDocument xmldoc;
    //xmldoc = new XmlDocument();
    //xmldoc.LoadXml(xml_string);

    //return xmldoc;
    //


    //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //{
    //    if (conn.State == System.Data.ConnectionState.Closed)
    //    {
    //        conn.Open();
    //        xml_string = "<XML>" +
    //                    "<REQUEST>" +
    //                        "<USERNAME>" + email + "</USERNAME>" +
    //                        "<PASSWORD><![CDATA[" + Password + "]]></PASSWORD>" +
    //                    "</REQUEST>";
    //        //string strSql = "Select first_name,last_name,user_id,user_password,active,create_date,email_id, utype_id,client_id,vendor_id from ovms_users where email_id='" + email + "' and user_password='" + Password + "' and active=1";
    //        string strSql = "Select a.first_name,a.last_name,a.user_id,a.user_password,a.active,a.create_date,a.email_id, a.utype_id,a.client_id,a.vendor_id, " +
    //            " (select primary_email from ovms_vendor_details where vendor_id = a.vendor_id) vendor_email, " +
    //                        " (select pmo_id from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmoid, " +
    //                        " (select first_name + ' ' + last_name from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmo_name " +
    //                        " from ovms_users a " +
    //                        " where a.email_id = '" + email + "' " +
    //                        " and a.user_password = '" + Password + "' " +
    //                        " and a.active = 1";
    //        try
    //        {
    //            //using (SqlCommand cmd = new SqlCommand(strSql, conn))
    //            //{
    //            //    using (SqlDataReader reader = cmd.ExecuteReader())
    //            //    {
    //            //        if (reader.Read())
    //            //        {       SqlCommand cmd = new SqlCommand(strSql, conn);
    //            SqlCommand cmd = new SqlCommand(strSql, conn);
    //            SqlDataReader reader = cmd.ExecuteReader();

    //            int RowID = 1;
    //            if (reader.HasRows == true)
    //            {
    //                while (reader.Read())
    //                {


    //                    xml_string = xml_string + "<RESPONSE>" +
    //                                                    "<USERID>" + reader["user_id"] + "</USERID>" +
    //                                                    "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
    //                                                    "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
    //                                                    "<EMAIL>" + reader["email_id"] + "</EMAIL>" +
    //                                                    "<USERTYPE>" + reader["utype_id"] + "</USERTYPE>" +
    //                                                    "<PMOID>" + reader["pmoid"] + "</PMOID>" +
    //                                                    "<PMO_NAME>" + reader["pmo_name"] + "</PMO_NAME>" +
    //                                                    "<CLIENTID>" + reader["client_id"] + "</CLIENTID>" +
    //                                                    "<VENDORID>" + reader["vendor_id"] + "</VENDORID>" +
    //                                                    "<VENDORID_EMAIL>" + reader["vendor_email"] + "</VENDORID_EMAIL>" +
    //                                   "</RESPONSE>";
    //                }
    //            }
    //            else
    //            {
    //                xml_string = xml_string + "<RESPONSE><ERROR>Invalid email or password</ERROR>" +
    //                    "<EMAIL>email</EMAIL></RESPONSE>";

    //                //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No User Found");
    //            }
    //            reader.Close();
    //            cmd.Dispose();
    //        }





    ////    catch (Exception ex)
    ////    {
    ////        xml_string = xml_string + "<RESPONSE>error:100.systemerror</RESPONSE>";
    ////        //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

    ////    }
    ////    finally
    ////    {
    ////        if (conn.State == System.Data.ConnectionState.Open)
    ////            conn.Close();
    ////    }
    ////}


    //////output final
    ////xml_string = xml_string + "</XML>";
    ////XmlDocument xmldoc;
    ////xmldoc = new XmlDocument();
    ////xmldoc.LoadXml(xml_string);

    ////return xmldoc;


//public XmlDocument get_Login(string email, string Password)
//{

//    string xml_string = "";


//    // int userid = 0;

//    //query database using sql client - google
//    //logAPI.Service logService = new logAPI.Service();


//    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
//    {
//        if (conn.State == System.Data.ConnectionState.Closed)
//        {
//            conn.Open();
//            xml_string = "<XML>" +
//                        "<REQUEST>" +
//                            "<USERNAME>" + email + "</USERNAME>" +
//                            "<PASSWORD><![CDATA[" + Password + "]]></PASSWORD>" +
//                        "</REQUEST>";
//            //string strSql = "Select first_name,last_name,user_id,user_password,active,create_date,email_id, utype_id,client_id,vendor_id from ovms_users where email_id='" + email + "' and user_password='" + Password + "' and active=1";
//            string strSql = "Select a.first_name,a.last_name,a.user_id,a.user_password,a.active,a.create_date,a.email_id, a.utype_id,a.client_id,a.vendor_id, " +
//                " (select primary_email from ovms_vendor_details where vendor_id = a.vendor_id) vendor_email, " +
//                            " (select pmo_id from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmoid, " +
//                            " (select first_name + ' ' + last_name from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmo_name " +
//                            " from ovms_users a " +
//                            " where a.email_id = '" + email + "' " +
//                            " and a.user_password = '" + Password + "' " +
//                            " and a.active = 1";
//            try
//            {
//                using (SqlCommand cmd = new SqlCommand(strSql, conn))
//                {
//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {


//                            xml_string = xml_string + "<RESPONSE>" +
//                                                        "<USERID>" + reader["user_id"] + "</USERID>" +
//                                                        "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
//                                                        "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
//                                                        "<EMAIL>" + reader["email_id"] + "</EMAIL>" +
//                                                        "<USERTYPE>" + reader["utype_id"] + "</USERTYPE>" +
//                                                        "<PMOID>" + reader["pmoid"] + "</PMOID>" +
//                                                        "<PMO_NAME>" + reader["pmo_name"] + "</PMO_NAME>" +
//                                                        "<CLIENTID>" + reader["client_id"] + "</CLIENTID>" +
//                                                        "<VENDORID>" + reader["vendor_id"] + "</VENDORID>" +
//                                                        "<VENDORID_EMAIL>" + reader["vendor_email"] + "</VENDORID_EMAIL>" +
//                                       "</RESPONSE>";
//                        }
//                        else
//                        {
//                            xml_string = xml_string + "<RESPONSE><ERROR>Invalid email or password</ERROR>" +
//                                "<EMAIL>email</EMAIL></RESPONSE>";

//                            //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No User Found");
//                        }
//                        reader.Close();
//                        cmd.Dispose();
//                    }
//                }
//            }

//            catch (Exception ex)
//            {
//                xml_string = xml_string + "<RESPONSE>error:100.systemerror</RESPONSE>";
//                //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

//            }
//        }
//        if (conn.State == System.Data.ConnectionState.Open)
//            conn.Close();
//    }


//    //output final
//    xml_string = xml_string + "</XML>";
//    XmlDocument xmldoc;
//    xmldoc = new XmlDocument();
//    xmldoc.LoadXml(xml_string);

//    return xmldoc;

//}
////public XmlDocument get_Login(string email, string Password)
//{

//    string xml_string = "";


//    // int userid = 0;

//    //query database using sql client - google
//   //


//    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
//    {
//        if (conn.State == System.Data.ConnectionState.Closed)
//        {
//            conn.Open();
//            xml_string = "<XML>" +
//                        "<REQUEST>" +
//                            "<USERNAME>" + email + "</USERNAME>" +
//                            "<PASSWORD>" + Password + "</PASSWORD>" +
//                        "</REQUEST>";
//            //string strSql = "Select first_name,last_name,user_id,user_password,active,create_date,email_id, utype_id,client_id,vendor_id from ovms_users where email_id='" + email + "' and user_password='" + Password + "' and active=1";
//            string strSql = "Select a.first_name,a.last_name,a.user_id,a.user_password,a.active,a.create_date,a.email_id, a.utype_id,a.client_id,a.vendor_id, " +
//                            " (select pmo_id from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmoid, " +
//                            " (select first_name + ' ' + last_name from ovms_pmo where vendor_id = a.vendor_id and employee_id is not null) pmo_name " +
//                            " from ovms_users a " +
//                            " where a.email_id = '" + email + "' " +
//                            " and a.user_password = '" + Password + "' " +
//                            " and a.active = 1";
//            try
//            {
//                using (SqlCommand cmd = new SqlCommand(strSql, conn))
//                {
//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {


//                            xml_string = xml_string + "<RESPONSE>" +
//                                       "<USERID>" + reader["user_id"] + "</USERID>" +
//                                       "<FIRSTNAME>" + reader["first_name"] + "</FIRSTNAME>" +
//                                       "<LASTNAME>" + reader["last_name"] + "</LASTNAME>" +
//                                      "<EMAIL>" + reader["email_id"] + "</EMAIL>" +
//                                      "<USERTYPE>" + reader["utype_id"] + "</USERTYPE>" +
//                                      "<PMOID>" + reader["pmoid"] + "</PMOID>" +
//                                      "<PMO_NAME>" + reader["pmo_name"] + "</PMO_NAME>" +
//                                      "<CLIENTID>" + reader["client_id"] + "</CLIENTID>" +
//                                       "<VENDORID>" + reader["vendor_id"] + "</VENDORID>" +


//                                       "</RESPONSE>";
//                        }
//                        else
//                        {
//                            xml_string = xml_string + "<RESPONSE><ERROR>Invalid email or password</ERROR>" +
//                                "<EMAIL>email</EMAIL></RESPONSE>";

//                            //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No User Found");
//                        }
//                    }
//                }
//            }

//            catch (Exception ex)
//            {
//                xml_string = xml_string + "<RESPONSE>error:100.systemerror</RESPONSE>";
//                //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

//            }
//        }
//        if (conn.State == System.Data.ConnectionState.Open)
//            conn.Close();
//    }


//    //output final
//    xml_string = xml_string + "</XML>";
//    XmlDocument xmldoc;
//    xmldoc = new XmlDocument();
//    xmldoc.LoadXml(xml_string);

//    return xmldoc;

//}

[WebMethod]
    public XmlDocument get_forgotPassword(string email)
    {

        string xml_string = "";


        // int userid = 0;

        //query database using sql client - google
//       //


        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                 "<EMAIL>" + email + "</EMAIL>" +
                            "</REQUEST>";
                string strSql = "select user_password from ovms_users where email_id='" + email + "' and active=1";
                try
                {
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {


                                xml_string = xml_string + "<RESPONSE>" +
                                           "<PASSWORD><![CDATA[" + reader["user_password"] + "]]></PASSWORD>" +
                                              "</RESPONSE>";
                            }
                            else
                            {
                                xml_string = xml_string + "<RESPONSE><ERROR>Invalid email</ERROR>" +
                                    "</RESPONSE>";

                            }
                            reader.Close();
                            cmd.Dispose();
                        }

                    }
                }

                catch (Exception ex)
                {
                    xml_string = xml_string + "<RESPONSE>error:100.systemerror</RESPONSE>";
                }
            }
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }

        xml_string = xml_string + "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }

    [WebMethod]
    public XmlDocument get_Profile(string userEmailId, string userPassword, string User_id)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new /logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<EMPLOYEE_ID>" + User_id + "</EMPLOYEE_ID>" +
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
                    string strSql = "select  first_name, last_name, email_id, user_password from ovms_users where User_id='" + User_id + "' and active=1";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;

                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<USER_NO ID ='" + RowID + "'>" +
                                                        "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                                        "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                                        "<EMAIL>" + reader["email_id"] + "</EMAIL>" +
                                                        "<PASSWORD><![CDATA[" + reader["user_password"] + "]]></PASSWORD>" +
                                                        "</USER_NO>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<STATUS><ERROR>Invalid Username</ERROR>" +
                            "</STATUS>";
                        // logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No User Found");

                    }
                    reader.Close();
                    cmd.Dispose();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";
                //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

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
    public XmlDocument update_Profile(string userEmailId, string userPassword, string first_name, string last_name, string email_id, string User_id, string new_password)
    {
        SqlConnection conn;
        string xml_string = "";
//       //
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<USER_ID>" + User_id + "</USER_ID>" +
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
                    string strSql = " update ovms_users set " +
                                   " first_name = '" + first_name + "', last_name = '" + last_name + "'," +
                                  "  email_id = '" + email_id + "'," +
                                   " user_password = '" + new_password + "' where " +
                                   " User_id = '" + User_id + "' and active = 1";



                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Password Updated successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Password not upadated</ERROR> </INSERT_STRING>";

                    }
                    //reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";
                // logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

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
    //[WebMethod]
    //public XmlDocument insert_users(string first_name, string last_name, string email_id, string user_password, int utype)
    //{

    //    string xml_string = "";
    //   //

    //    string strSql;


    //    //  query database using sql client -google

    //    SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
    //    try
    //    {
    //        if (conn.State == System.Data.ConnectionState.Closed)
    //        {
    //            conn.Open();
    //            xml_string = "<XML>" +
    //                "<REQUEST>" +
    //                "<FIRST_NAME>" + first_name + "</FIRST_NAME>" +
    //                "<LAST_NAME>" + last_name + "</LAST_NAME>" +
    //                "<EMAIL_ID>" + email_id + "</EMAIL_ID>" +
    //                "<PASSWORD>" + user_password + "</PASSWORD>" +
    //                "<USER_TYPE>" + utype + "</USER_TYPE>" +
    //                "</REQUEST>";

    //            strSql = "INSERT INTO ovms_users (first_name,last_name,email_id,user_password,utype_id)VALUES('" + first_name + "','" + last_name + "','" + email_id + "','" + user_password + "','" + utype + "')";
    //            SqlCommand cmd = new SqlCommand(strSql, conn);

    //            if (cmd.ExecuteNonQuery() > 0)
    //            {
    //                xml_string += "<RESPONSE>" +
    //                    "<INSERT_STRING>user inserted successfully</INSERT_STRING>" +

    //                    "</RESPONSE>";
    //            }
    //            else
    //            {

    //                xml_string = xml_string + "<RESPONSE><ERROR>NOT INSERTED</ERROR></RESPONSE>";
    //                //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "USER NOT INSERTED");
    //            }

    //            //       output final



    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        if (e.InnerException==2333)
    //            xml_string = xml_string + "<RESPONSE><ERROR>EMAIL already exists</ERROR></RESPONSE>";
    //    }
    //    finally
    //    {
    //        if (conn.State == System.Data.ConnectionState.Open)
    //            conn.Close();
    //    };



    //    xml_string = xml_string + "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;

    //}
    [WebMethod]
    public XmlDocument update_user(string userEmailId, string userPassword, int userId, string first_name, string last_name, string email_id, string user_password, string utype)
    {



        string xml_string = "";
        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<USERNAME>" + email_id + "</USERNAME>" +
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
                int FStatus = 0;
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    //xml_string = "<XML>" +
                    //            "<REQUEST>" +
                    //                "<USERNAME>" + email_id + "</USERNAME>" +
                    //            "</REQUEST>";

                    string strSql = "update ovms_users set ";

                    if (first_name != "")
                    {
                        strSql += " first_name='" + first_name + "'";
                        FStatus = 1;
                    }


                    if (last_name != "")
                    {
                        strSql += (FStatus == 1 ? "," : "") + " last_name='" + last_name + "'";
                        FStatus = 1;
                    }

                    if (user_password != "")
                    {
                        strSql += (FStatus == 1 ? "," : "") + " user_password ='" + user_password + "'";
                        FStatus = 1;
                    }

                    if (email_id != "")
                    {
                        strSql += (FStatus == 1 ? "," : "") + " email_id = '" + email_id + "'";
                        FStatus = 1;
                    }

                    if (utype != "")
                        if (int.Parse(utype) > 0)
                        {
                            strSql += (FStatus == 1 ? "," : "") + " utype_id = " + int.Parse(utype);
                        }

                    strSql += " where user_id = " + userId;


                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>saved successfully</RESPONSE_MESSAGE>";
                    }





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

    public XmlDocument delete_user(string userEmailId, string userPassword, int userId)
    {
        {
            string xml_string = "";
            string strSql;
            string errString = "";

            xml_string = "<XML>" +
                                "<REQUEST>" +
                                    "<USERID>" + userId + "</USERID>" +
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
                    // int FStatus = 0;
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        //xml_string = "<XML>" +
                        //            "<REQUEST>" +
                        //                "<USERID>" + userId + "</USERID>" +
                        //            "</REQUEST>";

                        strSql = "update ovms_users set active=0 where User_id= " + userId;
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        //xml_string = xml_string + "<RESPONSE>";



                        if (cmd.ExecuteNonQuery() > 0)
                        {






                            xml_string += "<USERID>" + userId + "</USERID>";



                        }
                        else
                        {
                            xml_string = xml_string + "<RESPONSE><ERROR>User does not exist</ERROR></RESPONSE>";


                        }
                        //xml_string += "</RESPONSE>";





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


    }
    [WebMethod]

    public XmlDocument get_usertype(string userEmailId, string userPassword, int userId)
    {
        {
            string xml_string = "";
            string strSql;
            string errString = "";

            xml_string = "<XML>" +
                                "<REQUEST>" +
                                    "<USERID>" + userId + "</USERID>" +
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

                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        strSql = "Select user_type from ovms_user_type where utype_id='" + userId + "' and active = 1";
                        using (SqlCommand cmd = new SqlCommand(strSql, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    xml_string = xml_string + "<USERTYPE>" + reader["user_type"] + "</USERTYPE>";

                                }
                                else
                                {
                                    xml_string = xml_string + "<ERROR>no records found</ERROR>";

                                }
                                reader.Close();
                                cmd.Dispose();
                            }
                        }
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

    }
    [WebMethod]

    public XmlDocument insert_usertype(string userEmailId, string userPassword, string user_type)
    {

        string xml_string = "";

        string strSql;
        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<USER_TYPE>" + user_type + "</USER_TYPE>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            //  query database using sql client -google

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    //xml_string = "<XML>" +
                    //    "<REQUEST>" +

                    //    "<FIRST_NAME>" + user_type + "</FIRST_NAME>" +

                    //    "</REQUEST>";

                    strSql = "INSERT INTO ovms_user_type (user_type)VALUES ('" + user_type + "')";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>user inserted successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>user  not inserted</ERROR> </INSERT_STRING>";

                    }
                    //reader.Close();
                    cmd.Dispose();
                    //       output final



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
            };
        }


        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }


    [WebMethod]
    public XmlDocument set_user_type(string userEmailId, string userPassword, int usertype_id, string utype)
    {
        string xml_string = "";

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<USERID>" + usertype_id + "</USERID>" +
                                    "<USERTYPE>" + utype + "</USERTYPE>" +
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

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    if (utype != "")
                    {
                        string strSql = "update ovms_user_type set user_type ='" + utype + "' where utype_id='" + usertype_id + "' and active=1";

                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<RESPONSE_MESSAGE>saved successfully</RESPONSE_MESSAGE>";
                        }
                        else
                        {
                            xml_string = xml_string + "<RESPONSE_MESSAGE>Update not possible</RESPONSE_MESSAGE>";
                        }
                        cmd.Dispose();

                    }
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
    public XmlDocument ext_contract(string userEmailId, string userPassword, string employee_id)
    {

        string xml_string = "";

        string strSql;
        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<USER_TYPE>" + employee_id + "</USER_TYPE>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            //  query database using sql client -google

            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "update  ovms_employee_details set ext_requested=1 where employee_id='" + employee_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Request Sent</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Request not set</ERROR> </INSERT_STRING>";
                    }
                    cmd.Dispose();

                    //       output final



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
            };
        }


        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }

    [WebMethod]
    public XmlDocument delete_usertype(string userEmailId, string userPassword, int utype_id)
    {
        string xml_string = "";
        // query database using sql client -google

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<USERID>" + utype_id + "</USERID>" +
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
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "update ovms_user_type set  active=0 where utype_id ='" + utype_id + "'";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>deleted  successfully</RESPONSE_MESSAGE>";
                    }
                    else
                    {
                        xml_string = xml_string + "<RESPONSE_MESSAGE>Deletion not possible</RESPONSE_MESSAGE>";

                    }
                    cmd.Dispose();
                }
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        //  output final
        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;

    }

    //[WebMethod]
    //public XmlDocument get_employees(string userEmailId, string userPassword, string employee_id, string vendor_id, string fromdate, string enddate, String active)
    //{
    //    string xml_string = "";
    //    int rowid = 1;
    //    //query database using sql client - google

    //   //

    //    string errString = "";

    //    xml_string = "<XML>" +
    //                        "<REQUEST>" +
    //                           "<EMPLOYEEID>" + employee_id + "</EMPLOYEEID>" +
    //                                 "<VENDORID>" + vendor_id + "</VENDORID>" +
    //                                 "<FROMDATE>" + fromdate + "</FROMDATE>" +
    //                                 "<ENDDATE>" + enddate + "</ENDDATE>" +
    //                        "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {

    //        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();

    //                //xml_string = "<XML>" +
    //                //            "<REQUEST>" +
    //                //                "<EMPLOYEEID>" + employee_id + "</EMPLOYEEID>" +
    //                //                 "<VENDORID>" + vendor_id + "</VENDORID>" +
    //                //                 "<FROMDATE>" + fromdate + "</FROMDATE>" +
    //                //                 "<ENDDATE>" + enddate + "</ENDDATE>" +


    //                //                "</REQUEST>";
    //                string strSql = "select concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_id," +
    // " ed.first_name, em.employee_id,ed.first_name, ed.last_name, ed.email, ed.phone, ed.address1, ed.address2," +
    // " concat(ed.city, ', ', ed.province)location, ed.postal,ed.province,ed.country,ed.skype_id, ed.start_date, ed.end_date,ed.create_date," +
    //" ed.active,ed.securityID,ven.vendor_name,em.job_id, clt.client_name ,ja.std_pay_rate_from, ja.st_pay_rate_to, ja.st_bill_rate_to, ja.st_bill_rate_from," +
    //" ja.ot_pay_rate_to, ja.ot_pay_rate_from,ja.ot_factor_of_st, ja.ot_bill_rate_to, ja.ot_bill_rate_from, ja.markup, ja.job_accounting_id, ja.job_id," +
    //" ja.dt_bill_rate_to, ja.dt_bill_rate_from, ja.dbl_pay_rate_to, ja.dbl_pay_rate_from, ja.dbl_factor_of_st, ja.cost_allocation" +
    // " from ovms_employees as em join ovms_employee_details as ed on em.employee_id = ed.employee_id join ovms_vendors as ven on em.vendor_id = ven.vendor_id join ovms_clients as clt" +
    //   " on em.client_id = clt.client_id" +
    //   " join ovms_job_accounting as ja" +
    //   " on ja.job_id = em.job_id" +
    //    " where 1 = 1";
    //                if (employee_id != "" & employee_id != "0")
    //                {
    //                    strSql = strSql + " and  em.employee_id='" + employee_id + "' and em.vendor_id = " + vendor_id;
    //                }
    //                if (employee_id == "" & employee_id == "0")
    //                {
    //                    strSql = strSql + " and em.vendor_id = " + vendor_id;
    //                }
    //                if (fromdate != "")
    //                {
    //                    strSql = strSql + " and (start_date >= '" + fromdate + "' )";
    //                }
    //                if (enddate != "")
    //                {
    //                    strSql = strSql + " and (end_date <= '" + enddate + "')";
    //                }
    //                if (active != "")
    //                    strSql += " and em.active = " + active;

    //                try
    //                {
    //                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
    //                    {
    //                        //xml_string = xml_string + "<RESPONSE>";
    //                        using (SqlDataReader reader = cmd.ExecuteReader())
    //                        {
    //                            //if (reader.Read())
    //                            //{
    //                            while (reader.Read())
    //                            {
    //                                xml_string += "<EMPLOYEE_NAME_ID ID='" + rowid + "'>" +
    //                                    "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
    //                                    "<FIRSTNAME>" + reader["first_name"] + "</FIRSTNAME>" +
    //                                    "<LASTNAME>" + reader["last_name"] + "</LASTNAME>" +
    //                                    "<EMAIL>" + reader["email"] + "</EMAIL>" +
    //                                    "<PHONE>" + reader["phone"] + "</PHONE>" +
    //                                    "<ADDRESS1>" + reader["address1"] + "</ADDRESS1>" +
    //                                    "<ADDRESS2>" + reader["address2"] + "</ADDRESS2>" +
    //                                    "<LOCATION>" + reader["location"] + "</LOCATION>" +
    //                                    //"<PROVINCE>" + reader["province"] + "</PROVINCE>" +
    //                                    "<POSTAL>" + reader["postal"] + "</POSTAL>" +
    //                                    "<COUNTRY>" + reader["country"] + "</COUNTRY>" +
    //                                    "<SECURITY_ID>" + reader["securityID"] + "</SECURITY_ID>" +
    //                                    "<SKYPE>" + reader["skype_id"] + "</SKYPE>" +
    //                                    "<ACTIVE>" + ((reader["active"].ToString() == "1") ? "Working" : "Not Working") + "</ACTIVE>" +
    //                                    "<STARTDATE>" + reader["start_date"] + "</STARTDATE>" +
    //                                    "<ENDDATE>" + reader["end_date"] + "</ENDDATE>" +
    //                                    "<STANDARD_PAY_RATE_FROM>" + reader["std_pay_rate_from"] + "</STANDARD_PAY_RATE_FROM>" +
    //                                    "<STANDARD_PAY_RATE_TO>" + reader["st_pay_rate_to"] + "</STANDARD_PAY_RATE_TO>" +
    //                                "<STANDARD_BILL_RATE_FROM>" + reader["st_bill_rate_from"] + "</STANDARD_BILL_RATE_FROM>" +
    //                                 "<STANDARD_BILL_RATE_TO>" + reader["st_bill_rate_to"] + "</STANDARD_BILL_RATE_TO>" +
    //                                   "<OVERTIME_PAY_RATE_FROM>" + reader["ot_pay_rate_from"] + "</OVERTIME_PAY_RATE_FROM>" +
    //                                "<OVERTIME_PAY_RATE_TO>" + reader["ot_pay_rate_to"] + "</OVERTIME_PAY_RATE_TO>" +
    //                               "<OVERTIME_FACTOR_OF_STANDARD_TIME>" + reader["ot_factor_of_st"] + "</OVERTIME_FACTOR_OF_STANDARD_TIME>" +
    //                                 "<OVERTIME_BILL_RATE_FROM>" + reader["ot_bill_rate_from"] + "</OVERTIME_BILL_RATE_FROM>" +
    //                               "<OVERTIME_BILL_RATE_TO>" + reader["ot_bill_rate_to"] + "</OVERTIME_BILL_RATE_TO>" +
    //                              "<MARKUP>" + reader["markup"] + "</MARKUP>" +
    //                              "<JOB_ACCOUNTING_ID>" + reader["job_accounting_id"] + "</JOB_ACCOUNTING_ID>" +
    //                             "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
    //                             "<DOUBLE_BILL_RATE_FROM>" + reader["dt_bill_rate_from"] + "</DOUBLE_BILL_RATE_FROM>" +
    //                             "<DOUBLE_TIME_BILL_RATE_TO>" + reader["dt_bill_rate_to"] + "</DOUBLE_TIME_BILL_RATE_TO>" +
    //                                  "<DOUBLE_PAY_RATE_FROM>" + reader["dbl_pay_rate_from"] + "</DOUBLE_PAY_RATE_FROM>" +

    //                                "<DOUBLE_PAY_RATE_TO>" + reader["dbl_pay_rate_to"] + "</DOUBLE_PAY_RATE_TO>" +
    //                                "<DOUBLE_FACTOR_OF_STANDARD_TIME>" + reader["dbl_factor_of_st"] + "</DOUBLE_FACTOR_OF_STANDARD_TIME>" +
    //                                "<COST_ALLOCATION>" + reader["cost_allocation"] + "</COST_ALLOCATION>" +
    //                                    "</EMPLOYEE_NAME_ID>";

    //                            }

    //                        }
    //                        //}


    //                        //else
    //                        //{
    //                        //    xml_string = xml_string + "<ERROR>no records found</ERROR>";
    //                        //    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No employee Found");

    //                        //}

    //                    }
    //                    //xml_string = xml_string + "</RESPONSE> ";
    //                }

    //                catch (Exception ex)
    //                {
    //                    xml_string = xml_string + "<RESPONSE_MESSAGE>error:xxx.systemerror</RESPONSE_MESSAGE>";
    //                    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

    //                }

    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }
    //        }
    //    }


    //    //output final
    //    xml_string = xml_string + "</RESPONSE></XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;

    //}

    //[WebMethod]
    //public XmlDocument get_vendor(string vendorid)
    //{

    //   //
    //    SqlConnection conn;
    //    string xml_string = "";
    //    string strSub = "";
    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
    //    if (vendorid != "" & vendorid != "0")
    //    {
    //        strSub = " and vendor_id=" + vendorid;
    //    }
    //    try
    //    {
    //        if (conn.State == System.Data.ConnectionState.Closed)
    //        {
    //            conn.Open();
    //            xml_string +=
    //                 "<XML>" +
    //               "<REQUEST>" + "<VENDOR_ID>" + vendorid + "</VENDOR_ID>" +

    //                        "</REQUEST>";
    //            string strSql = "SELECT vm.vendor_name,vm.vendor_id, vd.vendor_address1, vd.vendor_address2,vd.vendor_city,vd.vendor_postal_code,vd.vendor_country,vd.vendor_PhoneNumber,vd.vendor_FaxNumber FROM ovms_vendor_details as vd join ovms_vendors as vm on vd.vendor_id = vm.vendor_id where vd.active=1 ";
    //            SqlCommand cmd = new SqlCommand(strSql, conn);
    //            SqlDataReader reader = cmd.ExecuteReader();
    //            xml_string += "<RESPONSE>";

    //            int RowID = 1;

    //            if (reader.HasRows == true)
    //            {


    //                while (reader.Read())


    //                {
    //                    xml_string += "<VENDOR_ID ID ='" + RowID + "'>" +
    //                    "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
    //               "<VENDOR_NAME>" + reader["vendor_name"] + "</VENDOR_NAME>" +
    //               "<VENDOR_ADDRESS1>" + reader["vendor_address1"] + "</VENDOR_ADDRESS1>" +
    //               "<VENDOR_ADDRESS2>" + reader["vendor_address2"] + "</VENDOR_ADDRESS2>" +
    //               "<VENDOR_CITY>" + reader["vendor_city"] + "</VENDOR_CITY>" +
    //               "<VENDOR_POSTAL_CODE>" + reader["vendor_postal_code"] + "</VENDOR_POSTAL_CODE>" +
    //               "<VENDOR_COUNTRY>" + reader["vendor_country"] + "</VENDOR_COUNTRY>" +
    //               "<VENDOR_PHONENUMBER>" + reader["vendor_PhoneNumber"] + "</VENDOR_PHONENUMBER>" +
    //               "<VENDOR_FAXNUMBER>" + reader["vendor_FaxNumber"] + "</VENDOR_FAXNUMBER>" +
    //               "</VENDOR_ID>";
    //                    RowID++;
    //                }
    //            }
    //            else
    //            {
    //                xml_string = xml_string + "<DATA>no records found</DATA>";
    //                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

    //        xml_string = "<XML>" +
    //                    "<REQUEST>" + vendorid + "</REQUEST>" +
    //                    "<RESPONSE> Unable to select vendor" +
    //                    "</RESPONSE>";
    //    }
    //    finally
    //    {
    //        if (conn.State == System.Data.ConnectionState.Open)
    //            conn.Close();
    //    }
    //    //}

    //    //else
    //    //{
    //    //    xml_string += "<VENDOE_ID>vendorid should not be null</VENDOE_ID>";
    //    //}

    //    xml_string += "</RESPONSE>" +
    //                  "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}
    //    [WebMethod]
    //    public XmlDocument search_employees(string first_name, string Last_name, string city, string country, string province, string postal, string skype_id, string vendor_id, string active)
    //    {
    //        string xml_string = "";
    //        //query database using sql client - google
    //       //
    //        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();


    //                xml_string = "<XML>" +
    //                            "<REQUEST>" +
    //                                "<VENDORID>" + vendor_id + "</VENDORID>" +
    //                                "<FIRSTNAME>" + first_name + "</FIRSTNAME>" +
    //                                 "<LASTNAME>" + Last_name + "</LASTNAME>" +
    //                                 "<CITY>" + city + "</CITY>" +
    //                                 "<COUNTRY>" + country + "</COUNTRY>" +
    //                                 "<PROVINCE>" + province + "</PROVINCE>" +
    //                                 "<POSTAL>" + postal + "</POSTAL>" +
    //                                 "<SKYPEID>" + skype_id + "</SKYPEID>" +
    //                                "</REQUEST>";
    //                string strSql = "select concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_id," +
    //"ed.first_name, ed.last_name, ed.email, ed.phone, ed.address1, ed.address2,ed.city, ed.province, ed.postal," +
    //"ed.province,ed.country,ed.skype_id,ed.start_date, ed.end_date,ed.active,ed.create_date,ed.active,ed.securityID,ven.vendor_name," +
    //"em.job_id ,clt.client_name, ja.std_pay_rate_from, ja.job_id,ja.st_bill_rate_to, ja.st_pay_rate_to, ja.markup, ja.ot_factor_of_st," +
    //"ja.ot_pay_rate_from, ja.ot_pay_rate_to, ja.ot_bill_rate_to,ja.job_accounting_id, ja.dbl_pay_rate_from, ja.dbl_pay_rate_to, ja.dbl_factor_of_st, ja.st_bill_rate_from," +
    //"ja.st_bill_rate_from, ja.ot_bill_rate_from, ja.ot_bill_rate_to,ja.dt_bill_rate_from, ja.dt_bill_rate_to, ja.cost_allocation" +
    //" from ovms_employees as em join ovms_employee_details as ed on" +
    //" em.employee_id = ed.employee_id join ovms_vendors as ven on" +
    //" em.vendor_id = ven.vendor_id join ovms_clients as clt on em.client_id = clt.client_id join ovms_job_accounting as ja on" +
    //" ja.job_id = em.job_id ";

    //                if (first_name != "")
    //                {
    //                    strSql = strSql + " and ed.first_name='" + first_name + "'";
    //                }
    //                if (Last_name != "")
    //                {
    //                    strSql = strSql + " and ed.last_name = '" + Last_name + "'";
    //                }
    //                if (city != "")
    //                {
    //                    strSql = strSql + " and ed.city= '" + city + "'";
    //                }
    //                if (country != "")
    //                {
    //                    strSql = strSql + " and ed.country= '" + country + "'";
    //                }
    //                if (province != "")
    //                {
    //                    strSql = strSql + " and ed.province= '" + province + "'";
    //                }
    //                if (postal != "")
    //                {
    //                    strSql = strSql + " and ed.postal= '" + postal + "'";
    //                }
    //                if (skype_id != "")
    //                {
    //                    strSql = strSql + " and ed.skype= '" + skype_id + "'";
    //                }
    //                if (vendor_id != "" & vendor_id != "0")
    //                {
    //                    strSql = strSql + " and em.vendor_id= " + vendor_id;
    //                }

    //                if (active != "")
    //                {
    //                    strSql += " and em.active = " + active;
    //                }
    //                try
    //                {
    //                    SqlCommand cmd = new SqlCommand(strSql, conn);
    //                    //using (SqlCommand cmd = new SqlCommand(strSql, conn))
    //                    //{
    //                    xml_string = xml_string + "<RESPONSE>";
    //                    SqlDataReader reader = cmd.ExecuteReader();
    //                    //using (SqlDataReader reader = cmd.ExecuteReader())
    //                    //{
    //                    if (reader.HasRows)
    //                    {

    //                        while (reader.Read())
    //                        {

    //                            xml_string = xml_string + "<EMPLOYEE_ID ID=\"" + reader["employee_id"] + "\">" +
    //                            "<FIRSTNAME>" + reader["first_name"] + "</FIRSTNAME>" +
    //                            "<LASTNAME>" + reader["last_name"] + "</LASTNAME>" +
    //                            "<EMAIL>" + reader["email"] + "</EMAIL>" +
    //                            "<PHONE>" + reader["phone"] + "</PHONE>" +
    //                            "<ADDRESS1>" + reader["address1"] + "</ADDRESS1>" +
    //                            "<ADDRESS2>" + reader["address2"] + "</ADDRESS2>" +
    //                            "<CITY>" + reader["city"] + "</CITY>" +
    //                            "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
    //                            "<POSTAL>" + reader["postal"] + "</POSTAL>" +
    //                            "<COUNTRY>" + reader["country"] + "</COUNTRY>" +
    //                            "<SKYPE>" + reader["skype_id"] + "</SKYPE>" +
    //                            "<STARTDATE>" + reader["start_date"] + "</STARTDATE>" +
    //                             "<ACTIVE>" + ((reader["active"].ToString() == "1") ? "Working" : "Not Working") + "</ACTIVE>" +

    //                            "<ENDDATE> " + reader["end_date"] + " </ENDDATE >" +
    //                            "<SECURITY_ID> " + reader["securityID"] + " </SECURITY_ID >" +
    //                            "<STANDARD_PAY_RATE_FROM>" + reader["std_pay_rate_from"] + "</STANDARD_PAY_RATE_FROM>" +
    //                             "<STANDARD_PAY_RATE_TO>" + reader["st_pay_rate_to"] + "</STANDARD_PAY_RATE_TO>" +
    //                             "<STANDARD_BILL_RATE_FROM>" + reader["st_bill_rate_from"] + "</STANDARD_BILL_RATE_FROM>" +
    //                             "<STANDARD_BILL_RATE_TO>" + reader["st_bill_rate_to"] + "</STANDARD_BILL_RATE_TO>" +
    //                                  "<OVERTIME_PAY_RATE_FROM>" + reader["ot_pay_rate_from"] + "</OVERTIME_PAY_RATE_FROM>" +
    //                               "<OVERTIME_PAY_RATE_TO>" + reader["ot_pay_rate_to"] + "</OVERTIME_PAY_RATE_TO>" +
    //                              "<OVERTIME_FACTOR_OF_STANDARD_TIME>" + reader["ot_factor_of_st"] + "</OVERTIME_FACTOR_OF_STANDARD_TIME>" +
    //                                "<OVERTIME_BILL_RATE_FROM>" + reader["ot_bill_rate_from"] + "</OVERTIME_BILL_RATE_FROM>" +
    //                              "<OVERTIME_BILL_RATE_TO>" + reader["ot_bill_rate_to"] + "</OVERTIME_BILL_RATE_TO>" +
    //                             "<MARKUP>" + reader["markup"] + "</MARKUP>" +
    //                             "<JOB_ACCOUNTING_ID>" + reader["job_accounting_id"] + "</JOB_ACCOUNTING_ID>" +
    //                            "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
    //                            "<DOUBLE_BILL_RATE_FROM>" + reader["dt_bill_rate_from"] + "</DOUBLE_BILL_RATE_FROM>" +
    //                            "<DOUBLE_TIME_BILL_RATE_TO>" + reader["dt_bill_rate_to"] + "</DOUBLE_TIME_BILL_RATE_TO>" +
    //                                 "<DOUBLE_PAY_RATE_FROM>" + reader["dbl_pay_rate_from"] + "</DOUBLE_PAY_RATE_FROM>" +

    //                               "<DOUBLE_PAY_RATE_TO>" + reader["dbl_pay_rate_to"] + "</DOUBLE_PAY_RATE_TO>" +
    //                               "<DOUBLE_FACTOR_OF_STANDARD_TIME>" + reader["dbl_factor_of_st"] + "</DOUBLE_FACTOR_OF_STANDARD_TIME>" +
    //                               "<COST_ALLOCATION>" + reader["cost_allocation"] + "</COST_ALLOCATION>" +

    //                            "</EMPLOYEE_ID > ";


    //                        }

    //                    }


    //                    else
    //                    {
    //                        xml_string = xml_string + "<ERROR>no records found</ERROR>";
    //                        //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No employee Found");

    //                    }

    //                    //}
    //                    xml_string = xml_string + "</RESPONSE> ";
    //                    //}
    //                }
    //                catch (Exception ex)
    //                {
    //                    xml_string = xml_string + "<RESPONSE>error:xxx.systemerror</RESPONSE>";
    //                    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

    //                }

    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }
    //        }

    //        //output final
    //        xml_string = xml_string + "</XML>";
    //        XmlDocument xmldoc;
    //        xmldoc = new XmlDocument();
    //        xmldoc.LoadXml(xml_string);

    //        return xmldoc;

    //    }
    //    [WebMethod]
    //    public XmlDocument set_employee(string first_name, string last_name, string email, string phone, string address1, string address2, string city, string province, string postal, string country, string skype_id, string startDate, string endDate, int job_id, int vendor_id, int client_id, string securityID)
    //    {
    //        //logAPI.Service logService = new logAPI.Service();
    //        string xml_string = "";
    //        int employee_id;

    //        //int newVid = 0;
    //        //query database using sql client - google
    //        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //            try
    //            {

    //                if (conn.State == System.Data.ConnectionState.Closed)
    //                {
    //                    conn.Open();
    //                    SqlCommand cmd;
    //                    xml_string = "<XML>" +

    //                                         "<REQUEST>" +
    //                                    "<FIRSTNAME>" + first_name + "</FIRSTNAME>" +
    //                                    "<LASTNAME>" + last_name + "</LASTNAME>" +
    //                                    "<EMAIL>" + email + "</EMAIL>" +
    //                                    "<PHONE>" + phone + "</PHONE>" +
    //                                    "<ADDRESS1>" + address1 + "</ADDRESS1>" +
    //                                    "<ADDRESS2>" + address2 + "</ADDRESS2>" +
    //                                    "<CITY>" + city + "</CITY>" +
    //                                    "<PROVINCE>" + province + "</PROVINCE>" +
    //                                    "<POSTAL>" + postal + "</POSTAL>" +
    //                                    "<COUNTRY>" + country + "</COUNTRY>" +
    //                                    "<SKYPE_ID>" + skype_id + "</SKYPE_ID>" +
    //                                    "<START_DATE>" + startDate + "</START_DATE>" +
    //                                    "<END_DATE>" + endDate + "</END_DATE>" +

    //                                     "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
    //                                    "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
    //                                    "<JOB_ID>" + job_id + "</JOB_ID>" +
    //                                    "<SECURITYID>" + securityID + "</SECURITYID>" +
    //                                   "</REQUEST>";

    //                    string strSql = " INSERT INTO ovms_employees(vendor_id,client_id,job_id) values (" + vendor_id + "," + client_id + "," + job_id + ")" +
    //                        "select cast(scope_identity() as int)";
    //                    cmd = conn.CreateCommand();
    //                    cmd.CommandText = strSql;
    //                    employee_id = (int)cmd.ExecuteScalar();

    //                    strSql = "INSERT INTO ovms_employee_details (employee_id,first_name,last_name,email,phone,address1,address2,city,province,postal,country,skype_id,start_date,end_date,securityID)" +
    //                        " VALUES(" + employee_id + ", '" + first_name + "', '" + last_name + "', '" + email + "', '" + phone + "', '" + address1 + "', '" + address2 + "', '" + city + "','" + province + "','" + postal + "','" + country + "','" + skype_id + "','" + startDate + "','" + endDate + "','" + securityID + "')";

    //                    //cmd.CommandText = strSql;
    //                    cmd = new SqlCommand(strSql, conn);
    //                    if (cmd.ExecuteNonQuery() > 0)
    //                    {
    //                        xml_string += "<RESPONSE>" +
    //                            "<INSERT_STRING>user inserted successfully</INSERT_STRING>" +

    //                            "</RESPONSE>";
    //                    }
    //                    else
    //                    {
    //                        xml_string += "<RESPONSE>" +
    //                                                 "<INSERT_STRING><ERROR>user  not inserted</ERROR> </INSERT_STRING>" +

    //                                                    "</RESPONSE>";

    //                    }

    //                    //       output final



    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine(e.Message);
    //            }
    //            finally
    //            {
    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            };



    //            xml_string = xml_string + "</XML>";
    //            XmlDocument xmldoc;
    //            xmldoc = new XmlDocument();
    //            xmldoc.LoadXml(xml_string);

    //            return xmldoc;

    //        }

    //    }

    [WebMethod]
    public XmlDocument update_user_type(string userEmailId, string userPassword, int employee_detail_id, int employee_id, string first_name, string last_name, string email, string phone,
       string address1, string address2, string city, string province, string postal, string country, string skype_id, string startDate, string endDate, int vendor_id, int client_id)
    {
        string xml_string = "";

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                    "<FIRSTNAME><![CDATA[" + first_name + "]]></FIRSTNAME>" +
                                    "<LASTNAME><![CDATA[" + last_name + "]]></LASTNAME>" +
                                    "<EMAIL>" + email + "</EMAIL>" +
                                    "<PHONE>" + phone + "</PHONE>" +
                                    "<ADDRESS1><![CDATA[" + address1 + "]]></ADDRESS1>" +
                                    "<ADDRESS2>" + address2 + "</ADDRESS2>" +
                                    "<CITY>" + city + "</CITY>" +
                                    "<PROVINCE>" + province + "</PROVINCE>" +
                                    "<POSTAL>" + postal + "</POSTAL>" +
                                    "<COUNTRY>" + country + "</COUNTRY>" +
                                    "<SKYPE_ID><![CDATA[" + skype_id + "]]></SKYPE_ID>" +
                                    "<START_DATE>" + startDate + "</START_DATE>" +
                                    "<END_DATE>" + endDate + "</END_DATE>" +
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

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "update ovms_employee_details set employee_id  =" + employee_id + ", first_name='" + first_name + "',last_name='" + last_name + "',email='" + email + "',phone='" + phone + "',address1='" + address1 + "',address2='" + address2 + "',city='" + city + "',province='" + province + "',postal='" + postal + "',country='" + country + "',skype_id='" + skype_id + "',start_date='" + startDate + "',end_date='" + endDate + "' where employee_details_id=" + employee_detail_id + "";
                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_MESSAGE>saved successfully</RESPONSE_MESSAGE>";
                    }

                    else
                    {
                        xml_string = xml_string + "<RESPONSE_MESSAGE>Update not possible</RESPONSE_MESSAGE>";
                    }
                    strSql = "update ovms_employees set vendor_id=" + vendor_id + ", client_id = " + client_id + "";

                    cmd = new SqlCommand(strSql, conn);

                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }


                // output final

            }
        }

        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    //[WebMethod]
    //public XmlDocument delete_employee(int employeeId)
    //{
    //    {
    //        string xml_string = "";
    //        string strSql;

    //        // int userid = 0;

    //        //query database using sql client - google

    //        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //        {
    //            // int FStatus = 0;
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();
    //                xml_string = "<XML>" +
    //                            "<REQUEST>" +
    //                                "<USERID>" + employeeId + "</USERID>" +
    //                            "</REQUEST>";

    //                strSql = " update ovms_employees set active=0 where employee_id= " + employeeId + ";" +
    //                    " update ovms_employee_details set active=0 where employee_id= " + employeeId + ";";
    //                SqlCommand cmd = new SqlCommand(strSql, conn);

    //                xml_string = xml_string + "<RESPONSE>";



    //                if (cmd.ExecuteNonQuery() > 0)
    //                {






    //                    xml_string += "<USERID>" + employeeId + "</USERID>";



    //                }
    //                else
    //                {
    //                    xml_string = xml_string + "<RESPONSE><ERROR>User does not exist</ERROR></RESPONSE>";


    //                }
    //                xml_string += "</RESPONSE>";




    //                if (conn.State == System.Data.ConnectionState.Open)
    //                    conn.Close();
    //            }
    //        }

    //        //output final
    //        xml_string = xml_string + "</XML>";
    //        XmlDocument xmldoc;
    //        xmldoc = new XmlDocument();
    //        xmldoc.LoadXml(xml_string);

    //        return xmldoc;

    //    }


    //}

    [WebMethod]

    public XmlDocument message_detail(string userEmailId, string userPassword, int message_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        string xml_string = "";
        string strSql;

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDORID>" + message_id + "</VENDORID>" +
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

                        strSql = "select m.vendor_id, v.vendor_name,m.Msg_Subject,dbo.GetDateDifference(m.create_date) createDate, " +
                                " SUBSTRING(m.message,1,14) message " +
                                " from ovms_messages  as m " +
                                " join ovms_vendors as v on m.vendor_id = v.vendor_id " +
                                " where m.message_id = " + message_id + " order by m.create_date desc; ";

                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        SqlDataReader reader = cmd.ExecuteReader();


                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                xml_string = xml_string + "<VENDORNAME><![CDATA[" + reader["vendor_name"] + "]]></VENDORNAME>" +
                                           "<SUBJECT><![CDATA[" + reader["Msg_Subject"] + "]]></SUBJECT>" +
                                           "<MESSAGE><![CDATA[" + reader["message"] + "]]></MESSAGE>" +
                                           "<CREATEDATE>" + reader["createDate"] + "</CREATEDATE>";
                            }
                        }
                        else
                        {
                            xml_string = xml_string + "<ERROR>Invalid Data</ERROR>";

                            //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "No User Found");
                        }
                        reader.Dispose();
                        cmd.Dispose();
                    }

                }


                catch (Exception ex)
                {
                    xml_string = xml_string + "<RESPONSE_MESSAGE>error:xxx.systemerror</RESPONSE_MESSAGE>";
                    //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

                }

            }

        }      //finally
               //{
               //    if (conn.State == System.Data.ConnectionState.Open)
               //        conn.close();
               //}

        xml_string = xml_string + "</RESPONSE></XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }


    //[WebMethod]
    //public XmlDocument Employee_TO_Vendor(string Msg_Subject, string message, string employee_id,string IsSend, string IsRend, string Actions)
    //{
    //   //
    //    string xml_string = "";
    //    //  int newclient_id = 0;
    //    //query database using sql client - google
    //    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
    //    {
    //        try
    //        {
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();
    //                xml_string = "<xml>" +

    //                                     "<request>" +
    //                                "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" + "</request>";

    //                string sql = "insert into ovms_messages(vendor_id,message,employee_id,Msg_Subject, User_id, Actions,IsRead,IsSend)" +
    //                             "values('" + vendor_id + "', '" + message + "','" + employee_id + "', '" + Msg_Subject + "','" + User_id + "', '" + "Send_E_P" + "','" + 0 + "', '" + 1 + "')";
    //                //insert into ovms_clients (client_name, pm_id, business_type_id)values('" + Msg_Subject + "','" + message + "') select cast(scope_identity() as int)";
    //                SqlCommand cmd = new SqlCommand(sql, conn);
    //                xml_string = xml_string + "<RESPONSE>";
    //                if (cmd.ExecuteNonQuery() != 0)
    //                {
    //                    xml_string += "<EMPLOYEE_SUBJECT>" + Msg_Subject + "</EMPLOYEE_SUBJECT>" +
    //                 "<EMPLOYEE_MESSAGE>" + message + "</EMPLOYEE_MESSAGE>" +
    //                 "<IS_SEND>" + 1 + "</IS_SEND>" +
    //                 "<IS_READ>" + 0 + "</IS_READ>" +
    //                  "<ACTIONS>" + "Send_E_P" + "</ACTIONS>";

    //                }
    //                else
    //                {
    //                    xml_string += "<client_name>Message not sent</client_name>";
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
    //            xml_string += "<INSERT_STRING>vendor not inserted</INSERT_STRING>";
    //            //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }


    //    }
    //    xml_string = xml_string + "</RESPONSE></xml>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}
    [WebMethod]
    public XmlDocument Msg_Emp_to_Vendor(string userEmailId, string userPassword, string vendor_id, string employee_id, string Msg_Subject, string message, string message_id)
    {
//       //
        string xml_string = "";
        //int new_Reply = 0;
        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                             "<SUBJECT><![CDATA[" + Msg_Subject + "]]></SUBJECT>" +
                                             "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                              "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +

                                               "<MESSAGE><![CDATA[" + message + "]]></MESSAGE>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            //query database using sql client - google
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
            {
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        string sql = "insert into ovms_messages(vendor_id, message, employee_id, Msg_Subject, SourceID, User_id, IsSend,IsRead, Actions)" +
                         " values('" + vendor_id + "','" + message + "','" + employee_id + "',(select Msg_Subject from ovms_messages where message_id = '" + message_id + "'),'" + message_id + "',1,1,1,'Send_E_V')";

                        SqlCommand cmd = new SqlCommand(sql, conn);

                        if (cmd.ExecuteNonQuery() >= 0)
                        {
                            xml_string += "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +

                                            "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                            "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                                             "<SUBJECT><![CDATA[" + Msg_Subject + "]]></SUBJECT>" +
                                            "<MESSAGE><![CDATA[" + message + "]]></MESSAGE>";
                        }
                        cmd.Dispose();
                        
                    }
                }
                catch (Exception ex)
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<INSERT_STRING>Message not sent</INSERT_STRING>";
                    //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }
        }
        xml_string = xml_string + "</RESPONSE></xml>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_All_Messages_From_emp_to_PMO(string userEmailId, string userPassword, string pmo_id)
    {

        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string strSub = "";
        string xml_string = "";

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<PMO_ID>" + pmo_id + "</PMO_ID>" +
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
            if (pmo_id != "" & pmo_id != "0")
            {
                strSub = " and msg.pmo_id=" + pmo_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    //xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                    //    "</REQUEST>";
                    string strSQL = "select msg.message_id, ed.first_name,  msg.Msg_Subject, SUBSTRING(msg.message,1,12) , dbo.GetDateDifference(msg.create_date) as createDate from ovms_messages as msg join ovms_employee_details as ed on msg.employee_id = ed.employee_id and msg.pmo_id=1 and msg.employee_id=1 order by createdate asc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;

                    //xml_string = xml_string + "<RESPONSE>";
                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(0).ToString() + "</MESSAGE_ID>" +
                                                  "<EMPLOYEE_NAME>" + reader.GetValue(1).ToString() + "</EMPLOYEE_NAME>" +
                                                  "<MESSAGE_SUBJECT>" + reader.GetValue(2).ToString() + "</MESSAGE_SUBJECT>" +
                                                  "<MESSAGE>" + reader.GetValue(3).ToString() + "</MESSAGE>" +
                                                  "<DATE>" + reader.GetValue(4).ToString() + "</DATE>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                    //xml_string = xml_string + "</RESPONSE>";

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string += "<RESPONSE_MESSAGE> Unable to select vendor id</RESPONSE_MESSAGE>";
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
    public XmlDocument get_All_Messages_From_EMP_TO_Vendor(string userEmailId, string userPassword, string vendor_id)
    {

        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string strSub = "";
        string xml_string = "";

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                               "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
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
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub = " and msg.pmo_id=" + vendor_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                    //xml_string += "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                    //    "</REQUEST>";
                    string strSQL = "select msg.message_id, ed.first_name,  msg.Msg_Subject, msg.message , dbo.GetDateDifference(msg.create_date) as createDate from ovms_messages as msg join ovms_employee_details as ed on msg.employee_id = ed.employee_id and msg.vendor_id='" + vendor_id + "' and msg.Actions='Send_E_V' order by createdate asc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;

                    //xml_string = xml_string + "<RESPONSE>";
                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(0).ToString() + "</MESSAGE_ID>" +
                                                  "<EMPLOYEE_NAME>" + reader.GetValue(1).ToString() + "</EMPLOYEE_NAME>" +
                                                  "<MESSAGE_SUBJECT>" + reader.GetValue(2).ToString() + "</MESSAGE_SUBJECT>" +
                                                  "<MESSAGE>" + reader.GetValue(3).ToString() + "</MESSAGE>" +
                                                  "<DATE>" + reader.GetValue(4).ToString() + "</DATE>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                    //xml_string = xml_string + "</RESPONSE>";

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string += "<RESPONSE_MESSAGE> Unable to select vendor id</RESPONSE>";
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
    public XmlDocument get_Jobs_Client(string userEmailId, string userPassword, string client_id, string vendor_id, string user_id)
    {

        string xml_string = "";
        // int userid = 0;
        //query database using sql client - google
        //logAPI.Service logService = new logAPI.Service();

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                               "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                    "<USER_ID>" + user_id + "</USER_ID>" +
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
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                  
                    string strSql = "   select job_id, job_status_id, job_title, client_id, department_id, " +
                                    "   no_of_openings, hired, urgent, job_Timezone, contract_start_date, contract_end_date " +
                                    "   from ovms_jobs " +
                                    "   where active = 1 and job_status_id = 2 ";
                    if (client_id != "")
                    {
                        strSql = strSql + " and client_id = " + client_id;
                    }
                    if (vendor_id != "")
                    {
                        strSql = strSql + " and vendor_id = " + vendor_id;
                    }
                    if (user_id != "")
                    {
                        strSql = strSql + " and user_id = " + user_id;
                    }
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(strSql, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows == true)
                                {
                                    while (reader.Read())
                                    {
                                        xml_string = xml_string + "<JOB_ID ID='" + reader["job_id"] + "'>" +
                                                   "<JOB_TITLE><![CDATA[" + reader["job_title"] + "]]></JOB_TITLE>" +
                                                   "<NO_OF_OPENINGS>" + reader["no_of_openings"] + "</NO_OF_OPENINGS>" +
                                                  "<JOB_TIMEZONE>" + reader["job_Timezone"] + "</JOB_TIMEZONE>" +
                                                  "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                                                  "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +
                                                   "<URGENT>" + reader["urgent"] + "</URGENT>" +
                                                   "</JOB_ID>";
                                    }
                                }
                                else
                                {
                                    xml_string = xml_string + "<ERROR>No data found</ERROR>";
                                }
                                //xml_string = xml_string + "</RESPONSE>";
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        xml_string = xml_string + "<RESPONSE_MESSAGE>error:100.systemerror</RESPONSE_MESSAGE>";
                        //logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

                    }
                }
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
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
    public XmlDocument get_All_Messages_For_PMO(string userEmailId, string userPassword, string pmo_id)
    {
        //logAPI.Service logService = new //logAPI.Service();

        SqlConnection conn;
        //string strSub = "";
        string xml_string = "";



        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        //if (pmo_id != "" & pmo_id != "0")
        //{
        //    strSub = " and msg.pmo_id=" + pmo_id;
        //}


        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                               "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

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
                    //xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                    //    "</REQUEST>";
                    string strSQL = "select msg.message_id, msg.pmo_id,msg.client_id,  msg.User_id,msg.vendor_id, msg.Msg_Subject, msg.message, dbo.GetDateDifference(msg.create_date) as createDate" +
                                    " from ovms_messages as msg" +
                                    " join ovms_pmo as pmo" +
                                    " on msg.pmo_id = pmo.pmo_id" +
                                    " where msg.pmo_id = '" + pmo_id + "'" +
                                    " order by createDate asc";
                    SqlCommand cmd = new SqlCommand(strSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;

                    //xml_string = xml_string + "<RESPONSE>";
                    // for (int j = 0; j < count; j++)
                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<MESSAGE_ID>" + reader.GetValue(0).ToString() + "</MESSAGE_ID>" +
                                                 "<PMO_ID>" + reader.GetValue(1).ToString() + "</PMO_ID>" +
                                                  "<CLIENT_ID>" + reader.GetValue(2).ToString() + "</CLIENT_ID>" +
                                                  "<USER_ID>" + reader.GetValue(3).ToString() + "</USER_ID>" +

                                                   "<VENDOR_ID>" + reader.GetValue(4).ToString() + "</VENDOR_ID>" +
                                                  "<MESSAGE_SUBJECT>" + reader.GetValue(5).ToString() + "</MESSAGE_SUBJECT>" +
                                                  "<MESSAGE>" + reader.GetValue(6).ToString() + "</MESSAGE>" +
                                                  "<DATE>" + reader.GetValue(7).ToString() + "</DATE>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                    //xml_string = xml_string + "</RESPONSE>";

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string += "<RESPONSE_MESSAGE> Unable to select pmo id</RESPONSE_MESSAGE>";
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
    public XmlDocument get_Message(string userEmailId, string userPassword, string message_id, int IsSource)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;

        string xml_string = "";
        string strSub = "";



        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

        if (IsSource == 0)
        {
            //strSub = "select  message, create_date,message_id,sourceid from ovms_messages where message_id='" + message_id + "'";
            strSub = "select case when SUBSTRING(m.actions,6,1)='P' then concat(p.first_name,' '+p.last_name) when SUBSTRING(m.actions,6,1)='V' then v.vendor_name" +
                    " when SUBSTRING(m.actions, 6, 1)= 'C' then c.client_name when SUBSTRING(m.actions, 6, 1) = 'E' then concat(ed.first_name, ' ' + ed.last_name) end sender_name," +
                    " m.message, m.create_date,m.message_id,m.sourceid,m.actions, m.IsRead" +
                    " from ovms_messages as m" +
                    " left join ovms_pmo as p on m.pmo_id = p.pmo_id" +
                    " left join ovms_vendors as v on m.vendor_id = v.vendor_id" +
                    " left join ovms_clients as c on m.client_id = c.client_id" +
                    " left join ovms_employees as e on m.employee_id = e.employee_id" +
                    " left join ovms_employee_details as ed on e.employee_id = ed.employee_id" +
                    " where m.message_id = " + message_id;
        }
        else
        {
            //strSub = "with name_tree as ("+
            //       " select message_id, SourceID, message, create_date"+
            //       " from ovms_messages"+
            //       " where message_id = " + message_id + " /*this is the starting point you want in your recursion*/" +
            //       " union all"+
            //       " select c.message_id, c.SourceID, c.message,c.create_date"+
            //       " from ovms_messages c"+
            //       " join name_tree p on c.SourceID = p.message_id /*this is the recursion*/"+
            //       ")"+
            //       " select message,  create_date,message_id, sourceid"+
            //       " from name_tree"+
            //       " where message_id <> " + message_id + " order by message_id";

            strSub = "with name_tree as ( " +
                    " select message_id, pmo_id, vendor_id, client_id, employee_id, SourceID, message, create_date, actions,IsRead from ovms_messages where message_id =" + message_id + "/*this is the starting point you want in your recursion*/ " +
                    " union all" +
                    " select c.message_id,c.pmo_id,c.vendor_id,c.client_id,c.employee_id, c.SourceID, c.message,c.create_date,c.actions,c.IsRead from ovms_messages c join name_tree p on c.SourceID = p.message_id /*this is the recursion*/) " +
                    " select case when SUBSTRING(m.actions, 6, 1)= 'P' then concat(p.first_name, ' ' + p.last_name) when SUBSTRING(m.actions, 6, 1)= 'V' then v.vendor_name " +
                     " when SUBSTRING(m.actions, 6, 1) = 'C' then c.client_name when SUBSTRING(m.actions, 6, 1) = 'E' then concat(ed.first_name, ' ' + ed.last_name) end sender_name, " +
                      " m.message,m.create_date,m.message_id, m.sourceid,m.actions " +
                                       " from name_tree as m " +
                    " left join ovms_pmo as p on m.pmo_id = p.pmo_id " +
                   " left join ovms_vendors as v on m.vendor_id = v.vendor_id " +
                    " left join ovms_clients as c on m.client_id = c.client_id " +
                   " left join ovms_employees as e on m.employee_id = e.employee_id " +
                   " left join ovms_employee_details as ed on e.employee_id = ed.employee_id " +
                   " where m.message_id <> " + message_id + "" +
                   " order by m.message_id";
        }

        string errString = "";

        xml_string = "<XML>" +
                            "<REQUEST>" +
                               "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);

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
                    //xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
                    //    "</REQUEST>";
                    //string strSQL = "select  message, create_date from ovms_messages where message_id='"+message_id+"'";
                    //string strSQL = "select  message, create_date from ovms_messages where " + strSub ;

                    SqlCommand cmd = new SqlCommand(strSub, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 1;


                    while (reader.Read())
                    {
                        // reader.Read();
                        xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
                                                "<SENDER_NAME>" + reader.GetValue(0).ToString() + "</SENDER_NAME>" +
                                                "<MESSAGE_ID>" + reader.GetValue(3).ToString() + "</MESSAGE_ID>" +
                                                "<MESSAGE>" + reader.GetValue(1).ToString() + "</MESSAGE>" +
                                                 "<DATE>" + reader.GetValue(2).ToString() + "</DATE>" +
                                                 "<ACTIONS>" + reader.GetValue(4).ToString() + "</ACTIONS>" +
                                                 "<ISREAD>" + reader.GetValue(5).ToString() + "</ISREAD>" +
                                                  "</MESSAGE>";
                        count++;
                    }
                    //xml_string = xml_string + "</RESPONSE>";

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
                xml_string += "<RESPONSE_MESSAGE> Unable to select</RESPONSE_MESSAGE>";
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
    //public XmlDocument get_Message(string message_id, string userEmailId, string userPassword)
    //{
    //   //

    //    SqlConnection conn;

    //    string xml_string = "";



    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);


    //    string errString = "";

    //    xml_string = "<XML>" +
    //                        "<REQUEST>" +
    //                           "<MESSAGE_ID>" + message_id + "</MESSAGE_ID>" +
    //                        "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {

    //        try
    //        {
    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();
    //                //xml_string += "<PMO_ID>" + pmo_id + "</PMO_ID>" +
    //                //    "</REQUEST>";
    //                string strSQL = "select  message from ovms_messages where message_id='" + message_id + "'";
    //                //strSQL = "UPDATE ovms_messages SET IsRead = 0 WHERE message_id = "+Message_id+"";             
    //                SqlCommand cmd = new SqlCommand(strSQL, conn);
    //                SqlDataReader reader = cmd.ExecuteReader();
    //                int count = 1;


    //                while (reader.Read())
    //                {
    //                    // reader.Read();
    //                    xml_string = xml_string + "<MESSAGE ID=\"" + count + "\">" +
    //                                            "<MESSAGES>" + reader.GetValue(0).ToString() + "</MESSAGES>" +
    //                                              //"<ISREAD>" + reader.GetValue(1).ToString() + "</ISREAD>" +

    //                                              "</MESSAGE>";
    //                    count++;
    //                }
    //                //xml_string = xml_string + "</RESPONSE>";

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //            xml_string += "<RESPONSE_MESSAGE> Unable to select</RESPONSE_MESSAGE>";
    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //    }

    //    //}

    //    //else
    //    //    {
    //    //    xml_string += "<UTYPE_ID>Vendor id should not be null</UTYPE_ID>";
    //    //    }
    //    //output final
    //    xml_string += "</RESPONSE></XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}
}