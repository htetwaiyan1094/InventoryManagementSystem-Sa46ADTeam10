﻿using LUSSISADTeam10Web.API;
using LUSSISADTeam10Web.Constants;
using LUSSISADTeam10Web.Models;
using LUSSISADTeam10Web.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace LUSSISADTeam10Web.Controllers
{
    [Authorize(Roles = "HOD")]
    public class HODController : Controller
    {
        // GET: HOD
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequisitionsList()
        {
            string token = "";
            token = (string)Session["token"];
            List<RequisitionModel> reqms = APIRequisition.GetAllRequisition(token, out string error);

            if (error == "")
            {
                return View(reqms);
            }

            return View(new List<RequisitionModel>());
        }
        public ActionResult TrackRequisition(int id)
        {
            string token = "";
            token = (string)Session["token"];
            RequisitionModel reqm = APIRequisition.GetRequisitionByReqid(id, token, out string error);
            return View(reqm);
        }
    }
}