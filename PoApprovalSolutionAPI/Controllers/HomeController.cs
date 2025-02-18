using PoApprovalSolutionAPI.Models;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoApprovalSolutionAPI.Controllers
{
    public class HomeController : ApiController
    {
        private readonly string UserGmail;
        string Msg = string.Empty;
        string url = string.Empty;
        string StatusCode = "NA";
        RfcRepository repo;
        List<string> Responce = new List<string>();
        // GET: api/Home/5
        public List<string> Get(int id)
        {


            // Configure SAP connection
            RfcDestination designation = null;
            HANADesignationConfig config = new HANADesignationConfig();
            //avd
            try
            {


                // Register the SAP destination configuration
                RfcDestinationManager.RegisterDestinationConfiguration(config);
                designation = RfcDestinationManager.GetDestination("SHX");
                // Get the SAP repository
                repo = designation.Repository;

                designation.Ping();

                try
                {

                    Msg = "Connected To SAP";
                }
                catch (Exception ex)
                {
                    Msg = ex.Message;

                    RfcDestinationManager.UnregisterDestinationConfiguration(config);
                    repo = null;
                    designation = null;
                }


                IRfcFunction rfcFunction = designation.Repository.CreateFunction("ZMM_PO_APPR");
                rfcFunction.SetValue("IM_USRID", id);
                // Call the SAP function for this single row
                rfcFunction.Invoke(designation);
                url = rfcFunction.GetValue("EX_RE_URL").ToString();

           

               
                RfcDestinationManager.UnregisterDestinationConfiguration(config);
                repo = null;
                designation = null;

                StatusCode = "1";

                Responce.Add(url);
                Responce.Add(Msg);
                Responce.Add(StatusCode);
                // return Json<object>(obj);
                return Responce;
            }
            catch (Exception ex)
            {
                
                {
                    Msg = ex.Message;
                    url = "Unthorised Tarnsaction Unable To Connect SAP";
                    RfcDestinationManager.UnregisterDestinationConfiguration(config);
                    repo = null;
                    designation = null;
                  
                    StatusCode = "0";

                    Responce.Add(url);
                    Responce.Add(Msg);
                    Responce.Add(StatusCode);
                    return Responce;
                }
            }

        }
    }
}
