using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for PaymentFormHelper
/// </summary>
namespace MyCompanyName
{
    public enum RESULT_DELIVERY_METHOD
    {
        UNKNOWN,
        POST,
        SERVER,
        SERVER_PULL
    }
    public enum HASH_METHOD
    {
        UNKNOWN,
        MD5,
        SHA1,
        HMACMD5,
        HMACSHA1
    }

	public class TransactionResult
	{
		private int m_nStatusCode;
		private string m_szMessage;
		private int? m_nPreviousStatusCode;
		private string m_szPreviousMessage;
		private string m_szCrossReference;
		private UInt64 m_nAmount;
		private int m_nCurrencyCode;
		private string m_szOrderID;
		private string m_szTransactionType;
		private string m_szTransactionDateTime;
		private string m_szOrderDescription;
		private string m_szCustomerName;
		private string m_szAddress1;
		private string m_szAddress2;
		private string m_szAddress3;
		private string m_szAddress4;
		private string m_szCity;
		private string m_szState;
		private string m_szPostCode;
		private int? m_nCountryCode;

		public int StatusCode 
		{
            get { return (m_nStatusCode); }  
	        set { m_nStatusCode = value; }	
        }
		public string Message
		{ 
			get { return (m_szMessage); } 
			set { m_szMessage = value; }
		}
		public int? PreviousStatusCode
		{
			get { return (m_nPreviousStatusCode); }
			set { m_nPreviousStatusCode = value; }
		}
		public string PreviousMessage
		{
			get { return (m_szPreviousMessage); }
			set { m_szPreviousMessage = value; }
		}
		public string CrossReference
		{
			get { return (m_szCrossReference); }
			set { m_szCrossReference = value; }
		}
		public UInt64 Amount
		{
			get { return (m_nAmount); }
			set { m_nAmount = value; }
		}
		public int CurrencyCode
		{
			get { return (m_nCurrencyCode); }
			set { m_nCurrencyCode = value; }
		}
		public string OrderID
		{
			get { return (m_szOrderID); }
			set { m_szOrderID = value; }
		}
		public string TransactionType
		{
			get { return (m_szTransactionType); }
			set { m_szTransactionType = value; }
		}
		public string TransactionDateTime
		{
			get { return (m_szTransactionDateTime); }
			set { m_szTransactionDateTime = value; }
		}
		public string OrderDescription
		{
			get { return (m_szOrderDescription); }
			set { m_szOrderDescription = value; }
		}
		public string CustomerName
		{
			get { return (m_szCustomerName); }
			set { m_szCustomerName = value; }
		}
		public string Address1
		{
			get { return (m_szAddress1); }
			set { m_szAddress1 = value; }
		}
		public string Address2
		{
			get { return (m_szAddress2); }
			set { m_szAddress2 = value; }
		}
		public string Address3
		{
			get { return (m_szAddress3); }
			set { m_szAddress3 = value; }
		}
		public string Address4
		{
			get { return (m_szAddress4); }
			set { m_szAddress4 = value; }
		}
		public string City
		{
			get { return (m_szCity); }
			set { m_szCity = value; }
		}
		public string State
		{
			get { return (m_szState); }
			set { m_szState = value; }
		}
		public string PostCode
		{
			get { return (m_szPostCode); }
			set { m_szPostCode = value; }
		}
		public int? CountryCode
		{
			get { return (m_nCountryCode); }
			set { m_nCountryCode = value; }
		}
	}

    public class PaymentFormHelper
    {
        public static string GetHashMethod(HASH_METHOD hmHashMethod)
        {
            return (hmHashMethod.ToString());
        }
        public static HASH_METHOD GetHashMethod(string szHashMethod)
        {
            HASH_METHOD hmHashMethod = HASH_METHOD.UNKNOWN;

            if (String.IsNullOrEmpty(szHashMethod))
            {
                throw new Exception("Hash method must not be null or empty");
            }
            if (szHashMethod.ToUpper() == "MD5")
            {
                hmHashMethod = HASH_METHOD.MD5;
                goto Finished;
            }
            if (szHashMethod.ToUpper() == "SHA1")
            {
                hmHashMethod = HASH_METHOD.SHA1;
                goto Finished;
            }
            if (szHashMethod.ToUpper() == "HMACMD5")
            {
                hmHashMethod = HASH_METHOD.HMACMD5;
                goto Finished;
            }
            if (szHashMethod.ToUpper() == "HMACSHA1")
            {
                hmHashMethod = HASH_METHOD.HMACSHA1;
                goto Finished;
            }
        Finished: ;
            if (hmHashMethod == HASH_METHOD.UNKNOWN)
            {
                throw new Exception("Invalid hash method: " + szHashMethod);
            }

            return (hmHashMethod);
        }
        public static string GetResultDeliveryMethod(RESULT_DELIVERY_METHOD rdmResultDeliveryMethod)
        {
            return (rdmResultDeliveryMethod.ToString());
        }
        public static RESULT_DELIVERY_METHOD GetResultDeliveryMethod(string szResultDeliveryMethod)
        {
            RESULT_DELIVERY_METHOD rdmResultDeliveryMethod = RESULT_DELIVERY_METHOD.UNKNOWN;

            if (String.IsNullOrEmpty(szResultDeliveryMethod))
            {
                throw new Exception("Result delivery method must not be null or empty");
            }
            if (szResultDeliveryMethod.ToUpper() == "POST")
            {
                rdmResultDeliveryMethod = RESULT_DELIVERY_METHOD.POST;
                goto Finished;
            }
            if (szResultDeliveryMethod.ToUpper() == "SERVER")
            {
                rdmResultDeliveryMethod = RESULT_DELIVERY_METHOD.SERVER;
                goto Finished;
            }
            if (szResultDeliveryMethod.ToUpper() == "SERVER_PULL")
            {
                rdmResultDeliveryMethod = RESULT_DELIVERY_METHOD.SERVER_PULL;
                goto Finished;
            }
        Finished: ;
            if (rdmResultDeliveryMethod == RESULT_DELIVERY_METHOD.UNKNOWN)
            {
                throw new Exception("Invalid result delivery method: " + szResultDeliveryMethod);
            }

            return (rdmResultDeliveryMethod);
        }
        public static string GetSiteSecureBaseURL(HttpRequest hrHTTPRequest)
        {
            string szReturnString = null;
            int nIndex;

            szReturnString = hrHTTPRequest.Url.AbsoluteUri;

            nIndex = szReturnString.LastIndexOf('/');

            szReturnString = szReturnString.Substring(0, nIndex + 1);

            return (szReturnString);
        }        
        public static bool GetTransactionReferenceFromQueryString(NameValueCollection nvcQueryStringVariables, out string szCrossReference, out string szOrderID, out string szHashDigest, out string szOutputMessage)
		{
            bool boErrorOccurred;

			szHashDigest = "";
			szOutputMessage = "";
			boErrorOccurred = false;
            szCrossReference = null;
            szOrderID = null;

			try
			{
				// hash digest
				if (nvcQueryStringVariables["HashDigest"] != null)
				{
					szHashDigest = nvcQueryStringVariables["HashDigest"];
				}

				// cross reference of transaction
				if (nvcQueryStringVariables["CrossReference"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [CrossReference] not received");
					boErrorOccurred = true;
				}
				else
				{
					szCrossReference = nvcQueryStringVariables["CrossReference"];
				}
				// order ID (same as value passed into payment form - echoed back out by payment form)
				if (nvcQueryStringVariables["OrderID"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [OrderID] not received");
					boErrorOccurred = true;
				}
				else
				{
					szOrderID = nvcQueryStringVariables["OrderID"];
				}
			}
			catch (Exception e)
			{
				boErrorOccurred = true;
				szOutputMessage = e.Message;
			}

			return (!boErrorOccurred);
		}
        public static bool GetTransactionReferenceFromQueryString(NameValueCollection nvcFormVariables, out TransactionResult trTransactionResult, out string szHashDigest, out string szOutputMessage)
        {
            bool boErrorOccurred;
            int nStatusCode = 30;
            string szMessage = null;
            string szOrderID = null;
            string szTransactionType = null;
            string szTransactionDateTime = null;
            string szOrderDescription = null;
            string szCustomerName = null;
            string szAddress1 = null;
            string szAddress2 = null;
            string szAddress3 = null;
            string szAddress4 = null;
            string szCity = null;
            string szState = null;
            string szPostCode = null;
            int? nCountryCode = null;

            trTransactionResult = null;
            szHashDigest = "";
            szOutputMessage = "";
            boErrorOccurred = false;

            try
            {
                // hash digest
                if (nvcFormVariables["HashDigest"] != null)
                {
                    szHashDigest = nvcFormVariables["HashDigest"];
                }

                // transaction status code
                if (nvcFormVariables["StatusCode"] == null)
                {
                    szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [StatusCode] not received");
                    boErrorOccurred = true;
                }
                else
                {
                    nStatusCode = Convert.ToInt32(nvcFormVariables["StatusCode"]);
                }
                // transaction message
                if (nvcFormVariables["Message"] == null)
                {
                    szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Message] not received");
                    boErrorOccurred = true;
                }
                else
                {
                    szMessage = nvcFormVariables["Message"];
                }
                // order ID (same as value passed into payment form - echoed back out by payment form)
                if (nvcFormVariables["OrderID"] == null)
                {
                    szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [OrderID] not received");
                    boErrorOccurred = true;
                }
                else
                {
                    szOrderID = nvcFormVariables["OrderID"];
                }

                if (!boErrorOccurred)
                {
                    trTransactionResult = new TransactionResult();
                    trTransactionResult.StatusCode = nStatusCode; // transaction status code
                    trTransactionResult.Message = szMessage; // transaction message
                   
                    trTransactionResult.OrderID = szOrderID; // order ID echoed back
                    trTransactionResult.TransactionType = szTransactionType; // transaction type echoed back
                    trTransactionResult.TransactionDateTime = szTransactionDateTime; // transaction date/time echoed back
                    trTransactionResult.OrderDescription = szOrderDescription; // order description echoed back
                    // the customer details that were actually
                    // processed  = might be different
                    // from those passed to the payment form
                    trTransactionResult.CustomerName = szCustomerName;
                    trTransactionResult.Address1 = szAddress1;
                    trTransactionResult.Address2 = szAddress2;
                    trTransactionResult.Address3 = szAddress3;
                    trTransactionResult.Address4 = szAddress4;
                    trTransactionResult.City = szCity;
                    trTransactionResult.State = szState;
                    trTransactionResult.PostCode = szPostCode;
                    trTransactionResult.CountryCode = nCountryCode;
                }
            }
            catch (Exception e)
            {
                boErrorOccurred = true;
                szOutputMessage = e.Message;
            }

            return (!boErrorOccurred);
        }
		public static bool GetTransactionResultFromPostVariables(NameValueCollection nvcFormVariables, out TransactionResult trTransactionResult, out string szHashDigest, out string szOutputMessage)
		{
            bool boErrorOccurred;
		    int nStatusCode = 30;
		    string szMessage = null;
		    int? nPreviousStatusCode = null;
		    string szPreviousMessage = null;
		    string szCrossReference = null;
		    UInt64 nAmount = 0;
		    int nCurrencyCode = 0;
		    string szOrderID = null;
		    string szTransactionType = null;
		    string szTransactionDateTime = null;
		    string szOrderDescription = null;
		    string szCustomerName = null;
            string szAddress1 = null;
            string szAddress2 = null;
            string szAddress3 = null;
            string szAddress4 = null;
            string szCity = null;
            string szState = null;
            string szPostCode = null;
            int? nCountryCode = null;

			trTransactionResult = null;
			szHashDigest = "";
			szOutputMessage = "";			
			boErrorOccurred = false;

			try
			{
				// hash digest
				if (nvcFormVariables["HashDigest"] != null)
				{
					szHashDigest = nvcFormVariables["HashDigest"];
				}

				// transaction status code
				if (nvcFormVariables["StatusCode"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [StatusCode] not received");
					boErrorOccurred = true;
				}
				else
				{
					nStatusCode = Convert.ToInt32(nvcFormVariables["StatusCode"]);
				}
				// transaction message
				if (nvcFormVariables["Message"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Message] not received");
					boErrorOccurred = true;
				}
				else
				{
					szMessage = nvcFormVariables["Message"];
				}
				// status code of original transaction if this transaction was deemed a duplicate
				if (nvcFormVariables["PreviousStatusCode"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [PreviousStatusCode] not received");
					boErrorOccurred = true;
				}
				else
				{
					if (nvcFormVariables["PreviousStatusCode"] == "")
					{
						nPreviousStatusCode = null;
					}
					else
					{
						nPreviousStatusCode = Convert.ToInt32(nvcFormVariables["PreviousStatusCode"]);
					}
				}
				// status code of original transaction if this transaction was deemed a duplicate
				if (nvcFormVariables["PreviousMessage"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [PreviousMessage] not received");
					boErrorOccurred = true;
				}
				else
				{
					szPreviousMessage = nvcFormVariables["PreviousMessage"];
				}
				// cross reference of transaction
				if (nvcFormVariables["CrossReference"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [CrossReference] not received");
					boErrorOccurred = true;
				}
				else
				{
					szCrossReference = nvcFormVariables["CrossReference"];
				}
				// amount (same as value passed into payment form - echoed back out by payment form)
				if (nvcFormVariables["Amount"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Amount] not received");
					boErrorOccurred = true;
				}
				else
				{
					nAmount = Convert.ToUInt64(nvcFormVariables["Amount"]);
				}
				// currency code (same as value passed into payment form - echoed back out by payment form)
				if (nvcFormVariables["CurrencyCode"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [CurrencyCode] not received");
					boErrorOccurred = true;
				}
				else
				{
					nCurrencyCode = Convert.ToInt32(nvcFormVariables["CurrencyCode"]);
				}
				// order ID (same as value passed into payment form - echoed back out by payment form)
				if (nvcFormVariables["OrderID"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [OrderID] not received");
					boErrorOccurred = true;
				}
				else
				{
					szOrderID = nvcFormVariables["OrderID"];
				}
				// transaction type (same as value passed into payment form - echoed back out by payment form)
				if (nvcFormVariables["TransactionType"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [TransactionType] not received");
					boErrorOccurred = true;
				}
				else
				{
					szTransactionType = nvcFormVariables["TransactionType"];
				}
				// transaction date/time (same as value passed into payment form - echoed back out by payment form)
				if (nvcFormVariables["TransactionDateTime"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [TransactionDateTime] not received");
					boErrorOccurred = true;
				}
				else
				{
					szTransactionDateTime = nvcFormVariables["TransactionDateTime"];
				}
				// order description (same as value passed into payment form - echoed back out by payment form)
				if (nvcFormVariables["OrderDescription"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [OrderDescription] not received");
					boErrorOccurred = true;
				}
				else
				{
					szOrderDescription = nvcFormVariables["OrderDescription"];
				}
				// customer name (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["CustomerName"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [CustomerName] not received");
					boErrorOccurred = true;
				}
				else
				{
					szCustomerName = nvcFormVariables["CustomerName"];
				}
				// address1 (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["Address1"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Address1] not received");
					boErrorOccurred = true;
				}
				else
				{
					szAddress1 = nvcFormVariables["Address1"];
				}
				// address2 (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["Address2"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Address2] not received");
					boErrorOccurred = true;
				}
				else
				{
					szAddress2 = nvcFormVariables["Address2"];
				}
				// address3 (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["Address3"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Address3] not received");
					boErrorOccurred = true;
				}
				else
				{
					szAddress3 = nvcFormVariables["Address3"];
				}
				// address4 (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["Address4"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [Address4] not received");
					boErrorOccurred = true;
				}
				else
				{
					szAddress4 = nvcFormVariables["Address4"];
				}
				// city (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["City"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [City] not received");
					boErrorOccurred = true;
				}
				else
				{
					szCity = nvcFormVariables["City"];
				}
				// state (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["State"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [State] not received");
					boErrorOccurred = true;
				}
				else
				{
					szState = nvcFormVariables["State"];
				}
				// post code (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["PostCode"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [PostCode] not received");
					boErrorOccurred = true;
				}
				else
				{
					szPostCode = nvcFormVariables["PostCode"];
				}
				// country code (not necessarily the same as value passed into payment form - as the customer can change it on the form)
				if (nvcFormVariables["CountryCode"] == null)
				{
					szOutputMessage = AddStringToStringList(szOutputMessage, "Expected variable [CountryCode] not received");
					boErrorOccurred = true;
				}
				else
				{
					if (nvcFormVariables["CountryCode"] == "")
					{
						nCountryCode = null;
					}
					else
					{
						nCountryCode = Convert.ToInt32(nvcFormVariables["CountryCode"]);
					}
				}

				if (!boErrorOccurred)
				{
					trTransactionResult = new TransactionResult();
					trTransactionResult.StatusCode = nStatusCode; // transaction status code
					trTransactionResult.Message = szMessage; // transaction message
					trTransactionResult.PreviousStatusCode = nPreviousStatusCode; // status code of original transaction if duplicate transaction
					trTransactionResult.PreviousMessage = szPreviousMessage; // status code of original transaction if duplicate transaction
					trTransactionResult.CrossReference = szCrossReference;	// cross reference of transaction
					trTransactionResult.Amount = nAmount; // amount echoed back
					trTransactionResult.CurrencyCode = nCurrencyCode; // currency code echoed back
					trTransactionResult.OrderID = szOrderID; // order ID echoed back
					trTransactionResult.TransactionType = szTransactionType; // transaction type echoed back
					trTransactionResult.TransactionDateTime = szTransactionDateTime; // transaction date/time echoed back
					trTransactionResult.OrderDescription = szOrderDescription; // order description echoed back
					// the customer details that were actually
					// processed  = might be different
					// from those passed to the payment form
					trTransactionResult.CustomerName = szCustomerName;
					trTransactionResult.Address1 = szAddress1;
					trTransactionResult.Address2 = szAddress2;
					trTransactionResult.Address3 = szAddress3;
					trTransactionResult.Address4 = szAddress4;
					trTransactionResult.City = szCity;
					trTransactionResult.State = szState;
					trTransactionResult.PostCode = szPostCode;
					trTransactionResult.CountryCode = nCountryCode;
				}
			}
			catch (Exception e)
			{
				boErrorOccurred = true;
				szOutputMessage = e.Message;
			}

			return (!boErrorOccurred);
		}
		public static string AddStringToStringList(string szExistingStringList, string szStringToAdd)
		{
            string szCommaString;
            StringBuilder sbReturnString;

			sbReturnString = new StringBuilder();
			szCommaString = "";

			if (String.IsNullOrEmpty(szStringToAdd))
			{
				sbReturnString.Append(szExistingStringList);
			}
			else
			{
				if (!String.IsNullOrEmpty(szExistingStringList))
				{
					szCommaString = ", ";
				}
				sbReturnString.AppendFormat("{0}{1}{2}", szExistingStringList, szCommaString, szStringToAdd);
			}

			return (sbReturnString.ToString());
		}        
        public static byte[] StringToByteArray(string szString)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(szString);
        }
        public static string ByteArrayToHexString(byte[] aByte)
        {
            StringBuilder sbStringBuilder;
            int nCount = 0;

            sbStringBuilder = new StringBuilder();
            for (nCount = 0; nCount < aByte.Length; nCount++)
            {
                sbStringBuilder.Append(aByte[nCount].ToString("x2"));
            }

            return (sbStringBuilder.ToString());
        }
        public static string CalculateHashDigest(string szString, string szPreSharedKey, HASH_METHOD eHashMethod)
        {
            byte[] abKey;
            byte[] abBytes;
            byte[] abHashDigest;
            MD5 md5;
            SHA1 sha1;
            HMACMD5 hmacMD5;
            HMACSHA1 hmacSHA1;

            abKey = StringToByteArray(szPreSharedKey);
            abBytes = StringToByteArray(szString);

            switch (eHashMethod)
            {
                case HASH_METHOD.HMACMD5:
                    hmacMD5 = new HMACMD5(abKey);
                    abHashDigest = hmacMD5.ComputeHash(abBytes);
                    break;
                case HASH_METHOD.HMACSHA1:
                    hmacSHA1 = new HMACSHA1(abKey);
                    abHashDigest = hmacSHA1.ComputeHash(abBytes);
                    break;
                case HASH_METHOD.MD5:
                    md5 = new MD5CryptoServiceProvider();
                    abHashDigest = md5.ComputeHash(abBytes);
                    break;
                case HASH_METHOD.SHA1:
                    sha1 = new SHA1CryptoServiceProvider();
                    abHashDigest = sha1.ComputeHash(abBytes);
                    break;
                default:
                    throw new Exception("Invalid hash method: " + eHashMethod.ToString());
            }

            return (ByteArrayToHexString(abHashDigest));
        }
        public static string GenerateStringToHash(string szMerchantID,
        										  string szPassword,
        										  string szAmount,
												  string szCurrencyCode,
												  string szOrderID,
												  string szTransactionType,
                                                  string szTransactionDateTime,
                                                  string szDisplayCancelButton,
												  string szCallbackURL,
												  string szOrderDescription,
												  string szCustomerName,
												  string szAddress1,
												  string szAddress2,
												  string szAddress3,
												  string szAddress4,
												  string szCity,
												  string szState,
												  string szPostCode,
												  string szCountryCode,
												  string szCV2Mandatory,
												  string szAddress1Mandatory,
												  string szCityMandatory,
												  string szPostCodeMandatory,
												  string szStateMandatory,
												  string szCountryMandatory,
												  string szResultDeliveryMethod,
												  string szServerResultURL,
												  string szPaymentFormDisplaysResult,
         		                                  string szPreSharedKey,
         		                                  HASH_METHOD hmHashMethod)
        {
            StringBuilder sbReturnString;
            bool boIncludePreSharedKeyInString = false;

			sbReturnString = new StringBuilder();

			switch (hmHashMethod)
			{
				case HASH_METHOD.MD5:
					boIncludePreSharedKeyInString = true;
					break;
				case HASH_METHOD.SHA1:
					boIncludePreSharedKeyInString = true;
					break;
				case HASH_METHOD.HMACMD5:
					boIncludePreSharedKeyInString = false;
					break;
				case HASH_METHOD.HMACSHA1:
					boIncludePreSharedKeyInString = false;
					break;
			}

			if (boIncludePreSharedKeyInString)
			{
				sbReturnString.AppendFormat("PreSharedKey={0}&",szPreSharedKey);
			}

			sbReturnString.AppendFormat("MerchantID={0}&Password={1}&Amount={2}&CurrencyCode={3}&OrderID={4}&TransactionType={5}" +
                                        "&TransactionDateTime={6}&DisplayCancelButton={7}&CallbackURL={8}&OrderDescription={9}&CustomerName={10}" +
										"&Address1={11}&Address2={12}&Address3={13}&Address4={14}&City={15}&State={16}&PostCode={17}" +
										"&CountryCode={18}&CV2Mandatory={19}&Address1Mandatory={20}&CityMandatory={21}" +
										"&PostCodeMandatory={22}&StateMandatory={23}&CountryMandatory={24}&ResultDeliveryMethod={25}" + 
										"&ServerResultURL={26}&PaymentFormDisplaysResult={27}&ServerResultURLCookieVariables=" + 
										"&ServerResultURLFormVariables=&ServerResultURLQueryStringVariables=",
                                            szMerchantID, szPassword, szAmount, szCurrencyCode, szOrderID,
										    szTransactionType, szTransactionDateTime, szDisplayCancelButton, szCallbackURL, szOrderDescription,
										    szCustomerName, szAddress1, szAddress2, szAddress3, szAddress4,
										    szCity, szState, szPostCode, szCountryCode, szCV2Mandatory, szAddress1Mandatory,
										    szCityMandatory, szPostCodeMandatory, szStateMandatory, szCountryMandatory,
                                            szResultDeliveryMethod, szServerResultURL, szPaymentFormDisplaysResult);

            return (sbReturnString.ToString());
        }
        public static string GenerateStringToHash(string szMerchantID,
												  string szPassword,
												  TransactionResult trTransactionResult,
         		                                  string szPreSharedKey,
         		                                  HASH_METHOD hmHashMethod)
        {
            StringBuilder sbReturnString;
            bool boIncludePreSharedKeyInString = false;

			sbReturnString = new StringBuilder();

			switch (hmHashMethod)
			{
				case HASH_METHOD.MD5:
					boIncludePreSharedKeyInString = true;
					break;
				case HASH_METHOD.SHA1:
					boIncludePreSharedKeyInString = true;
					break;
				case HASH_METHOD.HMACMD5:
					boIncludePreSharedKeyInString = false;
					break;
				case HASH_METHOD.HMACSHA1:
					boIncludePreSharedKeyInString = false;
					break;
			}

			if (boIncludePreSharedKeyInString)
			{
				sbReturnString.AppendFormat("PreSharedKey={0}&",szPreSharedKey);
			}

			sbReturnString.AppendFormat("MerchantID={0}&Password={1}&StatusCode={2}&Message={3}" + 
										"&PreviousStatusCode={4}&PreviousMessage={5}&CrossReference={6}" + 
										"&Amount={7}&CurrencyCode={8}&OrderID={9}&TransactionType={10}" +
										"&TransactionDateTime={11}&OrderDescription={12}&CustomerName={13}" + 
										"&Address1={14}&Address2={15}&Address3={16}&Address4={17}&City={18}" + 
										"&State={19}&PostCode={20}&CountryCode={21}",
                                            szMerchantID, szPassword, trTransactionResult.StatusCode,
										    trTransactionResult.Message, trTransactionResult.PreviousStatusCode,
										    trTransactionResult.PreviousMessage, trTransactionResult.CrossReference,
										    trTransactionResult.Amount, trTransactionResult.CurrencyCode,
										    trTransactionResult.OrderID, trTransactionResult.TransactionType,
										    trTransactionResult.TransactionDateTime, trTransactionResult.OrderDescription,
										    trTransactionResult.CustomerName, trTransactionResult.Address1,
										    trTransactionResult.Address2, trTransactionResult.Address3,
										    trTransactionResult.Address4, trTransactionResult.City,
										    trTransactionResult.State, trTransactionResult.PostCode,
										    trTransactionResult.CountryCode);

            return (sbReturnString.ToString());
        }
        public static string GenerateStringToHashCancel(string szMerchantID,
                                                  string szPassword,
                                                  TransactionResult trTransactionResult,
                                                  string szPreSharedKey,
                                                  HASH_METHOD hmHashMethod)
        {
            StringBuilder sbReturnString;
            bool boIncludePreSharedKeyInString = false;

            sbReturnString = new StringBuilder();

            switch (hmHashMethod)
            {
                case HASH_METHOD.MD5:
                    boIncludePreSharedKeyInString = true;
                    break;
                case HASH_METHOD.SHA1:
                    boIncludePreSharedKeyInString = true;
                    break;
                case HASH_METHOD.HMACMD5:
                    boIncludePreSharedKeyInString = false;
                    break;
                case HASH_METHOD.HMACSHA1:
                    boIncludePreSharedKeyInString = false;
                    break;
            }

            if (boIncludePreSharedKeyInString)
            {
                sbReturnString.AppendFormat("PreSharedKey={0}&", szPreSharedKey);
            }

            sbReturnString.AppendFormat("MerchantID={0}&Password={1}&StatusCode={2}&Message={3}" +
                                        "&OrderID={4}",
                                            szMerchantID, szPassword, trTransactionResult.StatusCode,
                                            trTransactionResult.Message, trTransactionResult.OrderID);
            return (sbReturnString.ToString());
        }
        public static string GenerateStringToHash(string szMerchantID,
											      string szPassword,
												  string szCrossReference,
												  string szOrderID,
         		                                  string szPreSharedKey,
         		                                  HASH_METHOD hmHashMethod)
        {
            StringBuilder sbReturnString;
            bool boIncludePreSharedKeyInString = false;

			sbReturnString = new StringBuilder();

			switch (hmHashMethod)
			{
				case HASH_METHOD.MD5:
					boIncludePreSharedKeyInString = true;
					break;
				case HASH_METHOD.SHA1:
					boIncludePreSharedKeyInString = true;
					break;
				case HASH_METHOD.HMACMD5:
					boIncludePreSharedKeyInString = false;
					break;
				case HASH_METHOD.HMACSHA1:
					boIncludePreSharedKeyInString = false;
					break;
			}

			if (boIncludePreSharedKeyInString)
			{
				sbReturnString.AppendFormat("PreSharedKey={0}&",szPreSharedKey);
			}

            sbReturnString.AppendFormat("MerchantID={0}&Password={1}&CrossReference={2}&OrderID={3}", szMerchantID, szPassword, szCrossReference, szOrderID);

            return (sbReturnString.ToString());
        }
		public static bool ValidateTransactionResult_POST(string szMerchantID, 
													      string szPassword, 
													      string szPreSharedKey, 
													      HASH_METHOD hmHashMethod,
													      NameValueCollection aPostVariables,
													      out TransactionResult trTransactionResult,
													      out string szValidateErrorMessage)
		{
            bool boErrorOccurred;
            string szHashDigest;
            string szOutputMessage;
            string szStringToHash;
            string szCalculatedHashDigest;

			boErrorOccurred = false;

			szValidateErrorMessage = "";
			trTransactionResult = null;

			// read the transaction result variables from the post variable list
			if (!GetTransactionResultFromPostVariables(aPostVariables, out trTransactionResult, out szHashDigest, out szOutputMessage))
			{
				boErrorOccurred = true;
				szValidateErrorMessage = szOutputMessage;
			}
			else
			{
				// now need to validate the hash digest
				szStringToHash = GenerateStringToHash(szMerchantID, szPassword, trTransactionResult, szPreSharedKey,  hmHashMethod);
				szCalculatedHashDigest = CalculateHashDigest(szStringToHash, szPreSharedKey,  hmHashMethod);
			
				// does the calculated hash match the one that was passed?
				if (szHashDigest.ToUpper() != szCalculatedHashDigest.ToUpper())
				{
					boErrorOccurred = true;
					szValidateErrorMessage = "Hash digests don't match - possible variable tampering";
				}
				else
				{
					boErrorOccurred = false;
				}
			}

			return (!boErrorOccurred);
		}
		public static bool ValidateTransactionResult_SERVER(string szMerchantID, 
													        string szPassword, 
													        string szPreSharedKey, 
													        HASH_METHOD hmHashMethod,
													        NameValueCollection nvcQueryStringVariables,
													        out TransactionResult trTransactionResult,
													        out string szValidateErrorMessage)
		{
            bool boErrorOccurred;
            string szCrossReference;
            string szOrderID;
            string szHashDigest;
            string szOutputMessage;
            string szStringToHash;
            string szCalculatedHashDigest;

			boErrorOccurred = false;

			szValidateErrorMessage = "";
			trTransactionResult = null;

			// read the transaction reference variables from the query string variable list
			if (!GetTransactionReferenceFromQueryString(nvcQueryStringVariables, out szCrossReference, out szOrderID, out szHashDigest, out szOutputMessage))
			{
				boErrorOccurred = true;
				szValidateErrorMessage = szOutputMessage;
			}
			else
			{
				// now need to validate the hash digest
				szStringToHash = GenerateStringToHash(szMerchantID, szPassword, szCrossReference, szOrderID, szPreSharedKey,  hmHashMethod);
				szCalculatedHashDigest = CalculateHashDigest(szStringToHash, szPreSharedKey,  hmHashMethod);
			
				// does the calculated hash match the one that was passed?
				if (szHashDigest.ToUpper() != szCalculatedHashDigest.ToUpper())
				{
					boErrorOccurred = true;
					szValidateErrorMessage = "Hash digests don't match - possible variable tampering";
				}
				else
				{
					// use the cross reference and/or the order ID to pull the
					// transaction results out of storage
					if (!GetTransactionResultFromStorage(szCrossReference,
														 szOrderID,
														 out trTransactionResult,
														 out szOutputMessage))
					{
						szValidateErrorMessage = szOutputMessage;
						boErrorOccurred = true;
					}
					else
					{
						boErrorOccurred = false;
					}
				}
			}

			return (!boErrorOccurred);
		}
		public static bool ValidateTransactionResult_SERVER_PULL(string szMerchantID, 
													             string szPassword, 
													             string szPreSharedKey, 
													             HASH_METHOD hmHashMethod,
													             NameValueCollection nvcQueryStringVariables,
													             string szPaymentFormResultHandlerURL,
													             out TransactionResult trTransactionResult,
													             out string szValidateErrorMessage)
		{
            bool boErrorOccurred;
            string szCrossReference;
            string szOrderID;
            string szHashDigest;
            string szOutputMessage;
            string szStringToHash;
            string szCalculatedHashDigest;

			boErrorOccurred = false;

			szValidateErrorMessage = "";
			trTransactionResult = null;

			// read the transaction reference variables from the query string variable list
			if (!GetTransactionReferenceFromQueryString(nvcQueryStringVariables, out szCrossReference, out szOrderID, out szHashDigest, out szOutputMessage))
			{
				boErrorOccurred = true;
				szValidateErrorMessage = szOutputMessage;
			}
			else
			{
				// now need to validate the hash digest
				szStringToHash = GenerateStringToHash(szMerchantID, szPassword, szCrossReference, szOrderID, szPreSharedKey,  hmHashMethod);
				szCalculatedHashDigest = CalculateHashDigest(szStringToHash, szPreSharedKey,  hmHashMethod);
			
				// does the calculated hash match the one that was passed?
				if (szHashDigest.ToUpper() != szCalculatedHashDigest.ToUpper())
				{
					boErrorOccurred = true;
					szValidateErrorMessage = "Hash digests don't match - possible variable tampering";
				}
				else
				{
					// use the cross reference and/or the order ID to pull the
					// transaction results out of storage
					if (!GetTransactionResultFromPaymentFormHandler(szPaymentFormResultHandlerURL,
																	szMerchantID, 
																	szPassword,
																	szCrossReference,
																	out trTransactionResult,
																	out szOutputMessage))
					{
						szValidateErrorMessage = "Error querying transaction result (" + szCrossReference + ") from (" + szPaymentFormResultHandlerURL + "): " + szOutputMessage;
						boErrorOccurred = true;
					}
					else
					{
						boErrorOccurred = false;
					}
				}
			}

			return (!boErrorOccurred);
		}
        public static bool ValidateTransactionResult_CANCEL(string szMerchantID,
                                                          string szPassword,
                                                          string szPreSharedKey,
                                                          HASH_METHOD hmHashMethod,
                                                          NameValueCollection aPostVariables,
                                                          out TransactionResult trTransactionResult,
                                                          out string szValidateErrorMessage)
        {
            bool boErrorOccurred;
            string szHashDigest;
            string szOutputMessage;
            string szStringToHash;
            string szCalculatedHashDigest;

            boErrorOccurred = false;

            szValidateErrorMessage = "";
            trTransactionResult = null;

            // read the transaction result variables from the post variable list
            if (!GetTransactionReferenceFromQueryString(aPostVariables, out trTransactionResult, out szHashDigest, out szOutputMessage))
            {
                boErrorOccurred = true;
                szValidateErrorMessage = szOutputMessage;
            }
            else
            {
                // now need to validate the hash digest
                szStringToHash = GenerateStringToHashCancel(szMerchantID, szPassword, trTransactionResult, szPreSharedKey, hmHashMethod);
                szCalculatedHashDigest = CalculateHashDigest(szStringToHash, szPreSharedKey, hmHashMethod);

                // does the calculated hash match the one that was passed?
                if (szHashDigest.ToUpper() != szCalculatedHashDigest.ToUpper())
                {
                    boErrorOccurred = true;
                    szValidateErrorMessage = "Hash digests don't match - possible variable tampering";
                }
                else
                {
                    boErrorOccurred = false;
                }
            }

            return (!boErrorOccurred);
        }
		public static NameValueCollection ParseNameValueStringIntoArray(string szNameValueString, bool boURLDecodeValues)
		{
            string[] aszPostVariables;
            string[] aszSingleVariable;
            NameValueCollection nvcParsedVariables;
            string szName;
            string szValue;

			// break the reponse into an array
			// first break the variables up using the "&" delimter
			aszPostVariables = szNameValueString.Split('&');

			nvcParsedVariables = new NameValueCollection();

			foreach (string szVariable in aszPostVariables)
			{
				// for each variable, split is again on the "=" delimiter
				// to give name/value pairs
                aszSingleVariable = szVariable.Split('=');
                szName = aszSingleVariable[0];
				if (!boURLDecodeValues)
				{
                    szValue = aszSingleVariable[1];
				}
				else
				{
                    szValue = HttpUtility.UrlDecode(aszSingleVariable[1]);
				}

				nvcParsedVariables.Add(szName, szValue);
			}

            return (nvcParsedVariables);
		}
		public static bool GetTransactionResultFromPaymentFormHandler(string szPaymentFormResultHandlerURL,
																	  string szMerchantID,
																	  string szPassword,
																	  string szCrossReference,
																	  out TransactionResult trTransactionResult,
																	  out string szOutputMessage)
		{
            bool boErrorOccurred;
            string szResponse;
            StringBuilder sbPostString;
            NameValueCollection nvcParsedPostVariables;
            NameValueCollection nvcTransactionResultArray;
            string szErrorMessage;
            string szHashDigest;
            HttpWebRequest wrRequest;
            WebResponse wrResponse;
            StreamWriter swStreamWriter;
            Stream sResponseStream;
            StreamReader srStreamReader;
            Stream sRequestStream;

			boErrorOccurred = false;
			szOutputMessage = "";
			trTransactionResult = null;

			try
			{				
				// build up the post string
                sbPostString = new StringBuilder();
				sbPostString.AppendFormat("MerchantID={0}&Password={1}&CrossReference={2}", HttpUtility.UrlEncode(szMerchantID), HttpUtility.UrlEncode(szPassword), HttpUtility.UrlEncode(szCrossReference));

                wrRequest = (HttpWebRequest)WebRequest.Create(szPaymentFormResultHandlerURL);
                wrRequest.Method = "POST";
                wrRequest.ContentType = "application/x-www-form-urlencoded";
                wrRequest.KeepAlive = false;

                sRequestStream = wrRequest.GetRequestStream();
                swStreamWriter = new StreamWriter(sRequestStream);
                swStreamWriter.Write(sbPostString.ToString());
                swStreamWriter.Flush();
                sRequestStream.Close();

                wrResponse = wrRequest.GetResponse();
                sResponseStream = wrResponse.GetResponseStream();
                srStreamReader = new StreamReader(sResponseStream);
                szResponse = srStreamReader.ReadToEnd();
                sResponseStream.Close();
				
				if (szResponse == "")
				{
					boErrorOccurred = true;
					szOutputMessage = "Received empty response from payment form hander";
				}
				else
				{
					try
					{
						// parse the response into an array
						nvcParsedPostVariables = ParseNameValueStringIntoArray(szResponse, true);

						if (nvcParsedPostVariables["StatusCode"] == null ||
							Convert.ToInt32(nvcParsedPostVariables["StatusCode"]) != 0)
						{
							boErrorOccurred = true;

							// the message field is expected if the status code is non-zero
							if (nvcParsedPostVariables["Message"] == null)
							{
								szOutputMessage = "Received invalid response from payment form hander (" + szResponse + ")";
							}
							else
							{
								szOutputMessage = nvcParsedPostVariables["Message"];
							}
						}
						else
						{
							// status code is 0, so	get the transaction result
							if (nvcParsedPostVariables["TransactionResult"] == null)
							{
								boErrorOccurred = true;
								szOutputMessage = "No transaction result in response from payment form hander (" + szResponse + ")";
							}
							else
							{
								// parse the URL decoded transaction result field into a name value array
								nvcTransactionResultArray = ParseNameValueStringIntoArray(HttpUtility.UrlDecode(nvcParsedPostVariables["TransactionResult"]), false);

								// parse this array into a transaction result object
								if (!GetTransactionResultFromPostVariables(nvcTransactionResultArray, out trTransactionResult, out szHashDigest, out szErrorMessage))
								{
									boErrorOccurred = true;
									szOutputMessage = "Error (" + szErrorMessage + ") parsing transaction result (" + HttpUtility.UrlDecode(nvcParsedPostVariables["TransactionResult"]) + ") in response from payment form hander (" + szResponse + ")";
								}
								else
								{
									boErrorOccurred = false;
								}
							}
						}
					}
					catch (Exception e)
					{
						boErrorOccurred = true;
						szOutputMessage = "Exception (" + e.Message + ") when processing response from payment form handler (" + szResponse + ")";
					}
				}
			}
			catch (Exception e)
			{
				boErrorOccurred = true;
				szOutputMessage = e.Message;
			}

			return (!boErrorOccurred);
		}

        // These functions that are run to deal with storing and retrieving the
		// transaction results. They will be specific to the merchant environment, so cannot
		// be generalised. The developer MUST implement these functions

		// This function needs to be able to retrieve the saved transaction resultt
		// so that the result can be displayed to the customer
		public static bool GetTransactionResultFromStorage(string szCrossReference,
														   string szOrderID,
														   out TransactionResult trTransactionResult,
														   out string szOutputMessage)
		{
            bool boErrorOccurred;

			boErrorOccurred = true;
			szOutputMessage = "Environment specific function GetTransactionResultFromStorage() needs to be implemented by merchant developer";
			trTransactionResult = null;

			return (!boErrorOccurred);
		}

		// You should put your code that does any post transaction tasks
		// (e.g. updates the order object, sends the customer an email etc) in this function
        public static bool ReportTransactionResults(TransactionResult trTransactionResult,
												    out string szOutputMessage)
        {
            bool boErrorOccurred;

            boErrorOccurred = true;
			szOutputMessage = "Environment specific function ReportTransactionResults() needs to be implemented by merchant developer";

			try
			{
				switch (trTransactionResult.StatusCode)
				{
					// transaction authorised
					case 0:
						break;
					// card referred (treat as decline)
					case 4:
						break;
					// transaction declined
					case 5:
						break;
					// duplicate transaction
					case 20:
						// need to look at the previous status code to see if the
						// transaction was successful
						if (trTransactionResult.PreviousStatusCode.Value == 0)
						{
							// transaction authorised
						}
						else
						{
							// transaction not authorised
						}
						break;
                    // transaction cancelled
                    case 25:
                        break;
					// error occurred
					case 30:
						break;
					default:
						break;
				}

				// put code to update/store the order with the transaction result
			}
			catch (Exception e)
			{
				boErrorOccurred = true;
				szOutputMessage = e.Message;
			}
			return (!boErrorOccurred);
        }
    }
}