﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUSSISADTeam10API.Models.APIModels
{
    public class RequisitionWithOutstandingModel

    {
        public RequisitionWithOutstandingModel(int reqid, int? rasiedby, String rasiedbyname,
            int? approvedby, String approvedbyname, int cpid, string cpname,
            int depid, string depname, int status, DateTime? reqdate,
            int lockerid,
            string lockername,
            List<RequisitionDetailsWithOutstandingModel> rdms)
        {

            this.Reqid = reqid;
            this.Raisedby = rasiedby;
            this.Rasiedbyname = rasiedbyname;
            this.Approvedby = approvedby;
            this.Approvedbyname = approvedbyname;
            this.Cpid = cpid;
            this.Cpname = cpname;
            this.Depid = depid;
            this.Depname = depname;
            this.Status = status;
            this.Reqdate = reqdate;
            this.Requisitiondetails = rdms;
            this.LockerID = lockerid;
            this.LockerName = lockername;


        }
        public RequisitionWithOutstandingModel() : this(0, 0, "", 0, "", 0, "", 0, "", 0, null, 0, "", null)
        {
        }

        public int Reqid { get; set; }

        public int? Raisedby { get; set; }
        public String Rasiedbyname { get; set; }

        public int? Approvedby { get; set; }
        public String Approvedbyname { get; set; }

        public int Depid { get; set; }
        public String Depname { get; set; }

        public int Cpid { get; set; }
        public String Cpname { get; set; }

        public int LockerID { get; set; }
        public string LockerName { get; set; }

        public int Status { get; set; }


        public DateTime? Reqdate { get; set; }

        public List<RequisitionDetailsWithOutstandingModel> Requisitiondetails { get; set; }
    }
}