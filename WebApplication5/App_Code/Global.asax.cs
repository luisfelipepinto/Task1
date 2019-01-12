using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Global
/// </summary>
namespace MyCompanyName
{
    public class Global : System.Web.HttpApplication
    {
        private static string m_szPaymentProcessorDomain;
        private static string m_szMerchantID;
        private static string m_szPassword;
        private static string m_szPreSharedKey;
        private static HASH_METHOD m_hmHashMethod;
        private static RESULT_DELIVERY_METHOD m_rdmResultDeliveryMethod;

        public Global()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            // get the MerchantID and Password from the config file
            m_szMerchantID = ConfigurationManager.AppSettings["MerchantID"];
            m_szPassword = ConfigurationManager.AppSettings["Password"];

            // get the PaymentProcessorDomain
            m_szPaymentProcessorDomain = ConfigurationManager.AppSettings["PaymentProcessorDomain"];

            // get the PreSharedKey
            m_szPreSharedKey = ConfigurationManager.AppSettings["PreSharedKey"];
            // get the HashMethod
            m_hmHashMethod = PaymentFormHelper.GetHashMethod(ConfigurationManager.AppSettings["HashMethod"]);

            // get the ResultDeliveryMethod
            m_rdmResultDeliveryMethod = PaymentFormHelper.GetResultDeliveryMethod(ConfigurationManager.AppSettings["ResultDeliveryMethod"]);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        public static string PaymentProcessorDomain { get { return (m_szPaymentProcessorDomain); } }
        public static string MerchantID { get { return (m_szMerchantID); } }
        public static string Password { get { return (m_szPassword); } }
        public static string PreSharedKey { get { return (m_szPreSharedKey); } }
        public static HASH_METHOD HashMethod { get { return (m_hmHashMethod); } }
        public static RESULT_DELIVERY_METHOD ResultDeliveryMethod { get { return (m_rdmResultDeliveryMethod); } }
    }
}
