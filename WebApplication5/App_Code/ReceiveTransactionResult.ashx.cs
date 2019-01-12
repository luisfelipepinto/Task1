using System;
using System.Web;
using System.Text;
using MyCompanyName;

public class ReceiveTransactionResult : IHttpHandler
{
    public bool IsReusable { get { return false; } }

    public void ProcessRequest(HttpContext hcHttpContext)
    {
        StringBuilder sbOutputString;
        int nStatusCode = 30;
        string szMessage = "";
        string szHashDigest;
        TransactionResult trTransactionResult;
        string szUpdateOrderMessage;

        sbOutputString = new StringBuilder();

        try
        {
            if (!PaymentFormHelper.GetTransactionResultFromPostVariables(hcHttpContext.Request.Form, out trTransactionResult, out szHashDigest, out szMessage))
            {
                nStatusCode = 30;
            }
            else
            {
                if (!PaymentFormHelper.ReportTransactionResults(trTransactionResult,
                                                                out szUpdateOrderMessage))
                {
                    nStatusCode = 30;
                    szMessage = szMessage + szUpdateOrderMessage;
                }
                else
                {
                    nStatusCode = 0;
                }
            }
        }
        catch (Exception exc)
        {
            nStatusCode = 30;
            szMessage = exc.Message;
        }
        finally
        {
            if (nStatusCode != 0 &&
                String.IsNullOrEmpty(szMessage))
            {
                szMessage = "Unknown error";
            }
        }

        hcHttpContext.Response.ContentType = "text/plain";
        sbOutputString.AppendFormat("StatusCode={0}&Message={1}", nStatusCode, szMessage);
        hcHttpContext.Response.Write(sbOutputString.ToString());
    }
}