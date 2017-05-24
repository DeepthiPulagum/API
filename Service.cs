using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Xml;



[WebService(Namespace = "http://api.flentispro.com/service.asmx")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Service : System.Web.Services.WebService
{
    public Service()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public XmlDocument get_considered_candidate(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
    {
        // logAPI.Service logService = new logAPI.Service();
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
                    strSql = " select concat('W', clt.client_alias, '00', right('0000' + convert(varchar(4), em.employee_id), 4)) employee_id," +
                            "  em.user_id,ed.first_name, em.employee_id, ed.middle_name,ed.last_name, ed.comments, " +
                            "  ed.email, ed.date_of_birth, ed.phone,ed.suite_no, ed.address1, ed.address2,j.job_title," +
                             " ven.vendor_name,ed.ext_requested,  ed.licence_no, ed.city,ed.province," +
                             " concat(ed.city, ', ', ed.province)location,ed.profile_picture_path, ed.postal," +
                             " ed.province,ed.country,ed.active,ed.skype_id,   ed.availability_for_interview," +
                             "  ed.start_date,ed.Last_4_Digits_of_SSN_SIN,ed.pay_rate, ed.end_date,ed.create_date, " +
                              "  ed.active,ven.vendor_name,em.job_id, clt.client_name,eact.interview_requested,eact.candidate_rejected,eact.action_id, " +
                               "  eact.more_info,   eact.interview_date, eact.interview_time, eact.reason_of_rejection,eact.vendor_reject_candidate,eact.vendor_reject_candidate_comment,eact.interview_resheduled,eact.vendor_moreInfo_reply,  " +
                                 "eact.interview_request_comment,eact.interview_request_comment2,eact.interview_request_comment3,eact.interview_request_comment4,eact.interview_request_comment5, " +
                              " eact.interview_cancel_by_client,eact.interview_cancel_by_client_comment," +
                                 " eact.candidate_approve, eact.interview_confirm,  j.contract_start_date,j.contract_end_date,clt.client_name from" +

                                " ovms_employees as em  join ovms_employee_details as ed on em.employee_id = ed.employee_id" +

                                 " join ovms_vendors as ven on em.vendor_id = ven.vendor_id" +

                                  " join ovms_clients as clt on em.client_id = clt.client_id" +

                        " join ovms_jobs as j on j.job_id = em.job_id" +

                         "  left join ovms_employee_actions as eact on em.employee_id = eact.employee_id" +

                          "  where 1 = 1 and em.vendor_id = '" + vendor_id + "' and em.employee_id " +
                                    " in  (select employee_id from ovms_employee_actions as eact )";

                    // strSql = "select concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_id, " +
                    // " em.user_id,ed.first_name, em.employee_id, ed.middle_name,ed.last_name, ed.comments, ed.email, ed.date_of_birth, ed.phone," +
                    // "ed.suite_no, ed.address1, ed.address2,j.job_title,ven.vendor_name,ed.ext_requested,  ed.licence_no, ed.city,ed.province," +
                    // " concat(ed.city, ', ', ed.province)location,ed.profile_picture_path, ed.postal,ed.province,ed.country,ed.active,ed.skype_id, " +
                    // "  ed.availability_for_interview, ed.start_date,ed.Last_4_Digits_of_SSN_SIN,ed.pay_rate, ed.end_date,ed.create_date, " +
                    // " ed.active,ven.vendor_name,em.job_id, clt.client_name,eact.interview_requested,eact.candidate_rejected, eact.more_info, " +
                    //"  eact.interview_date, eact.interview_time, eact.reason_of_rejection, eact.candidate_approve, eact.interview_confirm, " +
                    // " j.contract_start_date,j.contract_end_date from ovms_employees as em " +
                    // " join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
                    // " join ovms_vendors as ven on em.vendor_id = ven.vendor_id " +
                    // " join ovms_clients as clt on em.client_id = clt.client_id " +
                    // "  join ovms_jobs as j on j.job_id = em.job_id " +
                    //" left join ovms_employee_actions as eact on em.employee_id = eact.employee_id " +
                    // " where 1 = 1";
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


                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<EMPLOYEE_NAME_ID ID='" + RowID + "'>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                        "<FIRSTNAME>" + reader["first_name"] + "</FIRSTNAME>" +
                        "<MIDDLE_NAME>" + reader["middle_name"] + "</MIDDLE_NAME>" +
                        "<LASTNAME>" + reader["last_name"] + "</LASTNAME>" +
                        "<EMAIL>" + reader["email"] + "</EMAIL>" +
                        "<PHONE>" + reader["phone"] + "</PHONE>" +
                        "<DATE_OF_BIRTH>" + reader["date_of_birth"] + "</DATE_OF_BIRTH>" +
                        "<SUITE_NO>" + reader["suite_no"] + "</SUITE_NO>" +
                        "<ADDRESS1>" + reader["address1"] + "</ADDRESS1>" +
                        "<ADDRESS2>" + reader["address2"] + "</ADDRESS2>" +
                        "<LOCATION>" + reader["location"] + "</LOCATION>" +
                        "<VENDOR_NAME>" + reader["vendor_name"] + "</VENDOR_NAME>" +
                        "<JOB_TITLE>" + HttpContext.Current.Server.HtmlDecode(reader["job_title"].ToString()) + "</JOB_TITLE>" +
                      "<CITY>" + reader["city"] + "</CITY>" +
                        "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
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
                        "<INTERVIEW_REQUESTED>" + reader["interview_requested"] + "</INTERVIEW_REQUESTED>" +
                        "<CANDIDATE_REJECTED>" + reader["candidate_rejected"] + "</CANDIDATE_REJECTED>" +
                         "<MORE_INFO>" + reader["more_info"] + "</MORE_INFO>" +
                         "<INTERVIEW_DATE>" + reader["interview_date"] + "</INTERVIEW_DATE>" +
                         "<INTERVIEW_TIME>" + reader["interview_time"] + "</INTERVIEW_TIME>" +
                         "<REASON_OF_REJECTION>" + reader["reason_of_rejection"] + "</REASON_OF_REJECTION>" +
                         "<CANDIDATE_APPROVE>" + reader["candidate_approve"] + "</CANDIDATE_APPROVE>" +
                         "<INTERVIEW_CONFIRM>" + reader["interview_confirm"] + "</INTERVIEW_CONFIRM>" +
                           "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                             "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +
                             "<ACTIVE>" + reader["active"] + "</ACTIVE>" +
                             "<INTERVIEW_REQUEST_COMMENT>" + reader["interview_request_comment"] + "</INTERVIEW_REQUEST_COMMENT>" +
                             "<INTERVIEW_REQUEST_COMMENT2>" + reader["interview_request_comment2"] + "</INTERVIEW_REQUEST_COMMENT2>" +
                             "<INTERVIEW_REQUEST_COMMENT3>" + reader["interview_request_comment3"] + "</INTERVIEW_REQUEST_COMMENT3>" +
                             "<INTERVIEW_REQUEST_COMMENT4>" + reader["interview_request_comment4"] + "</INTERVIEW_REQUEST_COMMENT4>" +
                              "<INTERVIEW_REQUEST_COMMENT5>" + reader["interview_request_comment5"] + "</INTERVIEW_REQUEST_COMMENT5>" +
                              "<INTERVIEW_RESHEDULED>" + reader["interview_resheduled"] + "</INTERVIEW_RESHEDULED>" +
                               "<CLIENT_NAME>" + reader["client_name"] + "</CLIENT_NAME>" +
                                "<VENDOR_MOREINFO_REPLY><![CDATA[" + reader["vendor_moreInfo_reply"] + "]]></VENDOR_MOREINFO_REPLY>" +
                                 "<VENDOR_REJECT_CANDIDATE>" + reader["vendor_reject_candidate"] + "</VENDOR_REJECT_CANDIDATE>" +
                                "<VENDOR_REJECT_CANDIDATE_COMMENT><![CDATA[" + reader["vendor_reject_candidate_comment"] + "]]></VENDOR_REJECT_CANDIDATE_COMMENT>" +
                              "<ACTION_ID>" + reader["action_id"] + "</ACTION_ID>" +
                               "<INTERVIEW_CANCEL_BY_CLIENT>" + reader["interview_cancel_by_client"] + "</INTERVIEW_CANCEL_BY_CLIENT>" +
                                 "<INTERVIEW_CANCEL_BY_CLIENT_COMMENT><![CDATA[" + reader["interview_cancel_by_client_comment"] + "]]></INTERVIEW_CANCEL_BY_CLIENT_COMMENT>" +
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
    public XmlDocument candidate_reject_after_interview(string userEmailId, string userPassword, string action_id, string reject, DateTime reject_time)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<ACTION_ID>" + action_id + "</ACTION_ID>" +
                         "<APPROVE>" + reject + "</APPROVE>" +
                        "<APPROVE_TIME>" + reject_time + "</APPROVE_TIME>" +



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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET interview_date =null,interview_time=null,interview_requested=null ,interview_confirm=null,interview_resheduled=null," +
                                  "candidate_approve=null," +
                                   " vendor_reject_candidate=null,interview_cancel_by_client=null," +
                                  " candidate_rejected=1,candidate_reject_time='" + reject_time + "'" +

                                  " WHERE action_id = " + action_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Candidate rejected</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Not rejected</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument candidate_approve_after_interview(string userEmailId, string userPassword, string action_id, string approve, DateTime approve_time)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<ACTION_ID>" + action_id + "</ACTION_ID>" +
                         "<APPROVE>" + approve + "</APPROVE>" +
                        "<APPROVE_TIME>" + approve_time + "</APPROVE_TIME>" +



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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET interview_date =null,interview_time=null,interview_requested=null ,interview_confirm=null,interview_resheduled=null," +
                                  " vendor_reject_candidate=null,candidate_rejected=0,interview_cancel_by_client=null," +

                                  " candidate_approve=1,candidate_aprove_time='" + approve_time + "'" +

                                  " WHERE action_id = " + action_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Interview canceled</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Interview not canceled</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument interview_Reschedule(string userEmailId, string userPassword, string action_id, DateTime interview_date, string interview_time, string interview_requested, string interview_confirm, string reschedule_flag, string comment2, string comment3, string comment4, string comment5, string more_info_msg, string more_info_reply, string resch_time)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<ACTION_ID>" + action_id + "</ACTION_ID>" +
                         "<INTERVIEW_DATE>" + interview_date + "</INTERVIEW_DATE>" +
                        "<INTERVIEW_TIME>" + interview_time + "</INTERVIEW_TIME>" +
                        "<INTERVIEW_REQUESTED>" + interview_requested + "</INTERVIEW_REQUESTED>" +
                        "<INTERVIEW_CONFIRM>" + interview_confirm + "</INTERVIEW_CONFIRM>" +
                         "<RESCHEDULE_FLAG>" + reschedule_flag + "</RESCHEDULE_FLAG>" +
                         "<COMMENT2>" + comment2 + "</COMMENT2>" +
                         "<COMMENT3>" + comment3 + "</COMMENT3>" +
                         "<COMMENT4>" + comment4 + "</COMMENT4>" +
                          "<COMMENT5>" + comment5 + "</COMMENT5>" +
                           "<MORE_INFO_MSG>" + more_info_msg + "</MORE_INFO_MSG>" +
                          "<MORE_INFO_REPLY>" + more_info_reply + "</MORE_INFO_REPLY>" +
                           "<RESCH_TIME>" + resch_time + "</RESCH_TIME>" +


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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET interview_date ='" + interview_date + "',interview_time='" + interview_time + "',interview_requested=" + interview_requested + " ,interview_confirm=null,interview_resheduled=1," +
                                  " interview_request_comment2='" + comment2 + "',candidate_rejected=0,interview_cancel_by_client=null,vendor_reject_candidate=null,interview_request_comment3='" + comment3 + "',interview_request_comment4='" + comment4 + "',interview_request_comment5='" + comment5 + "', interview_rschedule_time='" + resch_time + "'," +
                                  "more_info=Null,vendor_moreInfo_reply=Null" +
                                  " WHERE action_id = " + action_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Interview resheduled</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Interview not resheduled</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument interview_cancel_by_client(string userEmailId, string userPassword, string action_id, string interview_date, string interview_time, string interview_requested, string interview_confirm, string reschedule_flag, string interview_cancel_by_client, string interview_cancel_by_client_time, string interview_cancel_by_client_comment)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<ACTION_ID>" + action_id + "</ACTION_ID>" +
                         "<INTERVIEW_DATE>" + interview_date + "</INTERVIEW_DATE>" +
                        "<INTERVIEW_TIME>" + interview_time + "</INTERVIEW_TIME>" +
                        "<INTERVIEW_REQUESTED>" + interview_requested + "</INTERVIEW_REQUESTED>" +
                        "<INTERVIEW_CONFIRM>" + interview_confirm + "</INTERVIEW_CONFIRM>" +
                         "<RESCHEDULE_FLAG>" + reschedule_flag + "</RESCHEDULE_FLAG>" +
                          "<INTERVIEW_CANCEL_BY_CLIENT>" + interview_cancel_by_client + "</INTERVIEW_CANCEL_BY_CLIENT>" +
                           "<INTERVIEW_CANCEL_BY_CLIENT_TIME>" + interview_cancel_by_client_time + "</INTERVIEW_CANCEL_BY_CLIENT_TIME>" +
                             "<INTERVIEW_CANCEL_BY_CLIENT_COMMENT>" + interview_cancel_by_client_comment + "</INTERVIEW_CANCEL_BY_CLIENT_COMMENT>" +



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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET interview_date =null,interview_time=null,interview_requested=null ,interview_confirm=null,interview_resheduled=null," +
                               "interview_cancel_by_client='" + interview_cancel_by_client + "', " +
                                  "interview_cancel_by_client_time='" + interview_cancel_by_client_time + "', " +
                                  " candidate_approve=null,candidate_aprove_time=null,interview_schedule_time=null," +
                                    " interview_cancel_by_client_comment='" + interview_cancel_by_client_comment + "'" +
                                  " WHERE action_id = " + action_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Interview canceled</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Interview not canceled</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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

    public string VerifyUser(string emailId, string userPassword)
    {
        //aakashService.Service aService = new aakashService.Service();
        Aakash aService = new Aakash();

        //UserInfo.Service userinf = new UserInfo.Service();
        XmlDocument _xUserInfo = new XmlDocument();
        _xUserInfo.LoadXml("<XML>" + aService.get_Login(emailId, userPassword).InnerXml.Replace("<XML>", "").Replace("</XML>", "") + "</XML>");

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
    //[WebMethod]
    //public XmlDocument interview_Reschedule(string userEmailId, string userPassword, string action_id, DateTime interview_date, string interview_time, string interview_requested, string interview_confirm, string reschedule_flag, string comment2, string comment3, string comment4, string comment5, string more_info_msg, string more_info_reply, string resch_time)
    //{
    //    SqlConnection conn;
    //    string xml_string = "";
    //    // logAPI.Service logService = new logAPI.Service();
    //    string errString = "";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    xml_string = "<XML>" +
    //                "<REQUEST>" +


    //                     "<ACTION_ID>" + action_id + "</ACTION_ID>" +
    //                     "<INTERVIEW_DATE>" + interview_date + "</INTERVIEW_DATE>" +
    //                    "<INTERVIEW_TIME>" + interview_time + "</INTERVIEW_TIME>" +
    //                    "<INTERVIEW_REQUESTED>" + interview_requested + "</INTERVIEW_REQUESTED>" +
    //                    "<INTERVIEW_CONFIRM>" + interview_confirm + "</INTERVIEW_CONFIRM>" +
    //                     "<RESCHEDULE_FLAG>" + reschedule_flag + "</RESCHEDULE_FLAG>" +
    //                     "<COMMENT2>" + comment2 + "</COMMENT2>" +
    //                     "<COMMENT3>" + comment3 + "</COMMENT3>" +
    //                     "<COMMENT4>" + comment4 + "</COMMENT4>" +
    //                      "<COMMENT5>" + comment5 + "</COMMENT5>" +
    //                       "<MORE_INFO_MSG>" + more_info_msg + "</MORE_INFO_MSG>" +
    //                      "<MORE_INFO_REPLY>" + more_info_reply + "</MORE_INFO_REPLY>" +
    //                       "<RESCH_TIME>" + resch_time + "</RESCH_TIME>" +


    //                "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
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
    //                string strSql = " UPDATE ovms_employee_actions" +
    //                              "  SET interview_date ='" + interview_date + "',interview_time='" + interview_time + "',interview_requested=" + interview_requested + " ,interview_confirm=null,interview_resheduled=1," +
    //                              " interview_request_comment2='" + comment2 + "',interview_request_comment3='" + comment3 + "',interview_request_comment4='" + comment4 + "',interview_request_comment5='" + comment5 + "', interview_rschedule_time='" + resch_time + "'," +
    //                              "more_info=Null,vendor_moreInfo_reply=Null" +
    //                              " WHERE action_id = " + action_id + "";




    //                SqlCommand cmd = new SqlCommand(strSql, conn);

    //                if (cmd.ExecuteNonQuery() > 0)
    //                {
    //                    xml_string += "<INSERT_STRING>Interview resheduled</INSERT_STRING>";
    //                }
    //                else
    //                {
    //                    xml_string += "<INSERT_STRING><ERROR>Interview not resheduled</ERROR> </INSERT_STRING>";

    //                }
    //                cmd.Dispose();

    //            }
    //        }

    //        catch (Exception ex)
    //        {
    //            xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //    }
    //    xml_string += "</RESPONSE>" +
    //                         "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}
    ////[WebMethod]
    //public XmlDocument interview_Reschedule(string userEmailId, string userPassword, string action_id, DateTime interview_date, string interview_time, string interview_requested, string interview_confirm, string reschedule_flag, string comment2, string comment3, string comment4, string comment5, string more_info_msg, string more_info_reply)
    //{
    //    SqlConnection conn;
    //    string xml_string = "";
    //    // logAPI.Service logService = new logAPI.Service();
    //    string errString = "";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    xml_string = "<XML>" +
    //                "<REQUEST>" +


    //                     "<ACTION_ID>" + action_id + "</ACTION_ID>" +
    //                     "<INTERVIEW_DATE>" + interview_date + "</INTERVIEW_DATE>" +
    //                    "<INTERVIEW_TIME>" + interview_time + "</INTERVIEW_TIME>" +
    //                    "<INTERVIEW_REQUESTED>" + interview_requested + "</INTERVIEW_REQUESTED>" +
    //                    "<INTERVIEW_CONFIRM>" + interview_confirm + "</INTERVIEW_CONFIRM>" +
    //                     "<RESCHEDULE_FLAG>" + reschedule_flag + "</RESCHEDULE_FLAG>" +
    //                     "<COMMENT2>" + comment2 + "</COMMENT2>" +
    //                     "<COMMENT3>" + comment3 + "</COMMENT3>" +
    //                     "<COMMENT4>" + comment4 + "</COMMENT4>" +
    //                      "<COMMENT5>" + comment5 + "</COMMENT5>" +
    //                       "<MORE_INFO_MSG>" + more_info_msg + "</MORE_INFO_MSG>" +
    //                      "<MORE_INFO_REPLY>" + more_info_reply + "</MORE_INFO_REPLY>" +


    //                "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
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
    //                string strSql = " UPDATE ovms_employee_actions" +
    //                              "  SET interview_date ='" + interview_date + "',interview_time='" + interview_time + "',interview_requested=" + interview_requested + " ,interview_confirm=null,interview_resheduled=1," +
    //                              " interview_request_comment2='" + comment2 + "',interview_request_comment3='" + comment3 + "',interview_request_comment4='" + comment4 + "',interview_request_comment5='" + comment5 + "', " +
    //                              "more_info=Null,vendor_moreInfo_reply=Null" +
    //                              " WHERE action_id = " + action_id + "";




    //                SqlCommand cmd = new SqlCommand(strSql, conn);

    //                if (cmd.ExecuteNonQuery() > 0)
    //                {
    //                    xml_string += "<INSERT_STRING>Interview resheduled</INSERT_STRING>";
    //                }
    //                else
    //                {
    //                    xml_string += "<INSERT_STRING><ERROR>Interview not resheduled</ERROR> </INSERT_STRING>";

    //                }
    //                cmd.Dispose();

    //            }
    //        }

    //        catch (Exception ex)
    //        {
    //            xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //    }
    //    xml_string += "</RESPONSE>" +
    //                         "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}
    [WebMethod]
    public XmlDocument candidate_approve(string userEmailId, string userPassword, string client_id, string employee_id, string vendor_id, string approve, string job_id, string emp_enddate, string job_enddate, string approve_time)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +

                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                         "<APPROVE>" + approve + "</APPROVE>" +
                         "<JOB_ID>" + job_id + "</JOB_ID>" +
                            "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
                            "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +
                            "<APPROVE_TIME>" + approve_time + "</APPROVE_TIME>" +


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
                    string strSql = " INSERT INTO ovms_employee_actions" +
                                    " (client_id, employee_id,vendor_id, candidate_approve,candidate_enddate,job_enddate,job_id,candidate_aprove_time  " +
                                   " )" +
                                    " VALUES('" + client_id + "', '" + employee_id + "','" + vendor_id + "', '" + approve + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "','" + approve_time + "')";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Request sent successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Request not sent</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    //public XmlDocument candidate_approve(string userEmailId, string userPassword, string client_id, string employee_id, string approve, string job_id, string emp_enddate, string job_enddate)
    //{
    //    SqlConnection conn;
    //    string xml_string = "";
    //    // logAPI.Service logService = new logAPI.Service();
    //    string errString = "";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    xml_string = "<XML>" +
    //                "<REQUEST>" +

    //                     "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
    //                     "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
    //                     "<APPROVE>" + approve + "</APPROVE>" +
    //                     "<JOB_ID>" + job_id + "</JOB_ID>" +
    //                        "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
    //                        "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +


    //                "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
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
    //                string strSql = " INSERT INTO ovms_employee_actions" +
    //                                " (client_id, employee_id, candidate_approve,candidate_enddate,job_enddate,job_id  " +
    //                               " )" +
    //                                " VALUES('" + client_id + "', '" + employee_id + "', '" + approve + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "')";






    //                SqlCommand cmd = new SqlCommand(strSql, conn);

    //                if (cmd.ExecuteNonQuery() > 0)
    //                {
    //                    xml_string += "<INSERT_STRING>Request sent successfully</INSERT_STRING>";
    //                }
    //                else
    //                {
    //                    xml_string += "<INSERT_STRING><ERROR>Request not sent</ERROR> </INSERT_STRING>";

    //                }
    //                //dispose
    //                cmd.Dispose();
    //            }
    //        }

    //        catch (Exception ex)
    //        {
    //            xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //    }
    //    xml_string += "</RESPONSE>" +
    //                         "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}

    [WebMethod]
    public XmlDocument get_Job_Status(string jobStatusId, string userEmailId, string userPassword)
    {


        //logAPI.Service servicelog = new logAPI.Service();
        //servicelog.set_log();

        ////
        SqlConnection conn;

        string errString = "";
        string strSubSql = "";
        int rowCount = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<JOB_STATUS_ID>" + jobStatusId + "</JOB_STATUS_ID>" +
                            "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (jobStatusId != "" & jobStatusId != "0")
            {
                strSubSql = " and job_status_id=" + jobStatusId;
            }

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();


                    string strSql = "Select Job_Status,job_status_id from ovms_job_status where 1=1 " + strSubSql + " and active=1";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            xml_string += "<JOB_STATUS ID='" + rowCount + "'>" +
                                "<JOB_STATUS>" + reader["JOB_STATUS"] + "</JOB_STATUS>" +
                                "<JOB_STATUS_ID>" + reader["JOB_STATUS_ID"] + "</JOB_STATUS_ID>" +
                                "</JOB_STATUS>";
                        }

                    }
                    else
                    {
                        xml_string += "<JOB_STATUS>No records found</JOB_STATUS>";
                        //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, "No data found");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //string url = HttpContext.Current.Request.Url.AbsoluteUri;
                //// http://localhost:1302/TESTERS/Default6.aspx

                //string path = HttpContext.Current.Request.Url.AbsolutePath;
                //// /TESTERS/Default6.aspx

                //string host = HttpContext.Current.Request.Url.Host;
                //// localhost


                //logService.set_log(120, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);

                xml_string = "<ERROR>Error 120. Unable to select job status</ERROR>";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            //}
            //else
            //{
            //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
            //}
        }


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument set_job_status(string jobStatus, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new //logAPI.Service();
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                        "<JOB_STATUS>" + jobStatus + "</JOB_STATUS>" +
                        "</REQUEST>";
        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            SqlConnection conn;
            string strSql = "";


            if (jobStatus != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();

                        strSql = "Insert into ovms_job_status(job_status) values('" + jobStatus + "')";
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<INSERT_STRING>Job Status inserted successfully</INSERT_STRING>" +
                                       "<INSERT_VALUE>1</INSERT_VALUE>";
                        }
                        else
                        {
                            xml_string += "<INSERT_STRING>Job Status not inserted</INSERT_STRING>" +
                                        "<INSERT_VALUE>0</INSERT_VALUE>";
                            //logService.set_log(121, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert new job status");
                        }

                        //dispose
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
    public XmlDocument update_job_status(int intJobStatusID, string jobStatus, string userEmailId, string userPassword)
    {
        ////


        SqlConnection conn;
        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                        "<JOB_STATUS>" + jobStatus + "</JOB_STATUS>" +
                        "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (intJobStatusID > 0 & jobStatus != "")
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        strSql = "update ovms_job_status set job_status='" + jobStatus + "' where job_status_id=" + intJobStatusID;
                        SqlCommand cmd = new SqlCommand(strSql, conn);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<UPDATE_STRING>Job Status updated successfully</UPDATE_STRING>" +
                                       "<UPDATE_VALUE>1</UPDATE_VALUE>";
                        }
                        else
                        {
                            xml_string += "<UPDATE_STRING>Job Status not updated</UPDATE_STRING>" +
                                        "<UPDATE_VALUE>0</UPDATE_VALUE>";
                            //logService.set_log(122, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update job status");
                        }

                        //dispose
                        cmd.Dispose();


                    }
                }
                catch (Exception ex)
                {
                    //logService.set_log(122, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument delete_job_status(int intJobStatusID, string userEmailId, string userPassword)
    {
        //  l/ogAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                        "<JOB_STATUS_ID>" + intJobStatusID + "</JOB_STATUS_ID>" +
                        "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (intJobStatusID > 0)
            {
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
                try
                {
                    if (conn.State == System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                        strSql = "update ovms_job_status set active=0 where job_status_id=" + intJobStatusID;
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        //xml_string += "<RESPONSE>";
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<DELETE_STRING>Job Status deleted successfully</DELETE_STRING>" +
                                       "<DELETE_VALUE>1</DELETE_VALUE>";
                        }
                        else
                        {
                            xml_string += "<DELETE_STRING>Job Status not deleted</DELETE_STRING>" +
                                        "<DELETE_VALUE>0</DELETE_VALUE>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete job status");
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
        }

        xml_string += "</RESPONSE>";
        xml_string += "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument insert_feedback(string userEmailId, string userPassword, string comments, string vendor_id, string client_id, string job_id, string employee_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<COMMENTS><![CDATA[" + comments + "]]></COMMENTS>" +
                         "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                    "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                                     "<JOB_ID>" + job_id + "</JOB_ID>" +
                                    "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
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


                        string sql = "INSERT INTO ovms_candidate_feedback (v_comments,vendor_id,client_id,job_id,emplyee_id)VALUES('" + comments + "','" + vendor_id + "','" + client_id + "','" + job_id + "','" + employee_id + "') ";
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
    public XmlDocument get_candidate(string userEmailId, string userPassword, string vendor_id, string client_id)
    {
        //        logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                  + "<REQUEST>"
                  + "<JOB_POSITION_TYPE_ID>" + vendor_id + "</JOB_POSITION_TYPE_ID>"
                  + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub = strSub + " and em.vendor_id=" + vendor_id;
            }
            if (client_id != "" & client_id != "0")
            {
                strSub = strSub + " and em.client_id=" + client_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select distinct ed.employee_id, em.create_date,concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_full_id,dbo.GetJobNo(em.job_id)job_alias, " +
                                    "(select job_title from ovms_jobs where job_id = (select job_id from ovms_employees where employee_id = ed.employee_id)) as job_Title,  " +
                                    "ed.start_date,ed.end_date, dbo.CamelCase(ed.First_Name) as First_Name, " +
                                    "dbo.CamelCase(ed.Last_Name) as Last_Name,  em.vendor_id, em.client_Id,(ed.city+ ' ' +ed.province)location,  em.job_id,em.user_id " +
                                    "from ovms_employee_details ed, " +
                                    "ovms_employees em, ovms_clients  clt " +
                                    "where ed.employee_id = em.employee_id and em.client_id = clt.client_id  " +
                                    "and em.employee_id not in (select employee_id from ovms_employee_actions as ea where  active = 1 " + strSub + "and candidate_rejected = 0)" + strSub +
                                    "order by em.create_date desc";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {


                        while (reader.Read())


                        {
                            xml_string += "<EMPLOYEE_NAME_ID ID='" + RowID + "'>" +
                                            "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                            "<FIRSTNAME>" + reader["first_name"] + "</FIRSTNAME>" +
                                            "<LASTNAME>" + reader["last_name"] + "</LASTNAME>" +
                                          "<LOCATION>" + reader["location"] + "</LOCATION>" +
                                          "<EMPLOYEE_FULL_ID>" + reader["employee_full_id"] + "</EMPLOYEE_FULL_ID>" +
                                            "<JOB_ALIAS>" + reader["job_alias"] + "</JOB_ALIAS>" +
                                            "<JOB_TITLE>" + reader["job_Title"] + "</JOB_TITLE>" +
                                            "<START_DATE>" + reader["start_date"] + "</START_DATE>" +
                                            "<END_DATE>" + reader["end_date"] + "</END_DATE>" +
                                            "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                            "<CLIENT_ID>" + reader["client_Id"] + "</CLIENT_ID>" +
                                            "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                            "<USER_ID>" + reader["user_id"] + "</USER_ID>" +
                                            "</EMPLOYEE_NAME_ID>";
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
    public XmlDocument get_worker(string userEmailId, string userPassword, string vendor_id, string client_id)
    {
        //        logAPI.Service logService = new //logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                  + "<REQUEST>"
                  + "<JOB_POSITION_TYPE_ID>" + vendor_id + "</JOB_POSITION_TYPE_ID>"
                  + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub = strSub + " and em.vendor_id=" + vendor_id;
            }
            if (client_id != "" & client_id != "0")
            {
                strSub = strSub + " and em.client_id=" + client_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select distinct ed.employee_id, em.create_date,concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) employee_full_id,dbo.GetJobNo(em.job_id)job_alias, " +
                                    "(select job_title from ovms_jobs where job_id = (select job_id from ovms_employees where employee_id = ed.employee_id)) as job_Title,  " +
                                    "ed.start_date,ed.end_date, dbo.CamelCase(ed.First_Name) as First_Name, " +
                                    "dbo.CamelCase(ed.Last_Name) as Last_Name,  em.vendor_id, em.client_Id,(ed.city+ ' ' +ed.province)location,  em.job_id,em.user_id " +
                                    "from ovms_employee_details ed, " +
                                    "ovms_employees em, ovms_clients  clt " +
                                    "where ed.employee_id = em.employee_id and em.client_id = clt.client_id  " +
                                    "and em.employee_id  in (select employee_id from ovms_employee_actions as ea where  active = 1 " + strSub + "and candidate_approve=1)" + strSub +
                                    "order by em.create_date desc";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();


                    int RowID = 1;

                    if (reader.HasRows == true)
                    {


                        while (reader.Read())


                        {
                            xml_string += "<EMPLOYEE_NAME_ID ID='" + RowID + "'>" +
                                            "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                            "<FIRSTNAME>" + reader["first_name"] + "</FIRSTNAME>" +
                                            "<LASTNAME>" + reader["last_name"] + "</LASTNAME>" +
                                          "<LOCATION>" + reader["location"] + "</LOCATION>" +
                                          "<EMPLOYEE_FULL_ID>" + reader["employee_full_id"] + "</EMPLOYEE_FULL_ID>" +
                                            "<JOB_ALIAS>" + reader["job_alias"] + "</JOB_ALIAS>" +
                                            "<JOB_TITLE>" + reader["job_Title"] + "</JOB_TITLE>" +
                                            "<START_DATE>" + reader["start_date"] + "</START_DATE>" +
                                            "<END_DATE>" + reader["end_date"] + "</END_DATE>" +
                                            "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                            "<CLIENT_ID>" + reader["client_Id"] + "</CLIENT_ID>" +
                                            "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                            "<USER_ID>" + reader["user_id"] + "</USER_ID>" +
                                            "</EMPLOYEE_NAME_ID>";
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
    public XmlDocument get_v_feedback(string userEmailId, string userPassword, string job_id, string employee_id, string feedback_id, string vendor_id, string client_id)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                    "<REQUEST>" +
                    "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                    "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
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
            if (employee_id != "" & employee_id != "0")
            {
                strSub = "  emplyee_id=" + employee_id;
            }
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub += " and vendor_id=" + vendor_id;
            }
            if (client_id != "" & client_id != "0")
            {
                strSub += " and client_id=" + client_id;
            }
            if (job_id != "" & job_id != "0")
            {
                strSub += " and job_id=" + job_id;
            }
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select emplyee_id,job_id,v_comments,feeedback_id,c_comments,c_create_date,v_create_date,vendor_id,client_id from ovms_candidate_feedback where " + strSub;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<FEEDBACK ID ='" + RowID + "'>" +
                                            "<EMPLOYEE_ID>" + reader["emplyee_id"] + "</EMPLOYEE_ID>" +
                                            "<V_COMMENTS>" + reader["v_comments"] + "</V_COMMENTS>" +
                                            "<FEEEDBACK_ID>" + reader["feeedback_id"] + "</FEEEDBACK_ID>" +
                                            "<V_CREATE_DATE>" + reader["v_create_date"] + "</V_CREATE_DATE>" +
                                            "<C_COMMENTS>" + reader["c_comments"] + "</C_COMMENTS>" +
                                            "<C_CREATE_DATE>" + reader["c_create_date"] + "</C_CREATE_DATE>" +
                                            "<CLIENT_ID>" + reader["client_id"] + "</CLIENT_ID>" +
                                            "<VENDOR_ID>" + reader["vendor_id"] + " </VENDOR_ID>" +
                                            "</FEEDBACK>";
                            RowID++;
                        }
                        //close
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
    public XmlDocument insert_c_comments_feedback(string userEmailId, string userPassword, string job_id, string c_comment, string employee_id, string client_id, string vendor_id, string feedback_id)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                  "<REQUEST>" +
                                  "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                  "<C_COMMENT>" + c_comment + "</C_COMMENT>" +
                                   "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                                    "<FEEDBACK_ID>" + feedback_id + "</FEEDBACK_ID>" +
                                        "<JOB_ID>" + job_id + "</JOB_ID>" +
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

                    if (c_comment != "")
                    {
                        string strSql = "update ovms_candidate_feedback set c_comments ='" + c_comment + "',c_create_date='" + System.DateTime.Now + "' where emplyee_id =  '" + employee_id + "' and job_id='" + job_id + "'and client_id='" + client_id + "'and vendor_id='" + vendor_id + "'and feeedback_id='" + feedback_id + "' ";
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>feedback updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>feedback not updated</STRING>" +
                                        "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update resume_path");
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
    public XmlDocument preview_job(string jobId, string userEmailId, string userPassword, string vendorID, string userid, string clientId)
    {
        //  //

        SqlConnection conn;
        string errString = "";
        string strSub = "";
        string strSql = "";
        int Count = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<JOB_ID>" + jobId + "</JOB_ID>" +
                                "</REQUEST>";

        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string = xml_string + "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (jobId != "" & jobId != "0")
            {
                strSub = strSub + " and oj.job_id=" + jobId;
            }
            if (vendorID != "" & vendorID != "0")
            {
                //strSub = strSub + " and ov.vendor_id=" + vendorID;
                //strSub = strSub + " and ov.vendor_id=patindex('%" + vendorID + "%', vendors) ";
                // strSub = strSub + " and patindex('%" + vendorID + "%', vendors)>0 ";
            }
            if (userid != "" & userid != "0")
            {
                strSub = strSub + " and usr.User_id=" + userid;
            }
            if (clientId != "" & clientId != "0")
            {
                strSub = strSub + " and oc.client_id=" + clientId;
            }
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();


                    strSql = " (select distinct dbo.GetJobNo(oj.job_id) job_alias,oj.job_id,oj.create_date,(select comments from ovms_job_comments where job_id = oj.job_id) " +
                            "as comments,   oj.user_id, (select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname,  " +
                            " oj.job_title,  job_currency,oj.urgent,oj.job_timezone,oj.vendors ,ojd.Base_salary,ojd.Bonus,oj.contract_start_date,oj.Contract_end_date,     " +
                            " oj.job_status_id,os.job_status,oj.job_title,oj.no_of_openings,oj.hired,oj.hired,  " +
                            "  (oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  od.department_name,oj.client_id,oc.client_name, " +
                            "  op.job_position_type,op.position_type_id,oj.vendor_id,  " +

                            //Remove vendor names for all vendor
                            //" ov.vendor_name, " + 

                            "  ojd.posting_start_date,ojd.posting_end_date,    datediff(day, oj.create_date, GETDATE()) " +
                            "   recent,jl.address1,jl.job_location_id,  jl.post_code,   concat(jl.city, ', ', jl.province) " +
                            "  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,   jp.hiring_manager_name,  jp.coordinator,  " +
                            "   jp.Distributor,jp.creator,jp.create_date,jp.submit_date,   jp.max_submission_per_supplier, jp.auto_invoice_type,jp.purchase_order_number, " +
                            "   jp.reason_for_open,ja.markup,   ja.ot_bill_rate_from,ja.dbl_pay_rate_from, ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,  " +
                            "    ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,  " +
                            "    jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as " +
                            "    oj join ovms_job_status as os on oj.job_status_id = os.job_status_id join ovms_job_details as ojd " +
                            "   on ojd.job_id = oj.job_id    join ovms_departments as od on oj.department_id = od.department_id join ovms_clients as oc " +
                            "   on oj.client_id = oc.client_id  join ovms_users as usr on usr.client_id = oc.client_id " +
                            "   join ovms_job_position_type as op on oj.position_type_id = op.position_type_id " +

                            //
                            " join ovms_vendors as ov on ov.client_ID = oj.client_ID " +
                            "  left join ovms_job_posting_info as jp on oj.job_id = jp.job_id " +
                            "   left join ovms_job_accounting as ja on ja.job_id = oj.job_id " +
                            "   join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1 and oj.submit='0'" + strSub +

                            //
                            " and patindex('%" + vendorID + "%', vendors) >0  " +
                            "   and oj.active = 1 and ojd.active = 1 " +
                            "   and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1  and ov.active = 1 and oj.urgent = 1 " +
                            "  UNION select distinct dbo.GetJobNo(oj.job_id) job_alias,oj.job_id,oj.create_date,(select comments from ovms_job_comments where job_id = oj.job_id)  " +
                            "as comments,   oj.user_id, (select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname,   " +
                            " oj.job_title,  job_currency,'0' as urgent,oj.job_timezone,oj.vendors ,ojd.Base_salary,ojd.Bonus,  oj.contract_start_date,oj.Contract_end_date,   " +
                            " oj.job_status_id,os.job_status,oj.job_title,oj.no_of_openings,oj.hired,oj.hired,  " +
                            "  (oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  od.department_name,oj.client_id,oc.client_name,  " +
                            "  op.job_position_type,op.position_type_id,oj.vendor_id, " +

                            //Remove vendor names for all vendor
                            //" ov.vendor_name, " + 

                            "  ojd.posting_start_date,ojd.posting_end_date,    datediff(day, oj.create_date, GETDATE()) " +
                            "  recent,jl.address1,jl.job_location_id,  jl.post_code,   concat(jl.city, ', ', jl.province) " +
                            "  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,   jp.hiring_manager_name,  jp.coordinator,  " +
                            "  jp.Distributor,jp.creator,jp.create_date,jp.submit_date,   jp.max_submission_per_supplier, jp.auto_invoice_type,jp.purchase_order_number, " +
                            "  jp.reason_for_open,ja.markup,   ja.ot_bill_rate_from,ja.dbl_pay_rate_from, ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,  " +
                            "   ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,  " +
                            "    jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as " +
                            "   oj join ovms_job_status as os on oj.job_status_id = os.job_status_id join ovms_job_details as ojd " +
                            "   on ojd.job_id = oj.job_id    join ovms_departments as od on oj.department_id = od.department_id join ovms_clients as oc " +
                            "   on oj.client_id = oc.client_id  join ovms_users as usr on usr.client_id = oc.client_id " +
                            "   join ovms_job_position_type as op on oj.position_type_id = op.position_type_id " +

                            //
                            "   join ovms_vendors as ov on ov.client_ID = oj.client_ID " +
                            "   left join ovms_job_posting_info as jp on oj.job_id = jp.job_id " +
                            "   left join ovms_job_accounting as ja on ja.job_id = oj.job_id " +
                            "  join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1 and oj.submit='0'" + strSub +

                             //
                             " and patindex('%" + vendorID + "%', vendors) >0  " +
                            "   and oj.active = 1 and ojd.active = 1 " +
                            "  and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1 " +
                            "and ov.active = 1 and oj.urgent = 0)   " +
                            " order by urgent desc, oj.create_date desc";





                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    //xml_string += "<RESPONSE>";
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                            xml_string = xml_string + "<JOBS ID=\"" + Count + "\">" +
                                "<JOB_ID>" + reader["JOB_ID"] + "</JOB_ID>" +
                                    "<USERNAME>" + reader["firstname"] + "</USERNAME>" +
                                    "<JOB_STATUS_ID>" + reader["JOB_STATUS_ID"] + "</JOB_STATUS_ID>" +
                                    "<JOB_STATUS>" + reader["JOB_STATUS"] + "</JOB_STATUS>" +
                                    "<JOB_ALIAS>" + reader["JOB_ALIAS"] + "</JOB_ALIAS>" +
                                    "<JOB_TITLE><![CDATA[" + reader["JOB_TITLE"] + "]]></JOB_TITLE>" +
                                    "<NO_OF_OPENINGS>" + reader["NO_OF_OPENINGS"] + "</NO_OF_OPENINGS>" +
                                    "<DEPARTMENT_ID>" + reader["DEPARTMENT_ID"] + "</DEPARTMENT_ID>" +
                                    "<DEPARTMENT_NAME>" + reader["DEPARTMENT_NAME"] + "</DEPARTMENT_NAME>" +
                                    "<CLIENT_ID>" + reader["CLIENT_ID"] + "</CLIENT_ID>" +
                                    "<CLIENT_NAME><![CDATA[" + reader["CLIENT_NAME"] + "]]></CLIENT_NAME>" +
                        "<POSITION_TYPE_ID>" + reader["POSITION_TYPE_ID"] + "</POSITION_TYPE_ID>" +

                                    "<JOB_POSITION_TYPE>" + reader["JOB_POSITION_TYPE"] + "</JOB_POSITION_TYPE>" +
                                  //"<VENDOR_NAME><![CDATA[" + reader["VENDOR_NAME"] + "]]></VENDOR_NAME>" +
                                  "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                    "<POSTING_START_DATE>" + reader["posting_start_date"] + "</POSTING_START_DATE>" +
                                    "<URGENT>" + reader["urgent"] + "</URGENT>" +
                                    "<POSTING_END_DATE>" + reader["posting_end_date"] + "</POSTING_END_DATE>" +
                                    "<BASE_SALARY>" + reader["Base_salary"] + "</BASE_SALARY>" +
                                    "<BONUS>" + reader["Bonus"] + "</BONUS>" +
                                     "<JOB_LOCATION_ID>" + reader["job_location_id"] + "</JOB_LOCATION_ID>" +

                                    "<JOB_LOCATION>" + reader["job_location"] + "</JOB_LOCATION>" +
                                    "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
                                    "<HIRED>" + reader["hired"] + "</HIRED>" +
                                    "<ABLE_TO_MOVE>" + reader["able_to_move"] + "</ABLE_TO_MOVE>" +
                                    "<AVAILABLE_JOBS>" + reader["available_jobs"] + "</AVAILABLE_JOBS>" +
                                    "<RECENT>" + reader["RECENT"] + "</RECENT>" +
                                    "<TRAVEL_TIME>" + reader["travel_time"] + "</TRAVEL_TIME>" +
                                    "<HOURS_PER_DAY>" + reader["hours_per_day"] + "</HOURS_PER_DAY>" +
                                    "<HIRING_MANAGER_NAME>" + reader["hiring_manager_name"] + "</HIRING_MANAGER_NAME>" +
                                    "<COORDINATOR><![CDATA[" + reader["coordinator"] + "]]></COORDINATOR>" +
                                    "<DISTRIBUTOR><![CDATA[" + reader["distributor"] + "]]></DISTRIBUTOR>" +
                                    "<JOB_CURRENCY>" + reader["job_currency"] + "</JOB_CURRENCY>" +
                                    "<JOB_TIMEZONE>" + reader["job_timezone"] + "</JOB_TIMEZONE>" +
                                    "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                                    "<CONTRACT_END_DATE>" + reader["Contract_end_date"] + "</CONTRACT_END_DATE>" +
                                    "<CREATOR><![CDATA[" + reader["creator"] + "]]></CREATOR>" +
                                    "<CREATE_DATE>" + reader["create_date"] + "</CREATE_DATE>" +
                                    "<SUBMIT_DATE>" + reader["submit_date"] + "</SUBMIT_DATE>" +
                                    "<MAX_SUBMISSION_PER_SUPPLIER>" + reader["max_submission_per_supplier"] + "</MAX_SUBMISSION_PER_SUPPLIER>";


                            string strGetDescription = " select roles_and_responsibilities, Benifits from ovms_job_details where job_id = " + reader["JOB_ID"].ToString();
                            SqlCommand cmdGetDescription = new SqlCommand(strGetDescription, conn);
                            SqlDataReader readerGetDescription = cmdGetDescription.ExecuteReader();
                            if (readerGetDescription.HasRows == true)
                            {
                                while (readerGetDescription.Read())
                                {
                                    xml_string = xml_string + "<JOB_DESC><![CDATA[" + Server.HtmlDecode(readerGetDescription["roles_and_responsibilities"].ToString()) + "]]></JOB_DESC>" +
                                        "<BENIFITS><![CDATA[" + Server.HtmlDecode(readerGetDescription["Benifits"].ToString()) + "]]></BENIFITS>";
                                }
                            }
                            else
                            {
                                xml_string = xml_string + "<JOB_DESC>No Description found.</JOB_DESC>";

                            }
                            //close
                            readerGetDescription.Close();
                            cmdGetDescription.Dispose();

                            //GET vendor names
                            string strGetvendorName = "  select vendor_name from ovms_vendors where vendor_id = " + reader["vendor_id"] + "  ";
                            SqlCommand cmdGetVendorName = new SqlCommand(strGetvendorName, conn);
                            SqlDataReader readerGetVendorName = cmdGetVendorName.ExecuteReader();
                            if (readerGetVendorName.HasRows == true)
                            {
                                while (readerGetVendorName.Read())
                                {
                                    xml_string = xml_string + "<VENDOR_NAME><![CDATA[" + Server.HtmlDecode(readerGetVendorName["vendor_name"].ToString()) + "]]></VENDOR_NAME>";

                                }
                            }
                            //close
                            readerGetVendorName.Close();
                            cmdGetVendorName.Dispose();

                            xml_string = xml_string + "<STD_PAY_RATE>" + reader["std_pay_rate_from"] + "</STD_PAY_RATE>" +
                                    "<AUTO_INVOICE_TYPE>" + reader["auto_invoice_type"] + "</AUTO_INVOICE_TYPE>" +
                                    "<PURCHASE_ORDER_NUMBER>" + reader["purchase_order_number"] + "</PURCHASE_ORDER_NUMBER>" +
                                    "<REASON_FOR_OPEN>" + reader["reason_for_open"] + "</REASON_FOR_OPEN>" +
                                     "<INTERVIEW>" + reader["interview_requirement"] + "</INTERVIEW>" +
                                    "<MARK_UP>" + reader["markup"] + "</MARK_UP>" +
                                    "<STD_BILL_RATE>" + reader["st_bill_rate_from"] + "</STD_BILL_RATE>" +
                                    "<OVERTIME_PAY_RATE>" + reader["ot_pay_rate_from"] + "</OVERTIME_PAY_RATE>" +
                                    "<OVERTIME_BILL_RATE>" + reader["ot_bill_rate_from"] + "</OVERTIME_BILL_RATE>" +
                                    "<DOUBLE_PAY_RATE>" + reader["dbl_pay_rate_from"] + "</DOUBLE_PAY_RATE>" +
                                    "<DOUBLE_BILL_RATE>" + reader["dt_bill_rate_from"] + "</DOUBLE_BILL_RATE>" +
                                     "<VENDER_PAY_RATE>" + reader["vender_pay_rate"] + "</VENDER_PAY_RATE>" +
                                    "<VENDER_OT_PAY_RATE>" + reader["vender_ot_pay_rate"] + "</VENDER_OT_PAY_RATE>" +
                                    "<VENDER_DT_PAY_RATE>" + reader["vender_dt_pay_rate"] + "</VENDER_DT_PAY_RATE>" +
                                    "<COMMENTS><![CDATA[" + Server.HtmlDecode(reader["comments"].ToString()) + "]]></COMMENTS></JOBS>";
                            Count++;
                        }
                    //else
                    //{
                    //    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    //    xml_string += "<DATA>No records found</DATA>";
                    //    logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    //}
                    //dispose
                    reader.Close();
                    cmd.Dispose();


                }

            }
            catch (Exception ex)
            {
                //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string = xml_string + "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
        xml_string = "";
        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_Jobs(string jobId, string userEmailId, string userPassword, string vendorID, string userid, string clientId)
    {
        //  //

        SqlConnection conn;
        string errString = "";
        string strSub = "";
        string strSql = "";
        int Count = 1;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<JOB_ID>" + jobId + "</JOB_ID>" +
                                "</REQUEST>";

        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string = xml_string + "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (jobId != "" & jobId != "0")
            {
                strSub = strSub + " and oj.job_id=" + jobId;
            }
            if (vendorID != "" & vendorID != "0")
            {
                //strSub = strSub + " and ov.vendor_id=" + vendorID;
                //strSub = strSub + " and ov.vendor_id=patindex('%" + vendorID + "%', vendors) ";
                // strSub = strSub + " and patindex('%" + vendorID + "%', vendors)>0 ";
            }
            if (userid != "" & userid != "0")
            {
                strSub = strSub + " and usr.User_id=" + userid;
            }
            if (clientId != "" & clientId != "0")
            {
                strSub = strSub + " and oc.client_id=" + clientId;
            }
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();


                    strSql = " (select distinct dbo.GetJobNo(oj.job_id) job_alias,oj.job_id,oj.create_date,(select comments from ovms_job_comments where job_id = oj.job_id) " +
                            "as comments,   oj.user_id, (select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname,  " +
                            " oj.job_title,  job_currency,oj.urgent,oj.job_timezone,oj.vendors ,ojd.Base_salary,ojd.Bonus,oj.contract_start_date,oj.Contract_end_date,     " +
                            " oj.job_status_id,os.job_status,oj.job_title,oj.no_of_openings,oj.hired,oj.hired,  " +
                            "  (oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  od.department_name,oj.client_id,oc.client_name, " +
                            "  op.job_position_type,op.position_type_id,oj.vendor_id,  " +

                            //Remove vendor names for all vendor
                            //" ov.vendor_name, " + 

                            "  ojd.posting_start_date,ojd.posting_end_date,    datediff(day, oj.create_date, GETDATE()) " +
                            "   recent,jl.address1,jl.job_location_id,  jl.post_code,   concat(jl.city, ', ', jl.province) " +
                            "  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,   jp.hiring_manager_name,  jp.coordinator,  " +
                            "   jp.Distributor,jp.creator,jp.create_date,jp.submit_date,   jp.max_submission_per_supplier, jp.auto_invoice_type,jp.purchase_order_number, " +
                            "   jp.reason_for_open,ja.markup,   ja.ot_bill_rate_from,ja.dbl_pay_rate_from, ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,  " +
                            "    ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,  " +
                            "    jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as " +
                            "    oj join ovms_job_status as os on oj.job_status_id = os.job_status_id join ovms_job_details as ojd " +
                            "   on ojd.job_id = oj.job_id    join ovms_departments as od on oj.department_id = od.department_id join ovms_clients as oc " +
                            "   on oj.client_id = oc.client_id  join ovms_users as usr on usr.client_id = oc.client_id " +
                            "   join ovms_job_position_type as op on oj.position_type_id = op.position_type_id " +

                            //
                            " join ovms_vendors as ov on ov.client_ID = oj.client_ID " +
                            "  left join ovms_job_posting_info as jp on oj.job_id = jp.job_id " +
                            "   left join ovms_job_accounting as ja on ja.job_id = oj.job_id " +
                            "   join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1 and oj.submit='1'" + strSub +

                            //
                            " and patindex('%" + vendorID + "%', vendors) >0  " +
                            "   and oj.active = 1 and ojd.active = 1 " +
                            "   and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1  and ov.active = 1 and oj.urgent = 1 " +
                            "  UNION select distinct dbo.GetJobNo(oj.job_id) job_alias,oj.job_id,oj.create_date,(select comments from ovms_job_comments where job_id = oj.job_id)  " +
                            "as comments,   oj.user_id, (select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname,   " +
                            " oj.job_title,  job_currency,'0' as urgent,oj.job_timezone,oj.vendors ,ojd.Base_salary,ojd.Bonus,  oj.contract_start_date,oj.Contract_end_date,   " +
                            " oj.job_status_id,os.job_status,oj.job_title,oj.no_of_openings,oj.hired,oj.hired,  " +
                            "  (oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  od.department_name,oj.client_id,oc.client_name,  " +
                            "  op.job_position_type,op.position_type_id,oj.vendor_id, " +

                            //Remove vendor names for all vendor
                            //" ov.vendor_name, " + 

                            "  ojd.posting_start_date,ojd.posting_end_date,    datediff(day, oj.create_date, GETDATE()) " +
                            "  recent,jl.address1,jl.job_location_id,  jl.post_code,   concat(jl.city, ', ', jl.province) " +
                            "  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,   jp.hiring_manager_name,  jp.coordinator,  " +
                            "  jp.Distributor,jp.creator,jp.create_date,jp.submit_date,   jp.max_submission_per_supplier, jp.auto_invoice_type,jp.purchase_order_number, " +
                            "  jp.reason_for_open,ja.markup,   ja.ot_bill_rate_from,ja.dbl_pay_rate_from, ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,  " +
                            "   ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,  " +
                            "    jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as " +
                            "   oj join ovms_job_status as os on oj.job_status_id = os.job_status_id join ovms_job_details as ojd " +
                            "   on ojd.job_id = oj.job_id    join ovms_departments as od on oj.department_id = od.department_id join ovms_clients as oc " +
                            "   on oj.client_id = oc.client_id  join ovms_users as usr on usr.client_id = oc.client_id " +
                            "   join ovms_job_position_type as op on oj.position_type_id = op.position_type_id " +

                            //
                            "   join ovms_vendors as ov on ov.client_ID = oj.client_ID " +
                            "   left join ovms_job_posting_info as jp on oj.job_id = jp.job_id " +
                            "   left join ovms_job_accounting as ja on ja.job_id = oj.job_id " +
                            "  join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1 and oj.submit='1'" + strSub +

                             //
                             " and patindex('%" + vendorID + "%', vendors) >0  " +
                            "   and oj.active = 1 and ojd.active = 1 " +
                            "  and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1 " +
                            "and ov.active = 1 and oj.urgent = 0)   " +
                            " order by urgent desc, oj.create_date desc";





                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    //xml_string += "<RESPONSE>";
                    if (reader.HasRows)
                        while (reader.Read())
                        {
                            //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                            xml_string = xml_string + "<JOBS ID=\"" + Count + "\">" +
                                "<JOB_ID>" + reader["JOB_ID"] + "</JOB_ID>" +
                                    "<USERNAME>" + reader["firstname"] + "</USERNAME>" +
                                    "<JOB_STATUS_ID>" + reader["JOB_STATUS_ID"] + "</JOB_STATUS_ID>" +
                                    "<JOB_STATUS>" + reader["JOB_STATUS"] + "</JOB_STATUS>" +
                                    "<JOB_ALIAS>" + reader["JOB_ALIAS"] + "</JOB_ALIAS>" +
                                    "<JOB_TITLE><![CDATA[" + reader["JOB_TITLE"] + "]]></JOB_TITLE>" +
                                    "<NO_OF_OPENINGS>" + reader["NO_OF_OPENINGS"] + "</NO_OF_OPENINGS>" +
                                    "<DEPARTMENT_ID>" + reader["DEPARTMENT_ID"] + "</DEPARTMENT_ID>" +
                                    "<DEPARTMENT_NAME>" + reader["DEPARTMENT_NAME"] + "</DEPARTMENT_NAME>" +
                                    "<CLIENT_ID>" + reader["CLIENT_ID"] + "</CLIENT_ID>" +
                                    "<CLIENT_NAME><![CDATA[" + reader["CLIENT_NAME"] + "]]></CLIENT_NAME>" +
                        "<POSITION_TYPE_ID>" + reader["POSITION_TYPE_ID"] + "</POSITION_TYPE_ID>" +

                                    "<JOB_POSITION_TYPE>" + reader["JOB_POSITION_TYPE"] + "</JOB_POSITION_TYPE>" +
                                  //"<VENDOR_NAME><![CDATA[" + reader["VENDOR_NAME"] + "]]></VENDOR_NAME>" +
                                  "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                    "<POSTING_START_DATE>" + reader["posting_start_date"] + "</POSTING_START_DATE>" +
                                    "<URGENT>" + reader["urgent"] + "</URGENT>" +
                                    "<POSTING_END_DATE>" + reader["posting_end_date"] + "</POSTING_END_DATE>" +
                                    "<BASE_SALARY>" + reader["Base_salary"] + "</BASE_SALARY>" +
                                    "<BONUS>" + reader["Bonus"] + "</BONUS>" +
                                     "<JOB_LOCATION_ID>" + reader["job_location_id"] + "</JOB_LOCATION_ID>" +

                                    "<JOB_LOCATION>" + reader["job_location"] + "</JOB_LOCATION>" +
                                    "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
                                    "<HIRED>" + reader["hired"] + "</HIRED>" +
                                    "<ABLE_TO_MOVE>" + reader["able_to_move"] + "</ABLE_TO_MOVE>" +
                                    "<AVAILABLE_JOBS>" + reader["available_jobs"] + "</AVAILABLE_JOBS>" +
                                    "<RECENT>" + reader["RECENT"] + "</RECENT>" +
                                    "<TRAVEL_TIME>" + reader["travel_time"] + "</TRAVEL_TIME>" +
                                    "<HOURS_PER_DAY>" + reader["hours_per_day"] + "</HOURS_PER_DAY>" +
                                    "<HIRING_MANAGER_NAME>" + reader["hiring_manager_name"] + "</HIRING_MANAGER_NAME>" +
                                    "<COORDINATOR><![CDATA[" + reader["coordinator"] + "]]></COORDINATOR>" +
                                    "<DISTRIBUTOR><![CDATA[" + reader["distributor"] + "]]></DISTRIBUTOR>" +
                                    "<JOB_CURRENCY>" + reader["job_currency"] + "</JOB_CURRENCY>" +
                                    "<JOB_TIMEZONE>" + reader["job_timezone"] + "</JOB_TIMEZONE>" +
                                    "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                                    "<CONTRACT_END_DATE>" + reader["Contract_end_date"] + "</CONTRACT_END_DATE>" +
                                    "<CREATOR><![CDATA[" + reader["creator"] + "]]></CREATOR>" +
                                    "<CREATE_DATE>" + reader["create_date"] + "</CREATE_DATE>" +
                                    "<SUBMIT_DATE>" + reader["submit_date"] + "</SUBMIT_DATE>" +
                                    "<MAX_SUBMISSION_PER_SUPPLIER>" + reader["max_submission_per_supplier"] + "</MAX_SUBMISSION_PER_SUPPLIER>";


                            string strGetDescription = " select roles_and_responsibilities, Benifits from ovms_job_details where job_id = " + reader["JOB_ID"].ToString();
                            SqlCommand cmdGetDescription = new SqlCommand(strGetDescription, conn);
                            SqlDataReader readerGetDescription = cmdGetDescription.ExecuteReader();
                            if (readerGetDescription.HasRows == true)
                            {
                                while (readerGetDescription.Read())
                                {
                                    xml_string = xml_string + "<JOB_DESC><![CDATA[" + Server.HtmlDecode(readerGetDescription["roles_and_responsibilities"].ToString()) + "]]></JOB_DESC>" +
                                        "<BENIFITS><![CDATA[" + Server.HtmlDecode(readerGetDescription["Benifits"].ToString()) + "]]></BENIFITS>";
                                }
                            }
                            else
                            {
                                xml_string = xml_string + "<JOB_DESC>No Description found.</JOB_DESC>";

                            }
                            //close
                            readerGetDescription.Close();
                            cmdGetDescription.Dispose();

                            //GET vendor names
                            string strGetvendorName = "  select vendor_name from ovms_vendors where vendor_id = " + reader["vendor_id"] + "  ";
                            SqlCommand cmdGetVendorName = new SqlCommand(strGetvendorName, conn);
                            SqlDataReader readerGetVendorName = cmdGetVendorName.ExecuteReader();
                            if (readerGetVendorName.HasRows == true)
                            {
                                while (readerGetVendorName.Read())
                                {
                                    xml_string = xml_string + "<VENDOR_NAME><![CDATA[" + Server.HtmlDecode(readerGetVendorName["vendor_name"].ToString()) + "]]></VENDOR_NAME>";

                                }
                            }
                            //close
                            readerGetVendorName.Close();
                            cmdGetVendorName.Dispose();

                            xml_string = xml_string + "<STD_PAY_RATE>" + reader["std_pay_rate_from"] + "</STD_PAY_RATE>" +
                                    "<AUTO_INVOICE_TYPE>" + reader["auto_invoice_type"] + "</AUTO_INVOICE_TYPE>" +
                                    "<PURCHASE_ORDER_NUMBER>" + reader["purchase_order_number"] + "</PURCHASE_ORDER_NUMBER>" +
                                    "<REASON_FOR_OPEN>" + reader["reason_for_open"] + "</REASON_FOR_OPEN>" +
                                     "<INTERVIEW>" + reader["interview_requirement"] + "</INTERVIEW>" +
                                    "<MARK_UP>" + reader["markup"] + "</MARK_UP>" +
                                    "<STD_BILL_RATE>" + reader["st_bill_rate_from"] + "</STD_BILL_RATE>" +
                                    "<OVERTIME_PAY_RATE>" + reader["ot_pay_rate_from"] + "</OVERTIME_PAY_RATE>" +
                                    "<OVERTIME_BILL_RATE>" + reader["ot_bill_rate_from"] + "</OVERTIME_BILL_RATE>" +
                                    "<DOUBLE_PAY_RATE>" + reader["dbl_pay_rate_from"] + "</DOUBLE_PAY_RATE>" +
                                    "<DOUBLE_BILL_RATE>" + reader["dt_bill_rate_from"] + "</DOUBLE_BILL_RATE>" +
                                     "<VENDER_PAY_RATE>" + reader["vender_pay_rate"] + "</VENDER_PAY_RATE>" +
                                    "<VENDER_OT_PAY_RATE>" + reader["vender_ot_pay_rate"] + "</VENDER_OT_PAY_RATE>" +
                                    "<VENDER_DT_PAY_RATE>" + reader["vender_dt_pay_rate"] + "</VENDER_DT_PAY_RATE>" +
                                    "<COMMENTS><![CDATA[" + Server.HtmlDecode(reader["comments"].ToString()) + "]]></COMMENTS></JOBS>";
                            Count++;
                        }
                    //else
                    //{
                    //    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    //    xml_string += "<DATA>No records found</DATA>";
                    //    logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    //}
                    //dispose
                    reader.Close();
                    cmd.Dispose();


                }

            }
            catch (Exception ex)
            {
                //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string = xml_string + "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
        xml_string = "";
        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_message_count_interview_vendor(string userEmailId, string userPassword, string employee_id)
    {
        //logAPI.Service logService = ne/w logAPI.Service();
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        //int RowID = 1;
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                     "<REQUEST>" +
                     "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +

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

                    strSql = "select * from ovms_interview_messages where employee_id='" + employee_id + "' and isread_vendor!=1 and from_client=1";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<MESSAGE ID='" + RowID + "'>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                        "<ISREAD_VENDOR>" + reader["isread_vendor"] + "</ISREAD_VENDOR>" +
                        "<ISREAD_CLIENT>" + reader["isread_client"] + "</ISREAD_CLIENT>" +


                        "</MESSAGE>";
                        RowID = RowID + 1;
                    }
                    //dispose
                    reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
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
    public XmlDocument get_message_count_interview_client(string userEmailId, string userPassword, string employee_id)
    {
        //logAPI.Service logService = ne/w logAPI.Service();
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        //int RowID = 1;
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                     "<REQUEST>" +
                     "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +

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

                    strSql = "select * from ovms_interview_messages where employee_id='" + employee_id + "' and isread_client!=1 and from_vendor=1";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<MESSAGE ID='" + RowID + "'>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                        "<ISREAD_VENDOR>" + reader["isread_vendor"] + "</ISREAD_VENDOR>" +
                        "<ISREAD_CLIENT>" + reader["isread_client"] + "</ISREAD_CLIENT>" +


                        "</MESSAGE>";
                        RowID = RowID + 1;
                    }
                    //dispose
                    reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
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
    public XmlDocument message_has_been_read1(string userEmailId, string userPassword, string employee_id, string vendor_chek)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<VENDOR_CHEK>" + vendor_chek + "</VENDOR_CHEK>" +




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
                    string strSql = " update ovms_interview_messages set isread_vendor='" + vendor_chek + "' where employee_id='" + employee_id + "'";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>checked</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>not checked</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument message_has_been_read2(string userEmailId, string userPassword, string employee_id, string client_chek)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +

                           "<CLIENT_CHEK>" + client_chek + "</CLIENT_CHEK>" +



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
                    string strSql = " update ovms_interview_messages set isread_client='" + client_chek + "' where employee_id='" + employee_id + "'";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>checked</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>not checked</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument client_job_details_comments(string userEmailId, string userPassword, string job_id, string client_comments, DateTime commentTime)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<JOB_ID>" + job_id + "</JOB_ID>" +
                         "<CLIENT_COMMENTS>" + client_comments + "</CLIENT_COMMENTS>" +
                           "<COMMENTTIME>" + commentTime + "</COMMENTTIME>" +


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
                    string strSql = " INSERT INTO ovms_client_job_comment" +
                                    " (job_id,client_job_comment,client_comment_time )" +

                                    " VALUES('" + job_id + "', '" + client_comments + "','" + commentTime + "' )";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Comment added</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Comment is not addded</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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

    //  public XmlDocument get_Jobs(string jobId, string userEmailId, string userPassword, string vendorID, string userid, string clientId)
    //{
    //   //

    //    SqlConnection conn;
    //    string errString = "";
    //    string strSub = "";
    //    string strSql = "";
    //    int Count = 1;
    //    string xml_string = "<XML>" +
    //                        "<REQUEST>" +
    //                        "<JOB_ID>" + jobId + "</JOB_ID>" +
    //                            "</REQUEST>";

    //    xml_string += "<RESPONSE>";
    //    errString = VerifyUser(userEmailId, userPassword);
    //    if (errString != "")
    //    {
    //        xml_string += "<ERROR>" + errString + "</ERROR>";
    //    }
    //    else
    //    {
    //        if (jobId != "" & jobId != "0")
    //        {
    //            strSub = " and oj.job_id=" + jobId;
    //        }
    //        if (vendorID != "" & vendorID != "0")
    //        {
    //            strSub += " and ov.vendor_id=" + vendorID;
    //        }
    //        if (userid != "" & userid != "0")
    //        {
    //            strSub += " and usr.User_id=" + userid;
    //        }
    //        if (clientId != "" & clientId != "0")
    //        {
    //            strSub += " and oc.client_id=" + clientId;
    //        }
    //        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
    //        try
    //        {

    //            if (conn.State == System.Data.ConnectionState.Closed)
    //            {
    //                conn.Open();
    //                //strSql = "select dbo.GetJobNo(oj.job_id) job_alias,oj.job_id, job_currency,oj.urgent,oj.job_timezone, oj.contract_start_date, oj.Contract_end_date,oj.job_status_id," +
    //                //        "ojd.roles_and_responsibilities,os.job_status,oj.job_title,oj.no_of_openings,oj.department_id,od.department_name,oj.client_id," +
    //                //        "oc.client_name,op.job_position_type,oj.vendor_id,ov.vendor_name,ojd.posting_start_date,ojd.posting_end_date," +
    //                //        "concat(jl.address1, ', ', jl.post_code, ', ', jl.city, ', ', jl.province) job_location,ojd.travel_time," +
    //                //        "ojd.hours_per_day,jp.hiring_manager_name, jp.coordinator,jp.Distributor,jp.creator,jp.create_date,jp.submit_date," +
    //                //        "jp.max_submission_per_supplier,jp.auto_invoice_type,jp.purchase_order_number,jp.reason_for_open,jp.start_and_end_time," +
    //                //        "jp.interview_requirement,coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from" +
    //                //        " from ovms_jobs as oj" +
    //                //        " join ovms_job_status as os on oj.job_status_id = os.job_status_id" +
    //                //        " join ovms_job_details as ojd on ojd.job_id = oj.job_id" +
    //                //        " join ovms_departments as od on oj.department_id = od.department_id" +
    //                //        " join ovms_clients as oc on oj.client_id = oc.client_id" +
    //                //        " join ovms_job_position_type as op on oj.position_type_id = op.position_type_id" +
    //                //        " join ovms_vendors as ov on ov.vendor_id = oj.vendor_id" +
    //                //        " left join ovms_job_posting_info as jp on oj.job_id = jp.job_id" +
    //                //        " left join ovms_job_accounting as ja on ja.job_id = oj.job_id" +
    //                //        " join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1" + strSub +
    //                //        " and oj.active = 1 and ojd.active = 1 and os.active = 1 and od.active = 1 and oc.active = 1" +
    //                //        " and op.active = 1 and ov.active = 1 order by jp.submit_date desc";

    //                //strSql = "select dbo.GetJobNo(oj.job_id) job_alias,oj.job_id ,concat(usr.first_name, ' ', usr.last_name)" +
    //                //        "  username,usr.User_id,oj.job_title,  job_currency,oj.urgent,oj.job_timezone, " +
    //                //        "  oj.contract_start_date,oj.Contract_end_date,  oj.job_status_id,ojd.roles_and_responsibilities,os.job_status,oj.job_title,oj. " +
    //                //        "  no_of_openings,oj.hired,oj.hired,(oj.no_of_openings - oj.hired)available_jobs,  oj.department_id, " +
    //                //        "  od.department_name,oj.client_id,oc.client_name,op.job_position_type,oj.vendor_id,  ov.vendor_name," +
    //                //        "  ojd.posting_start_date,ojd.posting_end_date,datediff(day, oj.create_date, GETDATE()) recent,jl.address1," +
    //                //        "  jl.post_code, concat(jl.city, ', ', jl.province)  job_location,ojd.travel_time,ojd.hours_per_day,jp.hiring_manager_name," +
    //                //        "  jp.coordinator,  jp.Distributor,jp.creator,jp.create_date,jp.submit_date,jp.max_submission_per_supplier," +
    //                //        "  jp.auto_invoice_type,jp.purchase_order_number,jp.reason_for_open,ja.markup,ja.ot_bill_rate_from,ja.dbl_pay_rate_from," +
    //                //        "  ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,jp.interview_requirement,jc.comments,jp.start_and_end_time," +
    //                //        "  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as oj" +
    //                //        "  join ovms_job_status as os on oj.job_status_id = os.job_status_id" +
    //                //        "  join ovms_job_details as ojd on ojd.job_id = oj.job_id" +
    //                //        "  join ovms_job_comments as jc on oj.job_id = jc.job_id" +
    //                //        "  join ovms_departments as od on oj.department_id = od.department_id" +
    //                //        "  join ovms_clients as oc on oj.client_id = oc.client_id" +
    //                //        "  join ovms_users as usr  on usr.client_id = oc.client_id" +
    //                //        "  join ovms_job_position_type as op on oj.position_type_id = op.position_type_id" +
    //                //        "  join ovms_vendors as ov on ov.vendor_id = oj.vendor_id" +
    //                //        "  left join ovms_job_posting_info as jp on oj.job_id = jp.job_id" +
    //                //        "  left join ovms_job_accounting as ja on ja.job_id = oj.job_id" +
    //                //        "  join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id" +
    //                //        "  where 1 = 1  and ov.vendor_id = 1 and oj.active = 1 and ojd.active = 1" + strSub +
    //                //        "  and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1" +
    //                //        "  and ov.active = 1 order by jp.submit_date desc";


    //                //strSql = "select distinct  dbo.GetJobNo(oj.job_id) job_alias,oj.job_id ,oj.user_id, " +
    //                //        "(select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname," +
    //                //        "oj.job_title,  job_currency,oj.urgent,oj.job_timezone,  " +
    //                //        "oj.contract_start_date,oj.Contract_end_date,  oj.job_status_id,ojd.roles_and_responsibilities,os.job_status,oj.job_title,oj. " +
    //                //        "no_of_openings,oj.hired,oj.hired,(oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  " +
    //                //        "od.department_name,oj.client_id,oc.client_name,op.job_position_type,oj.vendor_id,  ov.vendor_name,   " +
    //                //        "ojd.posting_start_date,ojd.posting_end_date,datediff(day, oj.create_date, GETDATE()) recent,jl.address1,  " +
    //                //        "jl.post_code, concat(jl.city, ', ', jl.province)  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,jp.hiring_manager_name,  " +
    //                //        "jp.coordinator,  jp.Distributor,jp.creator,jp.create_date,jp.submit_date,jp.max_submission_per_supplier, " +
    //                //        "jp.auto_invoice_type,jp.purchase_order_number,jp.reason_for_open,ja.markup,ja.ot_bill_rate_from,ja.dbl_pay_rate_from, " +
    //                //        "ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,jc.comments, " +
    //                //        "jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as oj " +
    //                //        "join ovms_job_status as os on oj.job_status_id = os.job_status_id " +
    //                //        "join ovms_job_details as ojd on ojd.job_id = oj.job_id " +
    //                //        "join ovms_job_comments as jc on oj.job_id = jc.job_id " +
    //                //        "join ovms_departments as od on oj.department_id = od.department_id " +
    //                //        "join ovms_clients as oc on oj.client_id = oc.client_id  join ovms_users as usr " +
    //                //        "on usr.client_id = oc.client_id  join ovms_job_position_type as op " +
    //                //        "on oj.position_type_id = op.position_type_id  join ovms_vendors as ov " +
    //                //        "on ov.vendor_id = oj.vendor_id  left join ovms_job_posting_info as jp " +
    //                //        "on oj.job_id = jp.job_id  left join ovms_job_accounting as ja " +
    //                //        "on ja.job_id = oj.job_id  join ovms_job_locations as jl " +
    //                //        "on ojd.job_location_id = jl.job_location_id " +
    //                //        "where 1 = 1 " + strSub +
    //                //        "and oj.active = 1 and ojd.active = 1 " +
    //                //        "and os.active = 1 and od.active = 1  and oc.active = 1 and  " +
    //                //        "op.active = 1  and ov.active = 1 order by jp.submit_date desc";

    //                strSql = "(select distinct dbo.GetJobNo(oj.job_id) job_alias,oj.job_id,oj.create_date,  " +
    //                " oj.user_id, (select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname,  " +
    //                " oj.job_title,  job_currency,oj.urgent,oj.job_timezone,oj.vendors ,oj.contract_start_date,oj.Contract_end_date,   " +
    //                " oj.job_status_id,os.job_status,oj.job_title,oj.no_of_openings,oj.hired,oj.hired,  " +
    //                " (oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  od.department_name,oj.client_id,oc.client_name,  " +
    //                " op.job_position_type,oj.vendor_id,  ov.vendor_name,   ojd.posting_start_date,ojd.posting_end_date,  " +
    //                " datediff(day, oj.create_date, GETDATE()) recent,jl.address1,  jl.post_code,  " +
    //                " concat(jl.city, ', ', jl.province)  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,  " +
    //                " jp.hiring_manager_name,  jp.coordinator,  jp.Distributor,jp.creator,jp.create_date,jp.submit_date,  " +
    //                " jp.max_submission_per_supplier, jp.auto_invoice_type,jp.purchase_order_number,jp.reason_for_open,ja.markup,  " +
    //                " ja.ot_bill_rate_from,ja.dbl_pay_rate_from, ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,  " +
    //                " ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,jc.comments,   " +
    //                " jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as  " +
    //                " oj join ovms_job_status as os on oj.job_status_id = os.job_status_id join ovms_job_details as ojd  " +
    //                " on ojd.job_id = oj.job_id join ovms_job_comments as jc on oj.job_id = jc.job_id  " +
    //                " join ovms_departments as od on oj.department_id = od.department_id join ovms_clients as oc  " +
    //                " on oj.client_id = oc.client_id  join ovms_users as usr on usr.client_id = oc.client_id  " +
    //                " join ovms_job_position_type as op on oj.position_type_id = op.position_type_id  join ovms_vendors as  " +
    //                " ov on ov.vendor_id = oj.vendor_id  left join ovms_job_posting_info as jp on oj.job_id = jp.job_id  " +
    //                " left join ovms_job_accounting as ja on ja.job_id = oj.job_id  " +
    //                " join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1  " + strSub +
    //                " and oj.active = 1 and ojd.active = 1  " +
    //                " and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1  and ov.active = 1 and oj.urgent = 1  " +
    //                " UNION  " +
    //                " select distinct dbo.GetJobNo(oj.job_id) job_alias,oj.job_id,oj.create_date,  " +
    //                " oj.user_id, (select first_name + ' ' + last_name from ovms_users where user_id = oj.user_id) firstname,  " +
    //                " oj.job_title,  job_currency,'0' as urgent,oj.job_timezone,oj.vendors ,oj.contract_start_date,oj.Contract_end_date,   " +
    //                " oj.job_status_id,os.job_status,oj.job_title,oj.no_of_openings,oj.hired,oj.hired,  " +
    //                " (oj.no_of_openings - oj.hired)available_jobs,  oj.department_id,  od.department_name,oj.client_id,oc.client_name,  " +
    //                " op.job_position_type,oj.vendor_id,  ov.vendor_name,   ojd.posting_start_date,ojd.posting_end_date,  " +
    //                " datediff(day, oj.create_date, GETDATE()) recent,jl.address1,  jl.post_code,  " +
    //                " concat(jl.city, ', ', jl.province)  job_location,ojd.able_to_move,ojd.travel_time,ojd.hours_per_day,  " +
    //                " jp.hiring_manager_name,  jp.coordinator,  jp.Distributor,jp.creator,jp.create_date,jp.submit_date,  " +
    //                " jp.max_submission_per_supplier, jp.auto_invoice_type,jp.purchase_order_number,jp.reason_for_open,ja.markup,  " +
    //                " ja.ot_bill_rate_from,ja.dbl_pay_rate_from, ja.ot_pay_rate_from,ja.st_bill_rate_from,ja.dt_bill_rate_from,  " +
    //                " ja.vender_pay_rate,ja.vender_ot_pay_rate,ja.vender_dt_pay_rate,jp.interview_requirement,jc.comments,   " +
    //                " jp.start_and_end_time,  coalesce(ja.std_pay_rate_from, 0) std_pay_rate_from from ovms_jobs as  " +
    //                " oj join ovms_job_status as os on oj.job_status_id = os.job_status_id join ovms_job_details as ojd  " +
    //                " on ojd.job_id = oj.job_id join ovms_job_comments as jc on oj.job_id = jc.job_id  " +
    //                " join ovms_departments as od on oj.department_id = od.department_id join ovms_clients as oc  " +
    //                " on oj.client_id = oc.client_id  join ovms_users as usr on usr.client_id = oc.client_id  " +
    //                " join ovms_job_position_type as op on oj.position_type_id = op.position_type_id  join ovms_vendors as  " +
    //                " ov on ov.vendor_id = oj.vendor_id  left join ovms_job_posting_info as jp on oj.job_id = jp.job_id  " +
    //                " left join ovms_job_accounting as ja on ja.job_id = oj.job_id  " +
    //                " join ovms_job_locations as jl on ojd.job_location_id = jl.job_location_id where 1 = 1 " + strSub +
    //                " and oj.active = 1 and ojd.active = 1  " +
    //                " and os.active = 1 and od.active = 1  and oc.active = 1 and op.active = 1  and ov.active = 1 and urgent = 0)  " +
    //                " order by urgent desc,oj.create_date desc ";
    //                //dbo.GetJobNo(jb.job_id
    //                SqlCommand cmd = new SqlCommand(strSql, conn);
    //                SqlDataReader reader = cmd.ExecuteReader();
    //                //xml_string += "<RESPONSE>";
    //                if (reader.HasRows)
    //                    while (reader.Read())
    //                    {
    //                        //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
    //                        xml_string += "<JOBS ID=\"" + Count + "\">" +
    //                            "<JOB_ID>" + reader["JOB_ID"] + "</JOB_ID>" +
    //                                "<USERNAME>" + reader["firstname"] + "</USERNAME>" +
    //                                "<JOB_STATUS_ID>" + reader["JOB_STATUS_ID"] + "</JOB_STATUS_ID>" +
    //                                "<JOB_STATUS>" + reader["JOB_STATUS"] + "</JOB_STATUS>" +
    //                                "<JOB_ALIAS>" + reader["JOB_ALIAS"] + "</JOB_ALIAS>" +
    //                                "<JOB_TITLE><![CDATA[" + reader["JOB_TITLE"] + "]]></JOB_TITLE>" +
    //                                "<NO_OF_OPENINGS>" + reader["NO_OF_OPENINGS"] + "</NO_OF_OPENINGS>" +
    //                                "<DEPARTMENT_ID>" + reader["DEPARTMENT_ID"] + "</DEPARTMENT_ID>" +
    //                                "<DEPARTMENT_NAME>" + reader["DEPARTMENT_NAME"] + "</DEPARTMENT_NAME>" +
    //                                "<CLIENT_ID>" + reader["CLIENT_ID"] + "</CLIENT_ID>" +
    //                                "<CLIENT_NAME>" + reader["CLIENT_NAME"] + "</CLIENT_NAME>" +
    //                                "<JOB_POSITION_TYPE>" + reader["JOB_POSITION_TYPE"] + "</JOB_POSITION_TYPE>" +
    //                                "<VENDOR_NAME>" + reader["VENDOR_NAME"] + "</VENDOR_NAME>" +
    //                                "<POSTING_START_DATE>" + reader["posting_start_date"] + "</POSTING_START_DATE>" +
    //                                "<URGENT>" + reader["urgent"] + "</URGENT>" +
    //                                "<POSTING_END_DATE>" + reader["posting_end_date"] + "</POSTING_END_DATE>" +
    //                                "<JOB_LOCATION>" + reader["job_location"] + "</JOB_LOCATION>" +
    //                                 "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
    //                                "<HIRED>" + reader["hired"] + "</HIRED>" +
    //                                "<ABLE_TO_MOVE>" + reader["able_to_move"] + "</ABLE_TO_MOVE>" +
    //                                "<AVAILABLE_JOBS>" + reader["available_jobs"] + "</AVAILABLE_JOBS>" +
    //                                "<RECENT>" + reader["RECENT"] + "</RECENT>" +
    //                                "<TRAVEL_TIME>" + reader["travel_time"] + "</TRAVEL_TIME>" +
    //                                "<HOURS_PER_DAY>" + reader["hours_per_day"] + "</HOURS_PER_DAY>" +
    //                                "<HIRING_MANAGER_NAME>" + reader["hiring_manager_name"] + "</HIRING_MANAGER_NAME>" +
    //                                "<COORDINATOR>" + reader["coordinator"] + "</COORDINATOR>" +
    //                                "<DISTRIBUTOR>" + reader["distributor"] + "</DISTRIBUTOR>" +
    //                                "<JOB_CURRENCY>" + reader["job_currency"] + "</JOB_CURRENCY>" +
    //                                "<JOB_TIMEZONE>" + reader["job_timezone"] + "</JOB_TIMEZONE>" +
    //                                "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
    //                                "<CONTRACT_END_DATE>" + reader["Contract_end_date"] + "</CONTRACT_END_DATE>" +
    //                                "<CREATOR>" + reader["creator"] + "</CREATOR>" +
    //                                "<CREATE_DATE>" + reader["create_date"] + "</CREATE_DATE>" +
    //                                "<SUBMIT_DATE>" + reader["submit_date"] + "</SUBMIT_DATE>" +
    //                                "<MAX_SUBMISSION_PER_SUPPLIER>" + reader["max_submission_per_supplier"] + "</MAX_SUBMISSION_PER_SUPPLIER>";


    //                        string strGetDescription = " select roles_and_responsibilities from ovms_job_details where job_id = " + reader["JOB_ID"].ToString();
    //                        SqlCommand cmdGetDescription = new SqlCommand(strGetDescription, conn);
    //                        SqlDataReader readerGetDescription = cmdGetDescription.ExecuteReader();
    //                        if (readerGetDescription.HasRows == true)
    //                        {
    //                            while (readerGetDescription.Read())
    //                            {
    //                                xml_string = xml_string + "<JOB_DESC><![CDATA[" + readerGetDescription["roles_and_responsibilities"] + "]]></JOB_DESC>";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            xml_string = xml_string + "<JOB_DESC>No Description found.</JOB_DESC>";

    //                        }
    //                        //close
    //                        readerGetDescription.Close();
    //                        cmdGetDescription.Dispose();


    //                        xml_string = xml_string + "<STD_PAY_RATE>" + reader["std_pay_rate_from"] + "</STD_PAY_RATE>" +
    //                                "<AUTO_INVOICE_TYPE>" + reader["auto_invoice_type"] + "</AUTO_INVOICE_TYPE>" +
    //                                "<PURCHASE_ORDER_NUMBER>" + reader["purchase_order_number"] + "</PURCHASE_ORDER_NUMBER>" +
    //                                "<REASON_FOR_OPEN>" + reader["reason_for_open"] + "</REASON_FOR_OPEN>" +
    //                                 "<INTERVIEW>" + reader["interview_requirement"] + "</INTERVIEW>" +
    //                                "<MARK_UP>" + reader["markup"] + "</MARK_UP>" +
    //                                "<STD_BILL_RATE>" + reader["st_bill_rate_from"] + "</STD_BILL_RATE>" +
    //                                "<OVERTIME_PAY_RATE>" + reader["ot_pay_rate_from"] + "</OVERTIME_PAY_RATE>" +
    //                                "<OVERTIME_BILL_RATE>" + reader["ot_bill_rate_from"] + "</OVERTIME_BILL_RATE>" +
    //                                "<DOUBLE_PAY_RATE>" + reader["dbl_pay_rate_from"] + "</DOUBLE_PAY_RATE>" +
    //                                "<DOUBLE_BILL_RATE>" + reader["dt_bill_rate_from"] + "</DOUBLE_BILL_RATE>" +
    //                                 "<VENDER_PAY_RATE>" + reader["vender_pay_rate"] + "</VENDER_PAY_RATE>" +
    //                                "<VENDER_OT_PAY_RATE>" + reader["vender_ot_pay_rate"] + "</VENDER_OT_PAY_RATE>" +
    //                                "<VENDER_DT_PAY_RATE>" + reader["vender_dt_pay_rate"] + "</VENDER_DT_PAY_RATE>" +
    //                                "<COMMENTS><![CDATA[" + reader["comments"] + "]]></COMMENTS></JOBS>";

    //                        Count++;
    //                    }
    //                reader.Close();
    //                cmd.Dispose();


    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
    //            xml_string += "<DATA>No records found</DATA>";
    //            //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //        //}
    //        //else
    //        //{
    //        //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
    //        //}
    //    }
    //    xml_string += "</RESPONSE>" +
    //                "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);
    //    xml_string = "";
    //    return xmldoc;
    //}

    [WebMethod]
    public XmlDocument search_inactive_employees(string userEmailId, string userPassword, string vendor_id, string active)
    {
        ////
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                    "<REQUEST>" +
                        "<VENDORID>" + vendor_id + "</VENDORID>" +

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

                string strSql = "select em.employee_id, concat('W',clt.client_alias, '00', right('0000' + convert(varchar(4),em.employee_id),4)) fullemployee_id,ed.first_name, " +
                                "ed.middle_name,ed.last_name, concat(ed.city,+','+ ed.province)address1,ed.start_date,ed.end_date," +
                                " em.job_id,ja.job_title" +
                                " from ovms_employees as em" +
                                "  join ovms_employee_details as ed on em.employee_id = ed.employee_id" +
                                " join ovms_vendors as ven on em.vendor_id = ven.vendor_id" +
                                " join ovms_clients as clt on em.client_id = clt.client_id" +
                                " join ovms_jobs as ja on ja.job_id = em.job_id";

                if (vendor_id != "" & vendor_id != "0")
                {
                    strSql = strSql + " where em.vendor_id= " + vendor_id;
                }
                if (active != "")
                {
                    strSql += " and em.active = " + active;
                }
                strSql += " order by em.create_date desc";
                try
                {
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string = xml_string + "<EMPLOYEE_NAME_ID ID=\"" + reader["employee_id"] + "\">" +
                                "<FULLEMPLOYEE_ID>" + reader["fullemployee_id"] + "</FULLEMPLOYEE_ID>" +
                                "<FIRSTNAME><![CDATA[" + reader["first_name"] + "]]></FIRSTNAME>" +
                                "<MIDDLE_NAME><![CDATA[" + reader["middle_name"] + "]]></MIDDLE_NAME>" +
                                "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
                                "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
                                "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                "<JOB_TITLE><![CDATA[" + reader["job_title"] + "]]></JOB_TITLE>" +
                                 "<START_DATE>" + reader["start_date"] + "</START_DATE>" +
                                "<END_DATE>" + reader["end_date"] + "</END_DATE>" +
                                "</EMPLOYEE_NAME_ID>";
                        }
                        //close
                        reader.Close();
                        cmd.Dispose();
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

    public XmlDocument set_jobs(string jobStatusID, string jobTitle, string departmentID, string clientID, string positionTypeID,
            string noOfOpenings, string vendorID, string jobLocationID,
            string travelTime, string hoursPerDay, string rolesAndResponsibilities, string hiringManagerName, string coOrdinator,
          string con_start_date, string con_end_date, string maxSubmissionPerSupplier,
            string reasonForOpen, string interviewRequirement, string urgent, string userEmailId, string userPassword,
            string std_pay_rate_from, string able_to_move, string markup,
            string st_bill_rate_from,
            string ot_bill_rate_from, string dt_bill_rate_from,
       string vender_pay_rate, string vender_ot_pay_rate, string vender_dt_pay_rate,
       string user_id, string bonus, string benefits, string base_salary, string submit)
    {
        // logAPI.Service logService = new //logAPI.Service();

        SqlConnection conn;
        //SqlCommand cmd;
        string sVendors = "";
        string strSql = "";
        string errString = "";
        int jobId = 0;
        int jobdetail = 0;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<JOB_STATUS_ID>" + jobStatusID + "</JOB_STATUS_ID>" +
                            "<JOB_TITLE><![CDATA[" + Server.HtmlEncode(jobTitle) + "]]></JOB_TITLE>" +
                            "<DEPARTMENT_ID>" + departmentID + "</DEPARTMENT_ID>" +
                            "<CLIENT_ID>" + clientID + "</CLIENT_ID>" +
                            "<POSITION_TYPE_ID>" + positionTypeID + "</POSITION_TYPE_ID>" +
                            "<NO_OF_OPENINGS>" + noOfOpenings + "</NO_OF_OPENINGS>" +
                            "<VENDOR_ID>" + vendorID + "</VENDOR_ID>" +
                            "<JOB_LOCATION>" + jobLocationID + "</JOB_LOCATION>" +
                            "<BASE_SALARY>" + base_salary + "</BASE_SALARY>" +
                            "<BONUS>" + bonus + "</BONUS>" +
                            "<BENEFITS><![CDATA[" + benefits + "]]></BENEFITS>" +
                            "<TRAVEL_TIME>" + travelTime + "</TRAVEL_TIME>" +
                            "<HOURS_PER_DAY>" + hoursPerDay + "</HOURS_PER_DAY>" +
                            "<ABLE_TO_MOVE>" + able_to_move + "</ABLE_TO_MOVE>" +
                            "<ROLES_AND_RESPONSIBILITIES><![CDATA[" + Server.HtmlEncode(rolesAndResponsibilities) + "]]></ROLES_AND_RESPONSIBILITIES>" +
                            "<HIRING_MANAGER_NAME>" + Server.HtmlEncode(hiringManagerName) + "</HIRING_MANAGER_NAME>" +
                            "<COORDINATOR>" + coOrdinator + "</COORDINATOR>" +
                            "<CONTRACT_START_DATE>" + con_start_date + "</CONTRACT_START_DATE>" +
                            "<CONTRACT_END_DATE>" + con_end_date + "</CONTRACT_END_DATE>" +
                            "<MAX_SUBMISSION_PER_SUPPLIER>" + maxSubmissionPerSupplier + "</MAX_SUBMISSION_PER_SUPPLIER>" +
                            "<REASON_FOR_OPEN>" + reasonForOpen + "</REASON_FOR_OPEN>" +
                            "<INTERVIEW_REQUIREMENT>" + interviewRequirement + "</INTERVIEW_REQUIREMENT>" +
                            "<OVERTIME_BILL_RATE_FROM>" + ot_bill_rate_from + "</OVERTIME_BILL_RATE_FROM>" +
                            "<MARKUP>" + markup + "</MARKUP>" +
                            "<DOUBLE_BILL_RATE_FROM>" + dt_bill_rate_from + "</DOUBLE_BILL_RATE_FROM>" +
                            "<VENDER_PAY_RATE>" + vender_pay_rate + "</VENDER_PAY_RATE>" +
                            "<VENDER_OT_PAY_RATE>" + vender_ot_pay_rate + "</VENDER_OT_PAY_RATE>" +
                            "<VENDER_DT_PAY_RATE>" + vender_dt_pay_rate + "</VENDER_DT_PAY_RATE>" +
                            "<USER_ID>" + user_id + "</USER_ID>" +
                            "<SUBMIT>" + submit + "</SUBMIT>" +
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

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            //xml_string += "<RESPONSE>";
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {

                    if (vendorID == "4")
                    {
                        //string sqlGetAllVendorsforClient = " select distinct us.user_id, dbo.CamelCase(us.First_Name) as First_name, dbo.CamelCase(us.last_Name) as Last_Name  " +
                        //                                   " from ovms_users us, ovms_vendors ve " +
                        //                                   " where us.client_id = ve.client_id " +
                        //                                   " and us.utype_id = 2 and us.client_id = " + clientID + " and us.active = 1";

                        string sqlGetAllVendorsforClient = " select distinct ve.vendor_id as vendors, us.user_id, dbo.CamelCase(us.First_Name) as First_name, dbo.CamelCase(us.last_Name) as Last_Name  " +
                                                          " from ovms_users us, ovms_vendors ve,  ovms_jobs as j " +
                                                          " where us.client_id = ve.client_id " +
                                                          // " and j.job_id=ve.job_id " +
                                                          " and us.utype_id = 2 and us.client_id = " + clientID + " and us.active = 1  and patindex('%" + vendorID + "%', j.vendors) >0";


                        SqlCommand cmdGetAllVendors = new SqlCommand(sqlGetAllVendorsforClient, conn);
                        SqlDataReader rsGgetAllVendors = cmdGetAllVendors.ExecuteReader();
                        //string _svendorList = "";

                        //while (rsGgetAllVendors.Read())
                        //{
                        //    sVendors = sVendors + rsGgetAllVendors["vendors"].ToString() + ",";


                        //}

                        //GET VENDORS ONLY
                        string getvendorsonly = " select distinct ve.vendor_id  as vendors" +
                                   " from ovms_users us, ovms_vendors ve,  ovms_jobs as j " +
                                   " where us.client_id = ve.client_id " +
                                   // " and j.job_id=ve.job_id " +
                                   " and us.utype_id = 2 and us.client_id = " + clientID + " and us.active = 1";//  and patindex('%" + vendorID + "%', j.vendors) > 0";
                        SqlCommand cmdvendorsonly = new SqlCommand(getvendorsonly, conn);
                        SqlDataReader rsvendorsonly = cmdvendorsonly.ExecuteReader();
                        //string[] all_vendors = null;
                        //int count = rsvendorsonly.FieldCount;

                        //string count = "select count(vendor_id) from ovms_vendors where client_ID=1";
                        //int intcount = Convert.ToInt32(count);

                        while (rsvendorsonly.Read())
                        {
                            sVendors = sVendors + rsvendorsonly["vendors"].ToString() + ",";
                        }

                        //sContractEnding = iEnding.ToString();
                        rsGgetAllVendors.Close();
                        cmdGetAllVendors.Dispose();

                        rsvendorsonly.Close();
                        rsvendorsonly.Dispose();
                    }

                    else
                    {
                        sVendors = sVendors + vendorID;
                    }


                    //string strGetDescription = " insert into ovms_job_details( roles_and_responsibilities )values('" + rolesAndResponsibilities.Replace("'", "''") + "')";

                    //SqlCommand cmdGetDescription = new SqlCommand(strGetDescription, conn);
                    //cmdGetDescription.Dispose();

                    //get all vendors if ID = 4
                    strSql = "INSERT INTO ovms_jobs(job_status_id, job_title, department_id, client_id, position_type_id,no_of_openings,vendor_id,urgent,user_id,contract_start_date,contract_end_date, vendors,submit)" +
                    " VALUES('" + jobStatusID + "','" + Server.HtmlEncode(jobTitle) + "','" + departmentID + "','" + clientID + "','" + positionTypeID + "','" + noOfOpenings + "','" + vendorID.Trim().TrimStart().TrimEnd() + "','" + urgent + "','" + user_id + "','" + con_start_date + "','" + con_end_date + "','" + sVendors.Trim().TrimStart().TrimEnd()  + "','" + submit + "')" +
                    " SELECT CAST(scope_identity() AS int)";


                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    jobId = (int)cmd.ExecuteScalar();

                    //Dispose
                    cmd.Dispose();
                    strSql = "INSERT INTO OVMS_JOB_DETAILS(job_id,able_to_move,job_location_id,travel_time,hours_per_day,roles_and_responsibilities,Bonus,Base_salary,Benifits) " +
                        " values('" + jobId + "','" + able_to_move + "','" + jobLocationID + "','" + travelTime + "','" + hoursPerDay + "','" + Server.HtmlEncode(rolesAndResponsibilities.Replace("'", "''")) + "','" + bonus + "','" + base_salary + "','" + Server.HtmlEncode(benefits.Replace("'", "''")) + "') " +
                          " SELECT CAST(scope_identity() AS int) ";

                    SqlCommand cmd1 = new SqlCommand(strSql, conn);
                    jobdetail = (int)cmd1.ExecuteScalar();



                    strSql = "INSERT INTO OVMS_JOB_POSTING_INFO(JOB_ID,CLIENT_ID,HIRING_MANAGER_NAME,COORDINATOR," +
                        "MAX_SUBMISSION_PER_SUPPLIER,REASON_FOR_OPEN," +
                        "INTERVIEW_REQUIREMENT) " +
                        "VALUES('" + jobId + "','" + clientID + "','" + Server.HtmlEncode(hiringManagerName) + "','" + Server.HtmlEncode(coOrdinator) + "'," +
                        "'" + maxSubmissionPerSupplier + "'," +
                        "'" + reasonForOpen + "','" + interviewRequirement + "');";

                    strSql += "INSERT INTO OVMS_JOB_ACCOUNTING(std_pay_rate_from,job_id,markup," +
                        "st_bill_rate_from," +
                        "ot_bill_rate_from,dt_bill_rate_from,vender_pay_rate,vender_ot_pay_rate,vender_dt_pay_rate)" +
                        " values('" + std_pay_rate_from + "','" + jobId + "','" + markup + "','" +
                         st_bill_rate_from + "','" + ot_bill_rate_from + "','" + dt_bill_rate_from + "','" +
                         vender_pay_rate + "','" + vender_ot_pay_rate + "','" + vender_dt_pay_rate + "');";

                    //cmd.CommandText = strSql;
                    SqlCommand cmd2;
                    cmd2 = new SqlCommand(strSql, conn);
                    //jobId = (int)cmd.ExecuteScalar();
                    if (cmd2.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<INSERT_STRING>New job inserted successfully</INSERT_STRING>" +
                            "<JOB_ID>" + jobId + " </JOB_ID> " +
                            "<JOB_DETAIL_ID>" + jobdetail + "</JOB_DETAIL_ID>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<INSERT_STRING>Job not inserted</INSERT_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to create new job");
                    }
                    cmd1.Dispose();
                    //dispose
                    cmd2.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job not inserted</INSERT_STRING>";
                //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument get_submitted_candidates(string userEmailId, string userPassword, string vendor_id, string client_id)
    {
        //logAPI.Service logService = ne/w logAPI.Service();
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        //int RowID = 1;
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                     "<REQUEST>" +
                     "<VENDORID>" + vendor_id + "</VENDORID>" +
                     "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
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

                    strSql = " select distinct ed.employee_id, ed.create_date,  dbo.CamelCase(ed.First_Name) as First_Name, " +
                            " (dbo.CamelCase(ed.First_Name) + ' ' + dbo.CamelCase(ed.Last_Name) + ' submitted on: ' + convert(nvarchar(500), ed.create_date)) as FullName, " +
                            " dbo.CamelCase(ed.Last_Name) as Last_Name, " +
                            " em.vendor_id, em.client_Id, em.job_id,em.user_id from ovms_employee_details ed," +
                            " ovms_employees em, ovms_employee_actions ea" +
                            " where ed.employee_id = em.employee_id" +
                            " and ed.active = 1" +
                            " and em.active = 1" +
                            " and ea.client_id = em.client_id" +
                            " and em.client_id = " + client_id + "" +
                            " and ed.first_name <> '' " +
                            " and em.vendor_id = " + vendor_id + " " +
                            " and em.employee_id not in" +
                            " (select employee_id from ovms_employee_actions as ea ) " +
                            " order by ed.create_date desc ";
                    //" and ed.create_date <= getdate() - 1" +

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<SUBMITTED_CANDIDATE ID='" + RowID + "'>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                        "<CANDIDATE><![CDATA[" + reader["FullName"] + "]]></CANDIDATE>" +
                        "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                        "<LASTNAME><![CDATA[" + reader["last_name"] + "]]></LASTNAME>" +
                        "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                        "<CLIENT_ID>" + reader["client_id"] + "</CLIENT_ID>" +
                        "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                         "<USER_ID>" + reader["user_id"] + "</USER_ID>" +

                        "</SUBMITTED_CANDIDATE>";
                        RowID = RowID + 1;
                    }
                    //dispose
                    reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
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
    public XmlDocument update_job(string jobId, string jobDetailId, string jobStatusId, string jobTitle, string departmentId, string clientId, string positionTypeId,
             string noOfOpenings, string vendorId, string jobLocationID,
             string travelTime, string hoursPerDay, string rolesAndResponsibilities, string hiringManagerName, string coOrdinator,
           string con_start_date, string con_end_date, string maxSubmissionPerSupplier,
             string reasonForOpen, string interviewRequirement, string urgent, string userEmailId, string userPassword,
             string std_pay_rate_from, string able_to_move, string markup,
             string st_bill_rate_from, string ot_bill_rate_from, string dt_bill_rate_from, string comments,
        string vender_pay_rate, string vender_ot_pay_rate, string vender_dt_pay_rate, string user_id, string bonus, string benefits, string base_salary, string submit)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        SqlCommand cmd;
        string strSql = "";
        string errString = "";
        int CommaStatus = 0;
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<JOB_ID>" + jobId + "</JOB_ID>" +
                            "<JOBDETAILID>" + jobDetailId + "</JOBDETAILID>" +
                            "<JOB_STATUS_ID>" + jobStatusId + "</JOB_STATUS_ID>" +
                            "<JOB_TITLE><![CDATA[" + jobTitle + "]]></JOB_TITLE>" +
                            "<DEPARTMENT_ID>" + departmentId + "</DEPARTMENT_ID>" +
                            "<CLIENT_ID>" + clientId + "</CLIENT_ID>" +
                            "<POSITION_TYPE_ID>" + positionTypeId + "</POSITION_TYPE_ID>" +
                            "<NO_OF_OPENINGS>" + noOfOpenings + "</NO_OF_OPENINGS>" +
                            "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
                            "<ABLE_TO_MOVE>" + able_to_move + "</ABLE_TO_MOVE>" +
                            "<JOB_LOCATION><![CDATA[" + jobLocationID + "]]></JOB_LOCATION>" +
                            "<TRAVEL_TIME>" + travelTime + "</TRAVEL_TIME>" +
                            "<HOURS_PER_DAY>" + hoursPerDay + "</HOURS_PER_DAY>" +
                            "<BASE_SALARY>" + base_salary + "</BASE_SALARY>" +
                            "<BONUS>" + bonus + "</BONUS>" +
                            "<BENEFITS><![CDATA[" + benefits + "]]></BENEFITS>" +
                            "<HIRING_MANAGER_NAME><![CDATA[" + hiringManagerName + "]]></HIRING_MANAGER_NAME>" +
                              "<ROLESANDRESPONSIBILITIES><![CDATA[" + rolesAndResponsibilities + "]]></ROLESANDRESPONSIBILITIES>" +
                            "<COORDINATOR>" + coOrdinator + "</COORDINATOR>" +
                            "<MAX_SUBMISSION_PER_SUPPLIER>" + maxSubmissionPerSupplier + "</MAX_SUBMISSION_PER_SUPPLIER>" +
                            "<REASON_FOR_OPEN>" + reasonForOpen + "</REASON_FOR_OPEN>" +
                            "<INTERVIEW_REQUIREMENT>" + interviewRequirement + "</INTERVIEW_REQUIREMENT>" +
                            "<USER_ID>" + user_id + "</USER_ID>" +
                            "<CONTRACT_START_DATE>" + con_start_date + "</CONTRACT_START_DATE>" +
                            "<CONTRACT_END_DATE>" + con_end_date + "</CONTRACT_END_DATE>" +
                            "<MARK_UP>" + markup + "</MARK_UP>" +
                            "<STD_PAY_RATE>" + std_pay_rate_from + "</STD_PAY_RATE>" +
                            "<STD_BILL_RATE>" + st_bill_rate_from + "</STD_BILL_RATE>" +
                            //"<OVERTIME_PAY_RATE>" + ot_pay_rate_from + "</OVERTIME_PAY_RATE>" +
                            "<OVERTIME_BILL_RATE>" + ot_bill_rate_from + "</OVERTIME_BILL_RATE>" +
                            //"<DOUBLE_PAY_RATE>" + dbl_pay_rate_from + "</DOUBLE_PAY_RATE>" +
                            "<DOUBLE_BILL_RATE>" + dt_bill_rate_from + "</DOUBLE_BILL_RATE>" +
                            "<VENDER_PAY_RATE>" + vender_pay_rate + "</VENDER_PAY_RATE>" +
                            "<VENDER_OT_PAY_RATE>" + vender_ot_pay_rate + "</VENDER_OT_PAY_RATE>" +
                            "<VENDER_DT_PAY_RATE>" + vender_dt_pay_rate + "</VENDER_DT_PAY_RATE>" +
                            "<COMMENTS><![CDATA[" + comments + "]]></COMMENTS>" +
                             "<SUBMIT>" + submit + "</SUBMIT>" +
                            "</REQUEST>";

        xml_string = xml_string + "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string = xml_string + "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "UPDATE OVMS_JOBS SET";

                    if (jobStatusId != "")
                    {
                        strSql += " JOB_STATUS_ID = " + jobStatusId;
                        CommaStatus = 1;
                    }
                    if (jobTitle != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " job_title='" + Server.HtmlEncode(jobTitle) + "'";
                        CommaStatus = 1;
                    }
                    if (departmentId != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " department_id='" + departmentId + "'";
                        CommaStatus = 1;
                    }
                    if (clientId != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " client_id='" + clientId + "'";
                        CommaStatus = 1;
                    }
                    if (positionTypeId != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " position_type_id='" + positionTypeId + "'";
                        CommaStatus = 1;
                    }
                    if (con_start_date != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " contract_start_date='" + con_start_date + "'";
                        CommaStatus = 1;
                    }
                    if (con_end_date != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " Contract_end_date='" + con_end_date + "'";
                        CommaStatus = 1;
                    }
                    if (noOfOpenings != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " no_of_openings='" + noOfOpenings + "'";
                        CommaStatus = 1;
                    }
                    if (vendorId != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " vendor_id='" + vendorId + "'";
                        CommaStatus = 1;
                    }
                    if (submit != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " submit='" + submit + "'";
                        CommaStatus = 1;
                    }
                    if (user_id != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " user_id='" + user_id + "'";
                    }

                    strSql += " WHERE JOB_ID=" + jobId + ";";


                    strSql += "update ovms_job_details set";


                    if (able_to_move != "")
                    {
                        strSql += " able_to_move='" + able_to_move + "'";
                        CommaStatus = 1;
                    }

                    if (rolesAndResponsibilities != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " roles_and_responsibilities='" + Server.HtmlEncode(rolesAndResponsibilities) + "'";
                        CommaStatus = 1;
                    }
                    if (travelTime != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " travel_time=" + travelTime;
                        CommaStatus = 1;
                    }
                    if (hoursPerDay != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " hours_per_day=" + hoursPerDay;
                    }
                    if (benefits != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " Benifits='" + Server.HtmlEncode(benefits) + "'";
                        CommaStatus = 1;
                    }
                    if (base_salary != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " Base_salary=" + base_salary;
                        CommaStatus = 1;
                    }
                    if (bonus != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " Bonus=" + bonus;
                    }
                    strSql += " where job_id=" + jobId + ";";



                    strSql += " Update ovms_job_posting_info set ";
                    CommaStatus = 0;
                    if (hiringManagerName != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " hiring_manager_name='" + Server.HtmlEncode(hiringManagerName) + "'";
                        CommaStatus = 1;
                    }
                    if (coOrdinator != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " coordinator='" + Server.HtmlEncode(coOrdinator) + "'";
                        CommaStatus = 1;
                    }

                    if (maxSubmissionPerSupplier != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " max_submission_per_supplier='" + maxSubmissionPerSupplier + "'";
                        CommaStatus = 1;
                    }

                    if (reasonForOpen != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " reason_for_open='" + reasonForOpen + "'";
                        CommaStatus = 1;
                    }

                    if (interviewRequirement != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " interview_requirement='" + interviewRequirement + "'";
                        CommaStatus = 1;
                    }

                    strSql += " where job_id=" + jobId + ";";

                    strSql += " Update ovms_job_comments set comments='" + Server.HtmlEncode(comments) + "' where job_id=" + jobId + ";";

                    strSql += "UPDATE ovms_job_accounting SET";

                    CommaStatus = 0;
                    if (markup != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " markup='" + markup + "'";
                        CommaStatus = 1;
                    }
                    if (std_pay_rate_from != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " std_pay_rate_from='" + std_pay_rate_from + "'";
                        CommaStatus = 1;
                    }
                    if (st_bill_rate_from != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " st_bill_rate_from='" + st_bill_rate_from + "'";
                        CommaStatus = 1;
                    }

                    if (ot_bill_rate_from != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " ot_bill_rate_from='" + ot_bill_rate_from + "'";
                        CommaStatus = 1;
                    }
                    if (dt_bill_rate_from != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " dt_bill_rate_from='" + noOfOpenings + "'";
                        CommaStatus = 1;
                    }
                    if (vender_pay_rate != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " vender_pay_rate='" + vender_pay_rate + "'";
                        CommaStatus = 1;
                    }
                    if (vender_ot_pay_rate != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " vender_ot_pay_rate='" + vender_ot_pay_rate + "'";
                        CommaStatus = 1;
                    }
                    if (vender_dt_pay_rate != "")
                    {
                        strSql += (CommaStatus == 1 ? "," : "") + " vender_dt_pay_rate='" + vender_dt_pay_rate + "'";
                        CommaStatus = 1;
                    }
                    strSql += " WHERE JOB_ID=" + jobId;

                    cmd = new SqlCommand(strSql, conn);
                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        xml_string = xml_string + "<UPDATE_STRING>Job updated successfully</UPDATE_STRING>" +
                                      "<UPDATE_VALUE>1</UPDATE_VALUE>";

                    }
                    else
                    {
                        xml_string = xml_string + "<UPDATE_STRING>Job detail table not updated successfully</UPDATE_STRING>" +
                                      "<UPDATE_VALUE>0</UPDATE_VALUE>";
                    }

                    //string strGetDescription = " update ovms_job_details set roles_and_responsibilities='" + Server.HtmlEncode(rolesAndResponsibilities) + "' WHERE JOB_ID=" + jobId;

                    //SqlCommand cmdGetDescription = new SqlCommand(strGetDescription, conn);

                    //if (cmdGetDescription.ExecuteNonQuery() != 0)
                    //{
                    //    xml_string = xml_string + "<UPDATE_STRING>Job description updated successfully</UPDATE_STRING>" +
                    //                  "<UPDATE_VALUE>1</UPDATE_VALUE>";
                    //}
                    //else
                    //{
                    //    xml_string = xml_string + "<UPDATE_STRING>Job detail table not updated successfully</UPDATE_STRING>" +
                    //                  "<UPDATE_VALUE>0</UPDATE_VALUE>";
                    //}
                    ////close
                    //cmdGetDescription.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
        xml_string = xml_string + "</RESPONSE>";
        xml_string = xml_string + "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml("<?xml version='1.0' encoding='utf-8'?>" + xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_interview_msg(string userEmailId, string userPassword, string employee_id)
    {
        //logAPI.Service logService = ne/w logAPI.Service();
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        //int RowID = 1;
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                     "<REQUEST>" +
                     "<VENDORID>" + employee_id + "</VENDORID>" +

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

                    strSql = " SELECT * FROM ovms_interview_messages WHERE  " +
                           " employee_id='" + employee_id + "' order by more_info_msg_id desc";
                    //" and ed.create_date <= getdate() - 1" +

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<MESSAGE ID='" + RowID + "'>" +
                            "<MORE_INFO_MSG_TIME>" + reader["more_info_msg_time"] + "</MORE_INFO_MSG_TIME> " +
                             "<FROM_VENDOR>" + reader["from_vendor"] + "</FROM_VENDOR> " +
                              "<FROM_CLIENT>" + reader["from_client"] + "</FROM_CLIENT> " +
                               "<MORE_INFO_MSG>" + reader["more_info_msg"] + "</MORE_INFO_MSG> " +

                        " </MESSAGE>";
                        RowID = RowID + 1;
                    }
                    //dispose
                    reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
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
    public XmlDocument delete_a_job(string userEmailId, string userPassword, string job_id, DateTime time)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<JOB_ID>" + job_id + "</JOB_ID>" +
                           "<TIME>" + time + "</TIME>" +




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
                    string strSql = " UPDATE ovms_jobs" +
                                  "  SET active=0, job_delete_time='" + time + "'  " +
                                  " WHERE job_id = " + job_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Job is deactivated</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Job is not deactivated</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();













                }


            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument delete_Job(int jobId, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        SqlCommand cmd;
        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                        "<JOB_ID>" + jobId + "</JOB_ID>" +
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
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "UPDATE OVMS_JOB_DETAILS SET ACTIVE=0 WHERE JOB_ID=" + jobId + ";";
                    strSql += "UPDATE OVMS_JOBS SET ACTIVE=0 WHERE JOB_ID=" + jobId + ";";
                    strSql += "UPDATE ovms_job_accounting SET ACTIVE=0 WHERE JOB_ID=" + jobId + ";";
                    strSql += "UPDATE ovms_job_comments SET ACTIVE=0 WHERE JOB_ID=" + jobId + ";";
                    strSql += "UPDATE ovms_job_posting_info SET ACTIVE=0 WHERE JOB_ID=" + jobId + ";";

                    //xml_string += "<RESPONSE>";
                    cmd = new SqlCommand(strSql, conn);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<STATUS_VALUE>1</STATUS_VALUE>" +
                                    "<STATUS>Deleted successfully</STATUS>";

                    }
                    else
                    {
                        xml_string += "<STATUS_VALUE>0</STATUS_VALUE>" +
                                    "<STATUS>Cannot be deleted</STATUS>";
                        //logService.set_log(127, HttpContext.Current.Request.Url.AbsoluteUri, "127: Unable to delete");
                    }

                }
            }
            catch (Exception ex)
            {
                //logService.set_log(127, HttpContext.Current.Request.Url.AbsoluteUri, "127: Unable to delete. System error");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
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
    public XmlDocument get_JobView(string UserEmailID, string UserPassword, string VendorID, string startDate, string endDate,
        string JobNo, string Title, string Location, string isAvailable, string jobStatus, string UserID)
    {
        // //

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSubSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<VENDOR_ID>" + VendorID + "</VENDOR_ID>" +
                                "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(UserEmailID, UserPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (VendorID != "" & VendorID != "0")
            {
                strSubSql = " and usr.vendor_id = " + VendorID;
            }
            if (JobNo != "")
            {
                strSubSql += " and concat(clt.client_alias, '000', right('0000' + convert(varchar(4), jb.job_id), 4))='" + JobNo + "'";
            }
            if (Title != "")
            {
                strSubSql += " and jb.job_title='" + Title + "'";
            }
            if (Location != "")
            {
                strSubSql += " and jd.job_location='" + Location + "'";
            }
            if (startDate != "")
            {
                strSubSql += " and jd.posting_start_date >= '" + startDate + "' and jd.posting_end_date >= '" + startDate + "'";
            }
            if (endDate != "")
            {
                strSubSql += " and jd.posting_start_date <= '" + endDate + "' and jd.posting_end_date <= '" + endDate + "'";
            }
            if (jobStatus != "" & jobStatus != "0")
            {
                strSubSql += " and jb.job_status_id =" + jobStatus;
            }
            if (isAvailable != "" & isAvailable.ToUpper() == "TRUE")
            {
                strSubSql += " and jb.no_of_openings - jb.hired!=0";
            }
            if (UserID != "" & UserID != "0")
            {
                strSubSql += " and usr.user_id=" + UserID;
            }


            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    //strSql= "select top 100 js.job_status," +
                    //    "concat('J',clt.client_alias, '000', right('0000' + convert(varchar(4), jb.job_id), 4)) job_no,jb.job_title," +
                    //    "jd.job_location JobLocation, jd.posting_start_date,jd.posting_end_date,jb.no_of_openings, jb.hired," +
                    //    "jb.no_of_openings - jb.hired available_jobs,jb.urgent,datediff(day,jb.create_date,GETDATE()) recent" +
                    //    " from ovms_users as usr join ovms_clients as clt on usr.client_id = clt.client_id" +
                    //    " join ovms_jobs as jb on jb.client_id = clt.client_id and jb.vendor_id = usr.vendor_id" +
                    //    " join ovms_job_details as jd on jd.job_id = jb.job_id" +
                    //    " join ovms_job_status as js on jb.job_status_id = js.job_status_id" +
                    //    " where usr.email_id = '" + UserEmailID + "' and usr.user_password = '" + UserPassword + "'" +
                    //    " and usr.active = 1 and clt.active = 1 and jb.active = 1 and jd.active = 1" +
                    //    " and js.active = 1 " + strSubSql +
                    //    " order by recent";
                    strSql = "select top 100 js.job_status,dbo.GetJobNo(jb.job_id) job_no," +
                        "jb.job_title,concat(jl.address1, ', ', jl.post_code, ', ', jl.city, ', ', jl.province, ', ', jl.country) JobLocation," +
                        "jd.posting_start_date,jd.posting_end_date,jb.no_of_openings, jb.hired," +
                        "jb.no_of_openings - jb.hired available_jobs,jb.urgent,datediff(day, jb.create_date, GETDATE()) recent" +
                        " from ovms_users as usr" +
                        " join ovms_clients as clt on usr.client_id = clt.client_id" +
                        " join ovms_jobs as jb on jb.client_id = clt.client_id and jb.vendor_id = usr.vendor_id" +
                        " join ovms_job_details as jd on jd.job_id = jb.job_id" +
                        " join ovms_job_status as js on jb.job_status_id = js.job_status_id" +
                        " join ovms_job_locations as jl on jd.job_location_id = jl.job_location_id" +
                        " where usr.email_id = '" + UserEmailID + "' and usr.user_password = '" + UserPassword + "' and usr.active = 1 and clt.active = 1" +
                        " and jb.active = 1 and jd.active = 1 and js.active = 1 " + strSubSql + " order by recent";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    //xml_string += "<RESPONSE>";
                    if (reader.HasRows)
                    {
                        //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        while (reader.Read())
                        {
                            xml_string += "<JOB_NO ID='" + RowID + "'>" +
                                          "<JOB_STATUS>" + reader["JOB_STATUS"] + "</JOB_STATUS>" +
                                          "<JOB_ID>" + reader["JOB_NO"] + "</JOB_ID>" +
                                          "<JOB_TITLE>" + reader["JOB_TITLE"] + "</JOB_TITLE>" +
                                          "<JOB_LOCATION>" + reader["JOBLOCATION"] + "</JOB_LOCATION>" +
                                          "<POSTING_START_DATE>" + reader["POSTING_START_DATE"] + "</POSTING_START_DATE>" +
                                          "<POSTING_END_DATE>" + reader["POSTING_END_DATE"] + "</POSTING_END_DATE>" +
                                          "<NO_OF_OPENINGS>" + reader["NO_OF_OPENINGS"] + "</NO_OF_OPENINGS>" +
                                          "<HIRED>" + reader["HIRED"] + "</HIRED>" +
                                          "<AVAILABLE_JOBS>" + reader["AVAILABLE_JOBS"] + "</AVAILABLE_JOBS>" +
                                          "<URGENT>" + reader["URGENT"] + "</URGENT>" +
                                          "<RECENT>" + reader["RECENT"] + "</RECENT>" +
                                      "</JOB_NO>";
                            RowID++;
                        }
                        //dispose
                        reader.Close();
                        cmd.Dispose();
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    }

                }
            }
            catch (Exception ex)
            {
                //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            //}
            //else
            //{
            //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
            //}
        }


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument set_job_location(string client_id, string address1, string address2, string postCode, string city, string province, string country,
        string userEmailId, string userPassword)
    {
        // logAPI.Service logService = new //logAPI.Service();

        SqlConnection conn;
        SqlCommand cmd;

        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                        "<ADDRESS1><![CDATA[" + address1 + "]]></ADDRESS1>" +
                        "<ADDRESS2><![CDATA[" + address2 + "]]></ADDRESS2>" +
                        "<POST_CODE>" + postCode + "</POST_CODE>" +
                        "<CITY>" + city + "</CITY>" +
                        "<PROVINCE>" + province + "</PROVINCE>" +
                        "<COUNTRY>" + country + "</COUNTRY>" +
                        "</REQUEST>" +
                        "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "INSERT INTO ovms_job_locations(client_id,address1,address2,post_code,city,province,country)" +
                    " VALUES(" + client_id + ",'" + address1 + "','" + address2 + "','" + postCode + "','" + city + "','" + province + "','" + country + "')";

                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<INSERT_STRING>New job location inserted successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<INSERT_STRING>Job location not inserted</INSERT_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to create new job location");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job location not inserted</INSERT_STRING>";
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
    public XmlDocument get_job_location(string jobLocationId, string clientId, string address1, string address2, string postCode,
        string city, string province, string country, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSubSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<JOB_LOCATION_ID>" + jobLocationId + "</JOB_LOCATION_ID>" +
                                "<CLIENT_ID>" + clientId + "</CLIENT_ID>" +
                                "<ADDRESS1><![CDATA[" + address1 + "]]></ADDRESS1>" +
                                "<ADDRESS2><![CDATA[" + address2 + "]]></ADDRESS2>" +
                                "<POST_CODE>" + postCode + "</POST_CODE>" +
                                "<CITY>" + city + "</CITY>" +
                                "<PROVINCE>" + province + "</PROVINCE>" +
                                "<COUNTRY>" + country + "</COUNTRY>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (jobLocationId != "" & jobLocationId != "0")
            {
                strSubSql = " and jl.job_location_id = " + jobLocationId;
            }
            if (clientId != "" & clientId != "0")
            {
                strSubSql += " and jl.client_id=" + clientId;
            }
            if (address1 != "")
            {
                strSubSql += " and upper(jl.address1)='" + address1.ToUpper() + "'";
            }
            if (address2 != "")
            {
                strSubSql += " and upper(jl.address2)='" + address2.ToUpper() + "'";
            }
            if (postCode != "")
            {
                strSubSql += " and upper(jl.post_code) = '" + postCode.ToUpper() + "'";
            }
            if (city != "")
            {
                strSubSql += " and upper(jl.city)='" + city.ToUpper() + "'";
            }
            if (province != "")
            {
                strSubSql += " and upper(jl.province) ='" + province.ToUpper() + "'";
            }
            if (country != "")
            {
                strSubSql += " and upper(jl.country)='" + country.ToUpper() + "'";
            }


            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "SELECT JL.JOB_LOCATION_ID,CL.client_name,JL.address1,JL.address2,JL.post_code,JL.city,JL.province,JL.country" +
                            " FROM OVMS_JOB_LOCATIONS AS JL JOIN OVMS_CLIENTS AS CL ON JL.CLIENT_ID = CL.CLIENT_ID where jl.active=1 and cl.active=1 " + strSubSql;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    //xml_string += "<RESPONSE>";
                    if (reader.HasRows)
                    {
                        //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        while (reader.Read())
                        {
                            xml_string += "<JOB_LOCATIONS ID=\"" + RowID + "\">" +
                                          "<JOB_LOCATION_ID>" + reader["JOB_LOCATION_ID"] + "</JOB_LOCATION_ID>" +
                                          "<CLIENT_NAME><![CDATA[" + reader["client_name"] + "]]></CLIENT_NAME>" +
                                          "<ADDRESS1><![CDATA[" + reader["address1"] + "]]></ADDRESS1>" +
                                          "<ADDRESS2><![CDATA[" + reader["address2"] + "]]></ADDRESS2>" +
                                          "<POSTAL_CODE>" + reader["post_code"] + "</POSTAL_CODE>" +
                                          "<CITY>" + reader["city"] + "</CITY>" +
                                          "<PROVINCE>" + reader["province"] + "</PROVINCE>" +
                                          "<COUNTRY>" + reader["country"] + "</COUNTRY>" +
                                      "</JOB_LOCATIONS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            //}
            //else
            //{
            //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
            //}
        }


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument delete_job_location(string job_location_id, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new /logAPI.Service();

        SqlConnection conn;
        SqlCommand cmd;

        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                            "<JOB_LOCATION_ID>" + job_location_id + "</JOB_LOCATION_ID>" +
                        "</REQUEST>" +
                        "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "update ovms_job_locations set active=0 where job_location_id=" + job_location_id;

                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job location deleted successfully</DELETE_STRING>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job location not deleted</DELETE_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete job location");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job location not deleted</INSERT_STRING>";
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
    public XmlDocument update_job_location(string job_location_id, string client_id, string address1, string address2, string post_code,
        string city, string province, string country, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        SqlCommand cmd;

        string strSql = "";
        string errString = "";
        //string strSubSql = "";
        //bool FirstCol = false;
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                            "<JOB_LOCATION_ID>" + job_location_id + "</JOB_LOCATION_ID>" +
                            "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                            "<ADDRESS1><![CDATA[" + address1 + "]]></ADDRESS1>" +
                            "<ADDRESS2><![CDATA[" + address2 + "]]></ADDRESS2>" +
                            "<POST_CODE>" + post_code + "</POST_CODE>" +
                            "<CITY>" + city + "</CITY>" +
                            "<PROVINCE>" + province + "</PROVINCE>" +
                            "<COUNTRY>" + country + "</COUNTRY>" +
                        "</REQUEST>" +
                        "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            //if (client_id != "" & client_id!="0")
            //{
            //    strSubSql += " client_id=" + client_id;
            //    FirstCol = true;
            //}
            //if (address1 != "")
            //{
            //    strSubSql +=(FirstCol==true?",":"")+ " address1='" + address1 +"'";
            //    FirstCol = true;
            //}
            //if (address2 != "")
            //{
            //    strSubSql += (FirstCol == true ? "," : "") + " address2='" + address2 +"'";
            //    FirstCol = true;
            //}
            //if (post_code != "")
            //{
            //    strSubSql += (FirstCol == true ? "," : "") + " post_code='" + post_code +"'";
            //    FirstCol = true;
            //}
            //if (city != "")
            //{
            //    strSubSql += (FirstCol == true ? "," : "") + " city='" + city +"'";
            //    FirstCol = true;
            //}
            //if (province != "")
            //{
            //    strSubSql += (FirstCol == true ? "," : "") + " province='" + province + "'";
            //    FirstCol = true;
            //}
            //if (country != "")
            //{
            //    strSubSql += (FirstCol == true ? "," : "") + " country='" + country + "'";
            //    FirstCol = true;
            //}



            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "update ovms_job_locations set client_id=" + client_id + ",address1='" + address1 + "',address2='" + address2 + "'," +
                        "post_code='" + post_code + "',city='" + city + "',province='" + province + "',country='" + country + "' where job_location_id=" + job_location_id;

                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job location updated successfully</DELETE_STRING>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job location not updated</DELETE_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update job location");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job location not updated</INSERT_STRING>";
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
    public XmlDocument set_job_managers(string client_id, string firstName, string lastName, string email,
        string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        SqlCommand cmd;

        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                            "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                            "<FIRST_NAME><![CDATA[" + firstName + "]]></FIRST_NAME>" +
                            "<LAST_NAME><![CDATA[" + lastName + "]]></LAST_NAME>" +
                            "<EMAIL>" + email + "</EMAIL>" +
                        "</REQUEST>" +
                        "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "INSERT INTO ovms_job_managers(client_id,first_name,last_name,email)" +
                    " VALUES(" + client_id + ",'" + firstName + "','" + lastName + "','" + email + "')";

                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<INSERT_STRING>New job manager inserted successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<INSERT_STRING>Job manager not inserted</INSERT_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to create new job manager");
                    }
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job manager not inserted</INSERT_STRING>";
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
    public XmlDocument get_job_managers(string jobManagerId, string clientId, string firstName, string lastName, string email,
        string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSubSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<JOB_MANAGER_ID>" + jobManagerId + "</JOB_MANAGER_ID>" +
                                "<CLIENT_ID>" + clientId + "</CLIENT_ID>" +
                                "<FIRST_NAME><![CDATA[" + firstName + "]]></FIRST_NAME>" +
                                "<LAST_NAME><![CDATA[" + lastName + "]]></LAST_NAME>" +
                                "<EMAIL>" + email + "</EMAIL>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (jobManagerId != "" & jobManagerId != "0")
            {
                strSubSql = " and jm.job_manager_id = " + jobManagerId;
            }
            if (clientId != "" & clientId != "0")
            {
                strSubSql += " and jm.client_id=" + clientId;
            }
            if (firstName != "")
            {
                strSubSql += " and upper(jm.first_name)='" + firstName.ToUpper() + "'";
            }
            if (lastName != "")
            {
                strSubSql += " and upper(jm.last_name)='" + lastName.ToUpper() + "'";
            }
            if (email != "")
            {
                strSubSql += " and upper(jm.email) = '" + email.ToUpper() + "'";
            }


            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "  SELECT JM.JOB_MANAGER_ID,CL.CLIENT_NAME,JM.FIRST_NAME,JM.LAST_NAME,JM.EMAIL" +
                            " FROM ovms_job_managers AS JM JOIN OVMS_CLIENTS AS CL ON JM.CLIENT_ID = CL.client_id where JM.active=1 and CL.active=1 " + strSubSql;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    //xml_string += "<RESPONSE>";
                    if (reader.HasRows)
                    {
                        //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        while (reader.Read())
                        {
                            xml_string += "<JOB_MANAGERS ID=\"" + RowID + "\">" +
                                          "<JOB_MANAGER_ID>" + reader["JOB_MANAGER_ID"] + "</JOB_MANAGER_ID>" +
                                          "<CLIENT_NAME><![CDATA[" + reader["CLIENT_NAME"] + "]]></CLIENT_NAME>" +
                                          "<FIRST_NAME><![CDATA[" + reader["FIRST_NAME"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["LAST_NAME"] + "]]></LAST_NAME>" +
                                          "<EMAIL>" + reader["EMAIL"] + "</EMAIL>" +
                                      "</JOB_MANAGERS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    }
                }
            }
            catch (Exception ex)
            {
                //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            //}
            //else
            //{
            //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
            //}
        }


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument delete_job_manager(string job_manager_id, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        SqlCommand cmd;

        string strSql = "";
        string errString = "";
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                            "<JOB_MANAGER_ID>" + job_manager_id + "</JOB_MANAGER_ID>" +
                        "</REQUEST>" +
                        "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "update ovms_job_managers set active=0 where job_manager_id=" + job_manager_id;

                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job manager deleted successfully</DELETE_STRING>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job manager not deleted</DELETE_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to delete job manager");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job manager not deleted</INSERT_STRING>";
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
    public XmlDocument update_job_manager(string job_manager_id, string client_id, string firstName, string lastName, string email,
        string userEmailId, string userPassword)
    {
        //       //

        SqlConnection conn;
        SqlCommand cmd;


        string strSql = "";
        string errString = "";
        //string strSubSql = "";
        //bool FirstCol = false;
        string xml_string = "<XML>" +
                        "<REQUEST>" +
                            "<JOB_MANAGER_ID>" + job_manager_id + "</JOB_MANAGER_ID>" +
                            "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                            "<FIRST_NAME><![CDATA[" + firstName + "]]></FIRST_NAME>" +
                            "<LAST_NAME><![CDATA[" + lastName + "]]></LAST_NAME>" +
                            "<EMAIL>" + email + "</EMAIL>" +
                        "</REQUEST>" +
                        "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    strSql = "update ovms_job_managers set client_id=" + client_id + ",first_name='" + firstName + "',last_name='" + lastName + "'," +
                        "email='" + email + "' where job_manager_id=" + job_manager_id;

                    cmd = conn.CreateCommand();
                    cmd.CommandText = strSql;

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job manager updated successfully</DELETE_STRING>";
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DELETE_STRING>Job manager not updated</DELETE_STRING>";
                        //logService.set_log(125, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to update job manager");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<INSERT_STRING>Job manager not updated</INSERT_STRING>";
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
    public XmlDocument get_all_vendors_for_a_client(string clientId, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<CLIENT_ID>" + clientId + "</CLIENT_ID>" +
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
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    //strSql = "select vm.vendor_id,vm.vendor_name,case vm.active when 1 then 'Active' when 0 then 'Inactive' end status,"+
                    //    "(select count(oe.employee_id) active_employees from ovms_employees as oe" +
                    //    " where oe.vendor_id = em.vendor_id and oe.client_id = em.client_id and oe.active = 1) active_employees," +
                    //    "(select count(oe.employee_id) active_employees from ovms_employees as oe where oe.vendor_id = em.vendor_id" +
                    //    " and oe.client_id = em.client_id and oe.active = 0) inactive_employees,"+
                    //    " (select concat(isnull(DATEDIFF(month, min(us.create_date), getdate()), 0),"+
                    //    " case when isnull(DATEDIFF(month, min(us.create_date), getdate()), 0) < 2 then  ' day, ' else ' days, ' end,"+
                    //    " isnull(DATEDIFF(day, min(us.create_date), getdate()), 0),"+
                    //    " case when isnull(DATEDIFF(day, min(us.create_date), getdate()), 0) < 2 then ' month and ' else ' months and ' end,"+
                    //    " isnull(DATEDIFF(year, min(us.create_date), getdate()), 0),"+
                    //    " case when isnull(DATEDIFF(year, min(us.create_date), getdate()), 0) < 2 then ' year' else ' years' end) vendor_since"+
                    //    " from ovms_users as us where us.vendor_id = em.vendor_id and us.client_id = em.client_id) vendor_since"+
                    //    " from ovms_vendors as vm join(select distinct vendor_id, client_id from ovms_employees) as em on vm.vendor_id = em.vendor_id"+
                    //    " and em.client_id = "+ clientId;

                    strSql = "select vm.vendor_id,vm.vendor_name,case vm.active when 1 then 'Active' when 0 then 'Inactive' end status," +
                        "(select count(oe.employee_id) active_employees from ovms_employees as oe" +
                        " where oe.vendor_id = em.vendor_id" +
                        " and oe.client_id = em.client_id and oe.active = 1) active_employees," +
                        "(select count(oe.employee_id) active_employees from ovms_employees as oe where oe.vendor_id = em.vendor_id" +
                        " and oe.client_id = em.client_id and oe.active = 0) inactive_employees," +
                        " (select  dbo.getYearMonthDayDifference(min(us.create_date))" +
                        " from ovms_users us where em.vendor_id = us.vendor_id) as vendor_since" +
                        " from ovms_vendors as vm join(select distinct vendor_id, client_id from ovms_employees) as em" +
                        " on vm.vendor_id = em.vendor_id and em.client_id = " + clientId + " order by active_employees desc ";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<VENDORS ID=\"" + RowID + "\">" +
                                          "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                          "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
                                          "<STATUS>" + reader["status"] + "</STATUS>" +
                                          "<ACTIVE_EMPLOYEES>" + reader["active_employees"] + "</ACTIVE_EMPLOYEES>" +
                                          "<INACTIVE_EMPLOYEES>" + reader["inactive_employees"] + "</INACTIVE_EMPLOYEES>" +
                                          "<VENDOR_SINCE>" + reader["vendor_since"] + "</VENDOR_SINCE>" +
                                      "</VENDORS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    }
                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument get_name_for_status3(string userEmailId, string userPassword, string status_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<STATUS_ID>" + status_id + "</STATUS_ID>"
                    + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (status_id != "" & status_id != "0")
            {
                strSub = " ts.timesheet_status_id=" + status_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select distinct ts.timesheet_status_id,ts.timesheet_status, " +
                                    "(select sum(a.hours) from ovms_timesheet a, ovms_timesheet_details b where a.employee_id = e.employee_id and a.timesheet_id = b.timesheet_id  and a.active = 1 " +
                                    "and b.active = 1 and b.timesheet_status_id = ts.timesheet_status_id group by DatePart(week, CONCAT(month, '-', day - 1, '-', year)), a.employee_id) hours_reported, " +
                                    "DatePart(week, CONCAT(month, '-', day - 1, '-', year)) weeknum, " +
                                    "(select top 1 CONCAT(month, '-', day, '-', year) from ovms_timesheet  where employee_id = e.employee_id  and DatePart(week, CONCAT(month,'-', day - 1, '-', year)) = DatePart(week, CONCAT(month, '-', day - 1, '-', year))) From_Date, " +
                                    "(select top 1 CONCAT(month, '-', day + 6, '-', year) from ovms_timesheet  where employee_id = e.employee_id  and DatePart(week, CONCAT(month,'-', day - 1, '-', year)) = DatePart(week, CONCAT(month, '-', day - 1, '-', year))) To_date, " +
                                    "e.employee_id,dbo.CamelCase(ed.first_name) as first_name,dbo.CamelCase(ed.last_name) as last_name,ed.pay_rate from ovms_timesheet_status as ts  " +
                                    " join ovms_timesheet_details as td on ts.timesheet_status_id = td.timesheet_status_id " +
                                    " join ovms_timesheet as t on td.timesheet_id = t.timesheet_id " +
                                    " join ovms_employees as e on t.employee_id = e.employee_id " +
                                    " join ovms_employee_details as ed on e.employee_id = ed.employee_id " +
                                    " where " + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<STATUS ID ='" + RowID + "'>" +
                                          "<TIMESHEET_STATUS_ID>" + reader["timesheet_status_id"] + "</TIMESHEET_STATUS_ID>" +
                                          "<TIMESHEET_STATUS>" + reader["timesheet_status"] + "</TIMESHEET_STATUS>" +
                                          "<HOURS>" + reader["hours_reported"] + "</HOURS>" +
                                          "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<PAY_RATE>" + reader["pay_rate"] + "</PAY_RATE>" +
                                          "<FROM_DATE>" + reader["From_Date"] + "</FROM_DATE>" +
                                          "<TO_DATE>" + reader["To_date"] + "</TO_DATE>" +
                                          "<WEEKNUM>" + reader["weeknum"] + "</WEEKNUM>" +
                                          "</STATUS>";
                            RowID++;
                        }
                        //dispose
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
    public XmlDocument interview_request(string userEmailId, string userPassword, string client_id, string vendor_id, string employee_id, string interview_requested, DateTime interview_date, string intw_time, string job_id, string emp_enddate, string job_enddate, string interview_commnt, string more_info_msg, string more_info_reply, string interview_schedule_time)
    {
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +

                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                         "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<INTERVIEW_REQUESTED>" + interview_requested + "</INTERVIEW_REQUESTED>" +
                         "<INTERVIEW_DATE>" + interview_date + "</INTERVIEW_DATE>" +
                            "<INTERVIEW_TIME>" + intw_time + "</INTERVIEW_TIME>" +
                            "<JOB_ID>" + job_id + "</JOB_ID>" +
                            "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
                            "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +
                            "<INTERVIEW_COMMNT>" + interview_commnt + "</INTERVIEW_COMMNT>" +
                              "<MORE_INFO_MSG>" + more_info_msg + "</MORE_INFO_MSG>" +
                          "<MORE_INFO_REPLY>" + more_info_reply + "</MORE_INFO_REPLY>" +
                             "<INTERVIEW_SCHEDULE_TIME>" + interview_schedule_time + "</INTERVIEW_SCHEDULE_TIME>" +

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
                    string strSql = " INSERT INTO ovms_employee_actions" +
                                    " (client_id, employee_id,vendor_id, interview_requested, interview_date,interview_time,candidate_enddate,job_enddate,job_id,interview_request_comment,interview_schedule_time " +
                                   " )" +

                                    " VALUES('" + client_id + "', '" + employee_id + "','" + vendor_id + "', '" + interview_requested + "', '" + interview_date + "','" + intw_time + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "','" + interview_commnt + "','" + interview_schedule_time + "')";



                    // "more_info=Null,vendor_moreInfo_reply=Null" +


                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Request sent successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Request not sent</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";

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
    public XmlDocument interview_request2(string userEmailId, string userPassword, string client_id, string employee_id, string interview_requested, DateTime interview_date, string intw_time, string job_id, string emp_enddate, string job_enddate, string more_info_msg, string more_info_reply, string action_id, string comment)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +

                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<INTERVIEW_REQUESTED>" + interview_requested + "</INTERVIEW_REQUESTED>" +
                         "<INTERVIEW_DATE>" + interview_date + "</INTERVIEW_DATE>" +
                            "<INTERVIEW_TIME>" + intw_time + "</INTERVIEW_TIME>" +
                            "<JOB_ID>" + job_id + "</JOB_ID>" +
                            "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
                            "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +

                              "<MORE_INFO_MSG>" + more_info_msg + "</MORE_INFO_MSG>" +
                          "<MORE_INFO_REPLY>" + more_info_reply + "</MORE_INFO_REPLY>" +
                            "<COMMENT>" + comment + "</COMMENT>" +
                          "<ACTION_ID>" + action_id + "</ACTION_ID>" +


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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET client_id ='" + client_id + "',employee_id='" + employee_id + "',interview_requested=" + interview_requested + " ,interview_confirm=null," +
                                  " interview_date='" + interview_date + "',interview_time='" + intw_time + "',candidate_enddate='" + emp_enddate + "',job_enddate='" + job_enddate + "', " +
                                 " job_id='" + job_id + "',interview_request_comment='" + comment + "'," +
                                  "more_info=Null,vendor_moreInfo_reply=Null" +
                                  " WHERE action_id = " + action_id + "";





                    //" INSERT INTO ovms_employee_actions" +
                    //            " (client_id, employee_id, interview_requested, interview_date,interview_time,candidate_enddate,job_enddate,job_id,interview_request_comment " +
                    //           " )" +

                    //            " VALUES('" + client_id + "', '" + employee_id + "', '" + interview_requested + "', '" + interview_date + "','" + intw_time + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "','" + interview_commnt + "')";



                    // "more_info=Null,vendor_moreInfo_reply=Null" +


                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Request sent successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Request not sent</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";

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
    public XmlDocument more_info_reply(string userEmailId, string userPassword, string action_id, string more_info_reply)
    {
        SqlConnection conn;
        string xml_string = "";
        //  logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<ACTION_ID>" + action_id + "</ACTION_ID>" +
                         "<MORE_INFO_REPLY>" + more_info_reply + "</MORE_INFO_REPLY>" +



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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET vendor_moreInfo_reply ='" + more_info_reply + "'" +
                                  " WHERE action_id = " + action_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>More Information replied</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>not replied</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument reject_candidate(string userEmailId, string userPassword, string client_id, string employee_id, string vendor_id, string candiadate_rejected, string reason, string job_id, string emp_enddate, string job_enddate, string reject_time)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<CANDIADATE_REJECTED>" + candiadate_rejected + "</CANDIADATE_REJECTED>" +
                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                         "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                          "<JOB_ID>" + job_id + "</JOB_ID>" +
                            "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
                            "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +
                             "<REJECT_TIME>" + reject_time + "</REJECT_TIME>" +

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
                    string strSql = " INSERT INTO ovms_employee_actions" +
                                    " (client_id, employee_id,vendor_id,candidate_rejected,reason_of_rejection ,candidate_enddate,job_enddate,job_id,candidate_reject_time " +
                                   " )" +
                                    " VALUES('" + client_id + "', '" + employee_id + "','" + vendor_id + "', '" + candiadate_rejected + "','" + reason + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "','" + reject_time + "' )";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Candiadate Rejected</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Candidate not rejected</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    //public XmlDocument reject_candidate(string userEmailId, string userPassword, string client_id, string employee_id, string candiadate_rejected, string reason, string job_id, string emp_enddate, string job_enddate)
    //{
    //    SqlConnection conn;
    //    string xml_string = "";
    //    //  logAPI.Service logService = new logAPI.Service();
    //    string errString = "";
    //    errString = VerifyUser(userEmailId, userPassword);

    //    xml_string = "<XML>" +
    //                "<REQUEST>" +
    //                     "<CANDIADATE_REJECTED>" + candiadate_rejected + "</CANDIADATE_REJECTED>" +
    //                     "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
    //                     "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
    //                      "<JOB_ID>" + job_id + "</JOB_ID>" +
    //                        "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
    //                        "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +

    //                "</REQUEST>";
    //    xml_string += "<RESPONSE>";
    //    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
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
    //                string strSql = " INSERT INTO ovms_employee_actions" +
    //                                " (client_id, employee_id,candidate_rejected,reason_of_rejection ,candidate_enddate,job_enddate,job_id " +
    //                               " )" +
    //                                " VALUES('" + client_id + "', '" + employee_id + "', '" + candiadate_rejected + "','" + reason + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "' )";






    //                SqlCommand cmd = new SqlCommand(strSql, conn);

    //                if (cmd.ExecuteNonQuery() > 0)
    //                {
    //                    xml_string += "<INSERT_STRING>Candiadate Rejected</INSERT_STRING>";
    //                }
    //                else
    //                {
    //                    xml_string += "<INSERT_STRING><ERROR>Candidate not rejected</ERROR> </INSERT_STRING>";
    //                }
    //                cmd.Dispose();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


    //        }
    //        finally
    //        {
    //            if (conn.State == System.Data.ConnectionState.Open)
    //                conn.Close();
    //        }
    //    }
    //    xml_string += "</RESPONSE>" +
    //                         "</XML>";
    //    XmlDocument xmldoc;
    //    xmldoc = new XmlDocument();
    //    xmldoc.LoadXml(xml_string);

    //    return xmldoc;
    //}

    [WebMethod]
    public XmlDocument reject_candidate2(string userEmailId, string userPassword, string client_id, string employee_id, string reason_of_rejection)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +

                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +


                           "<REASON_OF_REJECTION>" + reason_of_rejection + "</REASON_OF_REJECTION>" +


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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET client_id ='" + client_id + "',employee_id='" + employee_id + "',candidate_rejected='1',interview_requested=Null,interview_confirm=null," +
                                  " interview_date=null,interview_time=Null, " +
                                 " interview_request_comment=Null,question_asked=null,candidate_approve=null,vendor_interview_comment=null," +
                                 "interview_resheduled=null,interview_request_comment2=null,vendor_interview_comment2=null,interview_request_comment3=null," +
                                 "vendor_interview_comment3=null,interview_request_comment4=null,vendor_interview_comment4=null,interview_request_comment5=null," +
                                 "vendor_interview_comment5=null,vendor_moreInfo_reply=null," +
                                  "more_info=Null,reason_of_rejection='" + reason_of_rejection + "'" +
                                  " WHERE employee_id='" + employee_id + "'";

                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Candidate Rejected</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Candidate not rejected</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";

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
    public XmlDocument reject_candidate_from_vendor(string userEmailId, string userPassword, string employee_id, string reason_of_rejection, DateTime time)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +


                           "<REASON_OF_REJECTION>" + reason_of_rejection + "</REASON_OF_REJECTION>" +
                             "<TIME>" + time + "</TIME>" +



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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET candidate_rejected=null,interview_requested=Null,interview_confirm=null," +
                                  " interview_date=null,interview_time=Null,vendor_reject_candidate_time='" + time + "', " +
                                 " interview_request_comment=Null,question_asked=null,candidate_approve=null,vendor_interview_comment=null," +
                                 "interview_resheduled=null,interview_request_comment2=null,vendor_interview_comment2=null,interview_request_comment3=null," +
                                 "vendor_interview_comment3=null,interview_request_comment4=null,vendor_interview_comment4=null,interview_request_comment5=null," +
                                 "vendor_interview_comment5=null,vendor_moreInfo_reply=null," +
                                  "more_info=Null,vendor_reject_candidate=1,vendor_reject_candidate_comment='" + reason_of_rejection + "'" +
                                  " WHERE employee_id='" + employee_id + "'";

                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Candidate Rejected from vendor</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Candidate not rejected</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";

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
    public XmlDocument more_info_candidate(string userEmailId, string userPassword, string client_id, string employee_id, string more_info, string job_id, string emp_enddate, string job_enddate)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<MORE_INFO><![CDATA[" + more_info + "]]></MORE_INFO>" +
                         "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<JOB_ID>" + job_id + "</JOB_ID>" +
                            "<EMP_ENDDATE>" + emp_enddate + "</EMP_ENDDATE>" +
                            "<JOB_ENDDATE>" + job_enddate + "</JOB_ENDDATE>" +

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
                    string strSql = " INSERT INTO ovms_employee_actions" +
                                    " (client_id, employee_id,more_info ,candidate_enddate,job_enddate,job_id " +
                                   " )" +
                                    " VALUES('" + client_id + "', '" + employee_id + "', '" + more_info + "','" + emp_enddate + "','" + job_enddate + "','" + job_id + "')";


                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Information inserted</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Information inserted</ERROR> </INSERT_STRING>";

                    }
                    //dispose
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";
                //  logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

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
    public XmlDocument decline_status3(string userEmailId, string userPassword, string status_id, string employee_id)
    {
        //logAPI.Service logService = new logAPI.Service();

        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<STATUS_ID>" + status_id + "</STATUS_ID>"
                       + "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>"
                    + "</REQUEST>";
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
                        string strSql = "update ovms_timesheet_details set timesheet_status_id ='" + status_id + "' from ovms_timesheet_details as td  inner join ovms_timesheet as t on td.timesheet_id = t.timesheet_id where t.employee_id = '" + employee_id + "'";
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>status updated successfully</STRING>" +
                                          "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>status not updated</STRING>" +
                                          "<STATUS>0</STATUS>";
                            //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to updated employee rating");
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

    public XmlDocument get_timesheet_detail_for_emp(string userEmailId, string userPassword, string employee_id, string timesheet_status_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (employee_id != "" & employee_id != "0")
        {
            strSub = " t.employee_id =" + employee_id;
        }
        if (employee_id != "" & employee_id != "0")
        {
            strSub += " and ts.timesheet_status_id =" + timesheet_status_id;
        }
        //   and ts.timesheet_status_id = 3
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                                          "<TIMESHEET_STATUS_ID>" + timesheet_status_id + "</TIMESHEET_STATUS_ID>" +
                            "</REQUEST>";
                string strSql = "  select concat(t.day,'-',t.month,'-',t.year)date,t.hours,t.overtime,ts.timesheet_status,concat(ed.first_name,' ',ed.last_name)name,t.employee_id from ovms_timesheet as t join ovms_timesheet_details as td on t.timesheet_id = td.timesheet_id  join ovms_timesheet_status as ts  on td.timesheet_status_id = ts.timesheet_status_id  join ovms_employee_details as ed on t.employee_id=ed.employee_id  where " + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                xml_string += "<RESPONSE>";
                int RowID = 1;

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<EMPLOYEE ID ='" + RowID + "'>" +
                             "<NAME><![CDATA[" + reader["name"] + "]]></NAME>" +
                                        "<DAY>" + reader["date"] + "</DAY>" +
                                        "<HOURS>" + reader["hours"] + "</HOURS>" +
                                        "<OVERTIME>" + reader["overtime"] + "</OVERTIME>" +
                                        "<EMPLOYEE_ID>" + reader["employee_id"] + " </EMPLOYEE_ID>" +
                                        "</EMPLOYEE>";
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
    public XmlDocument get_name_startingtoday(string userEmailId, string userPassword, string vendor_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>" +
            "<REQUEST>" +
                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                     "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub = "and e.vendor_id=" + vendor_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select concat(em.first_name ,+' '+ em.last_name) name from ovms_employee_details as em join ovms_employees as e on em.employee_id=e.employee_id where em.start_date =(Convert(date, getdate()))  " + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<EMPLOYEE ID ='" + RowID + "'>" +
                                "<NAME>" + reader["name"] + "</NAME>" +
                            "</EMPLOYEE>";
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
    public XmlDocument get_jobend_in_fivedays(string userEmailId, string userPassword, string vendor_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>" +
            "<REQUEST>" +
                    "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                     "</REQUEST>";

        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub = "and j.vendor_id=" + vendor_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select j.job_title from ovms_jobs as j join ovms_job_details as jd on j.job_id = jd.job_id where posting_end_date = (select DATEADD(DAY, +5, (Convert(date, getdate()))))" + strSub;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<JOB ID ='" + RowID + "'>" +
                                "<JOB_TITLE><![CDATA[" + reader["job_title"] + "]]></JOB_TITLE>" +
                                 "</JOB>";
                            RowID++;
                            ;

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
    public XmlDocument insert_timesheet_comments_with_rejection_approve(string userEmailId, string userPassword, string timesheet_comments, string employee_id, string from_date, string to_date)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<TIMESHEET_COMMENTS><![CDATA[" + timesheet_comments + "]]></TIMESHEET_COMMENTS>" +
                        "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<FROM_DATE>" + from_date + "</FROM_DATE>" +
                         "<TO_DATE>" + to_date + "</TO_DATE>" +
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


                        string sql = "INSERT INTO ovms_timesheet_comments (timesheet_comments,employee_id,from_date,to_date)VALUES('" + timesheet_comments + "','" + employee_id + "','" + from_date + "','" + to_date + "') SELECT CAST(scope_identity() AS int)";
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

    //Nirmal - Forgot Password API

    [WebMethod]

    public XmlDocument get_userforgotpassword(string email)
    {
        string xml_string = "";

        //logAPI.Service logservice = new logAPI.Service();

        using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString))
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                 "<EMAIL>" + email + "</EMAIL>" +
                            "</REQUEST>";
                string strSql = "select user_password,first_name +' '+ last_name as UserName from ovms_users where email_id='" + email + "' and active=1";
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
                                             "<USERNAME><![CDATA[" + reader["UserName"] + "]]></USERNAME>" +
                                             "</RESPONSE>";


                            }


                            else
                            {
                                xml_string = xml_string + "<RESPONSE><ERROR> Invalid Email </ERROR></RESPONSE>";

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    xml_string = xml_string + "<RESPONSE>error:100.systemerror</RESPONSE>";
                }

            }
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        //OUTPUT FINAL
        xml_string = xml_string + "</XML>";
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }


    [WebMethod]
    public XmlDocument get_from_and_to_date(string userEmailId, string userPassword, string status_id, string client_id)
    {
        //logAPI.Service logService = new logAPI.Service();
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string strSub1 = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<STATUS_ID>" + status_id + "</STATUS_ID>"
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
            if (status_id != "" & status_id != "0")
            {
                strSub = " timesheet_status_id=" + status_id;
            }
            if (client_id != "" & client_id != "0")
            {
                strSub1 = " e.client_id=" + client_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select distinct timesheet_status_id, timesheet_id_from, timesheet_status," +
                       " weeknum, employee_id, first_name, last_name, Agency_Pay_Employee,job_id,JobManager_ID, " +
                       " (select  top 1 CONCAT(month, '-', day, '-', year) from ovms_timesheet  where employee_id = employee_id and DatePart(week, dateadd(d,-1, CONCAT(month, '-', day, '-', year))) = weeknum) as date_from, " +
                       " (select sum(a.hours) as hours_total from ovms_timesheet a, ovms_timesheet_details b where a.employee_id = employee_id and a.timesheet_id = b.timesheet_id and a.active = 1 and b.active = 1 and b.timesheet_status_id = 1 and DatePart(week, dateadd(d, -1, CONCAT(a.month, '-', a.day, '-', a.year))) = weeknum) as hours_reported, " +
                       " (select  top 1 dateadd(d, 6, CONCAT(month, '-', day, '-', year)) from ovms_timesheet  where employee_id = employee_id and DatePart(week, dateadd(d,-1, CONCAT(month, '-', day, '-', year))) = weeknum) as date_to" +
                       " from(select distinct ts.timesheet_status_id, ts.timesheet_status, e.job_id, (select user_id from ovms_jobs where job_id = e.job_id) as JobManager_ID,  " +
                       " DatePart(week, dateadd(d, -1, CONCAT(month, '-', day, '-', year))) as weeknum, " +
                        " (select top 1 a.timesheet_id from ovms_timesheet a, ovms_timesheet_details b where a.timesheet_id = b.timesheet_id and a.employee_id = e.employee_id and a.active = 1 and b.active = 1 and b.timesheet_status_id = 1) as timesheet_id_from," +
                       " e.employee_id,  dbo.CamelCase(ed.first_name) as first_name,  dbo.CamelCase(ed.last_name) as last_name,  ed.pay_rate as Agency_Pay_Employee" +
                       " from ovms_timesheet_status as ts" +
                        " join ovms_timesheet_details as td on" +
                        " ts.timesheet_status_id = td.timesheet_status_id  join ovms_timesheet as t on" +
                        " td.timesheet_id = t.timesheet_id  join ovms_employees as e on" +
                        " t.employee_id = e.employee_id  join ovms_employee_details as ed on" +
                       " e.employee_id = ed.employee_id" +
                       " where ts. " + strSub +
                       " and " + strSub1 +
                       " and e.active = 1) as times" +
                       " where " + strSub +
                        " order by first_name asc";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<STATUS ID ='" + RowID + "'>" +
                                          "<TIMESHEET_STATUS_ID>" + reader["timesheet_status_id"] + "</TIMESHEET_STATUS_ID>" +
                                          "<TIMESHEET_STATUS>" + reader["timesheet_status"] + "</TIMESHEET_STATUS>" +
                                          "<HOURS>" + reader["hours_reported"] + "</HOURS>" +
                                          "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                          "<FROM_DATE>" + reader["date_from"] + "</FROM_DATE>" +
                                          "<TO_DATE>" + reader["date_to"] + "</TO_DATE>" +
                                          "<WEEKNUM>" + reader["weeknum"] + "</WEEKNUM>" +
                                          "</STATUS>";
                            RowID++;
                        }
                        //dispose
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
    public XmlDocument get_rating_with_jobid(string userEmailId, string userPassword, string JOB_ID)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_rating_with_jobid(userEmailId, userPassword, JOB_ID);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument insert_rating_with_employeeid(string userEmailId, string userPassword, string emp_rating_1, string emp_rating_2,
       string emp_rating_3, string emp_rating_4, string emp_rating_5, string job_Id, string employee_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_rating_with_employeeid(userEmailId, userPassword, emp_rating_1, emp_rating_2,
         emp_rating_3, emp_rating_4, emp_rating_5, job_Id, employee_id);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_job_alias(string userEmailId, string userPassword, string JOB_ID)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_job_alias(userEmailId, userPassword, JOB_ID);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_dates(string userEmailId, string userPassword, string JOB_ID)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_dates(userEmailId, userPassword, JOB_ID);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_all_employees_for_a_vendor(string vendorId, string userEmailId, string userPassword)
    {
        //

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
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
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    //strSql = "select vm.vendor_id,vm.vendor_name,case vm.active when 1 then 'Active' when 0 then 'Inactive' end status," +
                    //    "ed.first_name,ed.last_name,em.job_id" +
                    //    " from ovms_vendors as vm join ovms_employees as em on vm.vendor_id = em.vendor_id" +
                    //    " join ovms_employee_details as ed on em.employee_id = ed.employee_id" +
                    //    " where vm.vendor_id = " + vendorId;

                    strSql = "select vm.vendor_id,vm.vendor_name,case vm.active when 1 then 'Active' when 0 then 'Inactive' end status,ed.first_name," +
                        "ed.last_name,em.job_id,datediff(day, jb.contract_start_date, jb.contract_end_date) contract_length," +
                        "dbo.GetJobNo(jb.job_id) job_no," +
                        "jb.contract_start_date,jb.contract_end_date" +
                        " from ovms_vendors as vm join ovms_employees as em on vm.vendor_id = em.vendor_id" +
                        " join ovms_employee_details as ed on em.employee_id = ed.employee_id" +
                        " join ovms_jobs as jb on em.job_id = jb.job_id" +
                        " where vm.vendor_id = " + vendorId;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<VENDORS ID=\"" + RowID + "\">" +
                                          "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                          "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "</VENDOR_NAME>" +
                                          "<STATUS>" + reader["status"] + "</STATUS>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                          "<JOB_NO>" + reader["job_no"] + "</JOB_NO>" +
                                          "<CONTRACT_LENGTH>" + reader["contract_length"] + "</CONTRACT_LENGTH>" +
                                          "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                                          "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +
                                      "</VENDORS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view ");
                    }
                    cmd.Dispose();
                    reader.Dispose();

                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument get_all_employees_timesheets_for_a_vendor(string vendorId, string employeeID, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (employeeID != "")
            {
                strSub = " and em.employee_id=" + employeeID;
            }

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "select vn.vendor_id,vn.vendor_name,case vn.active when 1 then 'Active' when 0 then 'Inactive' end vendor_status," +
                        "dbo.GetJobNo(jb.job_id) job_no,jb.job_id," +
                        "ed.first_name,ed.last_name,case em.active when 1 then 'Active' when 0 then 'Inactive' end employee_status," +
                        "tc.timesheet_calendar_id,ts.employee_id,sum(ts.hours) no_of_hours_st,sum(ts.overtime) no_of_hours_ot,tc.from_date,tc.to_date" +
                        " from ovms_timesheet as ts join ovms_timesheet_calendar tc" +
                        " on cast(concat(ts.year, '-', ts.month, '-', ts.day) as date) between tc.from_date and tc.to_date" +
                        " join ovms_employees em on ts.employee_id = em.employee_id" +
                        " join ovms_vendors vn on em.vendor_id = vn.vendor_id" +
                        " join ovms_jobs as jb on em.job_id = jb.job_id" +
                        " join ovms_employee_details ed on em.employee_id = ed.employee_id" +
                        " where vn.vendor_id = " + vendorId + strSub +
                        " group by vn.vendor_id,vn.vendor_name,vn.active,ed.first_name,ed.last_name,em.active," +
                        "tc.timesheet_calendar_id,ts.employee_id,tc.from_date,tc.to_date,jb.job_id" +
                        " order by tc.to_date desc";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<TIMESHEETS ID=\"" + RowID + "\">" +
                                          "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                          "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
                                          "<VENDOR_STATUS>" + reader["vendor_status"] + "</VENDOR_STATUS>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<EMPLOYEE_STATUS>" + reader["employee_status"] + "</EMPLOYEE_STATUS>" +
                                          "<JOB_NO>" + reader["job_no"] + "</JOB_NO>" +
                                          "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                          "<TIMESHEET_CALENDAR_ID>" + reader["timesheet_calendar_id"] + "</TIMESHEET_CALENDAR_ID>" +
                                          "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                          "<NO_OF_HOURS_ST>" + reader["no_of_hours_st"] + "</NO_OF_HOURS_ST>" +
                                          "<NO_OF_HOURS_OT>" + reader["no_of_hours_ot"] + "</NO_OF_HOURS_OT>" +
                                          "<TIMESHEET_FROM_DATE>" + reader["from_date"] + "</TIMESHEET_FROM_DATE>" +
                                          "<TIMESHEET_TO_DATE>" + reader["to_date"] + "</TIMESHEET_TO_DATE>" +
                                          "</TIMESHEETS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view ");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument get_expiring_workers(string vendorId, string expiringInDays, string userEmailId, string userPassword)
    {
        //

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (expiringInDays != "" & expiringInDays != "0")
                strSub = " and datediff(day, getdate(), jb.contract_end_date)<=" + expiringInDays;

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "select vm.vendor_id,vm.vendor_name,case vm.active when 1 then 'Active' when 0 then 'Inactive' end vendor_status," +
                        "case em.active when 1 then 'Active' when 0 then 'Inactive' end employee_status," +
                        "dbo.GetJobNo(jb.job_id) job_no," +
                        "ed.first_name,ed.last_name,em.job_id,datediff(day, jb.contract_start_date, jb.contract_end_date) contract_length," +
                        "jb.contract_start_date,jb.contract_end_date,datediff(day, getdate(), jb.contract_end_date) expiring_in" +
                        " from ovms_vendors as vm" +
                        " join ovms_employees as em on vm.vendor_id = em.vendor_id" +
                        " join ovms_employee_details as ed on em.employee_id = ed.employee_id" +
                        " join ovms_jobs as jb on em.job_id = jb.job_id" +
                        " where vm.vendor_id = " + vendorId +
                        " and datediff(day, getdate(), jb.contract_end_date)>= 0" + strSub +
                        " order by expiring_in";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<WORKERS ID=\"" + RowID + "\">" +
                                          "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                          "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
                                          "<VENDOR_STATUS>" + reader["vendor_status"] + "</VENDOR_STATUS>" +
                                          "<EMPLOYEE_STATUS>" + reader["employee_status"] + "</EMPLOYEE_STATUS>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                          "<JOB_NO>" + reader["job_no"] + "</JOB_NO>" +
                                          "<CONTRACT_LENGTH>" + reader["contract_length"] + "</CONTRACT_LENGTH>" +
                                          "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                                          "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +
                                          "<EXPIRING_IN>" + reader["expiring_in"] + "</EXPIRING_IN>" +
                                      "</WORKERS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view ");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument get_expiring_jobs(string vendorId, string expiringInDays, string userEmailId, string userPassword)
    {
        //

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (expiringInDays != "" & expiringInDays != "0")
                strSub = " and datediff(day, getdate(), jb.contract_end_date)<=" + expiringInDays;

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "select vm.vendor_id,vm.vendor_name,case vm.active when 1 then 'Active' when 0 then 'Inactive' end vendor_status," +
                        "case jb.active when 1 then 'Active' when 0 then 'Inactive' end job_status," +
                        "dbo.GetJobNo(jb.job_id) job_no,jb.job_id," +
                        "jb.job_title,datediff(day, jb.contract_start_date, jb.contract_end_date) contract_length," +
                        "jb.contract_start_date,jb.contract_end_date,datediff(day, getdate(), jb.contract_end_date) expiring_in" +
                        " from ovms_vendors as vm" +
                        " join ovms_employees as em on vm.vendor_id = em.vendor_id" +
                        " join ovms_jobs as jb on em.job_id = jb.job_id" +
                        " where vm.vendor_id = " + vendorId +
                        " and datediff(day, getdate(), jb.contract_end_date)>= 0" + strSub +
                        " order by expiring_in";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<EXPIRING_JOBS ID=\"" + RowID + "\">" +
                                          "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                          "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
                                          "<JOB_STATUS>" + reader["vendor_status"] + "</JOB_STATUS>" +
                                          "<VENDOR_STATUS>" + reader["job_status"] + "</VENDOR_STATUS>" +
                                          "<JOB_NO>" + reader["job_no"] + "</JOB_NO>" +
                                          "<JOB_TITLE><![CDATA[" + reader["job_title"] + "]]></JOB_TITLE>" +
                                          "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                          "<CONTRACT_LENGTH>" + reader["contract_length"] + "</CONTRACT_LENGTH>" +
                                          "<CONTRACT_START_DATE>" + reader["contract_start_date"] + "</CONTRACT_START_DATE>" +
                                          "<CONTRACT_END_DATE>" + reader["contract_end_date"] + "</CONTRACT_END_DATE>" +
                                          "<EXPIRING_IN>" + reader["expiring_in"] + "</EXPIRING_IN>" +
                                      "</EXPIRING_JOBS>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view ");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument todays_message_for_vendor(string vendorId, string userEmailId, string userPassword)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
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
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "select vn.vendor_id,vn.vendor_name,case ms.active when 1 then 'Active' when 0 then 'Inactive' end message_status," +
                            "case actions when 'Send_E_V' then" +
                            " (select concat(ed.first_name, ' ', ed.last_name) from ovms_employee_details as ed where ed.employee_id = ms.employee_id)" +
                            " when 'Send_P_V' then(select concat(ed.first_name, ' ', ed.last_name) from ovms_employee_details as ed" +
                            " where ed.employee_id = ms.employee_id) end message_from, ms.Msg_Subject,SUBSTRING(ms.message, 1, 200) message" +
                            " from ovms_messages as ms join ovms_vendors as vn on ms.vendor_id = vn.vendor_id" +
                            " where ms.Actions like('%V') and ms.IsRead = 0 and ms.vendor_id = " + vendorId +
                            " order by ms.create_date";

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<TODAYS_MESSAGES ID=\"" + RowID + "\">" +
                                          "<VENDOR_ID>" + reader["vendor_id"] + "</VENDOR_ID>" +
                                          "<VENDOR_NAME><![CDATA[" + reader["vendor_name"] + "]]></VENDOR_NAME>" +
                                          "<MESSAGE_FROM>" + reader["message_from"] + "</MESSAGE_FROM>" +
                                          "<MESSAGE_STATUS>" + reader["message_status"] + "</MESSAGE_STATUS>" +
                                          "<MESSAGE><![CDATA[" + reader["message"] + "]]></MESSAGE>" +
                                      "</TODAYS_MESSAGES>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view ");
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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
    public XmlDocument duplicate_employee(string vendorId, string employee_email, string ActiveStatus, string userEmailId, string userPassword)
    {
        //

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSub = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
                                "<EMPLOYEE_EMAIL>" + employee_email + "</EMPLOYEE_EMAIL>" +
                                "<ACTIVE_STATUS>" + ActiveStatus + "</ACTIVE_STATUS>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";
        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (ActiveStatus == "1")
                strSub = " and em.active = 1";

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    strSql = "select em.employee_id" +
                        " from ovms_employees as em" +
                        " join ovms_employee_details as ed on em.employee_id = ed.employee_id" +
                        " where em.vendor_id = " + vendorId + " and ed.email = '" + employee_email + "' " + strSub;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<DUPLICATE>YES</DUPLICATE>" +
                                          "<EMPLOYEE>" +
                                                "Employee already exists. Please try with another employee" +
                                          "</EMPLOYEE>";
                        }
                    }
                    else
                    {
                        xml_string += "<DUPLICATE>NO</DUPLICATE>" +
                                      "<EMPLOYEE>" +
                                                "It's a new Employee. Please proceed with it." +
                                      "</EMPLOYEE>";
                    }
                    cmd.Dispose();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
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


    ///Janal's Web Service
    ///
    [WebMethod]
    public XmlDocument get_job_detail_id(string userEmailId, string userPassword, string JOB_ID)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_job_detail_id(userEmailId, userPassword, JOB_ID);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument get_V_submitted_candidate(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
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
                    " em.user_id,ed.first_name, em.employee_id, ed.middle_name,ed.last_name, ed.comments, ed.email, ed.date_of_birth, ed.phone," +
                    "ed.suite_no, ed.address1, ed.address2,j.job_title,ven.vendor_name,ed.ext_requested,  ed.licence_no, ed.city,ed.province," +
                    " concat(ed.city, ', ', ed.province)location,ed.profile_picture_path, ed.postal,ed.province,ed.country,ed.active,ed.skype_id, " +
                    "  ed.availability_for_interview, ed.start_date,ed.Last_4_Digits_of_SSN_SIN,ed.pay_rate, ed.end_date,ed.create_date, " +
                    " ed.active,ven.vendor_name,em.job_id, clt.client_name,eact.interview_requested,eact.candidate_rejected, eact.more_info,eact.vendor_moreInfo_reply, " +
                   "  eact.interview_date, eact.interview_time, eact.reason_of_rejection, eact.candidate_approve, eact.interview_resheduled, eact.interview_confirm,eact.vendor_interview_comment ," +
                   "eact.vendor_interview_comment2,eact.vendor_reject_candidate,eact.vendor_reject_candidate_comment,eact.create_date as action_create_date,eact.vendor_interview_comment3,eact.vendor_interview_comment4,eact.vendor_interview_comment5, " +
                    "eact.interview_request_comment,eact.interview_request_comment2,eact.interview_request_comment3,eact.interview_request_comment4,eact.interview_request_comment5, " +
                    " eact.interview_cancel_by_client,eact.interview_cancel_by_client_comment," +
                     " eact.action_id,j.contract_start_date,j.contract_end_date from ovms_employees as em " +
                    " join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
                    " join ovms_vendors as ven on em.vendor_id = ven.vendor_id " +
                    " join ovms_clients as clt on em.client_id = clt.client_id " +
                    "  join ovms_jobs as j on j.job_id = em.job_id " +
                   " left join ovms_employee_actions as eact on em.employee_id = eact.employee_id " +
                    " where 1 = 1 and em.employee_id" +
                 " not in  (select employee_id from ovms_employee_actions as eact where vendor_id='" + vendor_id + "' and candidate_approve=1  )";


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
    [WebMethod]
    public XmlDocument get_submitted_employees(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
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
                    " em.user_id,ed.first_name, em.employee_id, ed.middle_name,ed.last_name, ed.comments, ed.email, ed.date_of_birth, ed.phone," +
                    "ed.suite_no, ed.address1, ed.address2,j.job_title,ven.vendor_name,ed.ext_requested,  ed.licence_no, ed.city,ed.province," +
                    " concat(ed.city, ', ', ed.province)location,ed.profile_picture_path, ed.postal,ed.province,ed.country,ed.active,ed.skype_id, " +
                    "  ed.availability_for_interview, ed.start_date,ed.Last_4_Digits_of_SSN_SIN,ed.pay_rate, ed.end_date,ed.create_date, " +
                    " ed.active,ven.vendor_name,em.job_id, clt.client_name,eact.interview_requested,eact.candidate_rejected, eact.more_info,eact.vendor_moreInfo_reply, " +
                   "  eact.interview_date, eact.interview_time, eact.reason_of_rejection, eact.candidate_approve, eact.interview_resheduled, eact.interview_confirm,eact.vendor_interview_comment ," +
                   "eact.vendor_interview_comment2,eact.vendor_reject_candidate,eact.vendor_reject_candidate_comment,eact.create_date as action_create_date,eact.vendor_interview_comment3,eact.vendor_interview_comment4,eact.vendor_interview_comment5, " +
                    "eact.interview_request_comment,eact.interview_request_comment2,eact.interview_request_comment3,eact.interview_request_comment4,eact.interview_request_comment5, " +
                    " eact.interview_cancel_by_client,eact.interview_cancel_by_client_comment," +
                     " eact.action_id,j.contract_start_date,j.contract_end_date from ovms_employees as em " +
                    " join ovms_employee_details as ed on em.employee_id = ed.employee_id " +
                    " join ovms_vendors as ven on em.vendor_id = ven.vendor_id " +
                    " join ovms_clients as clt on em.client_id = clt.client_id " +
                    "  join ovms_jobs as j on j.job_id = em.job_id " +
                   " left join ovms_employee_actions as eact on em.employee_id = eact.employee_id " +
                    " where 1 = 1";




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
    [WebMethod]
    public XmlDocument get_vendor(string clientid, string userEmailId, string userPassword)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_vendor(clientid, userEmailId, userPassword);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_vendor(string userEmailId, string userPassword, string vendorName, string vaddress1, string vaddress2, string vcity, string vpostal_code, string vcountry, string vPhoneNumber, string vFaxNumber)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_vendor(userEmailId, userPassword, vendorName, vaddress1, vaddress2, vcity, vpostal_code, vcountry, vPhoneNumber, vFaxNumber);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_vendor(string userEmailId, string userPassword, int vid, string vendorName, string vaddress1, string vaddress2, string vcity, string vpostal_code, string vcountry, string vPhoneNumber, string vFaxNumber)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_vendor(userEmailId, userPassword, vid, vendorName, vaddress1, vaddress2, vcity, vpostal_code, vcountry, vPhoneNumber, vFaxNumber);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_vendor(string userEmailId, string userPassword, string vendorid)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_vendor(userEmailId, userPassword, vendorid);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_department(string userEmailId, string userPassword, string department_id, string client_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_department(userEmailId, userPassword, department_id, client_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_department(string userEmailId, string userPassword, string department_name, string client_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_department(userEmailId, userPassword, department_name, client_id);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_job_submit(string userEmailId, string userPassword, string job_id, string submit)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                  "<REQUEST>" +
                                       "<SUBMIT>" + submit + "</SUBMIT>" +
                                       "<JOB_ID>" + job_id + "</JOB_ID>" +
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

                    if (submit != "")
                    {
                        string strSql = "update ovms_jobs set submit ='" + submit + "' where job_id =  '" + job_id + "'";
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

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_department(string userEmailId, string userPassword, int department_id, string department_name, int client_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_department(userEmailId, userPassword, department_id, department_name, client_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_department(string userEmailId, string userPassword, string department_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_department(userEmailId, userPassword, department_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_job_position_type(string userEmailId, string userPassword, string position_type_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_job_position_type(userEmailId, userPassword, position_type_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_job_position_type(string userEmailId, string userPassword, string job_position_type)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_job_position_type(userEmailId, userPassword, job_position_type);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_job_position_type(string userEmailId, string userPassword, int position_type_id, string job_position_type)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_job_position_type(userEmailId, userPassword, position_type_id, job_position_type);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_job_position_type(string userEmailId, string userPassword, int position_type_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_job_position_type(userEmailId, userPassword, position_type_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_timesheet_comment(string userEmailId, string userPassword, string timesheet_comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_timesheet_comment(userEmailId, userPassword, timesheet_comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_timesheet_comments(string userEmailId, string userPassword, string timesheet_comments)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_timesheet_comments(userEmailId, userPassword, timesheet_comments);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_timesheet_comments(string userEmailId, string userPassword, int timesheet_comment_id, string timesheet_comments)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_timesheet_comments(userEmailId, userPassword, timesheet_comment_id, timesheet_comments);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_timesheet_comments(string userEmailId, string userPassword, string timesheet_comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_timesheet_comments(userEmailId, userPassword, timesheet_comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_timesheet_status(string userEmailId, string userPassword, string timesheet_status_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_timesheet_status(userEmailId, userPassword, timesheet_status_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_timesheet_status(string userEmailId, string userPassword, string timesheet_status)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_timesheet_status(userEmailId, userPassword, timesheet_status);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_timesheet_status(string userEmailId, string userPassword, int timesheet_status_id, string timesheet_status)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_timesheet_status(userEmailId, userPassword, timesheet_status_id, timesheet_status);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_timesheet_status(string userEmailId, string userPassword, string timesheet_status_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_timesheet_status(userEmailId, userPassword, timesheet_status_id);

        return xmldoc;
    }

    ///AAkashs's Web Service

    [WebMethod]
    public XmlDocument interview_status(string userEmailId, string userPassword, string employee_id, string interview_confirmed, string comments, string comments2, string comments3, string comments4, string comments5, DateTime time)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<INTERVIEW_CONFIRMED>" + interview_confirmed + "</INTERVIEW_CONFIRMED>" +
                        "<COMMENTS>" + comments + "</COMMENTS>" +
                         "<COMMENTS2>" + comments2 + "</COMMENTS2>" +
                          "<COMMENTS3>" + comments3 + "</COMMENTS3>" +
                           "<COMMENTS4>" + comments4 + "</COMMENTS4>" +
                            "<COMMENTS5>" + comments5 + "</COMMENTS5>" +
                               "<TIME>" + time + "</TIME>" +



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
                    string strSql = " UPDATE ovms_employee_actions" +
                                  "  SET interview_confirm = " + interview_confirmed + ",interview_confirm_time='" + time + "' ,interview_requested=null,vendor_interview_comment='" + comments + "'," +
                                  "vendor_interview_comment2='" + comments2 + "',vendor_interview_comment3='" + comments3 + "',vendor_interview_comment4='" + comments4 + "',vendor_interview_comment5='" + comments5 + "'" +
                                   " WHERE employee_id = " + employee_id + "";




                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Interview Confirmed</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Interview not Confirmed</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument get_employees_for_preview(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
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
                    " where 1 = 1";


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


    [WebMethod]
    public XmlDocument get_job_position_type_MB(string userEmailId, string userPassword, string position_type_id)
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
                strSub = " and position_type_id!=" + position_type_id;
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
    public XmlDocument get_Profile(string userEmailId, string userPassword, string UserID)
    {
        Aakash A = new Aakash();

        XmlDocument xmldoc;
        xmldoc = A.get_Profile(userEmailId, userPassword, UserID);

        return xmldoc;
    }

    //AAkash's web service
    [WebMethod]
    public XmlDocument update_Profile(string userEmailId, string userPassword, string first_name, string last_name, string email_id, string UserID, string new_password)
    {
        Aakash A = new Aakash();

        XmlDocument xmldoc;
        xmldoc = A.update_Profile(userEmailId, userPassword, first_name, last_name, email_id, UserID, new_password);

        return xmldoc;
    }


    [WebMethod]
    public XmlDocument update_rating_with_jobid(string userEmailId, string userPassword, string employee_id, string vendorID, string clientID,
        string question_1, string rating1, string question_2, string rating2, string question_3, string rating3, string question_4,
        string rating4, string question_5, string rating5, string userID, string job_Id)
    {
        //

        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        string xml_string = "<XML>" +
                        "<REQUEST>" + "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                        "<JOB_ID>" + job_Id + "</JOB_ID>" +

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
                        //  Question_ID,question_1,rating_1,question_2,rating_2,question_3,rating_3,question_4,rating_4,question_5,rating_5,,employee_id,job_ID
                        string sql = "update ovms_job_question_rating set employee_id='0',Client_ID='" + clientID + "',Vendor_ID='" + vendorID + "',User_ID='" + userID + "',question_1='" + question_1 + "',question_2='" + question_2 + "',question_3='" + question_3 + "',question_4='" + question_4 + "',question_5='" + question_5 + "',rating_1='" + rating1 + "',rating_2='" + rating2 + "',rating_3='" + rating3 + "',rating_4='" + rating4 + "',rating_5='" + rating5 + "' where job_ID='" + job_Id + "'";
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




    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_timesheet(string userEmailId, string userPassword, string timesheet_id, string employee_id, int vendor_id, string start_date, string end_date)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_timesheet(userEmailId, userPassword, timesheet_id, employee_id, vendor_id, start_date, end_date);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument search_timesheet(string userEmailId, string userPassword, string first_name, string last_name, string timesheet_id, string end_date, string start_date, string timesheet_status, string vendor_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.search_timesheet(userEmailId, userPassword, first_name, last_name, timesheet_id, end_date, start_date, timesheet_status, vendor_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_timesheet(string userEmailId, string userPassword, string employee_id, string day, string month, string year,
           string hours, string overtime, string timesheet_status_id, string timesheet_comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_timesheet(userEmailId, userPassword, employee_id, day, month, year,
            hours, overtime, timesheet_status_id, timesheet_comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_timesheet(string userEmailId, string userPassword, int employee_id, int day, int month, int year,
        float hours, int overtime, int timesheet_id, int timesheet_status_id, int timesheet_comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_timesheet(userEmailId, userPassword, employee_id, day, month, year,
         hours, overtime, timesheet_id, timesheet_status_id, timesheet_comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_timesheet(string userEmailId, string userPassword, int timesheet_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_timesheet(userEmailId, userPassword, timesheet_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_business_type(string userEmailId, string userPassword, string business_type_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_business_type(userEmailId, userPassword, business_type_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_business_type(string userEmailId, string userPassword, string business_type_name)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_business_type(userEmailId, userPassword, business_type_name);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_business_type(string userEmailId, string userPassword, int business_type_id, string business_type_name)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_business_type(userEmailId, userPassword, business_type_id, business_type_name);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_business_type(string userEmailId, string userPassword, int business_type_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_business_type(userEmailId, userPassword, business_type_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_employee_comments(string userEmailId, string userPassword, string comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_employee_comments(userEmailId, userPassword, comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_employee_comments(string userEmailId, string userPassword, string comments, int employee_id, DateTime commentdate, int user_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_employee_comments(userEmailId, userPassword, comments, employee_id, commentdate, user_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_employee_comments(string userEmailId, string userPassword, int comment_id, string comments, int employee_id, DateTime commentdate, int user_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_employee_comments(userEmailId, userPassword, comment_id, comments, employee_id, commentdate, user_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_employee_comments(string userEmailId, string userPassword, string comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_employee_comments(userEmailId, userPassword, comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_job_comments(string userEmailId, string userPassword, string comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_job_comments(userEmailId, userPassword, comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_job_comments(string userEmailId, string userPassword, string comments, int job_id, DateTime commentdate, int user_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_job_comments(userEmailId, userPassword, comments, job_id, commentdate, user_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_job_comments(string userEmailId, string userPassword, int comment_id, string comments, int job_id, DateTime commentdate, int user_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_job_comments(userEmailId, userPassword, comment_id, comments, job_id, commentdate, user_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_job_comments(string userEmailId, string userPassword, int comment_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_job_comments(userEmailId, userPassword, comment_id);

        return xmldoc;
    }

    ///Janal's Web Service
    ///
    [WebMethod]
    public XmlDocument name_for_timesheet(string userEmailId, string userPassword, string vendor_id)

    {
        //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
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

                    string strSql = "select timesheet_status_id, timesheet_status, weeknum,  employee_id, first_name, last_name, pay_rate," +
                        " (select sum(a.hours) as hours_total from ovms_timesheet a, ovms_timesheet_details b where a.employee_id = employee_id and a.timesheet_id = b.timesheet_id and a.active = 1 and b.active = 1 and b.timesheet_status_id = 3 and DatePart(week, dateadd(d, -1, CONCAT(a.month, '-', a.day, '-', a.year))) = weeknum) as hours_reported, " +
                        "(select  top 1 CONCAT(month, '-', day, '-', year) from ovms_timesheet  where employee_id = employee_id and DatePart(week, dateadd(d, -1, CONCAT(month, '-', day, '-', year))) = weeknum) as date_from, " +
                                    "(select  top 1 dateadd(d, 6, CONCAT(month, '-', day, '-', year)) from ovms_timesheet  where employee_id = employee_id and DatePart(week, dateadd(d, -1, CONCAT(month, '-', day, '-', year))) = weeknum) as date_to " +
                                    "from(select distinct ts.timesheet_status_id, " +
                                    "ts.timesheet_status, " +
                                    "DatePart(week, dateadd(d, -1, CONCAT(month, '-', day, '-', year))) as weeknum, " +
                                    "e.employee_id, " +
                                    "dbo.CamelCase(ed.first_name) as first_name, " +
                                    "dbo.CamelCase(ed.last_name) as last_name, " +
                                    "ed.pay_rate " +
                                    "from ovms_timesheet_status as ts " +
                                    "join ovms_timesheet_details as td " +
                                    "on ts.timesheet_status_id = td.timesheet_status_id " +
                                    "join ovms_timesheet as t " +
                                    "on td.timesheet_id = t.timesheet_id " +
                                    "join ovms_employees as e " +
                                    "on t.employee_id = e.employee_id " +
                                    "join ovms_employee_details as ed " +
                                    "on e.employee_id = ed.employee_id " +
                                    "where ts.timesheet_status_id = 3 " +
                                    "and e.vendor_id =  " + vendor_id + " " +
                                    "and e.active = 1) as times " +
                                    "where timesheet_status_id=3 order by first_name asc ";
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<STATUS ID ='" + RowID + "'>" +
                                          "<TIMESHEET_STATUS_ID>" + reader["timesheet_status_id"] + "</TIMESHEET_STATUS_ID>" +
                                          "<TIMESHEET_STATUS>" + reader["timesheet_status"] + "</TIMESHEET_STATUS>" +
                                          "<HOURS>" + reader["hours_reported"] + "</HOURS>" +
                                          "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<PAY_RATE>" + reader["pay_rate"] + "</PAY_RATE>" +
                                               "<FROM_DATE>" + reader["date_from"] + "</FROM_DATE>" +
                                          "<TO_DATE>" + reader["date_to"] + "</TO_DATE>" +
                                               "<WEEKNUM>" + reader["weeknum"] + "</WEEKNUM>" +
                                          "</STATUS>";
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
    //NIRMAL - INSERT ERRORS
    [WebMethod]
    public XmlDocument insert_errors(string error_code, string description)
    {
        //
        SqlConnection conn;
        string xml_string = "<XML>" +
                      "<REQUEST>" + "<ERROR_CODE>" + error_code + "</ERROR_CODE>" +
                                         "<DESCRIPTION><![CDATA[" + description + "]]> </DESCRIPTION>" +
                                            "</REQUEST>";
        xml_string = xml_string + "<RESPONSE>";


        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                string sql = "INSERT INTO ovms_log_dictionary (error_code,description)VALUES('" + error_code + "','" + description + "') ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int Iinsert = cmd.ExecuteNonQuery();
                if (Iinsert > 0)
                {
                    xml_string = xml_string + "<STRING>Errors inserted successfully</STRING>" +
                                   "<STATUS>1</STATUS>";
                }
                else
                {
                    xml_string = xml_string + "<STRING>Errors not inserted</STRING>" +
                                    "<STATUS>0</STATUS>";
                    //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to insert employee comment");
                }
                //Dispose
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
        xml_string = xml_string + "</RESPONSE>";
        xml_string = xml_string + "</XML>";

        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_name_leave_request(string userEmailId, string userPassword, string vendor_id, string employee_id, string action, string client_id)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>"
                     + "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>"
                    + "</REQUEST>";
        xml_string += "<RESPONSE>";
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            if (vendor_id != "" & vendor_id != "0")
            {
                strSub = " where r.vendor_id=" + vendor_id;
            }
            if (employee_id != "" & employee_id != "0")
            {
                strSub = strSub + " and ed.employee_id=" + employee_id;
            }
            if (action != "" & action != "0")
            {
                strSub = strSub + " and r.action=" + action;
            }
            if (client_id != "" & client_id != "0")
            {
                strSub = strSub + " and client_id=" + client_id;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    string strSql = "select ed.employee_id,r.action,dbo.CamelCase(ed.first_name) as first_name,dbo.CamelCase(ed.last_name) as last_name, r.requested_date,r.Requested_Reason," +
                        "Requested_Comments from ovms_requests as r join ovms_employee_details as ed " +
                        "on r.employee_id=ed.employee_id  " + strSub;
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    if (reader.HasRows == true)
                    {
                        while (reader.Read())
                        {
                            xml_string += "<NAME ID ='" + RowID + "'>" +
                                          "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                                          "<FIRST_NAME><![CDATA[" + reader["first_name"] + "]]></FIRST_NAME>" +
                                          "<LAST_NAME><![CDATA[" + reader["last_name"] + "]]></LAST_NAME>" +
                                          "<REQUESTED_DATE>" + reader["requested_date"] + "</REQUESTED_DATE>" +
                                          "<REQUESTED_REASON><![CDATA[" + reader["Requested_Reason"] + "]]></REQUESTED_REASON>" +
                                          "<REQUESTED_COMMENTS><![CDATA[" + reader["Requested_Comments"] + "]]></REQUESTED_COMMENTS>" +
                                          "<ACTION>" + reader["action"] + "</ACTION>" +
                                          "</NAME>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string = xml_string + "<DATA>no records found</DATA>";

                    }
                    cmd.Dispose();
                    reader.Dispose();
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

    public XmlDocument leave_conform(string userEmailId, string userPassword, string employee_id, string date, string action)
    {
        SqlConnection conn;
        string xml_string = "";
        //
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<DATE>" + date + "</DATE>" +
                         "<ACTION>" + action + "</ACTION>" +
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
                    string strSql = " UPDATE ovms_requests SET action =" + action + " WHERE employee_id=" + employee_id + "and Requested_Date='" + date + "'";

                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>leave Confirmed</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>leave not Confirmed</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";
                //  logService.set_log(100, HttpContext.Current.Request.Url.AbsoluteUri, "System failed: login error");

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
    public XmlDocument get_rating_with_jobid2(string userEmailId, string userPassword, string JOB_ID)
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
                //  string strSql = "select Question_ID,question_1,rating_1,question_2,rating_2,question_3,rating_3,question_4,rating_4,question_5,rating_5,emp_rating_1,emp_rating_2,emp_rating_3,emp_rating_4,emp_rating_5,employee_id,job_ID from ovms_job_question_rating where " + strSub;
                string strSql = " select job_question_1,rating_1,job_question_2,rating_2,job_question_3,rating_3,job_question_4,rating_4,job_question_5,rating_5,rating_1,rating_2,rating_3,rating_4,rating_5,job_ID from ovms_jobquestion_rating where" + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int RowID = 1;
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<QUESTIONS_NO ID='" + RowID + "'>" +

                                        "<QUESTION1><![CDATA[" + reader["job_question_1"] + "]]></QUESTION1>" +
                                        "<RATING1>" + reader["rating_1"] + "</RATING1>" +
                                        "<QUESTION2><![CDATA[" + reader["job_question_2"] + "]]></QUESTION2>" +
                                        "<RATING2>" + reader["rating_2"] + "</RATING2>" +
                                        "<QUESTION3><![CDATA[" + reader["job_question_3"] + "]]></QUESTION3>" +
                                        "<RATING3>" + reader["rating_3"] + "</RATING3>" +
                                        "<QUESTION4><![CDATA[" + reader["job_question_4"] + "]]></QUESTION4>" +
                                        "<RATING4>" + reader["rating_4"] + "</RATING4>" +
                                        "<QUESTION5><![CDATA[" + reader["job_question_5"] + "]]></QUESTION5>" +
                                        "<RATING5>" + reader["rating_5"] + "</RATING5>" +
                                        //"<EMP_RATING_1>" + reader["emp_rating_1"] + "</EMP_RATING_1>" +
                                        //"<EMP_RATING_2>" + reader["emp_rating_2"] + "</EMP_RATING_2>" +
                                        //"<EMP_RATING_3>" + reader["emp_rating_3"] + "</EMP_RATING_3>" +
                                        //"<EMP_RATING_4>" + reader["emp_rating_4"] + "</EMP_RATING_4>" +
                                        //"<EMP_RATING_5>" + reader["emp_rating_5"] + "</EMP_RATING_5>" +
                                        //"<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
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
    public XmlDocument refer_a_frnd(string userEmailId, string userPassword, string employee_id, string job_name, string job_id, string resume, string comments, string phone, string email)
    {
        SqlConnection conn;
        string xml_string = "";
        //
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<JOB_NAME><![CDATA[" + job_name + "]]></JOB_NAME>" +
                          "<JOB_ID>" + job_id + "</JOB_ID>" +
                         "<RESUME>" + resume + "</RESUME>" +
                            "<COMMENTS>" + comments + "</COMMENTS>" +
                             "<PHONE>" + phone + "</PHONE>" +
                            "<EMAIL>" + email + "</EMAIL>" +


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
                    string strSql = " insert into ovms_refer_frnd(employee_id, job_name, candidate_name, resume, comments,email,phone) " +
                     " values('" + employee_id + "','" + job_name + "',  '" + job_id + "', '" + resume + "', '" + comments + "','" + email + "','" + phone + "')";


                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Request sent successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Request not sent</ERROR> </INSERT_STRING>";

                    }
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

    public XmlDocument social(string job_id)

    {
        //
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";

        xml_string += "<XML>"
                    + "<REQUEST>"
                    + "<JOB_ID>" + job_id + "</JOB_ID>"
                    + "</REQUEST>";
        xml_string += "<RESPONSE>";


        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (job_id != "" & job_id != "0")
        {
            strSub = "job_id=" + job_id;
        }

        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();

                string strSql = "  select J.job_id,J.client_id,j.user_id,(select email_id from ovms_users where user_id=j.user_id)email_id, (select user_password from ovms_users where user_id=j.user_id)p@ss from ovms_jobs as j where " + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int RowID = 1;
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        xml_string += "<JOB_DETAIL ID ='" + RowID + "'>" +
                                       
                                         "<CLIENT_ID>" + reader["client_id"] + "</CLIENT_ID>" +
                                        "<JOB_ID>" + reader["job_id"] + "</JOB_ID>" +
                                        "<USER_ID>" + reader["user_id"] + "</USER_ID>" +
                                        "<PASSWORD><![CDATA[" + reader["p@ss"] + "]]></PASSWORD>" +
                                        "<USER_EMAIL>" + reader["email_id"] + "</USER_EMAIL>" +
                                        "</JOB_DETAIL>";
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
            xml_string = "<status> Unable to select country_id </ status > ";
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
    public XmlDocument get_employees(string userEmailId, string userPassword, string employee_id, string vendor_id, string client_id, string fromdate, string enddate, string active, string user_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_employees(userEmailId, userPassword, employee_id, vendor_id, client_id, fromdate, enddate, active, user_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument search_employees(string userEmailId, string userPassword, string first_name, string Last_name, string city, string country, string province, string postal, string skype_id, string vendor_id, string active)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.search_employees(userEmailId, userPassword, first_name, Last_name, city, country, province, postal, skype_id, vendor_id, active);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_all_available_job_for_particuler_vendor(string userEmailId, string userPassword, string vendor_id)
    {
        Janal j = new Janal();
        XmlDocument xmldoc;
        xmldoc = j.get_all_available_job_for_particuler_vendor(userEmailId, userPassword, vendor_id);
        return xmldoc;
    }

    [WebMethod]
    public XmlDocument set_employee(string userEmailId, string userPassword, string first_name, string middle_name, string last_name, string email, string phone, string date_of_birth, string suite_no, string address1, string address2, string city, string province, string postal, string country, string comments, string profile_picture_path, string availability_for_interview, string skype_id, string startDate, string endDate, int job_id, int vendor_id, int client_id, string liecence_no, string Last_4_Digits_of_SSN_SIN, string pay_rate)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.set_employee(userEmailId, userPassword, first_name, middle_name, last_name, email, phone, date_of_birth,
            suite_no, address1, address2, city, province, postal, country, comments, profile_picture_path, availability_for_interview,
             skype_id, startDate, endDate, job_id, vendor_id, client_id, liecence_no, Last_4_Digits_of_SSN_SIN, pay_rate);

        return xmldoc;
    }
    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_employee(string userEmailId, string userPassword, int employeeId)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_employee(userEmailId, userPassword, employeeId);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_resume(string userEmailId, string userPassword, string emp_id, string resume_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_resume(userEmailId, userPassword, emp_id, resume_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument insert_resume(string userEmailId, string userPassword, string resume_path, string job_id, string employee_id, string user_id, string vendor_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.insert_resume(userEmailId, userPassword, resume_path, job_id, employee_id, user_id, vendor_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_resume(string userEmailId, string userPassword, string resume_path, string job_id, string employee_id, string user_id, string vendor_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_resume(userEmailId, userPassword, resume_path, job_id, employee_id, user_id, vendor_id);

        return xmldoc;
    }

    ///Janal's Web Service
    [WebMethod]
    public XmlDocument delete_resume(string userEmailId, string userPassword, int resume_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.delete_resume(userEmailId, userPassword, resume_id);

        return xmldoc;
    }
    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_candiate_for_that_particuler_job(string userEmailId, string userPassword, string job_id, string vendorid)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_candiate_for_that_particuler_job(userEmailId, userPassword, job_id, vendorid);

        return xmldoc;
    }
    ///Janal's Web Service
    [WebMethod]
    public XmlDocument search_anything(string userEmailId, string userPassword, string saerch_string)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.search_anything(userEmailId, userPassword, saerch_string);

        return xmldoc;
    }
    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_state(string userEmailId, string userPassword, string country_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_state(userEmailId, userPassword, country_id);

        return xmldoc;
    }
    ///Janal's Web Service
    [WebMethod]
    public XmlDocument get_country(string userEmailId, string userPassword, string country_id)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.get_country(userEmailId, userPassword, country_id);

        return xmldoc;
    }
    ///Janal's Web Service
    [WebMethod]
    public XmlDocument update_employee(string userEmailId, string userPassword, string employye_id, string job_id, string vendor_id, string client_id, string first_name, string middle_name, string last_name, string email, string phone, string date_of_birth, string suite_no, string address1, string address2, string city, string province, string postal, string country, string comments, string profile_picture_path, string availability_for_interview, string skype_id, string startDate, string endDate, string licence_id, string Last_4_Digits_of_SSN_SIN, string pay_rate)
    {
        Janal j = new Janal();

        XmlDocument xmldoc;
        xmldoc = j.update_employee(userEmailId, userPassword, employye_id, job_id, vendor_id, client_id, first_name, middle_name, last_name, email, phone, date_of_birth, suite_no, address1, address2, city, province, postal, country, comments, profile_picture_path, availability_for_interview, skype_id, startDate, endDate, licence_id, Last_4_Digits_of_SSN_SIN, pay_rate);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument update_Personal_Profile(string userEmailId, string userPassword, string User_id, string Password)
    {
        SqlConnection conn;
        string xml_string = "";
        //
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
                                    " user_password = '" + Password + "'" +
                                    " where " +
                                    " User_id = '" + User_id + "' and active = 1";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Password updated successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Password not upadated</ERROR> </INSERT_STRING>";

                    }
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






    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument update_user(string userEmailId, string userPassword, int userId, string first_name, string last_name, string email_id, string user_password, string utype)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.update_user(userEmailId, userPassword, userId, first_name, last_name, email_id, user_password, utype);

        return xmldoc;
    }

    [WebMethod]
    public XmlDocument ext_contract(string userEmailId, string userPassword, string employee_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.ext_contract(userEmailId, userPassword, employee_id);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument update_Basic_Profile(string userEmailId, string userPassword, string first_name, string last_name, string email_id, string User_id)
    {
        SqlConnection conn;
        string xml_string = "";
        //
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
                                  "  email_id = '" + email_id + "'" +
                                  " where " +
                                   " User_id = '" + User_id + "' and active = 1";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Basic Information updated successfully</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Information not upadated</ERROR> </INSERT_STRING>";

                    }
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

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument delete_user(string userEmailId, string userPassword, int userId)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.delete_user(userEmailId, userPassword, userId);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument get_usertype(string userEmailId, string userPassword, int userId)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.get_usertype(userEmailId, userPassword, userId);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument insert_usertype(string userEmailId, string userPassword, string user_type)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.insert_usertype(userEmailId, userPassword, user_type);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument set_user_type(string userEmailId, string userPassword, int usertype_id, string utype)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.set_user_type(userEmailId, userPassword, usertype_id, utype);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument delete_usertype(string userEmailId, string userPassword, int utype_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.delete_usertype(userEmailId, userPassword, utype_id);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument update_user_type(string userEmailId, string userPassword, int employee_detail_id, int employee_id, string first_name, string last_name, string email, string phone,
       string address1, string address2, string city, string province, string postal, string country, string skype_id, string startDate, string endDate, int vendor_id, int client_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.update_user_type(userEmailId, userPassword, employee_detail_id, employee_id, first_name, last_name, email, phone,
        address1, address2, city, province, postal, country, skype_id, startDate, endDate, vendor_id, client_id);

        return xmldoc;
    }
    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument message_detail(string userEmailId, string userPassword, int message_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.message_detail(userEmailId, userPassword, message_id);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument Msg_Emp_to_Vendor(string userEmailId, string userPassword, string vendor_id, string employee_id, string Msg_Subject, string message, string message_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.Msg_Emp_to_Vendor(userEmailId, userPassword, vendor_id, employee_id, Msg_Subject, message, message_id);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument get_All_Messages_From_emp_to_PMO(string userEmailId, string userPassword, string pmo_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.get_All_Messages_From_emp_to_PMO(userEmailId, userPassword, pmo_id);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument get_All_Messages_From_EMP_TO_Vendor(string userEmailId, string userPassword, string vendor_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.get_All_Messages_From_EMP_TO_Vendor(userEmailId, userPassword, vendor_id);

        return xmldoc;
    }

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument get_Jobs_Client(string userEmailId, string userPassword, string client_id, string vendor_id, string user_id)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.get_Jobs_Client(userEmailId, userPassword, client_id, vendor_id, user_id);

        return xmldoc;
    }

    ///Aakash's Web Service
    //[WebMethod]
    //public XmlDocument get_All_Messages_For_PMO(string userEmailId, string userPassword, string pmo_id)
    //{
    //    Aakash a = new Aakash();

    //    XmlDocument xmldoc;
    //    xmldoc = a.get_All_Messages_For_PMO( userEmailId,  userPassword,  pmo_id);

    //    return xmldoc;
    //}

    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument get_Login(string email, string Password)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.get_Login(email, Password);

        return xmldoc;
    }
    ///Aakash's Web Service
    [WebMethod]
    public XmlDocument get_forgotPassword(string email)
    {
        Aakash a = new Aakash();

        XmlDocument xmldoc;
        xmldoc = a.get_forgotPassword(email);

        return xmldoc;
    }

    //Aakash's Web Service
    [WebMethod]
    public XmlDocument get_Message(string userEmailId, string userPassword, string message_id, int IsSource)
    {
        Aakash r = new Aakash();

        XmlDocument xmldoc;
        xmldoc = r.get_Message(userEmailId, userPassword, message_id, IsSource);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument get_client_job_comment(string userEmailId, string userPassword, string job_id)
    {
        //logAPI.Service logService = ne/w logAPI.Service();
        SqlConnection conn;
        string xml_string = "";

        string errString = "";
        //int RowID = 1;
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                     "<REQUEST>" +
                     "<VENDORID>" + job_id + "</VENDORID>" +

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

                    strSql = " SELECT * FROM ovms_client_job_comment WHERE  " +
                           " job_id='" + job_id + "' order by client_comment_id desc";
                    //" and ed.create_date <= getdate() - 1" +

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<COMMENT_ID ID='" + RowID + "'>" +
                            "<CLIENT_JOB_COMMENT>" + reader["client_job_comment"] + "</CLIENT_JOB_COMMENT> " +
                             "<CLIENT_COMMENT_TIME>" + reader["client_comment_time"] + "</CLIENT_COMMENT_TIME> " +

                        " </COMMENT_ID>";
                        RowID = RowID + 1;
                    }
                    //dispose
                    reader.Close();
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string = xml_string + "<STATUS>error:xxx.systemerror</STATUS>";
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
    public XmlDocument get_interview(string userEmailId, string userPassword, string vendorId, string clientId, string interviewDate)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;
        string errString = "";
        string strSql = "";
        string strSubSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                                "<VENDOR_ID>" + vendorId + "</VENDOR_ID>" +
                                "<CLIENT_ID>" + clientId + "</CLIENT_ID>" +
                                "<INTERVIEW_DATE>" + interviewDate + "</INTERVIEW_DATE>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";

        errString = VerifyUser(userEmailId, userPassword);
        if (errString != "")
        {
            xml_string += "<ERROR>" + errString + "</ERROR>";
        }
        else
        {
            if (vendorId != "" & vendorId != "0")
            {
                strSubSql = " ea.vendor_id = " + vendorId;
            }
            if (clientId != "" & clientId != "0")
            {
                strSubSql += " and ea.client_id = " + clientId;
            }

            if (interviewDate != "" & interviewDate != "0")
            {
                strSubSql += " and ea.interview_date > Convert(DateTime, '" + interviewDate + "') - 1";
            }



            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
            try
            {

                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();

                    //strSql = " select ed.first_name + ' ' + ed.last_name as Candidate_name ,convert(datetime,ea.interview_date) as interview_date, ea.interview_time ," +
                    //        " dbo.GetJobNo(job_id) as job_num,  concat('W', clt.client_alias, '00', right('0000' + convert(varchar(4), ed.employee_id), 4)) as emp_id,  " +
                    //        " (select Job_Title from ovms_jobs where job_id = (select job_ID from ovms_employees where employee_id = ea.employee_id)) as Job_Title " +
                    //        " from ovms_employee_actions as ea " +
                    //        " inner join ovms_employee_details as ed on ed.employee_id = ea.employee_id " +
                    //        " inner join ovms_clients as clt on clt.client_id = ea.client_id " +
                    //        " where active = 1 " + 
                    //        strSubSql;

                    strSql = "(Select CASE WHEN ea.interview_requested = 1 THEN 'Requested'" +
                                        " WHEN ea.interview_confirm = 1 THEN 'Confirmed' " +
                                        " WHEN ea.interview_resheduled = 1 THEN 'Rescheduled' " +
                                        " WHEN ea.candidate_rejected = 1 THEN 'Rejected by Client' " +
                                        " WHEN ea.vendor_reject_candidate = 1 THEN 'Rejected by Vendor' " +
                                        " END AS Status " +
                                        " ,concat('W', clt.client_alias, '00', right('0000' + convert(varchar(4), ed.employee_id), 4)) as emp_id " +
                                        " ,dbo.CamelCase(ed.first_name) + ' ' + dbo.CamelCase(ed.last_name) as Candidate_name  " +
                                        " ,dbo.CamelCase(j.job_title) as Job_Title " +
                                        //" ,(select Job_Title from ovms_jobs where job_id = (select job_ID from ovms_employees where employee_id = ea.employee_id)) as Job_Title " +
                                        " ,convert(datetime,ea.interview_date) as interview_date " +
                                        " ,ea.interview_time as interview_start_time " +
                                        //" ,CONVERT(varchar(15),CAST(Convert(time,DATEADD(hour,1,convert(datetime,ea.interview_date))) AS TIME),100) as interview_end_time " +
                                        " ,dbo.GetJobNo(ea.job_id) as job_num " +
                                        //" ,clt.client_name as Interviewer " +
                                        " ,dbo.CamelCase(u.first_name) + ' ' + dbo.CamelCase(u.last_name) as Interviewer " +

                                        " from ovms_employee_actions as ea " +
                                        " inner join ovms_employee_details as ed on ed.employee_id = ea.employee_id " +
                                        " inner join ovms_clients as clt on clt.client_id = ea.client_id " +
                                        " inner join ovms_jobs as j on j.job_id=ea.job_id " +
                                        " inner join ovms_users as u on j.user_id=u.User_id " +

                                        " Where " + strSubSql + ")" +

                                        " UNION " +
                                        "(Select CASE WHEN ea.interview_requested = 1 THEN 'Requested'" +
                                        " WHEN ea.interview_confirm = 1 THEN 'Confirmed' " +
                                        " WHEN ea.interview_resheduled = 1 THEN 'Rescheduled' " +
                                        " WHEN ea.candidate_rejected = 1 THEN 'Rejected by Client' " +
                                        " WHEN ea.vendor_reject_candidate = 1 THEN 'Rejected by Vendor' " +
                                        " END AS Status " +
                                        " ,concat('W', clt.client_alias, '00', right('0000' + convert(varchar(4), ed.employee_id), 4)) as emp_id " +
                                        " ,dbo.CamelCase(ed.first_name) + ' ' + dbo.CamelCase(ed.last_name) as Candidate_name  " +
                                        " ,dbo.CamelCase(j.job_title) as Job_Title " +
                                        //" ,(select Job_Title from ovms_jobs where job_id = (select job_ID from ovms_employees where employee_id = ea.employee_id)) as Job_Title " +
                                        " ,convert(datetime,ea.interview_date) as interview_date " +
                                        " ,ea.interview_time as interview_start_time " +
                                        //" ,CONVERT(varchar(15),CAST(Convert(time,DATEADD(hour,1,convert(datetime,ea.interview_date))) AS TIME),100) as interview_end_time " +
                                        " ,dbo.GetJobNo(ea.job_id) as job_num " +
                                        //" ,clt.client_name as Interviewer " +
                                        " ,dbo.CamelCase(u.first_name) + ' ' + dbo.CamelCase(u.last_name) as Interviewer " +

                                        " from ovms_employee_actions as ea " +
                                        " inner join ovms_employee_details as ed on ed.employee_id = ea.employee_id " +
                                        " inner join ovms_clients as clt on clt.client_id = ea.client_id " +
                                        " inner join ovms_jobs as j on j.job_id=ea.job_id " +
                                        " inner join ovms_users as u on j.user_id=u.User_id " +

                                        " Where ea.candidate_rejected=1 or ea.vendor_reject_candidate=1 " +
                                        " and ea.vendor_id = " + vendorId +
                                        " and ea.client_id = " + clientId +
                                        " and ea.interview_date IS NULL " +
                                        " ) " +
                                        "order by interview_date desc";



                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    //xml_string += "<RESPONSE>";
                    if (reader.HasRows)
                    {
                        //xml_string += "<RESPONSE_STATUS>1</RESPONSE_STATUS>";
                        while (reader.Read())
                        {
                            //Processing interview time for start and end time
                            //string istart = Convert.ToDateTime(reader["interview_time"]).ToShortTimeString();
                            //string start = Convert.ToDateTime(istart).ToString(@"hh:00 tt", new CultureInfo("en-US"));

                            string iDate = "";
                            string istart = "";
                            string iend = "";
                            if (reader["interview_date"].ToString() != string.Empty)
                            {
                                iDate = reader["interview_date"].ToString();
                                istart = reader["interview_start_time"].ToString();
                                iend = Convert.ToDateTime(reader["interview_start_time"]).AddHours(1).ToShortTimeString();
                            }
                            else
                            {
                                iDate = " N/A ";
                                istart = " N/A ";
                                iend = " N/A ";

                            }

                            //string end = Convert.ToDateTime(iend).ToString(@"hh:00 tt", new CultureInfo("en-US"));



                            xml_string += "<INTERVIEW ID=\"" + RowID + "\">" +
                                            "<STATUS>" + reader["Status"] + "</STATUS>" +
                                            "<INTERVIEW_START_TIME>" + istart + "</INTERVIEW_START_TIME>" +
                                            "<INTERVIEW_END_TIME>" + iend + "</INTERVIEW_END_TIME>" +
                                            "<CANDIDATE_NAME>" + reader["Candidate_name"] + "</CANDIDATE_NAME>" +
                                            "<INTERVIEW_DATE>" + reader["interview_date"] + "</INTERVIEW_DATE>" +
                                            "<JOB_NUM>" + reader["job_num"] + "</JOB_NUM>" +
                                            "<EMP_ID>" + reader["emp_id"] + "</EMP_ID>" +
                                            "<JOB_TITLE>" + reader["Job_Title"] + "</JOB_TITLE>" +
                                            "<INTERVIEWER>" + reader["Interviewer"] + "</INTERVIEWER>" +
                                            "</INTERVIEW>";
                            RowID++;
                        }
                    }
                    else
                    {
                        xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                        xml_string += "<DATA>No records found</DATA>";
                        //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                xml_string += "<DATA>No records found</DATA>";
                //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            //}
            //else
            //{
            //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
            //}
        }


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]

    public XmlDocument more_info_msg_eye(string userEmailId, string userPassword, string employee_id, string vendor_id, string action_id, string client_id, string comments, DateTime message_time, string from_vendor, string from_client)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                         "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                           "<ACTION_ID>" + action_id + "</ACTION_ID>" +
                            "<MORE_INFO_MSG>" + comments + "</MORE_INFO_MSG>" +
                             "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                             "<MESSAGE_TIME>" + message_time + "</MESSAGE_TIME>" +
                              "<FROM_VENDOR>" + from_vendor + "</FROM_VENDOR>" +
                               "<FROM_CLIENT>" + from_client + "</FROM_CLIENT>" +


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
                    string strSql = " INSERT INTO ovms_interview_messages(action_id, vendor_id, client_id, more_info_msg, employee_id, more_info_msg_time,from_vendor,from_client) " +

                                   "  VALUES('" + action_id + "', '" + vendor_id + "', '" + client_id + "', '" + comments + "', '" + employee_id + "', '" + message_time + "','" + from_vendor + "','" + from_client + "')";






                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Comment added</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Comment is not addded</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument get_ip_address_count(string ip_address)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;

        string strSql = "";
        string strSubSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<IP_ADDRESS>" + ip_address + "</IP_ADDRESS>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";



        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        try
        {

            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();

                strSql = " SELECT count(ip_address)as times FROM ovms_login_fail_record where ip_address='" + ip_address + "'and attept_time BETWEEN DATEADD(minute, -15, GETDATE()) AND GETDATE()";

                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        xml_string +=
                                        "<TIMES>" + reader["times"] + "</TIMES>";


                    }
                }
                else
                {
                    xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
                    xml_string += "<DATA>No records found</DATA>";
                    //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                }
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {
            //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
            xml_string += "<DATA>No records found</DATA>";
            //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        //}
        //else
        //{
        //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
        //}


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }

    [WebMethod]

    public XmlDocument insert_fail_login(string username, string password, string ip_address)
    {
        SqlConnection conn;
        string xml_string = "";
        //logAPI.Service logService = new logAPI.Service();
        string errString = "";

        xml_string = "<XML>" +
                    "<REQUEST>" +
                         "<USERNAME>" + username + "</USERNAME>" +
                         "<PASSWORD>" + password + "</PASSWORD>" +
                           "<IP_ADDRESS>" + ip_address + "</IP_ADDRESS>" +
                     //"<ATTEMPT_TIME>" + attempt_time + "</ATTEMPT_TIME>" +
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
                    string strSql = " INSERT INTO ovms_login_fail_record(username, password, ip_address, attept_time) " +

                                   "  VALUES('" + username + "', '" + password + "', '" + ip_address + "', GETDATE())";

                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Comment added</INSERT_STRING>" +
                            "<STRING>1</STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Comment is not addded</ERROR></INSERT_STRING>" +
                            "<STRING>0</STRING>";

                    }
                    cmd.Dispose();
                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument blacklist_check(string ip_address)
    {
        //logAPI.Service logService = new logAPI.Service();

        SqlConnection conn;

        string strSql = "";
        string strSubSql = "";
        string xml_string = "<XML>" +
                            "<REQUEST>" +
                            "<IP_ADDRESS>" + ip_address + "</IP_ADDRESS>" +
                            "</REQUEST>";

        xml_string += "<RESPONSE>";



        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        try
        {

            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();

                strSql = "   SELECT count(ip_address) as times, max(attept_time) as lastattempt, DATEDIFF(hh, max(attept_time), GETDATE()) as Hours_Difference," +
    "DATEDIFF(mi, DATEADD(hh, DATEDIFF(hh, max(attept_time), GETDATE()), max(attept_time)), GETDATE()) as Minutes_Difference" +
    " from ovms_login_fail_record where ip_address = '" + ip_address + "'";

                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        xml_string += "<HOURS>" + reader["Hours_Difference"] + "</HOURS>" +
                            "<MINUTES_DIFFERENCE>" + reader["Minutes_Difference"] + "</MINUTES_DIFFERENCE>" +
                        "<TIMES>" + reader["times"] + "</TIMES>";


                    }
                }
                else
                {
                    //logService.set_log(123, HttpContext.Current.Request.Url.AbsoluteUri, "Unable to view job");
                }
                cmd.Dispose();
            }
        }
        catch (Exception ex)
        {
            //xml_string += "<RESPONSE_STATUS>0</RESPONSE_STATUS>";
            xml_string += "<DATA>No records found</DATA>";
            //logService.set_log(124, HttpContext.Current.Request.Url.AbsoluteUri, ex.Message);
        }
        finally
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
        //}
        //else
        //{
        //    xml_string += "<JOB_STATUS_ID>JobStatusID should not be null</JOB_STATUS_ID>";
        //}


        xml_string += "</RESPONSE>" +
                            "</XML>";
        XmlDocument xmldoc;
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(xml_string);

        return xmldoc;
    }





    //Raman's Web Service


    [WebMethod]
    public XmlDocument update_TimeSheet_Status(string userEmailId, string userPassword, string job_id, string employee_id, string client_id, string timesheetStausID, string timesheetID)
    {
        Raman r = new Raman();
        XmlDocument xmldoc;

        xmldoc = r.update_TimeSheet_Status(userEmailId, userPassword, job_id, employee_id, client_id, timesheetStausID, timesheetID);
        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Add_TimeSheet(string userEmailId, string userPassword, string employee_id)
    {
        Raman r = new Raman();
        XmlDocument xmldoc;

        xmldoc = r.Add_TimeSheet(userEmailId, userPassword, employee_id);
        return xmldoc;
    }

    [WebMethod]
    public XmlDocument Select_Jobs_For_BarChart(string userEmailId, string userPassword)
    {
        Raman r = new Raman();
        XmlDocument xmldoc;
        xmldoc = r.Select_Jobs_For_BarChart(userEmailId, userPassword);
        return xmldoc;
    }
    [WebMethod]
    public XmlDocument Get_Job_Questions(string userEmailId, string userPassword, string job_ID, string Vendor_ID)
    {
        Raman r = new Raman();
        XmlDocument xmldoc;
        xmldoc = r.Get_Job_Questions(userEmailId, userPassword, job_ID, Vendor_ID);

        return xmldoc;

    }


    [WebMethod]
    public XmlDocument Insert_Job_Questions(string userEmailId, string userPassword, string employee_id, string vendorID, string clientID,
        string question_1, string rating1, string question_2, string rating2, string question_3, string rating3, string question_4,
        string rating4, string question_5, string rating5, string userID, string jobID)
    {
        Janal j = new Janal();
        XmlDocument xmldoc;
        xmldoc = j.Insert_Job_Questions(userEmailId, userPassword, employee_id, vendorID, clientID, question_1, rating1, question_2,
            rating2, question_3, rating3, question_4,
         rating4, question_5, rating5, userID, jobID);

        return xmldoc;

    }

    [WebMethod]
    public XmlDocument Show_Employee_Reports(string userEmailId, string userPassword, string vendorID)
    {
        Raman r = new Raman();
        XmlDocument xmldoc;
        xmldoc = r.Show_Employee_Reports(userEmailId, userPassword, vendorID);

        return xmldoc;
    }



    [WebMethod]
    public XmlDocument get_CLient(string userEmailId, string userPassword, string client_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_CLient(userEmailId, userPassword, client_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument insert_client(string userEmailId, string userPassword, string clientname, string pm_id, string business_type_id, string caddress1, string caddress2, string ccity, string cpostal_code, string ccountry, string cphonenumber, string cfaxnumber)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.insert_client(userEmailId, userPassword, clientname, pm_id, business_type_id, caddress1, caddress2, ccity, cpostal_code, ccountry, cphonenumber, cfaxnumber);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument update_Client(string userEmailId, string userPassword, int ClientID, string ClientName, string pm_id, string business_type_id, string Caddress1, string Caddress2, string Ccity, string Cpostal_code, string Ccountry, string CPhoneNumber, string CFaxNumber)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.update_Client(userEmailId, userPassword, ClientID, ClientName, pm_id, business_type_id, Caddress1, Caddress2, Ccity, Cpostal_code, Ccountry, CPhoneNumber, CFaxNumber);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument delete_Client(string userEmailId, string userPassword, string ClientID)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.delete_Client(userEmailId, userPassword, ClientID);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument get_Recent_jobs(string userEmailId, string userPassword, string utype_id, string User_id, string VendorID)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_Recent_jobs(userEmailId, userPassword, utype_id, User_id, VendorID);

        return xmldoc;
    }


    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_PMO_To_Vendor(string userEmailId, string userPassword, string pmo_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_PMO_To_Vendor(userEmailId, userPassword, pmo_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument update_TimeSheet(string userEmailId, string userPassword, string employeeID, string sDay, string sMonth, string sYear, string sHours)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.update_TimeSheet(userEmailId, userPassword, employeeID, sDay, sMonth, sYear, sHours);

        return xmldoc;
    }



    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_PMO_To_Client(string userEmailId, string userPassword, string pmo_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_PMO_To_Client(userEmailId, userPassword, pmo_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_VENDOR_To_PMO(string userEmailId, string userPassword, string vendor_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_VENDOR_To_PMO(userEmailId, userPassword, vendor_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_Vendor_To_Employee(string userEmailId, string userPassword, string vendor_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_Vendor_To_Employee(userEmailId, userPassword, vendor_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_VENDOR_To_Client(string userEmailId, string userPassword, string vendor_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_VENDOR_To_Client(userEmailId, userPassword, vendor_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_CLient_To_PMO(string userEmailId, string userPassword, string client_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_CLient_To_PMO(userEmailId, userPassword, client_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument DropDown_CLient_To_Vendor(string userEmailId, string userPassword, string client_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.DropDown_CLient_To_Vendor(userEmailId, userPassword, client_id);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Send_Message_PMO(string userEmailId, string userPassword, string Msg_Subject, string user_id, string message, string pmo_id, string Vendor_id, string client_id, string Actions)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Send_Message_PMO(userEmailId, userPassword, Msg_Subject, user_id, message, pmo_id, Vendor_id, client_id, Actions);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Reply_From_PMO(string userEmailId, string userPassword, string user_id, string message_id, string message, string pmo_id, string Vendor_id, string client_id, string Actions, string Msg_Subject)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Reply_From_PMO(userEmailId, userPassword, user_id, message_id, message, pmo_id, Vendor_id, client_id, Actions, Msg_Subject);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument get_All_Messages_For_PMO(string userEmailId, string userPassword, string pmo_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_All_Messages_For_PMO(userEmailId, userPassword, pmo_id);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument get_All_Messages_for_Vendor(string userEmailId, string userPassword, string Vendor_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_All_Messages_for_Vendor(userEmailId, userPassword, Vendor_id);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Send_Message_Vendor(string userEmailId, string userPassword, string user_id, string Msg_Subject, string message, string pmo_id, string Vendor_id, string client_id, string Employee_id, string Actions)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Send_Message_Vendor(userEmailId, userPassword, user_id, Msg_Subject, message, pmo_id, Vendor_id, client_id, Employee_id, Actions);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Send_Message_Employee(string userEmailId, string userPassword, string vendor_id, string employee_id, string Msg_Subject, string Actions, string message)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Send_Message_Employee(userEmailId, userPassword, vendor_id, employee_id, Msg_Subject, Actions, message);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Reply_from_Vendor(string userEmailId, string userPassword, string user_id, string message_id, string message, string vendor_id, string pmo_id, string client_id, string employee_id, string Actions, string MsgSubject)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Reply_from_Vendor(userEmailId, userPassword, user_id, message_id, message, vendor_id, pmo_id, client_id, employee_id, Actions, MsgSubject);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Reply_from_Client(string userEmailId, string userPassword, string message_id, string message, string pmo_id, string Vendor_id, string client_id, string Actions, string MsgSubject)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Reply_from_Client(userEmailId, userPassword, message_id, message, pmo_id, Vendor_id, client_id, Actions, MsgSubject);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument get_All_Messages_for_Client(string userEmailId, string userPassword, string client_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_All_Messages_for_Client(userEmailId, userPassword, client_id);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument get_All_Messages_for_Employee(string userEmailId, string userPassword, string employee_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_All_Messages_for_Employee(userEmailId, userPassword, employee_id);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Reply_from_Employee(string userEmailId, string userPassword, string message_id, string message, string vendor_id, string employee_id, string Actions, string MsgSubject)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Reply_from_Employee(userEmailId, userPassword, message_id, message, vendor_id, employee_id, Actions, MsgSubject);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Send_Message_client(string userEmailId, string userPassword, string Msg_Subject, string message, string vendor_id, string pmo_id, string client_id, string Actions)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Send_Message_client(userEmailId, userPassword, Msg_Subject, message, vendor_id, pmo_id, client_id, Actions);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument Show_notification_for_Vendor(string userEmailId, string userPassword, string user_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Show_notification_for_Vendor(userEmailId, userPassword, user_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument Show_notification_for_PMO(string userEmailId, string userPassword, string user_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Show_notification_for_PMO(userEmailId, userPassword, user_id);

        return xmldoc;
    }
    //Raman's Web Service
    [WebMethod]
    public XmlDocument Show_notification_for_Employee(string userEmailId, string userPassword, string user_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Show_notification_for_Employee(userEmailId, userPassword, user_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument Show_notification_for_Client(string userEmailId, string userPassword, string user_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.Show_notification_for_Client(userEmailId, userPassword, user_id);

        return xmldoc;
    }

    //Raman's Web Service
    [WebMethod]
    public XmlDocument get_ISRead(string userEmailId, string userPassword, string message_id)
    {
        Raman r = new Raman();

        XmlDocument xmldoc;
        xmldoc = r.get_ISRead(userEmailId, userPassword, message_id);

        return xmldoc;
    }
    [WebMethod]
    public XmlDocument Insert_Jobquestions_ratings(string userEmailId, string userPassword, string vendorID, string clientID,
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
                        string sql = "insert into dbo.ovms_jobquestion_rating(job_question_1,rating_1,job_question_2,rating_2,job_question_3,rating_3,job_question_4,rating_4,job_question_5,rating_5,vendor_id,client_id,user_id,job_id) values( '" + question_1 + "', '" + rating1 + "', '" + question_2 + "','" + rating2 + "', '" + question_3 + "','" + rating3 + "' ,'" + question_4 + "','" + rating4 + "', '" + question_5 + "','" + rating5 + "','" + vendorID + "','" + clientID + "','" + userID + "','" + jobID + "') ";
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
    public XmlDocument Insert_emp_ratings(string userEmailId, string userPassword, string employee_id, string vendorID, string clientID,
        string rating1, string rating2, string rating3,
        string rating4, string rating5, string userID, string jobID)
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

                            "<RATING1>" + rating1 + "</RATING1>" +

                            "<RATING2>" + rating2 + "</RATING2>" +

                            "<RATING3>" + rating3 + "</RATING3>" +

                            "<RATING4>" + rating4 + "</RATING4>" +

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
                        string sql = "insert into dbo.ovms_employee_ratings(employee_id,rating_1,rating_2,rating_3,rating_4,rating_5,vendor_id,client_id,user_id,job_id) values('" + employee_id + "',  '" + rating1 + "', '" + rating2 + "','" + rating3 + "' ,'" + rating4 + "','" + rating5 + "','" + vendorID + "','" + clientID + "','" + userID + "','" + jobID + "') ";
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
    public XmlDocument get_jobrating(string userEmailId, string userPassword, string JOB_ID)
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
                string strSql = "select job_question_1,rating_1,job_question_2,rating_2,job_question_3,rating_3,job_question_4,rating_4,job_question_5,rating_5,job_ID from ovms_jobquestion_rating where " + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int RowID = 1;
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())

                    {
                        xml_string += "<QUESTIONS_NO ID='" + RowID + "'>" +

                                        "<QUESTION1><![CDATA[" + reader["job_question_1"] + "]]></QUESTION1>" +
                                        "<RATING1>" + reader["rating_1"] + "</RATING1>" +
                                        "<QUESTION2><![CDATA[" + reader["job_question_2"] + "]]></QUESTION2>" +
                                        "<RATING2>" + reader["rating_2"] + "</RATING2>" +
                                        "<QUESTION3><![CDATA[" + reader["job_question_3"] + "]]></QUESTION3>" +
                                        "<RATING3>" + reader["rating_3"] + "</RATING3>" +
                                        "<QUESTION4><![CDATA[" + reader["job_question_4"] + "]]></QUESTION4>" +
                                        "<RATING4>" + reader["rating_4"] + "</RATING4>" +
                                        "<QUESTION5><![CDATA[" + reader["job_question_5"] + "]]></QUESTION5>" +
                                        "<RATING5>" + reader["rating_5"] + "</RATING5>" +
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
    public XmlDocument get_emprating(string userEmailId, string userPassword, string employee_id)
    {
        ///
        SqlConnection conn;
        string xml_string = "";
        string strSub = "";
        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        if (employee_id != "" & employee_id != "0")
        {
            strSub = " employee_id =" + employee_id;
        }

        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                xml_string = "<XML>" +
                            "<REQUEST>" +
                                         "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                            "</REQUEST>";
                string strSql = "select rating_1,rating_2,rating_3,rating_4,rating_5,job_ID,employee_id from ovms_employee_ratings where " + strSub;
                SqlCommand cmd = new SqlCommand(strSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                int RowID = 1;
                xml_string += "<RESPONSE>";

                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                        xml_string += "<QUESTIONS_NO ID='" + RowID + "'>" +
                                        "<RATING1>" + reader["rating_1"] + "</RATING1>" +
                                        "<RATING2>" + reader["rating_2"] + "</RATING2>" +
                                        "<RATING3>" + reader["rating_3"] + "</RATING3>" +
                                        "<RATING4>" + reader["rating_4"] + "</RATING4>" +
                                        "<RATING5>" + reader["rating_5"] + "</RATING5>" +
                                        "<JOB_ID>" + reader["job_ID"] + "</JOB_ID>" +
                                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
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
    public XmlDocument update_jobrating(string userEmailId, string userPassword, string vendorID, string clientID,
      string question_1, string rating1, string question_2, string rating2, string question_3, string rating3, string question_4,
      string rating4, string question_5, string rating5, string userID, string jobID)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                  "<REQUEST>" +
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

                    if (question_1 != "")
                    {
                        string strSql = "update ovms_jobquestion_rating set job_question_1 ='" + question_1 + "',job_question_2 ='" + question_2 + "'," +
                            "job_question_3 ='" + question_3 + "',job_question_4 ='" + question_4 + "',job_question_5 ='" + question_5 + "'," +
                            "rating_1 ='" + rating1 + "',rating_2 ='" + rating2 + "',rating_3 ='" + rating3 + "',rating_4 ='" + rating4 + "'," +
                            "rating_5 ='" + rating5 + "' where job_id =  '" + jobID + "' and vendor_id='" + vendorID + "' and client_id='" + clientID + "'and user_id='" + userID + "'";
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>rating updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>rating not updated</STRING>" +
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
    public XmlDocument update_emprating(string userEmailId, string userPassword, string employee_id, string vendorID, string clientID,
        string rating1, string rating2, string rating3,
        string rating4, string rating5, string userID, string jobID)
    {
        //
        SqlConnection conn;
        string xml_string = "";
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);
        xml_string = "<XML>" +
                                  "<REQUEST>" +
                                 "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                            "<VENDOR_ID>" + vendorID + "</VENDOR_ID>" +
                            "<CLIENT_ID>" + clientID + "</CLIENT_ID>" +

                            "<RATING1>" + rating1 + "</RATING1>" +

                            "<RATING2>" + rating2 + "</RATING2>" +

                            "<RATING3>" + rating3 + "</RATING3>" +

                            "<RATING4>" + rating4 + "</RATING4>" +

                            "<RATING5>" + rating5 + "</RATING5>" +
                            "<USER_ID>" + userID + "</USER_ID>" +
                            "<JOB_ID>" + jobID + "</JOB_ID>" +
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

                    if (rating1 != "")
                    {
                        string strSql = "update ovms_employee_ratings set " +
                         "rating_1 ='" + rating1 + "',rating_2 ='" + rating2 + "',rating_3 ='" + rating3 + "',rating_4 ='" + rating4 + "'," +
                         "rating_5 ='" + rating5 + "' where employee_id='" + employee_id + "' and job_id =  '" + jobID + "' and vendor_id='" + vendorID + "' and client_id='" + clientID + "'and user_id='" + userID + "'";
                        SqlCommand cmd = new SqlCommand(strSql, conn);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            xml_string += "<STRING>emp rating updated successfully</STRING>" +
                                       "<STATUS>1</STATUS>";
                        }
                        else
                        {
                            xml_string += "<STRING>emp rating not updated</STRING>" +
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
    public XmlDocument send_bgINFO_from_candidate(string userEmailId, string userPassword, string field_name, string field_value, string employee_id, string client_id, string vendor_id, string form_id, string doc_title, string job_id)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<FIELD_NAME>" + field_name + "</FIELD_NAME>" +
                         "<FIELD_VALUE>" + field_value + "</FIELD_VALUE>" +
                        "<EMPLOYEE_ID>" + employee_id + "</EMPLOYEE_ID>" +
                        "<CLIENT_ID>" + client_id + "</CLIENT_ID>" +
                        "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +
                         "<FORM_ID>" + form_id + "</FORM_ID>" +
                         "<DOC_TITLE>" + doc_title + "</DOC_TITLE>" +



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
                    string strSql = " insert into ovms_background_check ( client_id, document_title, field_name,field_value,vendor_id, employee_id, form_id, job_id) " +
                                   "  values('" + client_id + "', '" + doc_title + "', '" + field_name + "', '" + field_value + "', '" + vendor_id + "', '" + employee_id + "','" + form_id + "','" + job_id + "')";

                    SqlCommand cmd = new SqlCommand(strSql, conn);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        xml_string += "<INSERT_STRING>Info inserted</INSERT_STRING>";
                    }
                    else
                    {
                        xml_string += "<INSERT_STRING><ERROR>Info not inserted</ERROR> </INSERT_STRING>";

                    }
                    cmd.Dispose();
                    conn.Close();

                }
            }

            catch (Exception ex)
            {
                xml_string += "<XML>" + "<STATUS>error:100.systemerror</STATUS>";


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
    public XmlDocument bg_check_candidates(string userEmailId, string userPassword, string vendor_id)
    {
        SqlConnection conn;
        string xml_string = "";
        // logAPI.Service logService = new logAPI.Service();
        string errString = "";
        errString = VerifyUser(userEmailId, userPassword);

        xml_string = "<XML>" +
                    "<REQUEST>" +


                         "<VENDOR_ID>" + vendor_id + "</VENDOR_ID>" +




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

                    strSql = "select dbo.CamelCase(ed.first_name + ' ' + ed.last_name) full_name, " +
                             "ed.email, concat('W', clt.client_alias, '00', right('0000' + convert(varchar(4), ed.employee_id), 4)) as employee_id, " +
                             " (ed.employee_id) as em_id " +
                             "from ovms_employee_details as ed " +
                             "join ovms_employee_actions as ea on ea.employee_id = ed.employee_id " +
                             "join ovms_clients as clt on clt.client_id = ea.client_id " +
                             "where ea.candidate_approve = 1 and bg_check_done = 0 and ea.vendor_id = " + vendor_id;

                    // " select ed.employee_id, dbo.CamelCase(ed.first_name + ' ' + ed.last_name) full_name, ed.email " +
                    //                " from ovms_employee_details as ed " +
                    //                " join ovms_employee_actions as ea on ea.employee_id = ed.employee_id " +
                    //                " where ea.candidate_approve = 1 and bg_check_done=0 and ea.vendor_id = " + vendor_id;

                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int RowID = 1;
                    while (reader.Read())
                    {
                        xml_string = xml_string + "<EMPLOYEE_NAME_ID ID='" + RowID + "'>" +
                        "<EMPLOYEE_ID>" + reader["employee_id"] + "</EMPLOYEE_ID>" +
                        "<EMPLOYEE_BID>" + reader["em_id"] + "</EMPLOYEE_BID>" +
                        "<EMPLOYEE_NAME>" + reader["full_name"] + "</EMPLOYEE_NAME>" +
                        "<EMPLOYEE_EMAIL>" + reader["email"] + "</EMPLOYEE_EMAIL>" +
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

    /* ************  sms notify api ************** */

    public class ViaNettSMS
    {
        // Declarations
        private string username;
        private string password;

        /// <summary>
        /// Constructor with username and password to ViaNett gateway. 
        /// </summary>
        public ViaNettSMS(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
        /// <summary>
        /// Send SMS message through the ViaNett HTTP API.
        /// </summary>
        /// <returns>Returns an object with the following parameters: Success, ErrorCode, ErrorMessage</returns>
        /// <param name="msgsender">Message sender address. Mobile number or small text, e.g. company name</param>
        /// <param name="destinationaddr">Message destination address. Mobile number.</param>
        /// <param name="message">Text message</param>
        public Result sendSMS(string msgsender, string destinationaddr, string message)
        {
            // Declarations
            string url;
            string serverResult;
            long l;
            Result result;

            // Build the URL request for sending SMS.
            url = "http://smsc.vianett.no/ActiveServer/MT/?"
                + "username=" + HttpUtility.UrlEncode(username)
                + "&password=" + HttpUtility.UrlEncode(password)
                + "&destinationaddr=" + HttpUtility.UrlEncode(destinationaddr, System.Text.Encoding.GetEncoding("ISO-8859-1"))
                + "&message=" + HttpUtility.UrlEncode(message, System.Text.Encoding.GetEncoding("ISO-8859-1"))
                + "&refno=1";

            // Check if the message sender is numeric or alphanumeric.
            if (long.TryParse(msgsender, out l))
            {
                url = url + "&sourceAddr=" + msgsender;
            }
            else
            {
                url = url + "&fromAlpha=" + msgsender;
            }
            // Send the SMS by submitting the URL request to the server. The response is saved as an XML string.
            serverResult = DownloadString(url);
            // Converts the XML response from the server into a more structured Result object.
            result = ParseServerResult(serverResult);
            // Return the Result object.
            return result;
        }
        /// <summary>
        /// Downloads the URL from the server, and returns the response as string.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns>Returns the http/xml response as string</returns>
        /// <exception cref="WebException">WebException is thrown if there is a connection problem.</exception>
        private string DownloadString(string URL)
        {
            using (System.Net.WebClient wlc = new System.Net.WebClient())
            {
                // Create WebClient instanse.
                try
                {
                    // Download and return the xml response
                    return wlc.DownloadString(URL);
                }
                catch (WebException ex)
                {
                    // Failed to connect to server. Throw an exception with a customized text.
                    throw new WebException("Error occurred while connecting to server. " + ex.Message, ex);
                }
            }
        }


        /// <summary>
        /// Parses the XML code and returns a Result object.
        /// </summary>
        /// <param name="ServerResult">XML data from a request through HTTP API.</param>
        /// <returns>Returns a Result object with the parsed data.</returns>
        private Result ParseServerResult(string ServerResult)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            System.Xml.XmlNode ack;
            Result result = new Result();
            xDoc.LoadXml(ServerResult);
            ack = xDoc.GetElementsByTagName("ack")[0];
            result.ErrorCode = int.Parse(ack.Attributes["errorcode"].Value);
            result.ErrorMessage = ack.InnerText;
            result.Success = (result.ErrorCode == 0);
            return result;
        }

        /// <summary>
        /// The Result object from the SendSMS function, which returns Success(Boolean), ErrorCode(Integer), ErrorMessage(String).
        /// </summary>
        public class Result
        {
            public bool Success;
            public int ErrorCode;
            public string ErrorMessage;
        }
    }
    [WebMethod]
    public void client_smsNotification(string vendorids, string messagesend, string clientID, string notify)
    {
        string username = "deepthi.karri1990@gmail.com";
        string password = "s3dr1";
        string msgsender = "16479663639";
        string[] vendorIDs = vendorids.Split(',');
        string message = messagesend;
        SqlConnection conn;
        /* need to add condition when only enables  */

        conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["dbconn"].ConnectionString);
        try
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
                string querystr = "SELECT TOP 1 " + notify + " FROM ovms_client_smsnotification WHERE client_id = " + clientID;

                SqlCommand cmd = new SqlCommand(querystr, conn);
                SqlDataReader readers = cmd.ExecuteReader();
                if (readers.HasRows == true)
                {
                    while (readers.Read())
                    {
                        if (Int32.Parse(readers[notify].ToString()) == 1)
                        {
                            ViaNettSMS s = new ViaNettSMS(username, password);
                            // Declare Result object returned by the SendSMS function
                            ViaNettSMS.Result result;

                            foreach (string vendorID in vendorIDs)
                            {
                                if (int.Parse(vendorID) != null)
                                {
                                    string pno = "select v.vendor_id, (vd.vendor_PhoneNumber)phone from ovms_vendors as v " +
                                                 " join ovms_vendor_details as vd on vd.vendor_id = v.vendor_id " +
                                                   "where v.vendor_id = " + vendorID;
                                    SqlCommand cmdph = new SqlCommand(pno, conn);
                                    SqlDataReader readersph = cmdph.ExecuteReader();
                                    if (readersph.HasRows == true)
                                    {
                                        while (readersph.Read())
                                        {
                                            string destinationaddr = readersph["phone"].ToString();
                                            try
                                            {
                                                // Send SMS through HTTP API
                                                result = s.sendSMS(msgsender, destinationaddr, message);
                                                //// Show Send SMS response
                                                if (result.Success)
                                                {
                                                    System.Diagnostics.Debug.WriteLine("Message successfully sent");
                                                }
                                                else
                                                {
                                                    Debug.WriteLine("Received error: " + result.ErrorCode + " " + result.ErrorMessage);
                                                }
                                            }
                                            catch (System.Net.WebException ex)
                                            {
                                                //Catch error occurred while connecting to server.
                                                Debug.WriteLine(ex.Message);
                                            }
                                        }

                                    }
                                    readersph.Close();
                                    cmdph.Dispose();
                                }
                            }
                        }

                    }
                    readers.Close();
                    cmd.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }

    }
}


