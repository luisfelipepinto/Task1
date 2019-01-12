using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyCompanyName;

public partial class DisplayTransactionResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TransactionResult trTransactionResult;
        string szMessage;
        string szMessageClass;
        bool boResultValidationSuccessful;
        string szValidateErrorMessage;
        bool boDuplicateTransaction;
        string szPreviousTransactionMessage;
        string szPaymentFormResultHandler;
        string szOutputMessage;

	    // what we do here depends on the ResultDeliveryMethod
	    boDuplicateTransaction = false;
	    szPreviousTransactionMessage = "";
        boResultValidationSuccessful = false;
        szValidateErrorMessage = null;
        trTransactionResult = null;

        // check if transaction was cancelled
        if (Request.QueryString["StatusCode"] != null && Request.QueryString["StatusCode"] == "25")
        {
            // validate cancel result
            boResultValidationSuccessful = PaymentFormHelper.ValidateTransactionResult_CANCEL(Global.MerchantID,
                                                                                        Global.Password,
                                                                                        Global.PreSharedKey,
                                                                                        Global.HashMethod,
                                                                                        Request.QueryString,
                                                                                        out trTransactionResult,
                                                                                        out szValidateErrorMessage);

            // the results need to be stored here as this is the first time
            // they will have touched this system
            if (boResultValidationSuccessful)
            {
                if (!PaymentFormHelper.ReportTransactionResults(trTransactionResult, out szOutputMessage))
                {
                    // handle the case where the results aren't stored correctly
                }
            }
        }
        else
        {

            switch (Global.ResultDeliveryMethod)
            {
                case RESULT_DELIVERY_METHOD.POST:
                    // the results will be delivered via POST variables to this
                    // page	
                    boResultValidationSuccessful = PaymentFormHelper.ValidateTransactionResult_POST(Global.MerchantID,
                                                                                               Global.Password,
                                                                                               Global.PreSharedKey,
                                                                                               Global.HashMethod,
                                                                                               Request.Form,
                                                                                               out trTransactionResult,
                                                                                               out szValidateErrorMessage);
                    // the results need to be stored here as this is the first time
                    // they will have touched this system
                    if (boResultValidationSuccessful)
                    {
                        if (!PaymentFormHelper.ReportTransactionResults(trTransactionResult, out szOutputMessage))
                        {
                            // handle the case where the results aren't stored correctly
                        }
                    }
                    break;
                case RESULT_DELIVERY_METHOD.SERVER:
                    // the results have already been delivered via a server-to-server
                    // call from the payment form to the ServerResultURL
                    // need to query these transaction results to display
                    boResultValidationSuccessful = PaymentFormHelper.ValidateTransactionResult_SERVER(Global.MerchantID,
                                                                                               Global.Password,
                                                                                               Global.PreSharedKey,
                                                                                               Global.HashMethod,
                                                                                               Request.QueryString,
                                                                                               out trTransactionResult,
                                                                                               out szValidateErrorMessage);
                    break;
                case RESULT_DELIVERY_METHOD.SERVER_PULL:
                    // need to query the results from the payment form using the passed
                    // cross reference
                    szPaymentFormResultHandler = "https://mms." + Global.PaymentProcessorDomain + "/Pages/PublicPages/PaymentFormResultHandler.ashx";

                    boResultValidationSuccessful = PaymentFormHelper.ValidateTransactionResult_SERVER_PULL(Global.MerchantID,
                                                                                                Global.Password,
                                                                                                Global.PreSharedKey,
                                                                                                Global.HashMethod,
                                                                                                Request.QueryString,
                                                                                                szPaymentFormResultHandler,
                                                                                                out trTransactionResult,
                                                                                                out szValidateErrorMessage);
                    // the results need to be stored here as this is the first time
                    // they will have touched this system
                    if (boResultValidationSuccessful)
                    {
                        if (!PaymentFormHelper.ReportTransactionResults(trTransactionResult, out szOutputMessage))
                        {
                            // handle the case where the results aren't stored correctly
                        }
                    }
                    break;
            }
        }

    	// display an error message if the transaction result couldn't be validated
	    if (!boResultValidationSuccessful)
	    {
		    szMessageClass = "ErrorMessage";
		    szMessage = szValidateErrorMessage;
	    }
	    else
	    {
		    switch (trTransactionResult.StatusCode)
		    {
			    case 0:
				    szMessageClass = "SuccessMessage";
				    break;
			    case 4:
				    szMessageClass = "ErrorMessage";
				    break;
			    case 5:
				    szMessageClass = "ErrorMessage";
				    break;
			    case 20:
				    boDuplicateTransaction = true;
				    if (trTransactionResult.PreviousStatusCode.Value == 0)
				    {
					    szMessageClass = "SuccessMessage";
				    }
				    else
				    {
					    szMessageClass = "ErrorMessage";
				    }
				    szPreviousTransactionMessage = trTransactionResult.PreviousMessage;
				    break;
                case 25:
                    szMessageClass = "ErrorMessage";
                    break;
			    case 30:
				    szMessageClass = "ErrorMessage";
				    break;
			    default:
				    szMessageClass = "ErrorMessage";
				    break;
		    }

		    szMessage = trTransactionResult.Message;
	    }
        lbMessage.Text = szMessage;
        pnMessagePanel.CssClass = szMessageClass;

        if (boDuplicateTransaction)
        {
            pnDuplicateTransactionPanel.Visible = true;
            lbPreviousTransactionMessage.Text = szPreviousTransactionMessage;
        }
    }
}
